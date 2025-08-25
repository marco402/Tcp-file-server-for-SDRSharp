using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Server_for_SDRSharp
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ////case freq + sample rate 
            //String F = GetFrequencyFromName("C:\\marc\\tnt\\fichiers_cu8_et_wav\\regroupes_rtl_433_tests-master\\abarth124_tpms_01_0_01_FR_1_433.92M_250k.cu8");
            //    Int32 Fint = GetFrequencyFromString(F);
            //    MessageBox.Show(Fint.ToString(),"freq + sample rate:433920000");
            //    //case nothing 
            //    F = GetFrequencyFromName("C:\\marc\\tnt\\fichiers_cu8_et_wav\\regroupes_rtl_433_tests-master\\acurite_Acurite_3-in-1_01_11_gfile017.cu8");
            //    Fint = GetFrequencyFromString(F);
            //    MessageBox.Show( Fint.ToString(),":nothing->pb return 11 char after last _");
            //    //case without unit
            //    F = GetFrequencyFromName("C:\\marc\\tnt\\fichiers_cu8_et_wav\\regroupes_rtl_433_tests-master\\abarth124_tpms_01_0_01_FR_120000_250k.cu8");
            //    Fint = GetFrequencyFromString(F);
            //    MessageBox.Show( Fint.ToString(),"without unit:120000");
            //    //case with only one _
            //    F = GetFrequencyFromName("C:\\marc\\tnt\\fichiers_cu8_et_wav\\regroupes_rtl_433_tests-master\\abarth124tpms0101FR120000_250k.cu8");
            //    Fint = GetFrequencyFromString(F);
            //    MessageBox.Show( Fint.ToString(),"only one _:nothing");
            //    //case lenght unit >1 char
            //    F = GetFrequencyFromName("C:\\marc\\tnt\\fichiers_cu8_et_wav\\regroupes_rtl_433_tests-master\\abarth124_tpms_01_0_01_FR_1_433920khz_250k.cu8");
            //    Fint = GetFrequencyFromString(F);
            //    MessageBox.Show( Fint.ToString(),"unit >1 char:433920000");
            //    //MessageBox.Show(F);
            //    //Int32 Fint = GetFrequencyFromString(F);
            //    //MessageBox.Show(Fint.ToString());
            Application.Run(new FormServerForSDRSharp());
        }

    static Int32 GetFrequencyFromString(String Frequency)
    {
        if (Frequency == string.Empty)
            return -1;
        string[] units = { "M", "Khz", "khz", "HZ", "Hz", "K", "k", "m" , "" };
        Int32[] coef = { 1000000, 1000, 1000, 1, 1, 1000, 1000, 1000000, 1 };
        float freqInt = 0;
        Int32 indice = 0;
        foreach (string unit in units)
        {
            if (Frequency.Contains(unit))
            {
                    try
                    {
                        //Frequency = Frequency+"a";  //test
                        freqInt = float.Parse(Frequency.Substring(0, Frequency.Length - unit.Length), CultureInfo.InvariantCulture);
                        freqInt *= coef[indice];
                        //        //if (freqInt <= _owner.MinimumTunableFrequency || freqInt > _owner.MaximumTunableFrequency)
                        if (freqInt <= 0 || freqInt > 2500000000)

                            return -1;
                        else
                            return (Int32)freqInt;
                    }
                    catch
                    {
                        return -1;
                    }
                //    if (float.TryParse(Frequency.Substring(0, Frequency.Length - unit.Length), out freqInt))
                //{
                //    freqInt *= coef[indice];
                //        //if (freqInt <= _owner.MinimumTunableFrequency || freqInt > _owner.MaximumTunableFrequency)
                //        if (freqInt <= 0 || freqInt > 2500000000)

                //            return -1;
                //    else
                //        return (Int32)freqInt;
                }
                //else
                //    return -1;
                //}
                indice += 1;
        }
        return -1;
    }
        //dup with WavRecorder
        static string GetFrequencyFromName(String fileName)
        {
            //string[] units = { "M", "Khz", "Hz", "k", "m", "khz", "HZ", "K" };
            String freqStr;
            fileName = Path.GetFileName(fileName);
            //foreach (string unit in units)
            //{
                freqStr = GetStringFrequency(fileName);
                if (freqStr != "")
                    return freqStr;  // + unit;
            //}
            return "";
        }

        //dup with WavRecorder
        static string GetStringFrequency(string fileName)
    {
        string[] units = { "M", "Khz", "Hz", "k", "m", "khz", "HZ", "K" ,""};
        Int32 lastCar = 0;
        Int32 startCar = 0;
        Int32 fin = fileName.Length - 3;
        string retString = "";
        Int32 cptUnderscore = 0;
        for (Int32 i = fin; i > 0; i--)
        {
            if (fileName.Substring(i, 1) == "_" && cptUnderscore==0)
            {
                lastCar = i - 1;
                cptUnderscore += 1;
            }
            else if (fileName.Substring(i, 1) == "_" && cptUnderscore == 1)
            {
                startCar = i + 1;
                    string foundUnit = string.Empty;
                    bool okUnit = false;
                    foreach (string unit in units)
                    {
                    foundUnit=fileName.Substring(startCar+lastCar - startCar-unit.Length+1, unit.Length);
                        if (foundUnit == unit)
                        {
                            okUnit = true;
                            break;
                        }
                    }






                    retString = fileName.Substring(startCar, lastCar - startCar- foundUnit.Length+1); 
                    //string foundUnit=fileName.Substring(startCar+lastCar - startCar, unit.Length); 
                    //retString = retString.Replace(".", ",");
                    // if (float.TryParse(retString, out float ret))   //found for another unit
 
                    try
                      {
                    float resultat = float.Parse(retString, CultureInfo.InvariantCulture);
                        //String CharUnit = fileName.Substring(lastCar, unit.Length);
                        if (okUnit)
                            return retString + foundUnit;
                        else
                            return fileName.Substring(startCar, lastCar - startCar+ foundUnit.Length);
                    }
                    catch
                    {
                        return "";
 //break;
                    }
                //    return resultat;
                //else
                   
            }
        }



            //for (Int32 i = fin; i > 0; i--)
            //{
            //    if (fileName.Substring(i, 1) == unit)
            //    {
            //        lastCar = i - 1;
            //        for (--i; i > 0; i--)
            //        {
            //            if (fileName.Substring(i, 1) == "_")
            //            {
            //                startCar = i + 1;
            //                retString = fileName.Substring(startCar, lastCar - startCar + 1);
            //                retString = retString.Replace(".", ",");
            //                if (float.TryParse(retString, out float ret))   //found for another unit
            //                    return retString;
            //                else
            //                    break;
            //            }
            //        }
            //    }
            //retString = retString.Replace(".", ",");
            //if (float.TryParse(retString, out float ret))   //found for another unit
            //    break;
            //}
            return retString;
        }
    }
}
