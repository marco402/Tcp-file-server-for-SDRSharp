/* Written by Marc Prieur (marco40_github@sfr.fr)
                                   ClassWav.cs 
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
using System.Text;
using System.Windows.Forms;

namespace Server_for_SDRSharp
{
    class ClassWav
    {
        private const string NAMEOUTFILEWAV = "concatenetsWav_";
        internal static String InfoWav()
        {
            using (OpenFileDialog openWav = new OpenFileDialog())
            {
                openWav.DefaultExt = "wav";
                openWav.Filter = "wav files|*.wav";
                openWav.Multiselect = true;
                if (openWav.ShowDialog() == DialogResult.OK)
                {
                    foreach (String file in openWav.FileNames)
                    {
                        byte[] byteArray = null;
                        float[] L = null;
                        float[] R = null;
                        Int32 size = 0;
                        String message = "";
                        Int32 sampleRate = 0;
                        readWav(file, ref byteArray, ref R, ref L, ref size, true, ref message, ref sampleRate);
                        MessageBox.Show(message);
                    }
                }
            }
            return "";
        }
        internal static Boolean readWav(string filename, ref byte[] byteArray, ref float[] R, ref float[] L, ref Int32 size, Boolean retLAndRElseAllInL, ref String message, ref Int32 SampleRate)
        {
            try
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    BinaryReader reader = new BinaryReader(fs);
                    message = filename + "\n";
                    byte[] tabByte = BitConverter.GetBytes(reader.ReadInt32());
                    String chunkID = ((ASCIIEncoding)new ASCIIEncoding()).GetString(tabByte);
                    message += $"chunkID:\t\t\t{chunkID}\n";
                    Int32 fileSize = reader.ReadInt32();
                    message += $"fileSize:\t\t\t{fileSize}\n";
                    tabByte = BitConverter.GetBytes(reader.ReadInt32());
                    String riffType = ((ASCIIEncoding)new ASCIIEncoding()).GetString(tabByte);
                    //Int32 riffType = reader.ReadInt32();
                    message += $"riffType:\t\t\t{riffType}\n";
                    Int32 bitDepth = 0;
                    Int32 nValues = 0;
                    Int32 channels = 0;
                    Int32 bytes = 0;
                    Int64 bytes64 = 0;
                    if (chunkID == "RIFF")
                    {
                        // chunk 1
                        //Int32 fmtID = reader.ReadInt32();
                        tabByte = BitConverter.GetBytes(reader.ReadInt32());
                        String fmtID = ((ASCIIEncoding)new ASCIIEncoding()).GetString(tabByte);
                        message += $"fmtID:\t\t\t{fmtID}\n";
                        Int32 fmtSize = reader.ReadInt32(); // bytes for this chunk (expect 16 or 18)
                        message += $"fmtSize:\t\t\t{fmtSize}\n";
                        // 16 bytes coming...
                        Int32 fmtCode = reader.ReadInt16();
                        message += $"fmtCode:\t\t\t{fmtCode}\n";
                        channels = reader.ReadInt16();
                        message += $"channels:\t\t{channels}\n";
                        SampleRate = reader.ReadInt32();
                        message += $"sampleRate:\t\t{SampleRate}\n";
                        Int32 byteRate = reader.ReadInt32();
                        message += $"byteRate:\t\t{byteRate}\n";
                        Int32 fmtBlockAlign = reader.ReadInt16();
                        message += $"fmtBlockAlign:\t\t{fmtBlockAlign}\n";
                        bitDepth = reader.ReadInt16();
                        message += $"bitDepth:\t\t{bitDepth}\n";
                        if (fmtSize == 18)
                        {
                            // Read any extra values
                            Int32 fmtExtraSize = reader.ReadInt16();

                            reader.ReadBytes(fmtExtraSize);
                            message += $"fmtExtraSize:\t\t{fmtExtraSize}\n";
                        }

                        // chunk 2
                        //Int32 dataID = reader.ReadInt32();
                        tabByte = BitConverter.GetBytes(reader.ReadInt32());
                        String dataID = ((ASCIIEncoding)new ASCIIEncoding()).GetString(tabByte);
                        message += $"dataID:\t\t\t{dataID}\n";
                        bytes = reader.ReadInt32();
                        message += $"bytes:\t\t\t{bytes}\n";

                        byteArray = reader.ReadBytes((Int32)bytes);

                        Int32 bytesForSamp = bitDepth / 8;
                        nValues = bytes / bytesForSamp;
                    }
                    else if (chunkID == "RF64")
                    {
                        bitDepth = 64;
                        tabByte = BitConverter.GetBytes(reader.ReadInt32());
                        String chunkId = ((ASCIIEncoding)new ASCIIEncoding()).GetString(tabByte);
                        message += $"fmtID:\t\t\t{chunkId}\n";
                        UInt32 chunkSize = reader.ReadUInt32();
                        message += $"chunkSize:\t\t{chunkSize}\n";


                        UInt32 riffSizeLow = reader.ReadUInt32();
                        //message += $"riffSizeLow:\t\t\t{riffSizeLow}\n";
                        UInt32 riffSizeHigh = reader.ReadUInt32();
                        //message += $"riffSizeHigh:\t\t\t{riffSizeHigh}\n";

                        UInt64 totalRiffSize = (UInt64)(riffSizeLow + (riffSizeHigh * Int32.MaxValue));
                        message += $"riffSize:\t\t\t{totalRiffSize}\n";

                        UInt32 dataSizeLow = reader.ReadUInt32();
                        //message += $"dataSizeLow:\t\t\t{dataSizeLow}\n";
                        UInt32 dataSizeHigh = reader.ReadUInt32();
                        //message += $"dataSizeHigh:\t\t\t{dataSizeHigh}\n";

                        bytes64 = (dataSizeLow + (dataSizeHigh * Int32.MaxValue));
                        message += $"dataSize:\t\t\t{bytes64}\n";

                        UInt32 sampleCountLow = reader.ReadUInt32(); // bytes for this chunk (expect 16 or 18)
                        //message += $"sampleCountLow:\t\t\t{sampleCountLow}\n";
                        UInt32 sampleCountHigh = reader.ReadUInt32(); // bytes for this chunk (expect 16 or 18)
                        //message += $"sampleCountHigh:\t\t\t{sampleCountHigh}\n";


                        UInt64 SampleRate64 = (sampleCountLow + (sampleCountHigh * Int32.MaxValue));
                        message += $"SampleRate(sampleCount):\t{SampleRate64}\n";



                        UInt32 tableLength = reader.ReadUInt32(); // bytes for this chunk (expect 16 or 18)
                        message += $"tableLength:\t\t{tableLength}\n";

                        message += $"Add bitDepth:\t\t{bitDepth}\n\n";
                        if (!retLAndRElseAllInL)
                            message = "";
                        message += $"Type RF64 Untreated:\t\n";
                        return false;
                    }

                    if (nValues == 0)
                        return false;
                    float[] asFloat = null;
                    switch (bitDepth)
                    {
                        case 64:
                            double[] asDouble = new double[nValues];
                            Buffer.BlockCopy(byteArray, 0, asDouble, 0, (Int32)bytes);
                            asFloat = Array.ConvertAll(asDouble, e => (float)e);
                            break;
                        case 32:
                            asFloat = new float[nValues];
                            Buffer.BlockCopy(byteArray, 0, asFloat, 0, bytes);
                            break;
                        case 16:
                            Int16[] asInt16 = new Int16[nValues];
                            Buffer.BlockCopy(byteArray, 0, asInt16, 0, bytes);
                            asFloat = Array.ConvertAll(asInt16, e => e / (float)(Int16.MaxValue + 1));
                            break;
                        default:
                            message = $"error:bitDepth different of 16,32 or 64";
                            return false;
                    }

                    switch (channels)
                    {
                        case 1:
                            L = asFloat;
                            //R = null;
                            return true;
                        case 2:
                            // de-interleave
                            Int32 nSamps = nValues / 2;
                            if (!retLAndRElseAllInL)
                            {
                                Array.Resize(ref L, nValues);
                                L = asFloat;
                            }
                            //else {
                            //    Array.Resize(ref R, size + nSamps);
                            //    Array.Resize(ref L, nValues/2);
                            //    for (Int32 s = 0; s < nValues; s++)
                            //    {
                            //        L[s] = asFloat[s];
                            //        R[s] = asFloat[s++];
                            //    }
                            //}
                            size = nValues;
                            return true;
                        ////// de-interleave
                        ////Int32 nSamps = nValues / 2;
                        ////Array.Resize(ref L, size + nSamps);
                        ////Array.Resize(ref R, size + nSamps);
                        //////L = new float[size];
                        //////R = new float[size];

                        ////for (Int32 s = size, v = 0; s < nSamps+size; s++)
                        ////{
                        ////    L[s] = asFloat[v++];
                        ////    R[s] = asFloat[v++];
                        ////}
                        ////size += nSamps;
                        ////return true;
                        default:
                            message = $"error:Nb channel different of 1 or 2 ";
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                message=$"error: {ex.Message}";    //+ filename
                return false;
            }
        }
        internal static void genereWavTest()
        {
            float[] dataIQ = new float[262144];
            for (Int32 i = 0; i < 262144; i += 2)
            {
                dataIQ[i] = 0;
                dataIQ[i + 1] = 0;
            }
            WavRecorder.WriteBufferToWav("xxxx.wav", dataIQ, 250000);
        }
        internal static String ConcateneWav(int tempoBetweenFile)
        {
            String message = "";
            using (OpenFileDialog openWav = new OpenFileDialog())
            {
                openWav.DefaultExt = "wav";
                openWav.Filter = "wav files|*.wav";
                openWav.Multiselect = true;
                String fileOut = "";
                float[] data = null;
                Int32 memoSampleRate = 0;
                Int32 nbFileOK = 0;
                if (openWav.ShowDialog() == DialogResult.OK)
                {
                    Int32 size = 0;
                    byte[] byteArray = null;
                    float[] R = null;
                    float[] L = null;
                    data = new float[0]; ;
                    Int32 memDataSize = 0;
                    String files = "";
                    foreach (String file in openWav.FileNames)
                    {
                        if (file.Contains(NAMEOUTFILEWAV))
                            continue;
                        try
                        {
                            files = file;
                            Int32 sampleRate = 0;
                            Boolean ret = ClassWav.readWav(file, ref byteArray, ref R, ref L, ref size, false, ref message, ref sampleRate);

                            if (!ret)
                                message+=($"pb file { file } concatenets wav");
                            else
                            {
                                if (memoSampleRate == 0)
                                {
                                    memoSampleRate = sampleRate;
                                    fileOut = Path.GetDirectoryName(file) + "\\" + NAMEOUTFILEWAV + "_" + (memoSampleRate / 1000).ToString() + "k_" + (DateTime.Now.Date.ToString("d").Replace("/", "_") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second) + Path.GetExtension(file);
                                }

                                if (memoSampleRate != sampleRate)
                                {
                                    message += ("Sample rate différent from the first.");
                                    message += ($"file {file}");
                                }
                                else
                                {
                                    //double coefficienttest = ClassUtils.GetMaxiTabFloat(L);  //not ok with .Max() not abs value
                                    //Debug.WriteLine(coefficienttest);
                                    //if (coefficient > 0.0)
                                    //{
                                    //    Debug.WriteLine(coefficient);
                                    Int32 tempo = 0;

                                        tempo = tempoBetweenFile * (memoSampleRate / 250);
                                    Int32 D = memDataSize;
                                    memDataSize += (L.Count()) + tempo;
                                    Array.Resize(ref data, memDataSize);
                                    //Buffer.BlockCopy(L, 0, data, D, L.Count());
                                    for (Int32 i = 0; i < (L.Count()); i++)
                                        data[i + D] = (L[i]);  // / coefficient  I    from -1 to +1
                                    nbFileOK++;
                                    //}
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            message += ($"Error {files}: {ex.Message}");

                        }
                    }

                    double coefficient = ClassUtils.GetMaxiTabFloat(data);  //not ok with .Max() not abs value

                    if (coefficient > 0.0 && coefficient != 1)
                    {

                        for (Int32 i = 0; i < (data.Count()); i++)
                            data[i] = (float)(data[i] / coefficient);  //  I    from -1 to +1
                    }

                    WavRecorder.WriteBufferToWav(fileOut, data, memoSampleRate);
                    message += ($"Completed Concatenets wav's ( {nbFileOK} / {openWav.FileNames.Count()}) to:");
                    message += ($"{fileOut}");
                }
            }
            return message;
        }
    }
}
