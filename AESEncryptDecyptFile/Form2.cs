using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AESEncryptDecyptFile
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cho trường Họ Tên!", "Thông báo");
            }
            if (txtEmail.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cho trường Email!", "Thông báo");
            }
            else if (txtName.Text != "" || txtEmail.Text != "")
            {
                File.WriteAllText("configName.cfg", txtName.Text);
                File.WriteAllText("configEmail.cfg", txtEmail.Text);
                AESEncryptDecrypt.FileEncrypt("configName.cfg","cVEEdRMrnJ");
                AESEncryptDecrypt.FileEncrypt("configEmail.cfg","Lm6NzzftiB");
                Form1 frmMain = new Form1();
                frmMain.Show();
                File.Delete("configName.cfg");
                File.Delete("configEmail.cfg");
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
