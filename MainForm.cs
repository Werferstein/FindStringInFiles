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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zuby.ADGV;

namespace FindStringInFile
{


    public partial class FindStringInFileForm : Form
    {
        
        private bool blockGui = false;
        AdvancedDataGridView DataGrid;

        public FindStringInFileForm()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;
            buttonCancel.Visible = false;

            this.Load += FindStringInFileForm_Load;

            #region Find notepad++.exe
            //Find notepad++.exe
            if (string.IsNullOrWhiteSpace(toolStripTextBoxNotepadPath.Text))
            {
                try
                {
                    Task.Run(() =>
                    {
                        string NotepadPath = (System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "notepad++.exe", System.IO.SearchOption.AllDirectories))[0];

                        if (string.IsNullOrWhiteSpace(NotepadPath) || !File.Exists(NotepadPath))
                        {
                            NotepadPath = (System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "notepad++.exe", System.IO.SearchOption.AllDirectories))[0];
                        }
                        if (!string.IsNullOrWhiteSpace(NotepadPath) && File.Exists(NotepadPath))
                        {
                            GetPath(NotepadPath);
                        }
                    }
                    );
                }
                catch { }
            }
            #endregion
        }

        private void FindStringInFileForm_Load(object sender, EventArgs e)
        {
            LoadData();
                      

        }

        private void GetPath(string path)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate { GetPath(path); };
                this.Invoke(del);
            }
            else
            {
                toolStripTextBoxNotepadPath.ToolTipText = path;
                toolStripTextBoxNotepadPath.Text = path;
                Program.ProgConfig.NodePadPlusPath = path;


                toolStripTextBoxNotepadPath.Invalidate();   
                this.Invalidate();
                this.Refresh();
            }
        }

        private void Button_RunButton_Click(object sender, EventArgs e)
        {
            if (!(Finder.ProgStatus == Status.Stop|| Finder.ProgStatus != Status.Stop)) {return;}

   
            Finder.MyFinderState -= Finder_MyFinderState;
            Finder.MyFinderState += Finder_MyFinderState;

            panelTable.Controls.Clear();

            if (string.IsNullOrWhiteSpace(textBoxNr.Text) || string.IsNullOrWhiteSpace(textBoxPath.Text))
            {                
                return;
            }


            StartProgressBar();
           

            Cursor.Current = Cursors.WaitCursor;

            SearchOption s = DirectorySelect.SelectedIndex != 0 ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            Finder.FindAll(textBoxNr.Text, textBoxPath.Text, textBoxFilter.Text, s, 6, Program.ProgConfig.StrEncoding);

            Cursor.Current = Cursors.Default;
        }

       

        private bool FillTable(DataTable result)
        {
            if (result == null) { return false; }


            
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate { FillTable(result); };
                this.Invoke(del);
            }
            else
            {
                //initialize dataset
                DataSet _dataSet = new DataSet();
                _dataSet.Tables.Add(result);

                //initialize bindingsource
                bindingSource_main = new BindingSource();
                bindingSource_main.ListChanged += BindingSource_main_ListChanged;
                bindingSource_main.DataSource = _dataSet;

                bindingSource_main.DataMember = result.TableName;





                DataGrid = new AdvancedDataGridView
                {
                    Dock = DockStyle.Fill
                };
                DataGrid.FilterStringChanged += Zuby_FilterStringChanged;
                DataGrid.SortStringChanged += Zuby_SortStringChanged;
                DataGrid.DoubleClick += DataGrid_DoubleClick;
                panelTable.Controls.Add(DataGrid);




                panel3.Controls.Clear();
                AdvancedDataGridViewSearchToolBar SearchToolBar2 = new AdvancedDataGridViewSearchToolBar
                {
                    Dock = DockStyle.Fill
                };
                SearchToolBar2.Search += SearchToolBar2_Search;
                panel3.Controls.Add(SearchToolBar2);


                //initialize datagridview
                DataGrid.DataSource = bindingSource_main;

                //initialize search            
                SearchToolBar2.SetColumns(DataGrid.Columns);
                SearchToolBar2.Visible = true;
                SearchToolBar2.Refresh();

                //Format Columns

                int x = 0;
                foreach (DataGridViewColumn item in DataGrid.Columns)
                {
                    if (x == 3)
                    {
                        item.Width = 300;
                    }
                    else { item.Width = 80; }

                    x++;
                }

            }
            //button_RunButton.Enabled = true;
            //buttonCancel.Visible = false;
            return true;
        }



        private bool Finder_MyFinderState(object sender, FindEventArgs e)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate { Finder_MyFinderState(sender, e); };
                this.Invoke(del);
            }
            else
            {

                if (e.State == Status.Error || e.State == Status.Cancel)
                {
                    button_RunButton.Enabled = true;
                    buttonCancel.Visible = false;
                    StopProgressBar();
                    return true;
                }

                else if (e.State == Status.Stop)
                {
                    if (e.Resuld != null && e.Resuld.Count > 0)
                    {
                        FillTable((Finder.ToDataTable(e.Resuld)));
                    }

                    button_RunButton.Enabled = true;
                    buttonCancel.Visible = false;
                    StopProgressBar();
                    return true;
                }

                else if (e.State == Status.FileMessage && e.Message != null)
                {
                    toolStripStatusLabelFile.Text = e.Message;
                    statusStrip1.Refresh();
                   
                    return true;
                }

                else if (e.State == Status.Start)
                {
                    toolStripStatusLabelTotalRows.Text = string.Empty;

                    button_RunButton.Enabled = false;
                    buttonCancel.Visible = true;
                    toolStripStatusLabelFile.Text = string.Empty;


                    this.Refresh();
                    this.Invalidate();
                    buttonCancel.Refresh();
                    buttonCancel.Invalidate();
                    //Thread.Sleep(1000);
                    return true;
                }

            }
            return false;
        }

        private void DataGrid_DoubleClick(object sender, EventArgs e)
        {
            if (DataGrid.SelectedRows != null && DataGrid.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow item in DataGrid.SelectedRows)
                {

                    string arguments = string.Empty;
                    string path = string.Empty;

                    if (Program.ProgConfig.SelectNodePadPlus)
                    {
                        if (string.IsNullOrWhiteSpace(Program.ProgConfig.NodePadPlusPath) || !File.Exists(Program.ProgConfig.NodePadPlusPath))
                        {
                            Program.ProgConfig.SelectNodePadPlus = false;
                        }
                        else
                        {
                            path = Program.ProgConfig.NodePadPlusPath;
                        }

                        if (string.IsNullOrWhiteSpace(Program.ProgConfig.NodePadPlusStart))
                        {
                            Program.ProgConfig.NodePadPlusStart = " -multiInst \"$path\" -n$linenr";
                            blockGui = true;
                            toolStripTextBoxNotepadPlusStart.Text = Program.ProgConfig.NodePadPlusStart;
                            blockGui = false;
                        }

                        arguments = Program.ProgConfig.NodePadPlusStart;
                    }


                    if (!Program.ProgConfig.SelectNodePadPlus)
                    {
                        if (string.IsNullOrWhiteSpace(Program.ProgConfig.NodePadStart))
                        {
                            Program.ProgConfig.NodePadStart = "\"$path\"";
                            blockGui = true;
                            toolStripTextBoxWinStart.Text = Program.ProgConfig.NodePadStart;
                            blockGui = false;
                        }




                        arguments = Program.ProgConfig.NodePadStart;
                        if (string.IsNullOrWhiteSpace(Program.ProgConfig.NodePadPath) || 
                            !File.Exists(Program.ProgConfig.NodePadPath))
                        {
                            path = Environment.SystemDirectory + "\\notepad.exe";
                            Program.ProgConfig.NodePadPath = path;
                            blockGui = true;
                            toolStripTextBoxWinPath.Text = path;
                            blockGui = false;
                        }


                        if (!System.IO.File.Exists(path))
                        {
                            Program.ProgConfig.NodePadPath = string.Empty;
                            path = string.Empty;
                            blockGui = true;
                            toolStripTextBoxWinPath.Text = string.Empty;
                            blockGui = false;
                            break;
                        }
                    }


                    if (!string.IsNullOrEmpty(path))
                    {
                        try
                        {
                            Process p = new Process
                            {
                                StartInfo = new ProcessStartInfo(path)
                            };

                            if (!string.IsNullOrWhiteSpace(item.Cells["File"].Value.ToString()))
                            {
                                arguments = arguments.Replace("$path", item.Cells["File"].Value.ToString());
                            }

                            if (!string.IsNullOrWhiteSpace(item.Cells["LineNr"].Value.ToString()))
                            {
                                arguments = arguments.Replace("$linenr", item.Cells["LineNr"].Value.ToString());
                            }

                            if (Program.ProgConfig.SelectNodePadPlus)
                            {
                                if (!string.IsNullOrWhiteSpace(textBoxNr.Text))
                                {
                                    arguments = arguments.Replace("$find", textBoxNr.Text);
                                }
                                if (!string.IsNullOrWhiteSpace(item.Cells["Pos"].Value.ToString()))
                                {
                                    arguments = arguments.Replace("$pos", item.Cells["Pos"].Value.ToString());
                                }                                
                            }
                                

                            p.StartInfo.Arguments = arguments;
#if DEBUG
                            //labelString.Text = path + p.StartInfo.Arguments;
#endif

                            p.Start();

                            if (!Program.ProgConfig.SelectNodePadPlus && !string.IsNullOrWhiteSpace(textBoxNr.Text))
                            {
                                Thread.Sleep(500);

                                IntPtr h = p.MainWindowHandle;
                                Finder.SetForegroundWindow(h);
                                SendKeys.SendWait("%b");
                                SendKeys.SendWait("u");
                                SendKeys.SendWait(textBoxNr.Text);
                                SendKeys.SendWait("{ENTER}");
                            }
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            //labelString.Text = ex.Message;
                             MessageBox.Show("ERROR: " + ex.Message);
#endif                       
                        }
                    }
                }
            }
        }

        private void BindingSource_main_ListChanged(object sender, ListChangedEventArgs e)
        {
            toolStripStatusLabelTotalRows.Text = "Rows: " + bindingSource_main.List.Count.ToString();
        }

        private void SearchToolBar2_Search(object sender, AdvancedDataGridViewSearchToolBarSearchEventArgs e)
        {
            bool restartsearch = true;
            int startColumn = 0;
            int startRow = 0;
            if (!e.FromBegin)
            {
                bool endcol = DataGrid.CurrentCell.ColumnIndex + 1 >= DataGrid.ColumnCount;
                bool endrow = DataGrid.CurrentCell.RowIndex + 1 >= DataGrid.RowCount;

                if (endcol && endrow)
                {
                    startColumn = DataGrid.CurrentCell.ColumnIndex;
                    startRow = DataGrid.CurrentCell.RowIndex;
                }
                else
                {
                    startColumn = endcol ? 0 : DataGrid.CurrentCell.ColumnIndex + 1;
                    startRow = DataGrid.CurrentCell.RowIndex + (endcol ? 1 : 0);
                }
            }
            DataGridViewCell c = DataGrid.FindCell(
                e.ValueToSearch,
                e.ColumnToSearch?.Name,
                startRow,
                startColumn,
                e.WholeWord,
                e.CaseSensitive);
            if (c == null && restartsearch)
                c = DataGrid.FindCell(
                    e.ValueToSearch,
                    e.ColumnToSearch?.Name,
                    0,
                    0,
                    e.WholeWord,
                    e.CaseSensitive);
            if (c != null)
                DataGrid.CurrentCell = c;
        }

        private void Zuby_SortStringChanged(object sender, AdvancedDataGridView.SortEventArgs e)
        {
            bindingSource_main.Sort = DataGrid.SortString;
            //textBox_sort.Text = bindingSource_main.Sort;
        }

        private void Zuby_FilterStringChanged(object sender, AdvancedDataGridView.FilterEventArgs e)
        {
            bindingSource_main.Filter = DataGrid.FilterString;
            //textBox_filter.Text = bindingSource_main.Filter;
        }

        private void TextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.Filter = textBoxFilter.Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                RootFolder = System.Environment.SpecialFolder.MyComputer
            };
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxPath.Text = folderBrowserDialog.SelectedPath;    
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelFile.Text = string.Empty;

            Finder.Cancel();
            StopProgressBar();

            try
            {
                if (Finder.MyTask != null && (Finder.MyTask.Status == TaskStatus.RanToCompletion|| Finder.MyTask.Status == TaskStatus.Faulted|| Finder.MyTask.Status == TaskStatus.Canceled))
                {
                   
                    Finder.MyTask.Dispose();
                    Thread.Sleep(100);
                    Finder.MyTask = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }            
        }



        private void TextBoxPath_TextChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            toolTip.SetToolTip(textBoxPath, textBoxPath.Text);
            Program.ProgConfig.FindInDir = textBoxPath.Text;
        }

        private void ToolStripStatusLabelFile_TextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelFile.ToolTipText = textBoxPath.Text;
        }



        private void DirectorySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blockGui) return;

            if (DirectorySelect.SelectedIndex != 0)
            {
                Program.ProgConfig.TopDirectoryOnly = false;
            }
            else
            {
                Program.ProgConfig.TopDirectoryOnly = true;
            }            
        }

        private void LoadData()
        {
            blockGui = true;

            if (Program.ProgConfig.TopDirectoryOnly)
            {
                DirectorySelect.SelectedIndex = 0;
            }
            else
            {
                DirectorySelect.SelectedIndex = 1;
            }

            textBoxNr.Text = Program.ProgConfig.FindString;
            toolStripTextBoxNotepadPath.Text = Program.ProgConfig.NodePadPlusPath;

            toolTip.SetToolTip(textBoxPath, Program.ProgConfig.FindInDir);

            textBoxPath.Text = Program.ProgConfig.FindInDir;

            textBoxFilter.Text = Program.ProgConfig.Filter;

            toolStripTextBoxWinPath.Text = Program.ProgConfig.NodePadPath;

            toolStripTextBoxWinStart.Text = Program.ProgConfig.NodePadStart;

            toolStripTextBoxNotepadPlusStart.Text = Program.ProgConfig.NodePadPlusStart;

            checkBoxIsHex.Checked = Program.ProgConfig.IsHex;

            comboBoxEncoding.Text = Program.ProgConfig.StrEncoding;
            comboBoxEncoding.SelectedItem = Program.ProgConfig.StrEncoding;

            if (Program.ProgConfig.SelectNodePadPlus)
            {
                toolStripMenuItemNotePadWindows.CheckState = CheckState.Unchecked;
                toolStripMenuItemNotepadPath.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripMenuItemNotePadWindows.CheckState = CheckState.Checked;
                toolStripMenuItemNotepadPath.CheckState = CheckState.Unchecked;
            }


            this.Invalidate();
            this.Refresh();

            blockGui = false;
        }

        private void ToolStripTextBoxNotepadPath_TextChanged_1(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.NodePadPlusPath = toolStripTextBoxNotepadPath.Text;
        }

        private void ToolStripTextBoxWinPath_TextChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.NodePadPath = toolStripTextBoxWinPath.Text;
        }

        private void ToolStripTextBoxWinStart_TextChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.NodePadStart = toolStripTextBoxWinStart.Text;
        }

        private void ToolStripTextBoxNotepadPlusStart_TextChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.NodePadPlusStart = toolStripTextBoxNotepadPlusStart.Text;
        }

        private void ToolStripMenuItemNotepadPath_DoubleClick(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.SelectNodePadPlus = true;
            toolStripMenuItemNotePadWindows.CheckState =  CheckState.Unchecked;
            toolStripMenuItemNotepadPath.CheckState = CheckState.Checked;
            toolStripMenuItemNotePadWindows.Invalidate();
            toolStripMenuItemNotepadPath.Invalidate();
            menuStrip1.Refresh();

        }

        private void ToolStripMenuItemNotePadWindows_DoubleClick(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.SelectNodePadPlus = false;
            toolStripMenuItemNotePadWindows.CheckState = CheckState.Checked;
            toolStripMenuItemNotepadPath.CheckState = CheckState.Unchecked;
            toolStripMenuItemNotePadWindows.Invalidate();
            toolStripMenuItemNotepadPath.Invalidate();
            menuStrip1.Refresh();

        }

        private void CheckBoxIsHex_CheckedChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.IsHex = checkBoxIsHex.Checked;
            if (Program.ProgConfig.IsHex)
            {
                string encoding = comboBoxEncoding.Text;
                Encoding enc;
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
                else
                {
                    enc = Encoding.UTF8;
                }

                byte[] bytes = enc.GetBytes(Program.ProgConfig.FindString);
                blockGui = true;
                textBoxNr.Text = BitConverter.ToString(bytes).Replace("-", ",");
                //Program.ProgConfig.FindString = textBoxNr.Text;
                blockGui = false;                
            }
            else
            {

                blockGui = true;
                    textBoxNr.Text = Program.ProgConfig.FindString;
                blockGui = false;
            }

        }

        private void TextBoxNr_TextChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            if (Program.ProgConfig.IsHex)
            {
                var t = textBoxNr.Text.Replace(" ", "").ToUpper().Split(',');
                if (t.Length > 0)
                {
                    try
                    {
                        string v = string.Empty;
                        foreach (var item in t)
                        {
                            string val = item;
                            if (val.Length == 1)
                            {
                                val = "0" + val;
                            }

                            val = val.Substring(0, 2);

                            if (!string.IsNullOrWhiteSpace(val.Trim()))
                            {
                                byte num = byte.Parse(val, System.Globalization.NumberStyles.HexNumber);
                                byte[] r = new byte[] { num };
                                v += System.Text.UTF8Encoding.UTF8.GetString(r);
                            }
                        }
                        Program.ProgConfig.FindString = v;
                    }
                    catch
                    { }
                }
            }
            else
            {
                Program.ProgConfig.FindString = textBoxNr.Text;
            }                
        }

        private void TextBoxNr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Program.ProgConfig.IsHex)
            {
                // Check for a naughty character in the KeyDown event.
                if (System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"[^0-9^a-f^A-F(^,)(^\b)]"))
                {
                    // Stop the character from being entered into the control since it is illegal.
                    e.Handled = true;
                }
            }
        }

        private void ComboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blockGui) return;
            Program.ProgConfig.StrEncoding = comboBoxEncoding.Text;

        }

        private bool stop = false;
        private void StopProgressBar()
        {
            stop = true;
            toolStripProgressBar1.Value = 0;
            this.Refresh();
        }



        private async void StartProgressBar()
        {
            toolStripProgressBar1.Maximum = 100;
            toolStripProgressBar1.Step = 1;
            stop = false;

            var progress = new Progress<int> (v =>
            {
                try{toolStripProgressBar1.Value = v;}catch {}
            });

            // Run operation in another thread
            await Task.Run(() => DoWork(progress));

        }

        public void DoWork(IProgress<int> progress)
        {
            int x = 0;
            while (true)
            {
                if (stop) { progress.Report(0); break; }
                progress.Report(x);
                x++;
                if (x > 100) x = 0;
                Thread.Sleep(50);
            }
        }



        public void UpdateProgres(int _value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(UpdateProgres), _value);
                return;
            }
            toolStripProgressBar1.Value = _value;
        }
    }

    public struct Container
    {
        public int Value { get; set; }
        public bool Run { get; set; }

        public CancellationToken Token { get; set; }

    }
}
