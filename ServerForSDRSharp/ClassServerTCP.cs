/* Written by Marc Prieur (marco40_github@sfr.fr)
                                   ClassServerTCP.cs 
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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server_for_SDRSharp
{
    class ClassServerTCP: IDisposable
    {
         string[] _Files;
         Boolean EndReceptTCP = false;
         private TcpListener Server;
         private TcpClient client;
         public event EventHandler<string> MessageReceived;
         NetworkStream stream;

        public async Task Start()
        {
                try
                {
                if (Server != null)
                {
                    EndReceptTCP = true;
                    if (client != null)
                    {
                        OnMessageReceived($"Connected! Client IP: {client.Client.RemoteEndPoint}");
                        _ = HandleClientAsync(client);
                    }
                 }
                else
                {
                    IPAddress localhost = IPAddress.Parse(FormServerForSDRSharp.IPAdress);

                    Server = new TcpListener(localhost, _PortTCP);
                    Server.Start(1000);

                    OnMessageReceived("Server started. Waiting for a connection...If radio SDRSharp already started:restart radio SDRSharp");
                    EndReceptTCP = true;
                    while (EndReceptTCP)
                    {
                        client = await Server.AcceptTcpClientAsync();
                        OnMessageReceived($"Connected! Client IP: {client.Client.RemoteEndPoint}");
                        _ = HandleClientAsync(client);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.HResult != -2147467259)
                {
                    OnMessageReceived($"Port already in use, change port {ClassConstMessage.ABANDON}");
                }
                else if(e.HResult != -2146232798)
                    OnMessageReceived(e.Message);

            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            stream = client.GetStream();
            Int32 NFile = 0;
            Int32 UsedSampleRate = 0;
            while (EndReceptTCP)
            {
                for (Int32 EmissionForAllFiles = 0; EmissionForAllFiles < _NEmissionForAllFiles; EmissionForAllFiles++)
                {
                    OnMessageReceived($"{ClassConstMessage.RESTLOOPALLFILE}{EmissionForAllFiles+1}/{_NEmissionForAllFiles}");
                    NFile = 0;
                    foreach (String file in _Files)
                    {
                        float[] L = null;
                        float[] R = null;
                        byte[] byteArray = null;
                        Int32 size = 0;
                        String message = "";
                        byte[] byteSend = null;
                        Int32 sampleRate = 0;

                        if (Path.GetExtension(file) == ".wav")
                        {
                            if (!ClassWav.readWav(file, ref byteArray, ref R, ref L, ref size, false, ref message, ref sampleRate))
                            {
                                OnMessageReceived($"{message} for {file}");
                                continue;
                            }
                            else
                            {
                                if(UsedSampleRate == 0)
                                {
                                    UsedSampleRate = sampleRate;
                                    OnMessageReceived($"{ClassConstMessage.SAMPLERATE} {UsedSampleRate}");
                                }
                            }
                            if(UsedSampleRate != sampleRate)
                            {
                                OnMessageReceived($"different sample rate for {file}");
                                continue;
                            }
                            NFile += 1;
                            OnMessageReceived($"{ClassConstMessage.NUMFILE}{NFile}/{_Files.Count()}");
                            OnMessageReceived(file);
                            byteSend = new byte[L.Length];
                            ClassUtils.ConvertFloatToByte(L, byteSend);
                            for (Int32 EmissionForEachFile = 0; EmissionForEachFile < _NEmissionForEachFile; EmissionForEachFile++)
                            {
                                OnMessageReceived($"{ClassConstMessage.RESTLOOPFILE}{EmissionForEachFile+1}/{_NEmissionForEachFile}");
                                try
                                {
                                    await stream.WriteAsync(byteSend, 0, byteSend.Length);
                                }
                                catch (SocketException e)
                                {
                                    if (e.HResult == -2146232800)
                                    {
                                        OnMessageReceived($"{e.Message} {ClassConstMessage.ABANDON}");
                                        return;
                                    }
                                    else
                                        OnMessageReceived(e.Message);
                                }
                                Thread.Sleep(_tempoBetweenFile);
                                if (!EndReceptTCP)
                                {
                                    OnMessageReceived(ClassConstMessage.STOPSERVER);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            
                            byteSend = ClassUtils.getDataFile(file);
                            if (byteSend.Length == 0)
                                continue;
                            sampleRate = WavRecorder.GetSampleRateFromName(file);
                            {
                                if (UsedSampleRate == 0 && sampleRate!=-1)
                                {
                                    UsedSampleRate = sampleRate;
                                    OnMessageReceived($"{ClassConstMessage.SAMPLERATE} {UsedSampleRate}");
                                }
                            }
                            if (UsedSampleRate != sampleRate && sampleRate !=-1)  //for raw if not sample rate from name keep file
                            {
                                OnMessageReceived($"different sample rate for {file}");
                                continue;
                            }
                            NFile += 1;
                            OnMessageReceived($"{ClassConstMessage.NUMFILE}{NFile}/{_Files.Count()}");
                            OnMessageReceived(file);
                            for (Int32 EmissionForEachFile = 0; EmissionForEachFile < _NEmissionForEachFile; EmissionForEachFile++)
                            {
                                OnMessageReceived($"{ClassConstMessage.RESTLOOPFILE}{EmissionForEachFile+1}/{_NEmissionForEachFile}");
                                try
                                {
                                    await stream.WriteAsync(byteSend, 0, byteSend.Length);
                                }
                                catch (SocketException e)
                                {
                                    if (e.HResult == -2146232800)
                                    {
                                        OnMessageReceived($"{e.Message}  {ClassConstMessage.ABANDON}");
                                        return;
                                    }
                                    else
                                        OnMessageReceived(e.Message);
                                }
                                Thread.Sleep(_tempoBetweenFile);
                                if (!EndReceptTCP)
                                {
                                    OnMessageReceived(ClassConstMessage.STOPPED);
                                    return;
                                }
                            }
                        }
                    }
                }
                OnMessageReceived(ClassConstMessage.COMPLETED);
                break;
            }     
        }
        protected virtual void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(this, message);
        }

        internal void Stop()
        {
            Server?.Stop();
            Server = null;
            EndReceptTCP = false;
        }

        public void Dispose()
        {
            ((IDisposable)stream).Dispose();
            ((IDisposable)client).Dispose();
        }

        private Int32 _PortTCP;
        private Int32 _tempoBetweenFile;
        private Int32 _NEmissionForAllFiles;
        private Int32 _NEmissionForEachFile;
        internal Int32 NEmissionForAllFiles
        {
            set
            {
                _NEmissionForAllFiles = value;
            }
        }
        internal Int32 NEmissionForEachFile
        {
            set
            {
                _NEmissionForEachFile = value;
            }
        }
        internal Int32 tempoBetweenFile
        {
            set
            {
                _tempoBetweenFile = value;
            }
        }
        internal Int32 PortTCP
        {
            set
            {
                _PortTCP = value;
            }
        }
        internal string[] Files
        {
            set
            {
                _Files = value;
            }
        }
     }
}
