﻿/*
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
namespace FindStringInFile
{
    partial class FindStringInFileForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindStringInFileForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelTotalRows = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.labelParameter2Label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxNr = new System.Windows.Forms.TextBox();
            this.button_RunButton = new System.Windows.Forms.Button();
            this.panelTable = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bindingSource_main = new System.Windows.Forms.BindingSource(this.components);
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNotepadPath = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxNotepadPath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxNotepadPlusStart = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItemNotePadWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.pathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxWinPath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxWinStart = new System.Windows.Forms.ToolStripTextBox();
            this.labelParameter1Label = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelEncoding = new System.Windows.Forms.Label();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.checkBoxIsHex = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DirectorySelect = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_main)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabelTotalRows,
            this.toolStripStatusLabelFile});
            this.statusStrip1.Location = new System.Drawing.Point(0, 417);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(779, 27);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelTotalRows
            // 
            this.toolStripStatusLabelTotalRows.AutoSize = false;
            this.toolStripStatusLabelTotalRows.Name = "toolStripStatusLabelTotalRows";
            this.toolStripStatusLabelTotalRows.Size = new System.Drawing.Size(200, 22);
            this.toolStripStatusLabelTotalRows.Text = "Total rows:";
            this.toolStripStatusLabelTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelFile
            // 
            this.toolStripStatusLabelFile.AutoToolTip = true;
            this.toolStripStatusLabelFile.Name = "toolStripStatusLabelFile";
            this.toolStripStatusLabelFile.Size = new System.Drawing.Size(0, 22);
            this.toolStripStatusLabelFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusLabelFile.TextChanged += new System.EventHandler(this.ToolStripStatusLabelFile_TextChanged);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.AcceptsReturn = true;
            this.textBoxFilter.AcceptsTab = true;
            this.textBoxFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFilter.Location = new System.Drawing.Point(4, 119);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(205, 26);
            this.textBoxFilter.TabIndex = 3;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.TextBoxFilter_TextChanged);
            // 
            // labelParameter2Label
            // 
            this.labelParameter2Label.AutoSize = true;
            this.labelParameter2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelParameter2Label.Location = new System.Drawing.Point(9, 95);
            this.labelParameter2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelParameter2Label.Name = "labelParameter2Label";
            this.labelParameter2Label.Size = new System.Drawing.Size(184, 20);
            this.labelParameter2Label.TabIndex = 23;
            this.labelParameter2Label.Text = "Filter:   (e.g.   *.txt|*.log)";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Location = new System.Drawing.Point(509, 7);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(265, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "SingleQueryWindow (C) Ingolf Hill (2015)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxNr
            // 
            this.textBoxNr.AcceptsReturn = true;
            this.textBoxNr.AcceptsTab = true;
            this.textBoxNr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNr.Location = new System.Drawing.Point(216, 63);
            this.textBoxNr.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxNr.Multiline = true;
            this.textBoxNr.Name = "textBoxNr";
            this.textBoxNr.Size = new System.Drawing.Size(450, 26);
            this.textBoxNr.TabIndex = 2;
            this.textBoxNr.TextChanged += new System.EventHandler(this.TextBoxNr_TextChanged);
            this.textBoxNr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxNr_KeyPress);
            // 
            // button_RunButton
            // 
            this.button_RunButton.BackColor = System.Drawing.Color.CornflowerBlue;
            this.button_RunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_RunButton.Location = new System.Drawing.Point(4, 43);
            this.button_RunButton.Margin = new System.Windows.Forms.Padding(4);
            this.button_RunButton.Name = "button_RunButton";
            this.button_RunButton.Size = new System.Drawing.Size(123, 47);
            this.button_RunButton.TabIndex = 1;
            this.button_RunButton.Text = "Find";
            this.button_RunButton.UseVisualStyleBackColor = false;
            this.button_RunButton.Click += new System.EventHandler(this.Button_RunButton_Click);
            // 
            // panelTable
            // 
            this.panelTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTable.Location = new System.Drawing.Point(0, 181);
            this.panelTable.Margin = new System.Windows.Forms.Padding(4);
            this.panelTable.Name = "panelTable";
            this.panelTable.Size = new System.Drawing.Size(779, 233);
            this.panelTable.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 147);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(779, 34);
            this.panel3.TabIndex = 10;
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.queryToolStripMenuItem.Text = "Info";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryToolStripMenuItem,
            this.toolStripMenuItemNotepadPath,
            this.toolStripMenuItemNotePadWindows});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // toolStripMenuItemNotepadPath
            // 
            this.toolStripMenuItemNotepadPath.DoubleClickEnabled = true;
            this.toolStripMenuItemNotepadPath.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripTextBoxNotepadPath,
            this.toolStripMenuItem1,
            this.toolStripTextBoxNotepadPlusStart});
            this.toolStripMenuItemNotepadPath.Name = "toolStripMenuItemNotepadPath";
            this.toolStripMenuItemNotepadPath.Size = new System.Drawing.Size(330, 26);
            this.toolStripMenuItemNotepadPath.Text = "notepad++ path                                  ";
            this.toolStripMenuItemNotepadPath.DoubleClick += new System.EventHandler(this.ToolStripMenuItemNotepadPath_DoubleClick);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(647, 26);
            this.toolStripMenuItem4.Text = "Path:";
            // 
            // toolStripTextBoxNotepadPath
            // 
            this.toolStripTextBoxNotepadPath.Name = "toolStripTextBoxNotepadPath";
            this.toolStripTextBoxNotepadPath.Size = new System.Drawing.Size(400, 27);
            this.toolStripTextBoxNotepadPath.TextChanged += new System.EventHandler(this.ToolStripTextBoxNotepadPath_TextChanged_1);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(647, 26);
            this.toolStripMenuItem1.Text = "Start parameter: ($path = file path, $linenr = line number, $find = search string" +
    ")";
            // 
            // toolStripTextBoxNotepadPlusStart
            // 
            this.toolStripTextBoxNotepadPlusStart.Name = "toolStripTextBoxNotepadPlusStart";
            this.toolStripTextBoxNotepadPlusStart.Size = new System.Drawing.Size(400, 27);
            this.toolStripTextBoxNotepadPlusStart.TextChanged += new System.EventHandler(this.ToolStripTextBoxNotepadPlusStart_TextChanged);
            // 
            // toolStripMenuItemNotePadWindows
            // 
            this.toolStripMenuItemNotePadWindows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemNotePadWindows.DoubleClickEnabled = true;
            this.toolStripMenuItemNotePadWindows.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pathToolStripMenuItem,
            this.toolStripTextBoxWinPath,
            this.toolStripMenuItem2,
            this.toolStripTextBoxWinStart});
            this.toolStripMenuItemNotePadWindows.Name = "toolStripMenuItemNotePadWindows";
            this.toolStripMenuItemNotePadWindows.Size = new System.Drawing.Size(330, 26);
            this.toolStripMenuItemNotePadWindows.Text = "notepad (Windows)";
            this.toolStripMenuItemNotePadWindows.DoubleClick += new System.EventHandler(this.ToolStripMenuItemNotePadWindows_DoubleClick);
            // 
            // pathToolStripMenuItem
            // 
            this.pathToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathToolStripMenuItem.Name = "pathToolStripMenuItem";
            this.pathToolStripMenuItem.Size = new System.Drawing.Size(647, 26);
            this.pathToolStripMenuItem.Text = "Path:";
            // 
            // toolStripTextBoxWinPath
            // 
            this.toolStripTextBoxWinPath.Name = "toolStripTextBoxWinPath";
            this.toolStripTextBoxWinPath.Size = new System.Drawing.Size(400, 27);
            this.toolStripTextBoxWinPath.TextChanged += new System.EventHandler(this.ToolStripTextBoxWinPath_TextChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(647, 26);
            this.toolStripMenuItem2.Text = "Start parameter: ($path = file path, $linenr = line number, $find = search string" +
    ")";
            // 
            // toolStripTextBoxWinStart
            // 
            this.toolStripTextBoxWinStart.Name = "toolStripTextBoxWinStart";
            this.toolStripTextBoxWinStart.Size = new System.Drawing.Size(400, 27);
            this.toolStripTextBoxWinStart.TextChanged += new System.EventHandler(this.ToolStripTextBoxWinStart_TextChanged);
            // 
            // labelParameter1Label
            // 
            this.labelParameter1Label.AutoSize = true;
            this.labelParameter1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelParameter1Label.Location = new System.Drawing.Point(212, 39);
            this.labelParameter1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelParameter1Label.Name = "labelParameter1Label";
            this.labelParameter1Label.Size = new System.Drawing.Size(65, 20);
            this.labelParameter1Label.TabIndex = 19;
            this.labelParameter1Label.Text = "String:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(779, 28);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.labelEncoding);
            this.panel1.Controls.Add(this.comboBoxEncoding);
            this.panel1.Controls.Add(this.checkBoxIsHex);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBoxPath);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.DirectorySelect);
            this.panel1.Controls.Add(this.textBoxFilter);
            this.panel1.Controls.Add(this.labelParameter2Label);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.textBoxNr);
            this.panel1.Controls.Add(this.button_RunButton);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.labelParameter1Label);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(779, 181);
            this.panel1.TabIndex = 3;
            // 
            // labelEncoding
            // 
            this.labelEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEncoding.AutoSize = true;
            this.labelEncoding.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEncoding.Location = new System.Drawing.Point(669, 39);
            this.labelEncoding.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEncoding.Name = "labelEncoding";
            this.labelEncoding.Size = new System.Drawing.Size(92, 20);
            this.labelEncoding.TabIndex = 41;
            this.labelEncoding.Text = "Encoding:";
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEncoding.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBoxEncoding.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Items.AddRange(new object[] {
            "UTF8",
            "UTF7",
            "UTF32",
            "Unicode",
            "ASCII"});
            this.comboBoxEncoding.Location = new System.Drawing.Point(673, 63);
            this.comboBoxEncoding.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(101, 25);
            this.comboBoxEncoding.TabIndex = 40;
            this.comboBoxEncoding.TabStop = false;
            this.comboBoxEncoding.Text = "UTF8";
            this.comboBoxEncoding.SelectedIndexChanged += new System.EventHandler(this.ComboBoxEncoding_SelectedIndexChanged);
            // 
            // checkBoxIsHex
            // 
            this.checkBoxIsHex.AutoSize = true;
            this.checkBoxIsHex.Location = new System.Drawing.Point(330, 41);
            this.checkBoxIsHex.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxIsHex.Name = "checkBoxIsHex";
            this.checkBoxIsHex.Size = new System.Drawing.Size(241, 21);
            this.checkBoxIsHex.TabIndex = 38;
            this.checkBoxIsHex.Text = "Is hexadecimal  ( sep.  FF,AE,15 )";
            this.checkBoxIsHex.UseVisualStyleBackColor = true;
            this.checkBoxIsHex.CheckedChanged += new System.EventHandler(this.CheckBoxIsHex_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(125, 43);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(85, 47);
            this.buttonCancel.TabIndex = 37;
            this.buttonCancel.TabStop = false;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(759, 118);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(16, 28);
            this.button1.TabIndex = 35;
            this.button1.Text = "....";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.AcceptsReturn = true;
            this.textBoxPath.AcceptsTab = true;
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPath.Location = new System.Drawing.Point(216, 119);
            this.textBoxPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.ShortcutsEnabled = false;
            this.textBoxPath.Size = new System.Drawing.Size(545, 26);
            this.textBoxPath.TabIndex = 33;
            this.toolTip.SetToolTip(this.textBoxPath, "Test");
            this.textBoxPath.TextChanged += new System.EventHandler(this.TextBoxPath_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(217, 95);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 20);
            this.label5.TabIndex = 34;
            this.label5.Text = "Path:";
            // 
            // DirectorySelect
            // 
            this.DirectorySelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectorySelect.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.DirectorySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirectorySelect.FormattingEnabled = true;
            this.DirectorySelect.Items.AddRange(new object[] {
            "TopDirectoryOnly",
            " AllDirectories"});
            this.DirectorySelect.Location = new System.Drawing.Point(586, 92);
            this.DirectorySelect.Margin = new System.Windows.Forms.Padding(4);
            this.DirectorySelect.Name = "DirectorySelect";
            this.DirectorySelect.Size = new System.Drawing.Size(189, 25);
            this.DirectorySelect.TabIndex = 26;
            this.DirectorySelect.TabStop = false;
            this.DirectorySelect.Text = "TopDirectoryOnly";
            this.DirectorySelect.SelectedIndexChanged += new System.EventHandler(this.DirectorySelect_SelectedIndexChanged);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 21);
            // 
            // FindStringInFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 444);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelTable);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(794, 481);
            this.Name = "FindStringInFileForm";
            this.Text = "Find String In File";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_main)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTotalRows;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label labelParameter2Label;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxNr;
        private System.Windows.Forms.Button button_RunButton;
        private System.Windows.Forms.Panel panelTable;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.BindingSource bindingSource_main;
        private System.Windows.Forms.ToolStripMenuItem queryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.Label labelParameter1Label;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox DirectorySelect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFile;
        internal System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNotepadPath;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxNotepadPath;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxNotepadPlusStart;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNotePadWindows;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxWinPath;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem pathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxWinStart;
        private System.Windows.Forms.CheckBox checkBoxIsHex;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.Label labelEncoding;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    }
}

