namespace Server_for_SDRSharp
{
    partial class FormServerForSDRSharp
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxTCPServer = new System.Windows.Forms.GroupBox();
            this.checkBoxSubFolder = new System.Windows.Forms.CheckBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.labelSampleRate = new System.Windows.Forms.Label();
            this.labelNumFile = new System.Windows.Forms.Label();
            this.labelNbSendingForAllFiles = new System.Windows.Forms.Label();
            this.labelNbSendingForEachFile = new System.Windows.Forms.Label();
            this.labelNbFile = new System.Windows.Forms.Label();
            this.labelIPAdress = new System.Windows.Forms.Label();
            this.buttonFolderSelect = new System.Windows.Forms.Button();
            this.buttonFilesSelect = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxTempoBetweenFile = new System.Windows.Forms.TextBox();
            this.textBoxNEmissionForAllFiles = new System.Windows.Forms.TextBox();
            this.labelTempoBetweenFile = new System.Windows.Forms.Label();
            this.textBoxNEmissionForEachFile = new System.Windows.Forms.TextBox();
            this.labelNEmissionForAllFiles = new System.Windows.Forms.Label();
            this.labelNEmissionForEachFile = new System.Windows.Forms.Label();
            this.buttonStartServer = new System.Windows.Forms.Button();
            this.richTextBoxMessages = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.groupBoxRbSr = new System.Windows.Forms.GroupBox();
            this.buttongenereWavTest = new System.Windows.Forms.Button();
            this.buttonExtractCu8 = new System.Windows.Forms.Button();
            this.buttonMessageToClipBoard = new System.Windows.Forms.Button();
            this.buttonInfosWav = new System.Windows.Forms.Button();
            this.buttonTriResultBatch = new System.Windows.Forms.Button();
            this.buttonGenereBatch = new System.Windows.Forms.Button();
            this.buttonConcateneWav = new System.Windows.Forms.Button();
            this.buttonGetDevicesList = new System.Windows.Forms.Button();
            this.buttonClearMessages = new System.Windows.Forms.Button();
            this.buttonConcateneRawIQ = new System.Windows.Forms.Button();
            this.folderBrowserDialogChooseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxTCPServer.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTCPServer
            // 
            this.groupBoxTCPServer.Controls.Add(this.checkBoxSubFolder);
            this.groupBoxTCPServer.Controls.Add(this.buttonHelp);
            this.groupBoxTCPServer.Controls.Add(this.labelSampleRate);
            this.groupBoxTCPServer.Controls.Add(this.labelNumFile);
            this.groupBoxTCPServer.Controls.Add(this.labelNbSendingForAllFiles);
            this.groupBoxTCPServer.Controls.Add(this.labelNbSendingForEachFile);
            this.groupBoxTCPServer.Controls.Add(this.labelNbFile);
            this.groupBoxTCPServer.Controls.Add(this.labelIPAdress);
            this.groupBoxTCPServer.Controls.Add(this.buttonFolderSelect);
            this.groupBoxTCPServer.Controls.Add(this.buttonFilesSelect);
            this.groupBoxTCPServer.Controls.Add(this.textBoxPort);
            this.groupBoxTCPServer.Controls.Add(this.labelPort);
            this.groupBoxTCPServer.Controls.Add(this.textBoxTempoBetweenFile);
            this.groupBoxTCPServer.Controls.Add(this.textBoxNEmissionForAllFiles);
            this.groupBoxTCPServer.Controls.Add(this.labelTempoBetweenFile);
            this.groupBoxTCPServer.Controls.Add(this.textBoxNEmissionForEachFile);
            this.groupBoxTCPServer.Controls.Add(this.labelNEmissionForAllFiles);
            this.groupBoxTCPServer.Controls.Add(this.labelNEmissionForEachFile);
            this.groupBoxTCPServer.Controls.Add(this.buttonStartServer);
            this.groupBoxTCPServer.ForeColor = System.Drawing.SystemColors.Window;
            this.groupBoxTCPServer.Location = new System.Drawing.Point(9, 19);
            this.groupBoxTCPServer.Name = "groupBoxTCPServer";
            this.groupBoxTCPServer.Size = new System.Drawing.Size(364, 269);
            this.groupBoxTCPServer.TabIndex = 6;
            this.groupBoxTCPServer.TabStop = false;
            this.groupBoxTCPServer.Text = "TCP server for Source RTL-SDR TCP";
            // 
            // checkBoxSubFolder
            // 
            this.checkBoxSubFolder.AutoSize = true;
            this.checkBoxSubFolder.Location = new System.Drawing.Point(107, 47);
            this.checkBoxSubFolder.Name = "checkBoxSubFolder";
            this.checkBoxSubFolder.Size = new System.Drawing.Size(74, 17);
            this.checkBoxSubFolder.TabIndex = 28;
            this.checkBoxSubFolder.Text = "SubFolder";
            this.checkBoxSubFolder.UseVisualStyleBackColor = true;
            // 
            // buttonHelp
            // 
            this.buttonHelp.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonHelp.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonHelp.Location = new System.Drawing.Point(291, 157);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(67, 24);
            this.buttonHelp.TabIndex = 27;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = false;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // labelSampleRate
            // 
            this.labelSampleRate.AutoSize = true;
            this.labelSampleRate.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelSampleRate.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelSampleRate.Location = new System.Drawing.Point(187, 69);
            this.labelSampleRate.Name = "labelSampleRate";
            this.labelSampleRate.Size = new System.Drawing.Size(13, 13);
            this.labelSampleRate.TabIndex = 26;
            this.labelSampleRate.Text = "0";
            // 
            // labelNumFile
            // 
            this.labelNumFile.AutoSize = true;
            this.labelNumFile.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelNumFile.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelNumFile.Location = new System.Drawing.Point(94, 69);
            this.labelNumFile.Name = "labelNumFile";
            this.labelNumFile.Size = new System.Drawing.Size(24, 13);
            this.labelNumFile.TabIndex = 25;
            this.labelNumFile.Text = "0/0";
            // 
            // labelNbSendingForAllFiles
            // 
            this.labelNbSendingForAllFiles.AutoSize = true;
            this.labelNbSendingForAllFiles.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelNbSendingForAllFiles.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelNbSendingForAllFiles.Location = new System.Drawing.Point(187, 120);
            this.labelNbSendingForAllFiles.Name = "labelNbSendingForAllFiles";
            this.labelNbSendingForAllFiles.Size = new System.Drawing.Size(24, 13);
            this.labelNbSendingForAllFiles.TabIndex = 24;
            this.labelNbSendingForAllFiles.Text = "0/0";
            // 
            // labelNbSendingForEachFile
            // 
            this.labelNbSendingForEachFile.AutoSize = true;
            this.labelNbSendingForEachFile.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelNbSendingForEachFile.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelNbSendingForEachFile.Location = new System.Drawing.Point(187, 97);
            this.labelNbSendingForEachFile.Name = "labelNbSendingForEachFile";
            this.labelNbSendingForEachFile.Size = new System.Drawing.Size(24, 13);
            this.labelNbSendingForEachFile.TabIndex = 23;
            this.labelNbSendingForEachFile.Text = "0/0";
            // 
            // labelNbFile
            // 
            this.labelNbFile.AutoSize = true;
            this.labelNbFile.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelNbFile.ForeColor = System.Drawing.SystemColors.Window;
            this.labelNbFile.Location = new System.Drawing.Point(14, 69);
            this.labelNbFile.Name = "labelNbFile";
            this.labelNbFile.Size = new System.Drawing.Size(45, 13);
            this.labelNbFile.TabIndex = 22;
            this.labelNbFile.Text = "Nb files:";
            // 
            // labelIPAdress
            // 
            this.labelIPAdress.AutoSize = true;
            this.labelIPAdress.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelIPAdress.ForeColor = System.Drawing.SystemColors.Window;
            this.labelIPAdress.Location = new System.Drawing.Point(257, 39);
            this.labelIPAdress.Name = "labelIPAdress";
            this.labelIPAdress.Size = new System.Drawing.Size(101, 13);
            this.labelIPAdress.TabIndex = 21;
            this.labelIPAdress.Text = "IP Always 127.0.0.1";
            // 
            // buttonFolderSelect
            // 
            this.buttonFolderSelect.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonFolderSelect.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonFolderSelect.Location = new System.Drawing.Point(6, 42);
            this.buttonFolderSelect.Name = "buttonFolderSelect";
            this.buttonFolderSelect.Size = new System.Drawing.Size(92, 24);
            this.buttonFolderSelect.TabIndex = 18;
            this.buttonFolderSelect.Text = "Folder select";
            this.buttonFolderSelect.UseVisualStyleBackColor = false;
            this.buttonFolderSelect.Click += new System.EventHandler(this.buttonFolderSelect_Click);
            // 
            // buttonFilesSelect
            // 
            this.buttonFilesSelect.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonFilesSelect.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonFilesSelect.Location = new System.Drawing.Point(6, 16);
            this.buttonFilesSelect.Name = "buttonFilesSelect";
            this.buttonFilesSelect.Size = new System.Drawing.Size(92, 24);
            this.buttonFilesSelect.TabIndex = 18;
            this.buttonFilesSelect.Text = "Files select";
            this.buttonFilesSelect.UseVisualStyleBackColor = false;
            this.buttonFilesSelect.Click += new System.EventHandler(this.buttonFilesSelect_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPort.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxPort.Location = new System.Drawing.Point(288, 16);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(70, 20);
            this.textBoxPort.TabIndex = 17;
            this.textBoxPort.Text = "1234";
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPort_KeyPress);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelPort.ForeColor = System.Drawing.SystemColors.Window;
            this.labelPort.Location = new System.Drawing.Point(246, 19);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(26, 13);
            this.labelPort.TabIndex = 16;
            this.labelPort.Text = "Port";
            // 
            // textBoxTempoBetweenFile
            // 
            this.textBoxTempoBetweenFile.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxTempoBetweenFile.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxTempoBetweenFile.Location = new System.Drawing.Point(133, 139);
            this.textBoxTempoBetweenFile.Name = "textBoxTempoBetweenFile";
            this.textBoxTempoBetweenFile.Size = new System.Drawing.Size(48, 20);
            this.textBoxTempoBetweenFile.TabIndex = 11;
            this.textBoxTempoBetweenFile.Text = "0";
            this.textBoxTempoBetweenFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTempoBetweenFile_KeyPress);
            // 
            // textBoxNEmissionForAllFiles
            // 
            this.textBoxNEmissionForAllFiles.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxNEmissionForAllFiles.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxNEmissionForAllFiles.Location = new System.Drawing.Point(133, 113);
            this.textBoxNEmissionForAllFiles.Name = "textBoxNEmissionForAllFiles";
            this.textBoxNEmissionForAllFiles.Size = new System.Drawing.Size(48, 20);
            this.textBoxNEmissionForAllFiles.TabIndex = 15;
            this.textBoxNEmissionForAllFiles.Text = "1";
            this.textBoxNEmissionForAllFiles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNEmissionForAllFiles_KeyPress);
            // 
            // labelTempoBetweenFile
            // 
            this.labelTempoBetweenFile.AutoSize = true;
            this.labelTempoBetweenFile.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelTempoBetweenFile.ForeColor = System.Drawing.SystemColors.Window;
            this.labelTempoBetweenFile.Location = new System.Drawing.Point(14, 146);
            this.labelTempoBetweenFile.Name = "labelTempoBetweenFile";
            this.labelTempoBetweenFile.Size = new System.Drawing.Size(121, 13);
            this.labelTempoBetweenFile.TabIndex = 10;
            this.labelTempoBetweenFile.Text = "Delay between files(ms.)";
            // 
            // textBoxNEmissionForEachFile
            // 
            this.textBoxNEmissionForEachFile.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxNEmissionForEachFile.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxNEmissionForEachFile.Location = new System.Drawing.Point(133, 90);
            this.textBoxNEmissionForEachFile.Name = "textBoxNEmissionForEachFile";
            this.textBoxNEmissionForEachFile.Size = new System.Drawing.Size(48, 20);
            this.textBoxNEmissionForEachFile.TabIndex = 14;
            this.textBoxNEmissionForEachFile.Text = "1";
            this.textBoxNEmissionForEachFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNEmissionForEachFile_KeyPress);
            // 
            // labelNEmissionForAllFiles
            // 
            this.labelNEmissionForAllFiles.AutoSize = true;
            this.labelNEmissionForAllFiles.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelNEmissionForAllFiles.ForeColor = System.Drawing.SystemColors.Window;
            this.labelNEmissionForAllFiles.Location = new System.Drawing.Point(14, 120);
            this.labelNEmissionForAllFiles.Name = "labelNEmissionForAllFiles";
            this.labelNEmissionForAllFiles.Size = new System.Drawing.Size(104, 13);
            this.labelNEmissionForAllFiles.TabIndex = 13;
            this.labelNEmissionForAllFiles.Text = "N sending for all files";
            // 
            // labelNEmissionForEachFile
            // 
            this.labelNEmissionForEachFile.AutoSize = true;
            this.labelNEmissionForEachFile.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelNEmissionForEachFile.ForeColor = System.Drawing.SystemColors.Window;
            this.labelNEmissionForEachFile.Location = new System.Drawing.Point(14, 97);
            this.labelNEmissionForEachFile.Name = "labelNEmissionForEachFile";
            this.labelNEmissionForEachFile.Size = new System.Drawing.Size(113, 13);
            this.labelNEmissionForEachFile.TabIndex = 12;
            this.labelNEmissionForEachFile.Text = "N sending for each file";
            // 
            // buttonStartServer
            // 
            this.buttonStartServer.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonStartServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonStartServer.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonStartServer.Location = new System.Drawing.Point(175, 19);
            this.buttonStartServer.Name = "buttonStartServer";
            this.buttonStartServer.Size = new System.Drawing.Size(65, 24);
            this.buttonStartServer.TabIndex = 0;
            this.buttonStartServer.Text = "Start";
            this.buttonStartServer.UseVisualStyleBackColor = false;
            this.buttonStartServer.Click += new System.EventHandler(this.buttonStartServer_ClickAsync);
            // 
            // richTextBoxMessages
            // 
            this.richTextBoxMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessages.Location = new System.Drawing.Point(3, 303);
            this.richTextBoxMessages.Name = "richTextBoxMessages";
            this.richTextBoxMessages.Size = new System.Drawing.Size(880, 135);
            this.richTextBoxMessages.TabIndex = 22;
            this.richTextBoxMessages.Text = "";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.richTextBoxMessages, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxServer, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(886, 441);
            this.tableLayoutPanelMain.TabIndex = 24;
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.groupBoxRbSr);
            this.groupBoxServer.Controls.Add(this.groupBoxTCPServer);
            this.groupBoxServer.Controls.Add(this.buttongenereWavTest);
            this.groupBoxServer.Controls.Add(this.buttonExtractCu8);
            this.groupBoxServer.Controls.Add(this.buttonMessageToClipBoard);
            this.groupBoxServer.Controls.Add(this.buttonInfosWav);
            this.groupBoxServer.Controls.Add(this.buttonTriResultBatch);
            this.groupBoxServer.Controls.Add(this.buttonGenereBatch);
            this.groupBoxServer.Controls.Add(this.buttonConcateneWav);
            this.groupBoxServer.Controls.Add(this.buttonGetDevicesList);
            this.groupBoxServer.Controls.Add(this.buttonClearMessages);
            this.groupBoxServer.Controls.Add(this.buttonConcateneRawIQ);
            this.groupBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxServer.Location = new System.Drawing.Point(3, 3);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(880, 294);
            this.groupBoxServer.TabIndex = 23;
            this.groupBoxServer.TabStop = false;
            // 
            // groupBoxRbSr
            // 
            this.groupBoxRbSr.Location = new System.Drawing.Point(390, 20);
            this.groupBoxRbSr.Name = "groupBoxRbSr";
            this.groupBoxRbSr.Size = new System.Drawing.Size(107, 268);
            this.groupBoxRbSr.TabIndex = 21;
            this.groupBoxRbSr.TabStop = false;
            // 
            // buttongenereWavTest
            // 
            this.buttongenereWavTest.Location = new System.Drawing.Point(768, 24);
            this.buttongenereWavTest.Name = "buttongenereWavTest";
            this.buttongenereWavTest.Size = new System.Drawing.Size(91, 32);
            this.buttongenereWavTest.TabIndex = 3;
            this.buttongenereWavTest.Text = "genere wav test";
            this.buttongenereWavTest.UseVisualStyleBackColor = true;
            this.buttongenereWavTest.Visible = false;
            this.buttongenereWavTest.Click += new System.EventHandler(this.buttongenereWavTest_Click);
            // 
            // buttonExtractCu8
            // 
            this.buttonExtractCu8.Location = new System.Drawing.Point(768, 69);
            this.buttonExtractCu8.Name = "buttonExtractCu8";
            this.buttonExtractCu8.Size = new System.Drawing.Size(83, 32);
            this.buttonExtractCu8.TabIndex = 4;
            this.buttonExtractCu8.Text = "extract cu8";
            this.buttonExtractCu8.UseVisualStyleBackColor = true;
            this.buttonExtractCu8.Visible = false;
            this.buttonExtractCu8.Click += new System.EventHandler(this.buttonExtractCu8_Click);
            // 
            // buttonMessageToClipBoard
            // 
            this.buttonMessageToClipBoard.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonMessageToClipBoard.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonMessageToClipBoard.Location = new System.Drawing.Point(808, 138);
            this.buttonMessageToClipBoard.Name = "buttonMessageToClipBoard";
            this.buttonMessageToClipBoard.Size = new System.Drawing.Size(63, 49);
            this.buttonMessageToClipBoard.TabIndex = 19;
            this.buttonMessageToClipBoard.Text = "Message to clipboard";
            this.buttonMessageToClipBoard.UseVisualStyleBackColor = false;
            this.buttonMessageToClipBoard.Click += new System.EventHandler(this.buttonMessageToClipBoard_Click);
            // 
            // buttonInfosWav
            // 
            this.buttonInfosWav.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonInfosWav.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonInfosWav.Location = new System.Drawing.Point(611, 24);
            this.buttonInfosWav.Name = "buttonInfosWav";
            this.buttonInfosWav.Size = new System.Drawing.Size(123, 47);
            this.buttonInfosWav.TabIndex = 0;
            this.buttonInfosWav.Text = "Infos Wav file";
            this.buttonInfosWav.UseVisualStyleBackColor = false;
            this.buttonInfosWav.Click += new System.EventHandler(this.buttonInfosWav_Click);
            // 
            // buttonTriResultBatch
            // 
            this.buttonTriResultBatch.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonTriResultBatch.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonTriResultBatch.Location = new System.Drawing.Point(611, 246);
            this.buttonTriResultBatch.Name = "buttonTriResultBatch";
            this.buttonTriResultBatch.Size = new System.Drawing.Size(123, 42);
            this.buttonTriResultBatch.TabIndex = 2;
            this.buttonTriResultBatch.Text = "tri result batch file for rtl433";
            this.buttonTriResultBatch.UseVisualStyleBackColor = false;
            this.buttonTriResultBatch.Click += new System.EventHandler(this.buttonTriResultBatch_Click);
            // 
            // buttonGenereBatch
            // 
            this.buttonGenereBatch.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonGenereBatch.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonGenereBatch.Location = new System.Drawing.Point(611, 195);
            this.buttonGenereBatch.Name = "buttonGenereBatch";
            this.buttonGenereBatch.Size = new System.Drawing.Size(123, 42);
            this.buttonGenereBatch.TabIndex = 2;
            this.buttonGenereBatch.Text = "genere batch for rtl433";
            this.buttonGenereBatch.UseVisualStyleBackColor = false;
            this.buttonGenereBatch.Click += new System.EventHandler(this.buttonGenereBatch_Click);
            // 
            // buttonConcateneWav
            // 
            this.buttonConcateneWav.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonConcateneWav.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonConcateneWav.Location = new System.Drawing.Point(611, 146);
            this.buttonConcateneWav.Name = "buttonConcateneWav";
            this.buttonConcateneWav.Size = new System.Drawing.Size(123, 42);
            this.buttonConcateneWav.TabIndex = 2;
            this.buttonConcateneWav.Text = "Concatenate Wav";
            this.buttonConcateneWav.UseVisualStyleBackColor = false;
            this.buttonConcateneWav.Click += new System.EventHandler(this.buttonConcateneWav_Click);
            // 
            // buttonGetDevicesList
            // 
            this.buttonGetDevicesList.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonGetDevicesList.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonGetDevicesList.Location = new System.Drawing.Point(740, 246);
            this.buttonGetDevicesList.Name = "buttonGetDevicesList";
            this.buttonGetDevicesList.Size = new System.Drawing.Size(111, 41);
            this.buttonGetDevicesList.TabIndex = 20;
            this.buttonGetDevicesList.Text = "get devices RTL_433";
            this.buttonGetDevicesList.UseVisualStyleBackColor = false;
            this.buttonGetDevicesList.Click += new System.EventHandler(this.buttonGetDevicesList_Click);
            // 
            // buttonClearMessages
            // 
            this.buttonClearMessages.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonClearMessages.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonClearMessages.Location = new System.Drawing.Point(740, 146);
            this.buttonClearMessages.Name = "buttonClearMessages";
            this.buttonClearMessages.Size = new System.Drawing.Size(62, 41);
            this.buttonClearMessages.TabIndex = 20;
            this.buttonClearMessages.Text = "Clear messages";
            this.buttonClearMessages.UseVisualStyleBackColor = false;
            this.buttonClearMessages.Click += new System.EventHandler(this.buttonClearMessages_Click);
            // 
            // buttonConcateneRawIQ
            // 
            this.buttonConcateneRawIQ.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonConcateneRawIQ.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonConcateneRawIQ.Location = new System.Drawing.Point(611, 82);
            this.buttonConcateneRawIQ.Name = "buttonConcateneRawIQ";
            this.buttonConcateneRawIQ.Size = new System.Drawing.Size(123, 47);
            this.buttonConcateneRawIQ.TabIndex = 1;
            this.buttonConcateneRawIQ.Text = "Concatenate Raw IQ";
            this.buttonConcateneRawIQ.UseVisualStyleBackColor = false;
            this.buttonConcateneRawIQ.Click += new System.EventHandler(this.buttonConcateneRawIQ_Click);
            // 
            // FormServerForSDRSharp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(886, 441);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "FormServerForSDRSharp";
            this.Text = "TCP server for SDRSharp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormServerForSDRSharp_FormClosing);
            this.groupBoxTCPServer.ResumeLayout(false);
            this.groupBoxTCPServer.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.groupBoxServer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxTCPServer;
        private System.Windows.Forms.Button buttonStartServer;
        private System.Windows.Forms.TextBox textBoxNEmissionForAllFiles;
        private System.Windows.Forms.TextBox textBoxNEmissionForEachFile;
        private System.Windows.Forms.Label labelNEmissionForAllFiles;
        private System.Windows.Forms.Label labelNEmissionForEachFile;
        private System.Windows.Forms.TextBox textBoxTempoBetweenFile;
        private System.Windows.Forms.Label labelTempoBetweenFile;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Button buttonFilesSelect;
        private System.Windows.Forms.Label labelIPAdress;
        private System.Windows.Forms.Label labelNbFile;
        private System.Windows.Forms.Label labelSampleRate;
        private System.Windows.Forms.Label labelNumFile;
        private System.Windows.Forms.Label labelNbSendingForAllFiles;
        private System.Windows.Forms.Label labelNbSendingForEachFile;
        private System.Windows.Forms.RichTextBox richTextBoxMessages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.Button buttongenereWavTest;
        private System.Windows.Forms.Button buttonExtractCu8;
        private System.Windows.Forms.Button buttonMessageToClipBoard;
        private System.Windows.Forms.Button buttonInfosWav;
        private System.Windows.Forms.Button buttonConcateneWav;
        private System.Windows.Forms.Button buttonClearMessages;
        protected internal System.Windows.Forms.Button buttonConcateneRawIQ;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.GroupBox groupBoxRbSr;
        private System.Windows.Forms.Button buttonGenereBatch;
        private System.Windows.Forms.Button buttonTriResultBatch;
        private System.Windows.Forms.Button buttonFolderSelect;
        private System.Windows.Forms.CheckBox checkBoxSubFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogChooseFolder;
        private System.Windows.Forms.Button buttonGetDevicesList;
    }
}

