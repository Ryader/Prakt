using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Practical1Client
{
    public class UdpFileClient
    {
        private const int LocalPort = 5002;
        private static readonly UdpClient ReceivingUdpClient = new UdpClient(LocalPort);
        private static IPEndPoint _remoteIpEndPoint;
        private static byte[] _receiveBytes = new byte[0];

        public static void Main()
        {
            try
            {
                Console.WriteLine("\nОжидайте получение файла ...");
                _receiveBytes = ReceivingUdpClient.Receive(ref _remoteIpEndPoint);
                Console.WriteLine("Файл получен!\nИдёт сохранение ...");
                using (var fs = new FileStream("temp.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Write(_receiveBytes, 0, _receiveBytes.Length);
                    Console.WriteLine("Файл сохранен!\nОткрытие файла...");
                    Process.Start(fs.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                ReceivingUdpClient.Close();
                Console.Read();
            }
        }
    }
}