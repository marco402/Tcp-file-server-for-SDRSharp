/* Written by Marc Prieur (marco40_github@sfr.fr)
                                ClassUtils.cs 
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
using System.Threading;

namespace Server_for_SDRSharp
{
    internal class ClassUtils
    {
        internal static double GetMaxiTabFloat(float[] bufferPtr)
        {
            double maxi = float.MinValue;
            for (var i = 0; i < bufferPtr.Length; i++)
                if (Math.Abs(bufferPtr[i]) > maxi)
                    maxi = Math.Abs(bufferPtr[i]);
            return maxi;
        }
        internal static Boolean ConvertCu8ToFloat(byte[] bufferByte, float[] bufferFloat)
        {
            double maxi = bufferByte.Max() / 2;  //Max() OK all > 0
            if (maxi > 0.0)
            {
                for (Int32 i = 0; i < bufferByte.Length; i++)
                    bufferFloat[i] = (float)((bufferByte[i] - 127) / maxi);  //from -1 to +1   0--->-1  255--->1   127--->0
                return true;
            }
            return false;
        }
        internal const float FLOATTOBYTE = 255f / 2f;
        internal static Boolean ConvertFloatToByte(float[] dataFloat, byte[] dataByte)
        {
            double coefficient = ClassUtils.GetMaxiTabFloat(dataFloat);
            if (coefficient <= 0.0)
                return false;
            Thread.BeginCriticalRegion();
            for (Int32 i = 0; i < dataFloat.Length; i++)
                try
                {
                    dataByte[i] = System.Convert.ToByte(FLOATTOBYTE + (dataFloat[i] / coefficient) * FLOATTOBYTE);
                }
                catch
                {
                    if ((FLOATTOBYTE + (dataFloat[i] / coefficient) * FLOATTOBYTE) < 0)
                        dataByte[i] = 0;
                    else
                        dataByte[i] = 255;
                }
            Thread.EndCriticalRegion();
            return true;
        }
        internal static byte[] getDataFile(String fileName)
        {
            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    byte[] dataIQ = new byte[reader.BaseStream.Length];
                    dataIQ = reader.ReadBytes((Int32)reader.BaseStream.Length);
                    return dataIQ;
                }
            }
            return null;
        }
    }
}
