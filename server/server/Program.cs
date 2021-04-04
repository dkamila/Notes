using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static List<Page> note;
        static void Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            TcpListener server = new TcpListener(localAddr, port);
            note = new List<Page>();
            server.Start();
            Console.WriteLine("Сервер запущен!");

            while (true)
            {
                try
                {
                    // Подключение клиента
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    // Обмен данными
                    try
                    {
                        if (stream.CanRead)
                        {
                            byte[] myReadBuffer = new byte[1024];
                            StringBuilder myCompleteMessage = new StringBuilder();
                            int numberOfBytesRead = 0;
                            do
                            {
                                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                            }
                            while (stream.DataAvailable);
                            Byte[] responseData = handler(myCompleteMessage.ToString());
                            stream.Write(responseData, 0, responseData.Length);
                        }
                    }
                    finally
                    {
                        stream.Close();
                        client.Close();
                    }
                }
                catch
                {
                    server.Stop();
                    break;
                }
            }
        }


        static byte[] handler(string request)
        {
            string[] comand = request.Split(new char[] { '@' });
            string[] edit = comand[1].Split(new char[] { '#' });

            switch (comand[0])
            {
                case "add":
                    { 
                        note.Add(new Page(edit[0], edit[1]));
                        return (Encoding.UTF8.GetBytes(note.Find(x => x.title == edit[0]).title.ToString()));
                    }
                case "dell":
                    {
                        note.Remove(note.Find(x => x.title == edit[0]));
                    }
                    break;
                case "count":
                    {
                        return (Encoding.UTF8.GetBytes(note.Count.ToString()));
                    }
                case "show_DATA":
                    {
                        if (edit[0] != "")
                            return (Encoding.UTF8.GetBytes(note.Find(x => x.title == edit[0]).data.ToString()));
                    }
                    break;
                case "show_TITLE":
                    {
                        string rez = "";
                        foreach (var item in note)
                        {
                            rez += item.title + "#";
                        }
                        return (Encoding.UTF8.GetBytes(rez));
                    }
                default:
                    break;
            }
            return (Encoding.UTF8.GetBytes(""));
        }
    }
}
