using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FtpClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            FadList.Items.Clear();

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(tbHost.Text);
            request.Credentials = new NetworkCredential(tbUser.Text, tbPass.Text);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            while (!reader.EndOfStream)
            {
                FadList.Items.Add(reader.ReadLine());
            }
            MessageBox.Show(response.WelcomeMessage);
            reader.Close();
            response.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Всі файли|*.*|png files|*.png|jpg files|*.jpg|gif files|*.gif|bmp files|*.bmp|exe files|*.exe|rar files|*.rar|zip files|*.zip|txt files|*.txt";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox10.Text = openFileDialog1.FileName;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(tbHost.Text + tbUpload.Text + openFileDialog1.SafeFileName);
                request.Credentials = new NetworkCredential(tbUser.Text, tbPass.Text);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                byte[] file = File.ReadAllBytes(textBox10.Text);
                Stream strz = request.GetRequestStream();
                strz.Write(file, 0, file.Length);
                strz.Close();
                strz.Dispose();

                MessageBox.Show(openFileDialog1.SafeFileName + " завантажено");
            }
            else
            {
                MessageBox.Show(openFileDialog1.SafeFileName + " не завантажено");
            }
        }

        private void tbnCreate_Click(object sender, EventArgs e)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(tbHost.Text + tbNewDir.Text);
            request.Credentials = new NetworkCredential(tbUser.Text, tbPass.Text);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            MessageBox.Show("Каталог " + tbNewDir.Text + " створено");
        }
    }
}
