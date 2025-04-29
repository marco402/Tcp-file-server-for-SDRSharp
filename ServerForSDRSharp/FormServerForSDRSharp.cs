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
            AddMessage(ClassRaw.ExtractFiles("", "", ""));
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
        private void buttonChooseFiles_Click(object sender, EventArgs e)
        {
            Files = getFiles("*.*");
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


