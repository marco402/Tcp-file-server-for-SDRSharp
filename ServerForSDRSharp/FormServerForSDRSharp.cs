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

namespace Server_for_SDRSharp
{
    public partial class FormServerForSDRSharp : Form
    {
        #region CONST

        public const String IPAdress = "127.0.0.1";

        #endregion
        #region declare
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
        private string[] getFiles(String ext)
        {
            using (OpenFileDialog openFiles = new OpenFileDialog())
            {
                openFiles.DefaultExt = ext;
                openFiles.Filter = ext + " files|*." + ext;
                openFiles.Multiselect = true;
                if (openFiles.ShowDialog() == DialogResult.OK)
                    return openFiles.FileNames;
                return null;
            }
        }


        /// <summary>
        /// tri les fichiers en fonction du sample rate au plus proche des sample rate de la source SDRSharp RTL-SDR_TCP 
        /// </summary>
        /// <param name="listFiles"></param>
        private Dictionary<string, string> triSampleRate(string[] Files, List<int> listSampleRate)
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
                myClassServerTCP.Files = Files;
            await myClassServerTCP.Start();
 
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
        private void buttonChooseFiles_Click(object sender, EventArgs e)
        {
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
            Files = getFiles("*.*");
            listFiles=triSampleRate(Files,sampleRate);
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
    }
}


