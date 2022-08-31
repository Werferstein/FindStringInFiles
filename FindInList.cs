/*
    2015 Ingolf Hill http://www.werferstein.org

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    Dieses Programm ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder neueren
    veröffentlichten Version, weiter verteilen und/oder modifizieren.

    Dieses Programm wird in der Hoffnung bereitgestellt, dass es nützlich sein wird, jedoch
    OHNE JEDE GEWÄHR,; sogar ohne die implizite
    Gewähr der MARKTFÄHIGKEIT oder EIGNUNG FÜR EINEN BESTIMMTEN ZWECK.
    Siehe die GNU General Public License für weitere Einzelheiten.

    Sie sollten eine Kopie der GNU General Public License zusammen mit diesem
    Programm erhalten haben. Wenn nicht, siehe <https://www.gnu.org/licenses/>
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindStringInFile
{
    #region EventArgs
    public delegate bool FinderStateHandler(object sender, FindEventArgs e);

    [Flags]
    public enum Status:UInt32
    {
        Null = 0,
        Run =    1 << 0,
        Result = 1 << 2,
        Error =  1 << 3,
        Count =  1 << 4,
        Cancel = 1 << 5,
        FileLoop = 1 << 6,
        FileMessage = 1 << 7,
    }

    public class FindEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Status State { get; set; }
        public List<FindPos> Resuld { get; set; }
    }
    #endregion

    #region class FindPos
    public class FindPos
    {
        public int Sort { get; set; }
        public int LineNr { get; set; }
        public int Pos { get; set; }
        public string File { get; set; }
        public long FileSize { get; set; }
        public DateTime LastModificationDate { get; set; }
    }
    #endregion

    public class FindThread
    {
        private Status mThreadStatus = Status.Null;
        /// <summary>
        /// actual state of the instance
        /// </summary>
        public Status ThreadStatus { get => mThreadStatus; }

        //public static event FinderStateHandler MyFinderState;
        private static int taskCount = 0;
        public static int TaskCount { get => taskCount; set => taskCount = value; }// set => taskCount = value; }

        private static int mfoundCount = 0;
        public static int FoundCount { get => mfoundCount; }


        private static int instanceNr = 0;
        private static List<FindPos> mFindInList = new List<FindPos>();
        public static List<FindPos> FindInList { get => mFindInList; }
       

        public static int SortNr = 0;


        public static void AddList(FindPos p)
        {
            mFindInList.Add(p);
            SortNr++;
        }

        public static void ÎNT_Find()
        {
            taskCount = 0;
            instanceNr = 0;
            SortNr = 0;
            mfoundCount = 0;
            mFindInList.Clear();
        }


        public async Task<int> FindIn(string nr, string path, FileInfo info, string encoding = "UTF8")
        {
            if (Finder.mStopWorking.IsCancellationRequested) return -1;
            
            await Task.Run(() =>
            {
                mThreadStatus |= Status.Run;
                if (instanceNr < int.MaxValue) instanceNr++;
#if DEBUG
                Console.WriteLine("Instance: " + instanceNr.ToString()); 
#endif
                int lineNr = 1;
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        //FindEventArgs ar = new FindEventArgs
                        //{
                        //    State = Finder.ProgStatus | Status.FileMessage,
                        //    Message = path
                        //};
                        //Finder.SendMessage(ar);

                        #region Encoding
                        Encoding enc = Encoding.UTF8;
                        if ("UTF8" == encoding.ToUpper())
                        {
                            enc = Encoding.UTF8;
                        }
                        else if ("ASCII" == encoding.ToUpper())
                        {
                            enc = Encoding.ASCII;
                        }
                        else if ("UTF7" == encoding.ToUpper())
                        {
                            enc = Encoding.UTF7;
                        }
                        else if ("UTF32" == encoding.ToUpper())
                        {
                            enc = Encoding.UTF32;
                        }
                        else if ("Unicode" == encoding.ToUpper())
                        {
                            enc = Encoding.Unicode;
                        }
                        #endregion


                        
                        foreach (var line in File.ReadLines(path, enc))
                        {
                            if (!Finder.mStopWorking.IsCancellationRequested)
                            {
                                if (line.Contains(nr))
                                {
                                    int posNr = 1;
                                    while ((posNr = line.IndexOf(nr, posNr)) != -1)
                                    {
                                        if (!Finder.mStopWorking.IsCancellationRequested)
                                        {
                                            FindThread.AddList(new FindPos { Sort = SortNr, LineNr = lineNr, Pos = posNr, File = path, FileSize = info.Length, LastModificationDate = info.LastWriteTime });
                                            posNr += nr.Length;
                                            mfoundCount++;
                                        }
                                        else
                                        {
                                            Finder.CancellationRequested("CancellationRequested -> Line: " + lineNr.ToString());
                                            break;
                                        }
                                    }
                                }
                                lineNr++;
                            }
                            else
                            {
                                Finder.CancellationRequested("CancellationRequested -> Line: " + lineNr.ToString());
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        mThreadStatus |= Status.Error;

                        FindEventArgs ar = new FindEventArgs
                        {
                            State = Status.Error,
                            Message = "Instance Nr: " + instanceNr.ToString() + " "+  ex.Message
                        };
                        Finder.SendMessage(ar);
#if DEBUG
                        Console.WriteLine("Instance Nr: " + instanceNr.ToString() + " ERROR: " + ex.Message);
#endif
                        FindThread.AddList(new FindPos { Sort = SortNr, LineNr = lineNr, Pos = -1, File = "ERROR: " + path + ", " + ex.Message, FileSize = 0, LastModificationDate = DateTime.MinValue});
                    }
                }
                else
                {
                    mThreadStatus |= Status.Error;
                    FindEventArgs ar = new FindEventArgs
                    {
                        State = Status.Error,
                        Message = "Instance Nr: " + instanceNr.ToString() + " ERROR: File? " + path
                    };
                    Finder.SendMessage(ar);

#if DEBUG
                    Console.WriteLine("Instance Nr: " + instanceNr.ToString() + " ERROR: File? " + path);
#endif

                    FindThread.AddList(new FindPos { Sort = SortNr, LineNr = -1, Pos = -1, File = "ERROR: File? " + path, FileSize = 0, LastModificationDate = DateTime.MinValue });
                }
#if DEBUG
                Console.WriteLine("Kill instance: " + instanceNr.ToString() + " Task count:" + taskCount.ToString());
#endif
                taskCount--;
                Finder.FileCount--;
            });
            return instanceNr;
        }
    }


    public static class Finder
    {
        public static event FinderStateHandler MyFinderState;
        public static List<Task> TaskList = new List<Task>();
        private static System.Timers.Timer Test_timer = new System.Timers.Timer();

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr point);

        private static StringBuilder mMessage = new StringBuilder();
        public static StringBuilder Message { get => mMessage; }
        public static void AddMessage(string st)
        {
            mMessage.AppendLine(st);
        }

        public static int FileCount { get; set; }

        //private static int taskCount = 0;
        //private static int instanceNr = 0;

        //private static List<FindPos> mFindInList;
        //private static StringBuilder mMessage = new StringBuilder();

        /// <summary>
        /// Nummer zum Sortieren in der Liste
        /// </summary>
        //private static int sortNr;

        #region Async Signal
        public static CancellationTokenSource mStopWorking;

        private static Status mProgStatus = Status.Result;
        public static Status ProgStatus { get { return mProgStatus; } }
        public static Status SetProgStatus(Status var)
        {
            mProgStatus = var;
            return mProgStatus;
        }


        public static Task MyTask { get; set; }

        public static void Cancel()
        {

            if (mStopWorking != null)
            {
                mStopWorking.Cancel();
            }

            mProgStatus &= ~Status.Run;
            mProgStatus |= Status.Cancel;
            mProgStatus |= Status.Result;

            FindThread.ÎNT_Find();
            FindThread.SortNr = 1;

            FindEventArgs ar = new FindEventArgs
            {
                State = mProgStatus,
                Message = Finder.Message.ToString()
            };
            SendMessage(ar);
        }


        public static void CancellationRequested(string caller)
        {
            mMessage.AppendLine(caller);

            mProgStatus |= Status.Cancel;

            FindEventArgs ar = new FindEventArgs
            {
                State = mProgStatus,
                Message = caller
            };
            SendMessage(ar);
        }

        public static void SendMessage(FindEventArgs ar)
        {
            if (MyFinderState != null && ar != null)
            {
                MyFinderState(typeof(Finder).ToString(), ar);
            }
        }


        private static void SignalStart(string caller)
        {
            mMessage = new StringBuilder();
            mStopWorking = new CancellationTokenSource();
            mMessage.AppendLine("Start");
            mMessage.AppendLine(caller);

           

            FindEventArgs ar = new FindEventArgs
            {
                State = mProgStatus,
                Message = caller
            };
            SendMessage(ar);
        }
        public static void SignalEnd()
        {
            mProgStatus &= ~Status.Run;
            mProgStatus |= Status.Result;
            


            FindEventArgs ar = new FindEventArgs
            {
                State = mProgStatus,
                Message = Finder.Message.ToString()
            };
            if (FindThread.FindInList != null & FindThread.FindInList.Count > 0)
            { ar.Resuld = FindThread.FindInList; }
            else
            { ar.Resuld = null; }
            SendMessage(ar);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region ToDataTable
        public static DataTable ToDataTable(List<FindPos> mInList)
        {
            if (mInList != null && mInList.Count > 0)
            {
                DataTable r = new DataTable();

                DataColumn column = new DataColumn("Sort", typeof(int));
                r.Columns.Add(column);
                column = new DataColumn("LineNr", typeof(int));
                r.Columns.Add(column);
                column = new DataColumn("Pos", typeof(int));
                r.Columns.Add(column);
                column = new DataColumn("File", typeof(string));
                r.Columns.Add(column);
                column = new DataColumn("Select", typeof(bool));
                r.Columns.Add(column);

                for (int i = 0; i < mInList.Count; i++)
                {
                    if (mInList[i] != null)
                    {
                        DataRow row = r.NewRow();
                        row["Sort"] = mInList[i].Sort;
                        row["LineNr"] = mInList[i].LineNr;
                        row["Pos"] = mInList[i].Pos;
                        row["File"] = mInList[i].File;
                        row["Select"] = true;
                        r.Rows.Add(row);
                    }
                }
                
                return r;
            }
            else
            {
                return null;
            }
        }
        #endregion

        private static void Test_TimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            if (Finder.TaskList.Count == 0 &&
               (Finder.ProgStatus & Status.Run) > 0)
            {
                Finder.SignalEnd();
            }

            #region Remove closed tasks from list
            List<Task> ToKill = new List<Task>();
            for (int i = 0; i < Finder.TaskList.Count; i++)
            {
                if ((Finder.TaskList[i] != null && (Finder.TaskList[i].Status == TaskStatus.RanToCompletion || Finder.TaskList[i].IsCompleted)) ||
                    Finder.TaskList[i] == null)
                {
                    ToKill.Add(Finder.TaskList[i]);
                }
            }

            if (ToKill != null && ToKill.Count > 0)
            {
                int p = 0;
                foreach (var item in ToKill)
                {
                    if (item != null && (item.Status == TaskStatus.RanToCompletion || item.Status == TaskStatus.Canceled || item.Status == TaskStatus.Faulted)) item.Dispose();
                    Finder.TaskList.Remove(item);
                    p++;
                }
#if DEBUG
                Console.WriteLine(p.ToString() + " Finished tasks have been deleted from the list!");
#endif
            }
            #endregion

        }

        public static void FindAll(Config conf)
        {
            System.IO.SearchOption s = SearchOption.AllDirectories;
            if (conf.TopDirectoryOnly)
            {
                s = SearchOption.TopDirectoryOnly;
            }            
            FindAll(conf.FindString, conf.FindInDir, conf.Filter, s, conf.MaxSearchInstances,  conf.StrEncoding,conf.MaxFileSizeMB );
        }
 


        /// <summary>
        /// In Einem Verzeichnis werden alle Dateien nach einem String durchsucht und Treffer in eine Liste eingetragen
        /// </summary>
        /// <param name="findStr"></param>
        /// <param name="dirPath"></param>
        /// <param name="filter"></param>
        /// <param name="searchOption"></param>
        /// 
        /// 
        /// <returns></returns>
        public static void FindAll(string findStr, string dirPath, string filter, System.IO.SearchOption searchOption, int maxTask = 6, string encoding = "UTF8", long maxFileSize = 1)
        {
            if (maxTask < 1) maxTask = 1;
            if (maxFileSize < 1) maxFileSize = 1;
            if (string.IsNullOrWhiteSpace(findStr) || string.IsNullOrWhiteSpace(dirPath)) return;
            
            FindThread u = new FindThread();
            
            Cancel();       //stop still running tasks

            #region int test timer
            if (TaskList.Count> 0)
            {
                while (TaskList.Count > 0)
                {
                    if (TaskList[0] != null && (TaskList[0].Status == TaskStatus.RanToCompletion || TaskList[0].Status == TaskStatus.Canceled || TaskList[0].Status == TaskStatus.Faulted) ) TaskList[0].Dispose();
                    TaskList.Remove(TaskList[0]);
                }
            }            
            if (Test_timer.Enabled) Test_timer.Stop();
            Test_timer.Elapsed += new System.Timers.ElapsedEventHandler(Test_TimerEvent);
            Test_timer.Interval = 1000;
            Test_timer.Start();
            #endregion

            MyTask = Task.Run(() =>
            {
                SignalStart("Find:" + findStr + " in " + dirPath + " only: " + filter + " with " + searchOption.ToString() +" and max. file size: " + maxFileSize.ToString() + " mb");
                //INT
                FindThread.ÎNT_Find();
                
  



                if (Directory.Exists(dirPath))
                {
                    var x = GetFiles(dirPath, filter, searchOption, maxFileSize);
                    if (x.Count() > 0)
                    {
                        foreach (string file in x)
                        {
                            Finder.mProgStatus |= Status.FileLoop;//file loop start
                            while (FindThread.TaskCount > maxTask)
                            {                                
                                Thread.Sleep(1);
                                System.Windows.Forms.Application.DoEvents();
                                if (FindThread.TaskCount < maxTask) break;
                            }

                            FileInfo info = new FileInfo(file);

                            if (info.Exists &&
                                info.Length < (maxFileSize * 1000000))
                            {
                                if (!mStopWorking.IsCancellationRequested)
                                {
                                    //Thread.Sleep(5);
                                    FindThread.TaskCount++;
                                    Task Task2 = Task.Run(() =>
                                    {
                                        TaskList.Add(u.FindIn(findStr, file, info, encoding));
                                        mProgStatus |= Status.Run;
                                    });
                                }
                                else
                                {
                                    CancellationRequested("CancellationRequested -> File: " + file);
                                    break;
                                }
                            }
                            Finder.mProgStatus &= ~Status.FileLoop;//file loop end
                        }
                    }
                }
                else
                {
                    FindThread.AddList(new FindPos { Sort = FindThread.SortNr, LineNr = -1, Pos = -1, File = "ERROR: Directory? " + dirPath, FileSize = 0, LastModificationDate = DateTime.MinValue });                    
                }

                //while (FindTest.TaskCount > 0)
                //{
                //    Thread.Sleep(500);
                //    if (mStopWorking.IsCancellationRequested) break;
                //}
                //SignalEnd();
            }
            
            );


            return;
        }

        #region put all file links in an array
        private static string[] GetFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption, long maxFileSize)
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters;
            if (Filter != string.Empty)
            {
                // Create an array of filter string
                MultipleFilters = Filter.Split('|');
            }
            else
            {
                MultipleFilters = new[] { "*" };
            }

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                if (!mStopWorking.IsCancellationRequested)
                {
                    if (FileFilter.Trim().ToLower() != string.Empty)
                    {
                        try
                        {
                            if (searchOption == SearchOption.TopDirectoryOnly)
                            {
                                try
                                {
                                    // add found file names to array list
                                    alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter.Trim().ToLower(), searchOption));
                                }
                                catch (Exception ex)
                                {
                                    FindThread.AddList(new FindPos { Sort = FindThread.SortNr, LineNr = -1, Pos = -1, File = ex.Message, FileSize = 0, LastModificationDate = DateTime.MinValue });
                                }
                            }
                            else
                            {
                                //erst TopDirectoryOnly
                                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter.Trim().ToLower(), SearchOption.TopDirectoryOnly));

                                //dann die anderen
                                DirectoryInfo dirInfo = new DirectoryInfo(SourceFolder);
                                DirectoryInfo[] dInfo = dirInfo.GetDirectories();

                                foreach (DirectoryInfo item in dInfo)
                                {
                                    if (
                                        !item.FullName.ToUpper().Contains("$RECYCLE.BIN") &&
                                        !item.FullName.ToUpper().Contains("SYSTEM VOLUME INFORMATION")
                                        )
                                    {
                                        try
                                        {
                                            // add found file names to array list
                                            alFiles.AddRange(Directory.GetFiles(item.FullName, FileFilter.Trim().ToLower(), searchOption));
                                        }
                                        catch (Exception ex)
                                        {
                                            mProgStatus |= Status.Error;
                                            Finder.SendMessage(new FindEventArgs
                                            {
                                                State = Status.Error,
                                                Message = ex.Message
                                            });
                                            FindThread.AddList(new FindPos { Sort = FindThread.SortNr, LineNr = -1, Pos = -1, File = ex.Message, FileSize = 0, LastModificationDate = DateTime.MinValue });
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            mProgStatus |= Status.Error;
                            Finder.SendMessage(new FindEventArgs
                            {
                                State = Status.Error,
                                Message = ex.Message
                            });

                            FindThread.AddList(new FindPos { Sort = FindThread.SortNr, LineNr = -1, Pos = -1, File = ex.Message });
                        }

                        #region send file count to gui
                        FileCount = alFiles.Count;
                        FindEventArgs ar = new FindEventArgs
                        {
                            State = Status.Count,
                            Message = alFiles.Count.ToString()
                        };
                        Finder.SendMessage(ar);
                        #endregion
                    }
                }
                else
                {
                    CancellationRequested("CancellationRequested -> File filter: " + FileFilter);
                    break;
                }
            }
            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        } 
        #endregion

        public static void ByteTest() 
        {
            //byte[] haystack = new byte[10000];

            byte[] haystack = Encoding.UTF8.GetBytes("FJNGAFUZIKJHKANPANMANGHJGFUIGERUNGJK");

            //byte[] bytePattern = { 0x00, 0x69, 0x73, 0x6F, 0x6D };

            byte[] bytePattern = Encoding.UTF8.GetBytes("ANPANMAN");

            // Put a few copies of the needle into the haystack.

            //for (int i = 1000; i <= 9000; i += 1000)
            //    Array.Copy(bytePattern, 0, haystack, i, bytePattern.Length);

            var searcher = new FindSequenceOfBytes(bytePattern);

            foreach (int index in searcher.Search(haystack))
                Console.WriteLine(index);
        }
    }
    

    
}