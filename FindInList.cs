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

    public enum Status
    {
        Null = 0,
        Start = 1,
        Stop = 2,
        Error = 3,
        Found = 4,
        Cancel = 5,
        FileMessage = 6
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
    } 
    #endregion

    public static class Finder
    {
        public static event FinderStateHandler MyFinderState;

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr point);
        private static int taskCount = 0;

        private static List<FindPos> mFindInList;
        private static StringBuilder mMessage = new StringBuilder();
        
        /// <summary>
        /// Nummer zum Sortieren in der Liste
        /// </summary>
        private static int sortNr;

        #region Async Signal
        private static CancellationTokenSource mStopWorking;

        private static Status mProgStatus = Status.Stop;
        public static Status ProgStatus { get { return mProgStatus; } }

        public static Task MyTask { get; set; }

        public static void Cancel()
        {
            if (mStopWorking != null)
            {
                mStopWorking.Cancel();
            }

            mProgStatus = Status.Stop;
            mFindInList = new List<FindPos>();
            sortNr = 1;

            if (MyFinderState != null)
            {
                FindEventArgs ar = new FindEventArgs
                {
                    State = Status.Cancel,
                    Message = mMessage.ToString()
                };
                MyFinderState(typeof(Finder).ToString(), ar);
            }
        }


        private static void CancellationRequested(string caller)
        {
            mMessage.AppendLine(caller);

            mProgStatus = Status.Cancel;

            if (MyFinderState != null)
            {
                FindEventArgs ar = new FindEventArgs
                {
                    State = Status.Cancel,
                    Message = caller
                };
                MyFinderState(typeof(Finder).ToString(), ar);
            }
        }
        private static void SignalStart(string caller)
        {
            mMessage = new StringBuilder();
            mStopWorking = new CancellationTokenSource();
            mMessage.AppendLine("Start");
            mMessage.AppendLine(caller);

            mProgStatus = Status.Start;

            if (MyFinderState != null)
            {
                FindEventArgs ar = new FindEventArgs
                {
                    State = Status.Start,
                    Message = caller
                };
                MyFinderState(typeof(Finder).ToString(), ar);
            }
        }
        private static void SignalEnd()
        {
            mProgStatus = Status.Stop;

            if (MyFinderState != null)
            {
                FindEventArgs ar = new FindEventArgs
                {
                    State = Status.Stop,
                    Message = mMessage.ToString()
                };

                if (mFindInList != null & mFindInList.Count > 0)
                { ar.Resuld = mFindInList; }
                else
                { ar.Resuld = null; }

                MyFinderState(typeof(Finder).ToString(), ar);
            }
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


        /// <summary>
        /// In Einem Verzeichnis werden alle Dateien nach einem String durchsucht und Treffer in eine Liste eingetragen
        /// </summary>
        /// <param name="findStr"></param>
        /// <param name="dirPath"></param>
        /// <param name="filter"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static void FindAll(string findStr, string dirPath, string filter, System.IO.SearchOption searchOption, int maxTask = 6, string encoding = "UTF8")
        {

            if (maxTask > 10) maxTask = 10;
            if (maxTask < 1) maxTask = 1;
            
            if (string.IsNullOrWhiteSpace(findStr) || string.IsNullOrWhiteSpace(dirPath))
            {
                return;
            }


            MyTask = Task.Run(() =>
            {
                SignalStart("Find:" + findStr + " in " + dirPath + " only: " + filter + " with " + searchOption.ToString());
                //INT
                mFindInList = new List<FindPos>();
                sortNr = 1;
                taskCount = 0;

    

                if (Directory.Exists(dirPath))
                {
                    var x = GetFiles(dirPath, filter, searchOption);

                    if (x.Count() > 0)
                    {
                        foreach (string file in x)
                        {

                            
                            while (taskCount > maxTask)
                            {
                                Thread.Sleep(50);
                                if (taskCount < maxTask) break;
                            }


                            if (!mStopWorking.IsCancellationRequested)
                            {
                                Thread.Sleep(50);

                                Task Task2 = Task.Run(() =>
                                {
                                    Find(findStr, file, encoding);          
                                });
                            }
                            else
                            {
                                CancellationRequested("CancellationRequested -> File: " + file);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    mFindInList.Add(new FindPos { Sort = sortNr, LineNr = -1, Pos = -1, File = "ERROR: Directory? " + dirPath });
                    sortNr++;
                }

                SignalEnd();
            }
            
            );


            return;
        }


        //private static void Find(byte[] bytes, string path)
        //{

        //}

        private static void Find(string nr, string path, string encoding = "UTF8")
        {
            taskCount++;

            int lineNr = 1;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    if (MyFinderState != null)
                    {
                        FindEventArgs ar = new FindEventArgs
                        {
                            State = Status.FileMessage,
                            Message = path
                        };
                        MyFinderState(typeof(Finder).ToString(), ar);
                    }

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




                    foreach (var line in File.ReadLines(path,enc))
                    {
                        if (!mStopWorking.IsCancellationRequested)
                        {
                            if (line.Contains(nr))
                            {
                                int posNr = 1;
                                while ((posNr = line.IndexOf(nr, posNr)) != -1)
                                {
                                    if (!mStopWorking.IsCancellationRequested)
                                    {
                                        mFindInList.Add(new FindPos { Sort = sortNr, LineNr = lineNr, Pos = posNr, File = path });
                                        posNr += nr.Length;
                                        sortNr++;
                                    }
                                    else
                                    {
                                        CancellationRequested("CancellationRequested -> Line: " + lineNr.ToString());
                                        break;
                                    }
                                }
                            }
                            lineNr++;
                        }
                        else
                        {
                            CancellationRequested("CancellationRequested -> Line: " + lineNr.ToString());
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (MyFinderState != null)
                    {
                        FindEventArgs ar = new FindEventArgs
                        {
                            State = Status.Error,
                            Message = ex.Message
                        };
                        MyFinderState(typeof(Finder).ToString(), ar);
                    }

                    mFindInList.Add(new FindPos { Sort = sortNr, LineNr = lineNr, Pos = -1, File = "ERROR: " + path + ", " + ex.Message });
                    sortNr++;
                }
            }
            else
            {
                if (MyFinderState != null)
                {
                    FindEventArgs ar = new FindEventArgs
                    {
                        State = Status.Error,
                        Message = "ERROR: File? " + path
                    };
                    MyFinderState(typeof(Finder).ToString(), ar);
                }

                mFindInList.Add(new FindPos { Sort = sortNr, LineNr = -1, Pos = -1, File = "ERROR: File? " + path });
                sortNr++;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            taskCount--;
        }

        private static string[] GetFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption)
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
                                    mFindInList.Add(new FindPos { Sort = sortNr, LineNr = -1, Pos = -1, File = ex.Message });
                                    sortNr++;
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
                                            if (MyFinderState != null)
                                            {
                                                FindEventArgs ar = new FindEventArgs
                                                {
                                                    State = Status.Error,
                                                    Message = ex.Message
                                                };
                                                MyFinderState(typeof(Finder).ToString(), ar);
                                            }

                                            mFindInList.Add(new FindPos { Sort = sortNr, LineNr = -1, Pos = -1, File = ex.Message });
                                            sortNr++;
                                        }
                                    }
                                }
                            }





                        }
                        catch (Exception ex)
                        {
                            if (MyFinderState != null)
                            {
                                FindEventArgs ar = new FindEventArgs
                                {
                                    State = Status.Error,
                                    Message = ex.Message
                                };
                                MyFinderState(typeof(Finder).ToString(), ar);
                            }

                            mFindInList.Add(new FindPos { Sort = sortNr, LineNr = -1, Pos = -1, File = ex.Message });
                            sortNr++;
                        }                        
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
    } 
}