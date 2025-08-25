/* Written by Marc Prieur (marco40_github@sfr.fr)
                                FormServerForSDRSharp.cs 
                               project ServerForSDRSharp
 **************************************************************************************
 Creative Commons Attrib Share-Alike License
 You are free to use/extend this library but please abide with the CC-BY-SA license: 
 Attribution-NonCommercial-ShareAlike 4.0 International License
 http://creativecommons.org/licenses/by-nc-sa/4.0/

 All text above must be included in any redistribution.
  **********************************************************************************/

using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace Server_for_SDRSharp
{
    public partial class FormServerForSDRSharp : Form
    {
        #region CONST

        public const String IPAdress = "127.0.0.1";

        #endregion
        #region declare
        System.Windows.Forms.RadioButton[] radioButtonsSR;
        private ClassServerTCP myClassServerTCP = null;
        private string[] Files;
        private Int32 tempoBetweenFile = 0;
        private Int32 NEmissionForAllFiles = 0;
        private Int32 NEmissionForEachFile = 0;
        Int32 PortTCP = 1234;
        #endregion
        #region constructor
        public FormServerForSDRSharp()
        {
            InitializeComponent();

            sampleRate.Add(250000);    //put this [0] if not sample rate in name file
            sampleRate.Add(900000);
            sampleRate.Add(1024000);
            sampleRate.Add(1400000);
            sampleRate.Add(1800000);
            sampleRate.Add(1920000);
            sampleRate.Add(2048000);
            sampleRate.Add(2400000);
            sampleRate.Add(2800000);
            sampleRate.Add(3200000);
            int i = 0;
            radioButtonsSR = new System.Windows.Forms.RadioButton[sampleRate.Count()+1];
            foreach (int sr in sampleRate)
            {
                radioButtonsSR[i] = new RadioButton();
                radioButtonsSR[i].Text = sampleRate[i].ToString();
                radioButtonsSR[i].Location = new System.Drawing.Point(10, 10 + i * 20);
                radioButtonsSR[i].ForeColor = System.Drawing.SystemColors.Control;
                radioButtonsSR[i].UseVisualStyleBackColor = true;
                radioButtonsSR[i].Size = new System.Drawing.Size(85, 17);
                radioButtonsSR[i].Tag=sampleRate[i].ToString();
                this.groupBoxRbSr.Controls.Add(radioButtonsSR[i]);
                i += 1;
            }
            radioButtonsSR[i] = new RadioButton();
            radioButtonsSR[i].Text = "ALL";
            radioButtonsSR[i].Location = new System.Drawing.Point(10, 10 + i * 20);
            radioButtonsSR[i].ForeColor = System.Drawing.SystemColors.Control;
            radioButtonsSR[i].UseVisualStyleBackColor = true;
            radioButtonsSR[i].Size = new System.Drawing.Size(85, 17);
            radioButtonsSR[i].Tag="0";
            this.groupBoxRbSr.Controls.Add(radioButtonsSR[i]);




            AddMessage (ClassConstMessage.WAITFILES);
            labelIPAdress.Text = $"IP Always {IPAdress}";
            ToolTip ttbuttonChooseFiles = new ToolTip();
            ttbuttonChooseFiles.SetToolTip(buttonChooseFiles, "Choose file Wav OR/AND raw IQ same sample rate.");
            ToolTip tttextBoxPort = new ToolTip();
            tttextBoxPort.SetToolTip(textBoxPort, "Port must be 1024 to 65535");
            ToolTip ttlabelNbFile = new ToolTip();
            ttlabelNbFile.SetToolTip(labelNbFile, "Number of files OK.");

            AddMessage("");
            AddMessage("For use server TCP:");
            AddMessage("  -After choose files and port");
            AddMessage("     -Start");
            AddMessage("        -SDRSHarp(source RTL-SDR TCP");
            AddMessage("        -SDRSHarp(port IP and sample rate");
            AddMessage("     -Start Radio SDRSHarp(or stop and start");
            buttonStartServer.Enabled = false;
            textBoxNEmissionForAllFiles.Text = "2";
            textBoxNEmissionForEachFile.Text = "1";
            textBoxTempoBetweenFile.Text = "10";
            this.Text = $"TCP server for SDRSharp V{ Application.ProductVersion}";
        }
        #endregion
        #region private functions
        private string[] getFiles(String ext,bool multiSelect=true)
        {
            using (OpenFileDialog openFiles = new OpenFileDialog())
            {
                openFiles.DefaultExt = ext;
                openFiles.Filter = ext + " files|" + ext;
                openFiles.Multiselect = multiSelect;
                if (openFiles.ShowDialog() == DialogResult.OK)
                    return openFiles.FileNames;
                return null;
            }
        }


        /// <summary>
        /// tri les fichiers en fonction du sample rate au plus proche des sample rate de la source SDRSharp RTL-SDR_TCP 
        /// </summary>
        /// <param name="listFiles"></param>
        private Dictionary<string, string> triSampleRate(string[] Files, List<int> listSampleRate,List<int> usedListSampleRate)
        {
            int sampleRate = 0;
            Dictionary<string, string> listFiles = new Dictionary<string, string>();
            foreach (String file in Files)
            {
                int nearSampleRate = 0;
                int deltaSampleRate = int.MaxValue;
                //string F=WavRecorder.GetFrequencyFromName(file);
                sampleRate = WavRecorder.GetSampleRateFromName(file);
                if (sampleRate == -1)
                    listFiles.Add(file, listSampleRate[0].ToString());
                else {
                    foreach (int sr in listSampleRate)
                    {
                        if (Math.Abs(sampleRate - sr) < deltaSampleRate)
                        {
                            nearSampleRate = sr;
                            deltaSampleRate = sampleRate - sr;
 
                            if (deltaSampleRate == 0)
                             break;
                        }
                    }
                    listFiles.Add( file,nearSampleRate.ToString());
                    bool found = false;
                    foreach (int usr in usedListSampleRate)
                    {
                        if (usr == nearSampleRate)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        usedListSampleRate.Add(nearSampleRate);
                }
            }
            return listFiles;

        }

            //#elif TESTRECURSIF && !TESTBATCH
            //            try
            //            {
            //                //from https://github.com/merbanan/rtl_433_tests/tree/master
            //                //Set a variable to the My Documents path.
            //                string srcPath = "C:\\marc\\tnt\\fichiers_cu8_et_wav\\fichiers_cu8\\rtl_433_tests-master\\tests";   // Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //        string dstPath = "C:\\marc\\tnt\\fichiers_cu8_et_wav\\regroupes_rtl_433_tests-master"; //create folder
            //        Int32 lenPath = srcPath.Length + 1;
            //        var files = from file in Directory.EnumerateFiles(srcPath, "*.cu8", SearchOption.AllDirectories)
            //                        //from line in File.ReadLines(file)
            //                        //where line.Contains(".cu8")
            //                    select new
            //                    {
            //                        File = file,
            //                        //Line = line
            //                    };
            //        //String memoDirectory = "";
            //        Int32 cptFile = 0;
            //                foreach (var f in files)
            //                {
            //                    try
            //                    {
            //                        String directory = Path.GetDirectoryName(f.File);
            //        //if (!(memoDirectory == directory))
            //        //{

            //        String newFile = directory.Substring(lenPath).Replace("\\", "_") + "_" + cptFile.ToString() + "_" + Path.GetFileName($"{f.File}");
            //        Debug.WriteLine(newFile);
            //                            Debug.WriteLine(dstPath + "\\" + newFile);

            //                            File.Copy($"{f.File}", dstPath + "\\" + newFile);
            //                            //memoDirectory = directory;
            //                            cptFile ++;
            //                        //}
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Debug.WriteLine(ex.Message);
            //                    }
            //                }
            //                Debug.WriteLine(cptFile.ToString());


            //            }
            //            catch (UnauthorizedAccessException uAEx)
            //            {
            //                Debug.WriteLine(uAEx.Message);
            //            }
            //            catch (PathTooLongException pathEx)
            //            {
            //                Debug.WriteLine(pathEx.Message);
            //            }
            //            if (cptPb == 0)
            //                MessageBox.Show("Translate is completed", "Translate cu8 to wav", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            else
            //                MessageBox.Show("Translate is NOT completed", "Translate cu8 to wav", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //#elif TESTBATCH

            //            //generate batch file for replay with RTL_433 console mode->dstFile
            //            //put srcPath,dstFile 
            //            //and PathRtl433_EXE where you are compiled RTL_433
            //            //run SDRSharp, enabled plugin and cu8 to Wav
            //            //open Console to PathRtl433_EXE
            //            //run AllFiles.bat

            //            string srcPath = "C:\\marc\\tnt\\fichiers_cu8_et_wav\\fichiers_cu8\\rtl_433_tests-master\\rtl_433_tests-master";   // Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //string dstFile = "C:\\marc\\tnt\\rtl_433\\rtl_433-master_06052024\\vs17_32\\Debug\\FilesRTL433AllOK.bat ";
            //var files = from file in Directory.EnumerateFiles(srcPath, "*.cu8", SearchOption.AllDirectories)
            //            select new
            //            {
            //                File = file,
            //            };
            //Int32 cptFile = 0;
            //            try
            //            {
            //                using (Stream stream = new FileStream(dstFile, FileMode.Create, FileAccess.Write, FileShare.None))
            //                {
            //                    String Line = "";
            //String PathRtl433_EXE = "C:\\marc\\tnt\\rtl_433\\rtl_433-master_06052024\\vs17_32\\Debug\\rtl_433";

            //StreamWriter str = new StreamWriter(stream);
            //str.WriteLine("cls");
            //                    str.WriteLine("REM: "+DateTime.Now);
            //                    foreach (var file in files)
            //                    {
            //                        Int32 sampleRate = 0;
            //Int32 sampleRateFromFileName = wavRecorder.getSampleRateFromName(file.File); //lacrosse_g2750_915M_1000k.cu8,9_ford-unlock002.cu8
            //                        if (sampleRateFromFileName == -1)
            //                        {
            //                            sampleRateFromFileName = 250;
            //                            sampleRate = 250000;
            //                            //MessageBox.Show("No sample rate detected in the file name", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                            //return -1;
            //                        }
            //                        else
            //                            sampleRate = sampleRateFromFileName;

            //                        String Option = " -s " + sampleRate.ToString() + " -C si -r ";

            //Line = PathRtl433_EXE + Option + file.File;
            //                        str.WriteLine(Line);
            //                        cptFile ++;
            //                        if(cptFile%20==0)
            //                            str.WriteLine("Pause");
            //                    }
            //                    str.Flush();
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Debug.WriteLine(ex.Message);
            //            }
            //            MessageBox.Show("Copyfile is completed for "+ cptFile.ToString() + " files", "Translate cu8 nameFile with options to batch file", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //#endif




            private void AddMessage(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)delegate
                {
                     AddMessage(message);
                });
            }
            else
            {
                if (message.Contains(ClassConstMessage.RESTLOOPFILE))
                {
                    labelNbSendingForEachFile.Text = message;
                    this.Refresh();
                    return;
                }
                if (message.Contains(ClassConstMessage.NUMFILE))
                {
                   labelNumFile.Text = message;
                    this.Refresh();
                    return;
                }
                if (message.Contains(ClassConstMessage.RESTLOOPALLFILE))
                {
                   labelNbSendingForAllFiles.Text = message;
                    this.Refresh();
                    return;
                }
                if (message.Contains(ClassConstMessage.SAMPLERATE))
                {
                    labelSampleRate.Text = message;
                    this.Refresh();
                    return;
                }
                else if (message.Contains(ClassConstMessage.NBFILES))
                {
                    if (Files == null || Files.Count() == 0)
                    {
                        buttonChooseFiles.BackColor = SystemColors.ControlDarkDark;
                        buttonStartServer.Text = ClassConstMessage.WAITFILES;
                        buttonChooseFiles.Enabled = true;
                        labelNbFile.Text = $"{ClassConstMessage.NBFILES}  0";
                    }
                    else
                    {
                        buttonChooseFiles.BackColor = Color.Green;
                        buttonStartServer.Text = ClassConstMessage.STARTSERVER;
                        buttonStartServer.Enabled = true;
                        labelNbFile.Text = $"{ClassConstMessage.NBFILES}  {Files.Count()}";
                    }
                    return;
                }
                else if (message == ClassConstMessage.WAITFILES)
                {
                    buttonStartServer.Text = message;  //foreColor black if disabled
                    buttonChooseFiles.Enabled = true;
                 }
                else if (message == ClassConstMessage.STARTSERVER)
                {
                    buttonStartServer.Text = ClassConstMessage.STOPSERVER;
                    buttonStartServer.BackColor = Color.Green;
                    buttonStartServer.Enabled = false;
                    buttonStartServer.Enabled = true;
                    textBoxPort.Enabled = false;
                }
                else if (message == ClassConstMessage.STOPSERVER || message == ClassConstMessage.STOPPED || message.Contains(ClassConstMessage.ABANDON) || message == ClassConstMessage.COMPLETED)
                {
                    buttonStartServer.Text = ClassConstMessage.STARTSERVER;
                    buttonStartServer.BackColor = SystemColors.ControlDarkDark;
                    buttonStartServer.Enabled = true;
                    buttonStartServer.Enabled = true;
                    textBoxPort.Enabled = true;
                }
                richTextBoxMessages.Text += (message + Environment.NewLine);
                richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                richTextBoxMessages.ScrollToCaret();
 
                Application.DoEvents();
            }
        }
 
        private async  Task treatServerAsync()
        {
                if (myClassServerTCP == null)
                {
                    myClassServerTCP = new ClassServerTCP();
                    myClassServerTCP.MessageReceived += Server_MessageReceived;
                }
                myClassServerTCP.tempoBetweenFile = tempoBetweenFile;

                myClassServerTCP.NEmissionForAllFiles = NEmissionForAllFiles;
                myClassServerTCP.NEmissionForEachFile = NEmissionForEachFile;
                myClassServerTCP.PortTCP = PortTCP;
                myClassServerTCP.listFiles = listFiles;
            int sampleRat = 0;
            foreach (System.Windows.Forms.RadioButton sr in radioButtonsSR)
            {
               if(sr.Checked)
                {
                    Int32.TryParse(sr.Tag.ToString(), out sampleRat);
                    break;
                }
            }
            await myClassServerTCP.Start(sampleRat);
 
        }
        private void Stop()
        {
            myClassServerTCP?.Stop();
        }
        private void Server_MessageReceived(object sender, string message)
        {
            richTextBoxMessages.Invoke((MethodInvoker)(() =>
            {
                AddMessage(message);
            }));
        }
        private Boolean testCharNum(char KeyChar)
        {
            if (Char.IsDigit(KeyChar) || KeyChar == '\b')
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        #region Form events
        private Int32 memPortTCP=0;
        private async void buttonStartServer_ClickAsync(object sender, EventArgs e)
        {
            if (buttonStartServer.Text == ClassConstMessage.STARTSERVER) //already change for stop
            {
                labelSampleRate.Text = "";
                labelNbSendingForEachFile.Text = "";
                labelNbSendingForAllFiles.Text = "";
                labelNumFile.Text = "";

                bool selectSR = false;
                foreach (System.Windows.Forms.RadioButton sr in radioButtonsSR)
                {
                    if (sr.Checked)
                    {
                        selectSR = true;
                        break;
                    }
                }
                if (!selectSR)
                 {
                    MessageBox.Show("Choose sample rate", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                    if (Files == null || Files.Count() == 0)
                {
                    MessageBox.Show("Choose files", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                richTextBoxMessages.Text = "";
                if (!Int32.TryParse(textBoxTempoBetweenFile.Text, out tempoBetweenFile))
                {
                    MessageBox.Show("Invalid input: Tempo Between File", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!Int32.TryParse(textBoxNEmissionForAllFiles.Text, out NEmissionForAllFiles))
                {
                    MessageBox.Show("Invalid input: N Emission For All Files", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!Int32.TryParse(textBoxNEmissionForEachFile.Text, out NEmissionForEachFile))
                {
                    MessageBox.Show("Invalid input: N Emission For Each File", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!Int32.TryParse(textBoxPort.Text, out PortTCP))
                {
                    MessageBox.Show("Invalid input: Port", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if(PortTCP<1024 || PortTCP>65535)
                {
                    MessageBox.Show("Invalid Port must be 1024 to 65535", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if(PortTCP!=memPortTCP)
                {
                    memPortTCP = PortTCP;
                    myClassServerTCP?.Stop();
                }
 
                AddMessage(ClassConstMessage.STARTSERVER);

                await treatServerAsync();
            }
            else
            {
                AddMessage(ClassConstMessage.STOPSERVER);
                Stop();
            }
        }
        private void buttonInfosWav_Click(object sender, EventArgs e)
        {
            richTextBoxMessages.Text = "";
            buttonInfosWav.Enabled = false;
            buttonInfosWav.BackColor = Color.Green;

            ClassWav.InfoWav();

            buttonInfosWav.BackColor = SystemColors.ControlDarkDark;
            buttonInfosWav.Enabled = true;
        }
        private void buttonConcateneRawIQ_Click(object sender, EventArgs e)
        {
            richTextBoxMessages.Text = "";
            buttonConcateneRawIQ.Enabled = false;
            buttonConcateneRawIQ.BackColor = Color.Green;

            if (Int32.TryParse(textBoxTempoBetweenFile.Text, out Int32 tempoBetweenFile))
                AddMessage(ClassRaw.ConcatRaw(tempoBetweenFile));

            buttonConcateneRawIQ.BackColor = SystemColors.ControlDarkDark;
            buttonConcateneRawIQ.Enabled = true;
        }
        private void buttonConcateneWav_Click(object sender, EventArgs e)
        {
            richTextBoxMessages.Text = "";
            buttonConcateneWav.Enabled = false;
            buttonConcateneWav.BackColor = Color.Green;

            if (Int32.TryParse(textBoxTempoBetweenFile.Text, out Int32 tempoBetweenFile))
                AddMessage(ClassWav.ConcateneWav(tempoBetweenFile));

            buttonConcateneWav.BackColor = SystemColors.ControlDarkDark;
            buttonConcateneWav.Enabled = true;
        }
        private void buttonExtractCu8_Click(object sender, EventArgs e)
        {
            string srcPath = "C:\\marc\\tnt\\fichiers_cu8_et_wav\\fichiers_cu8\\rtl_433_tests-master\\tests";   // Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dstPath = "C:\\marc\\tnt\\fichiers_cu8_et_wav\\regroupes_rtl_433_tests-master"; //create folder
            string ext = "cu8";
            AddMessage(ClassRaw.ExtractFiles(srcPath, dstPath, ext));
        }
        private void buttongenereWavTest_Click(object sender, EventArgs e)
        {
            ClassWav.genereWavTest();
        }
        private void textBoxTempoBetweenFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = testCharNum(e.KeyChar);
         }
        private void textBoxNEmissionForEachFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = testCharNum(e.KeyChar);
        }
        private void textBoxNEmissionForAllFiles_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = testCharNum(e.KeyChar);
        }
        private void textBoxPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = testCharNum(e.KeyChar);
        }
        Dictionary<string, string> listFiles;  // = new Dictionary<string, string>();
        List<int> sampleRate = new List<int>() ;
        List<int> usedListSampleRate = new List<int>() ;
        private void buttonChooseFiles_Click(object sender, EventArgs e)
        {
            usedListSampleRate.Clear();
          
            Files = getFiles("*.*");
            listFiles = triSampleRate(Files, sampleRate, usedListSampleRate);
            usedListSampleRate.Sort();
            foreach (System.Windows.Forms.RadioButton sr in radioButtonsSR)
            {
                sr.Enabled = false;
            }
            int i = 0;
            foreach (int sr in sampleRate)
            {
                foreach (int srUse in usedListSampleRate)
                {
                    if (srUse == sr)
                    {
                        radioButtonsSR[i].Enabled = true;
                        break;
                    }
                }
                i += 1;
            }
            radioButtonsSR[radioButtonsSR.Count()-1].Enabled = true;


                AddMessage($"{ClassConstMessage.NBFILES}  {Files?.Count()}");

        }
        private void buttonClearMessages_Click(object sender, EventArgs e)
        {
            richTextBoxMessages.Text="";
        }
        private void buttonMessageToClipBoard_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            if(richTextBoxMessages.Text!=String.Empty)
                Clipboard.SetText(richTextBoxMessages.Text);
        }
        private void FormServerForSDRSharp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            if(myClassServerTCP!=null)
            {
                myClassServerTCP = null;
            }
        }
        #endregion

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBoxMessages.LoadFile("./Readme.MD", RichTextBoxStreamType.PlainText);
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Help->error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //private Dictionary<string, infoFile> listFiles;
        private void buttonGenereBatch_Click(object sender, EventArgs e)
        {
            /*generate batch file for replay with rtl_433
           -Use this version RTL433 with compile option BATCH
                display only 
                 -name file
                 -protocol
                 -model
                 -some error
           -transferer with button 'genere batch for rtl_433' le fichier FilesRTL433AllOK.bat where is rtl_433.exe
           -copy FilesRTL433AllOK.bat to path where is rtl433.exe
           -open console
           -cd path where is rtl433.exe
           -execute commande:  FilesRTL433AllOK.bat > testRTL_433.txt 2>&1

           -tri with button 'tri result batch file for rtl433'
            -result to file triTestRTL_433.txt to path where is rtl433.exe

           -you can start again with other sample rates for the files that do not have one in their name
           -you can parameter frequency to 838Mhz other filter if f>800Mhz
            */

            /*
           test RTL_433--->https://github.com/merbanan/rtl_433_tests
            -download zip and dezip where is rtl_433.exe to test
            -open a console
            -cd path where is rtl433.exe
            -don't use 'make test' but
                   python bin/run_test.py -I time --first-line

            */

            if (Files != null)
                Files = null;

            Files = getFiles("*.cu8");
            if (Files == null)
                return;
  
            //string srcPath = "C:\\marc\\tnt\\fichiers_cu8_et_wav\\fichiers_cu8\\rtl_433_tests-master\\rtl_433_tests-master";   // Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dstFile ="FilesRTL433AllOK.bat";  // "C:\\marc\\tnt\\rtl_433\\rtl_433-master_06052024\\vs17_32\\Debug\\FilesRTL433AllOK.bat ";
            //var files = from file in Directory.EnumerateFiles(srcPath, "*.cu8", SearchOption.AllDirectories)
                        //select new
                        //{
                        //    File = file,
                        //};
            Int32 cptFile = 0;
            try
            {
                using (Stream stream = new FileStream(dstFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    String Line = "";
                    //String PathRtl433_EXE = "C:\\marc\\tnt\\rtl_433\\rtl_433-master_06052024\\vs17_32\\Debug\\rtl_433";
                    String PathRtl433_EXE = "rtl_433";
                    StreamWriter str = new StreamWriter(stream);
                    str.WriteLine("cls");
                    str.WriteLine("REM: " + DateTime.Now);
                    foreach (var file in Files)
                    {
                        FileInfo f = new FileInfo(file);
                        if (f.Length > 0)
                        {
                            //Int32 sampleRate = 0;FromFileName
                            Int32 sampleRate = WavRecorder.GetSampleRateFromName(file); //lacrosse_g2750_915M_1000k.cu8,9_ford-unlock002.cu8
                            if (sampleRate == -1)
                            //{
                                //sampleRateFromFileName = 250;
                                sampleRate = 250000;
                                //MessageBox.Show("No sample rate detected in the file name", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //return -1;
                            //}
                            //else
                                //sampleRate = sampleRateFromFileName;
                            //if (sampleRateFromFileName > 0)
                                
                        //}
                            //String Option = " -M protocol -s " + sampleRate.ToString() + " -C si -r ";
                            String Option = " -F json " + " -C si -r ";
                            Line = PathRtl433_EXE + Option + file;
                            str.WriteLine(Line);
                            cptFile++;
                            //if (cptFile % 20 == 0)
                            //    str.WriteLine("Pause");
                        }
                        f = null;
                    }
                    str.Flush();
                    str = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            MessageBox.Show("Batch file is completed for " + cptFile.ToString() + " files", "Translate cu8 nameFile with options to batch file", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void buttonTriResultBatch_Click(object sender, EventArgs e)
        {
            if (Files != null)
                Files = null;

            Files = getFiles("*.txt",false);
            if (Files == null)
                return;
            FileInfo f = new FileInfo(Files[0]);
            string path = f.DirectoryName + "\\";
            Dictionary<String, Int32> listData = new Dictionary<String, Int32>();
            try
            {
                Int32 Value = 0;
                Stream stream = new FileStream(Files[0], FileMode.Open, FileAccess.Read, FileShare.None);
                using (StreamReader str = new StreamReader(stream))
                {
                    while (str.Peek() >= 0)
                    {
                        String line = str.ReadLine();

                        if (line != null && line.Length > 0)
                        {
                            if (line.Contains("Protocol  :"))
                            {
                                Value = int.Parse(line.Substring(12), CultureInfo.CurrentCulture);
                            }
                            else if (line.Contains("model     :"))
                            {
                                if (!listData.ContainsKey(line.Substring(12)))
                                    listData.Add(line.Substring(12), Value);
                                Value = 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                //MessageBox.Show(e.Message, "Error import devices fct(deSerializeText).File:" + fileName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Dictionary<String, String> listDataTrie = new Dictionary<String, String>();
            //var sortOut = from entry in listData orderby entry.Key ascending select entry;
            //var myList = aDictionary.ToList();

            //myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            try
            {
                Stream stream = new FileStream(path+"triTestRTL_433.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                using (StreamWriter str = new StreamWriter(stream))
                {
                    var ordered = listData.OrderBy(x => x.Value);
                    foreach(KeyValuePair<string, Int32> entry in ordered)
                    {
                            str.WriteLine($"{entry.Value.ToString()} \t {entry.Key}");
                            //Debug.WriteLine($"{entry.Value.ToString()} \t {entry.Key}");
                    }
                }
            }
            catch
            {
            }
            MessageBox.Show("Tri file is completed for " + path + "triTestRTL_433.txt" , "Translate cu8 nameFile with options to batch file", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}


