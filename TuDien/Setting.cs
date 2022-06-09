using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TuDien
{
    public partial class Setting : Form
    {
        private int borderRadius = 20;
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(128, 128, 255);

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private string ip;
        private int port;
        private string username;
        private string password;
        private string database;

        public string Ip 
        { 
            get => ip;
            set
            {
                ip = value;
                this.txtIP.Text = ip;
            } 
        }
        public int Port 
        { 
            get => port;
            set 
            { 
                port = value;
                this.txtPort.Text = port.ToString();
            } 
        }
        public string Username 
        { 
            get => username;
            set
            {
                username = value;
                this.txtUser.Text = username;
            } 
        }
        public string Password 
        { 
            get => password;
            set
            {
                password = value;
                this.txtPass.Text = password;
            }
        }
        public string Database 
        { 
            get => database;
            set
            {
                database = value;
                this.txtDatabase.Text = database;
            } 
        }

        public Setting()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.panel2.BackColor = borderColor;
            this.BackColor = borderColor;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resetText()
        {
            this.txtIP.Text = "";
            this.txtPort.Text = "";
            this.txtUser.Text = "";
            this.txtPass.Text = "";
            this.txtDatabase.Text = "";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string ip = Md5.Encrypt(txtIP.Text);
            string port = Md5.Encrypt(txtPort.Text);
            string user = Md5.Encrypt(txtUser.Text);
            string pass = Md5.Encrypt(txtPass.Text);
            string database = Md5.Encrypt(txtDatabase.Text);

            string noidung = ip + "\r\n" + port + "\r\n" + user + "\r\n" + pass + "\r\n" + database;

            File.WriteAllText("dic.txt", noidung);
            resetText();
            MessageBox.Show("Cập nhật thành công");

            string[] a = File.ReadAllLines("dic.txt");
            this.txtIP.Text = Md5.Decrypt(a[0]);
            this.txtPort.Text = Md5.Decrypt(a[1]);
            this.txtUser.Text = Md5.Decrypt(a[2]);
            this.txtPass.Text = Md5.Decrypt(a[3]);
            this.txtDatabase.Text = Md5.Decrypt(a[4]);
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            if (File.Exists("dic.txt"))
            {
                string[] a = File.ReadAllLines("dic.txt");
                this.txtIP.Text = Md5.Decrypt(a[0]);
                this.txtPort.Text = Md5.Decrypt(a[1]);
                this.txtUser.Text = Md5.Decrypt(a[2]);
                this.txtPass.Text = Md5.Decrypt(a[3]);
                this.txtDatabase.Text = Md5.Decrypt(a[4]);
            }
        }
    }
}
