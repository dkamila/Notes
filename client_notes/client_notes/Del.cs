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
    public partial class Del : Form
    {
        public Del()
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

        private void Del_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Exchange("127.0.0.1", 8888, "show_TITLE@").Split(new char[] { '#' }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Exchange("127.0.0.1", 8888, "dell@" + comboBox1.SelectedItem.ToString());
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Exchange("127.0.0.1", 8888, "show_TITLE@").Split(new char[] { '#' }));
            richTextBox1.Text = "";
            comboBox1.SelectedItem = "";
            MessageBox.Show("удалено");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = Exchange("127.0.0.1", 8888, "show_DATA@" + comboBox1.SelectedItem.ToString());
        }
    }
}
