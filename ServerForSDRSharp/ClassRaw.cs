/* Written by Marc Prieur (marco40_github@sfr.fr)
                                   ClassRaw.cs 
                               project ServerForSDRSharp
 **************************************************************************************
 Creative Commons Attrib Share-Alike License
 You are free to use/extend this library but please abide with the CC-BY-SA license: 
 Attribution-NonCommercial-ShareAlike 4.0 International License
 http://creativecommons.org/licenses/by-nc-sa/4.0/

 All text above must be included in any redistribution.
  **********************************************************************************/
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Server_for_SDRSharp
{
    class ClassRaw
    {
        private static Int32 cptPb = 0;
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memcpy")]
        internal unsafe static extern void* Memcpy(void* dest, void* src, Int32 len);
        internal static unsafe Boolean CopyMem(byte[] retourIQ, byte[] ptrTabIQ, Int32 decaleSrc, Int32 decaleDst, Int32 len)
        {
            if ((decaleDst + len) > retourIQ.Length)
                decaleDst = retourIQ.Length - len;
            if (decaleDst < 0)
                decaleDst = 0;
            if (decaleDst + len < 0)
                return false;
            if ((decaleSrc + len) > ptrTabIQ.Length)
            {
                Int32 lenEnd = ptrTabIQ.Length - decaleSrc;
                Int32 lenStart = len - lenEnd;
                fixed (byte* pointeurSrc = &ptrTabIQ[decaleSrc])
                {
                    fixed (byte* pointeurDst = &retourIQ[decaleDst])
                    {
                        Memcpy(pointeurDst, pointeurSrc, lenEnd * sizeof(byte));
                    }
                }
                fixed (byte* pointeurSrc = &ptrTabIQ[0])
                {
                    fixed (byte* pointeurDst = &retourIQ[decaleDst + lenEnd])
                    {
                        Memcpy(pointeurDst, pointeurSrc, lenStart * sizeof(byte));
                    }
                }
            }
            else
            {
                fixed (byte* pointeurSrc = &ptrTabIQ[decaleSrc])
                {
                    fixed (byte* pointeurDst = &retourIQ[decaleDst])
                    {
                        Memcpy(pointeurDst, pointeurSrc, len * sizeof(byte));
                    }
                }
            }
            return true;
        }
        internal static void WriteByte(String fileName, byte[] dataRawIQ)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    writer.Write(dataRawIQ);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        internal static String ConcatRaw(Int32 tempoBetweenFile)
        {
            String message = "";
            Int32 SizeTotal = 0;
            Int32 memoSampleRate = 0;
            Int32 nbFileOK = 0;
            using (OpenFileDialog openRawIQ = new OpenFileDialog())
            {
                String files = "";
                openRawIQ.DefaultExt = "*";
                openRawIQ.Filter = "* files|*.*";
                openRawIQ.Multiselect = true;
                String fileOut = "";
                Byte[] dataOut = new Byte[0];
                Int32 pointerIQ = 0;

                if (openRawIQ.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                         foreach (String file in openRawIQ.FileNames)
                        {
                            Int32 sampleRate = WavRecorder.GetSampleRateFromName(file);
                            if (memoSampleRate == 0 && sampleRate != -1)
                            {
                                memoSampleRate = sampleRate;
                                fileOut = Path.GetDirectoryName(file) + "\\concateneRawIQ_" + (memoSampleRate / 1000).ToString() + "k_" + (DateTime.Now.Date.ToString("d").Replace("/", "_") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second) + Path.GetExtension(file);
                            }

                            if (memoSampleRate != sampleRate && sampleRate != -1)
                            {
                                message += ("Sample rate différent from the first.\r\n");
                                message += ($"file {file}\r\n");
                            }

                            else
                            {
                                FileInfo f = new FileInfo(file);
                                if (memoSampleRate > 0)
                                    tempoBetweenFile *= (memoSampleRate / 1000);
                                SizeTotal += (Int32)f.Length + tempoBetweenFile;
                                Array.Resize(ref dataOut, SizeTotal);


                                ConcateneRawIQ(file, dataOut, tempoBetweenFile,ref pointerIQ);
                                nbFileOK++;
                            }
                        }
                        if (fileOut == "")  //all files without sample rate=>fileOut without sample rate
                            fileOut = Path.GetDirectoryName(openRawIQ.FileNames[0]) + "\\concateneRawIQ_" + (DateTime.Now.Date.ToString("d").Replace("/", "_") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second) + Path.GetExtension(openRawIQ.FileNames[0]);

                        SaveRawIQ(fileOut, dataOut);

                        message += ($"Completed Concatenets raw IQ ( {nbFileOK} / {openRawIQ.FileNames.Count()}) to:\r\n");
                        message += ($"{fileOut}\r\n");
                    }
                    catch (Exception ex)
                    {
                        message += ($"Error {files}: {ex.Message}\r\n");
                    }
                }
            }
            return message;
        }
        /// <summary>
        /// Group all file from several folder to folder sorted by sample rate
        /// If there is no sample rate in the file name sample rate=250k
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="dstPath"></param>
        /// <param name="extension"></param>
        internal static String ExtractFiles(String srcPath, String dstPath, String extension)
            {
            cptPb = 0;
            String message = "";
            try
            {
                Int32 lenPath = srcPath.Length + 1;
                var files = from file in Directory.EnumerateFiles(srcPath, "*" + extension, SearchOption.AllDirectories)
                            select new
                            {
                                File = file,
                            };
                String memoDirectory = "";
                Int32 cptFile = 0;
                String FileName = "";
                String FolderSampleRate = "";
                Boolean OK = false;
                foreach (var f in files)
                {
                    try
                    {
                        OK = true;
                        String directory = Path.GetDirectoryName(f.File);
                        if (!(memoDirectory == directory))   //only one file by folder
                        {
                            FileName = Path.GetFileName($"{f.File}");
                            FolderSampleRate = "";

                            Match match = Regex.Match(FileName, "([0-9,.]+)k", RegexOptions.IgnoreCase);
                            if (match.Success)
                            {
                                FolderSampleRate = "\\" + match.Groups[1].Value + "k";
                            }
                            else
                            {
                                FolderSampleRate = "\\250k";
                            }
                            if (OK)
                            {
                                {
                                    if (!Directory.Exists(dstPath + FolderSampleRate))
                                    {
                                        Directory.CreateDirectory(dstPath + FolderSampleRate);
                                    }
                                }

                                String newFile = directory.Substring(lenPath).Replace("\\", "_") + "_" + FileName;
                                message+=(dstPath + "\\" + newFile);

                                File.Copy($"{f.File}", dstPath + FolderSampleRate + "\\" + newFile);
                                memoDirectory = directory;
                                cptFile++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        message += (ex.Message);
                    }
                }
                message += (cptFile.ToString());


            }
            catch (UnauthorizedAccessException uAEx)
            {
                message += (uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                message += (pathEx.Message);
            }
            if (cptPb == 0)
                message += ("Sorted is completed");
            else
                message += ($"Sorted is NOT completed");

            return message;
        }

        private static void SaveRawIQ(String file, Byte[] dataOut)
        {
            WriteByte(file, dataOut);
        }
        private static void ConcateneRawIQ(String fileName, Byte[] dataOut, Int32 tempoBetweenFile,ref Int32 pointerIQ)
            {
                if (File.Exists(fileName))
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                    {
                        byte[] dataIQ = new byte[reader.BaseStream.Length];
                        dataIQ = reader.ReadBytes((Int32)reader.BaseStream.Length);
                        CopyMem(dataOut, dataIQ, 0, pointerIQ, (Int32)reader.BaseStream.Length);
                        pointerIQ += (Int32)reader.BaseStream.Length + tempoBetweenFile;
                    }
                }
            }
    }
}
