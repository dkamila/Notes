using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client_notes
{
    public partial class Add_page : Form
    {
        public Add_page()
        {
            InitializeComponent();
        }

        static private string Exchange(string address, int port, string outMessage)
        {
            try
            {
                // Инициализация
                TcpClient client = new TcpClient(address, port);
                Byte[] data = Encoding.UTF8.GetBytes(outMessage);
                NetworkStream stream = client.GetStream();
                try
                {
                    // Отправка сообщения
                    stream.Write(data, 0, data.Length);
                    // Получение ответа
                    Byte[] readingData = new Byte[256];
                    String responseData = String.Empty;
                    StringBuilder completeMessage = new StringBuilder();
                    int numberOfBytesRead = 0;
                    do
                    {
                        numberOfBytesRead = stream.Read(readingData, 0, readingData.Length);
                        completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(readingData, 0, numberOfBytesRead));
                    }
                    while (stream.DataAvailable);
                    responseData = completeMessage.ToString();
                    return responseData;
                }
                finally
                {
                    stream.Close();
                    client.Close();
                }
            }
            catch (Exception)
            {
                return ("Ожидание сервера...");
            }

        }

        static bool srch_line_in_mas(string[] mas, string line)
        {
            for (int i = 0; i < mas.Length; i++)
                if (mas[i] == line)
                    return (true);
            return (false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] pages = Exchange("127.0.0.1", 8888, "show_TITLE@").Split(new char[] { '#' });
            if (textBox1.Text != "")
            {
                if (!srch_line_in_mas(pages, textBox1.Text))
                {
                    Exchange("127.0.0.1", 8888, $"add@{textBox1.Text}#{richTextBox1.Text}");
                    MessageBox.Show("добавлено");
                }
                else
                    MessageBox.Show("заметка с таким названием уже есть!!");
            }
            else
                MessageBox.Show("Введите данные!");
        }
    }
}
