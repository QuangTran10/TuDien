using MySql.Data.MySqlClient;
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

        private void loadConnectionString()
        {
            string[] a = File.ReadAllLines("dic.txt");
            this.txtIP.Text = a[0];
            this.txtPort.Text = a[1];
            this.txtUser.Text = a[2];
            this.txtPass.Text = a[3];
            this.txtDatabase.Text = a[4];
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text;
            string port = txtPort.Text;
            string user = txtUser.Text;
            string pass = txtPass.Text;
            string database = txtDatabase.Text;

            string noidung = ip + "\r\n" + port + "\r\n" + user + "\r\n" + pass + "\r\n" + database;
            MySqlConnection conn = Connection.getDBConnection(ip, Convert.ToInt32(port), database, user, pass);
            try
            {
                conn.Open();
            }
            catch(Exception)
            {
                MessageBox.Show("Kết nối thất bại");
                loadConnectionString();
                return;
            }
            File.WriteAllText("dic.txt", noidung);
            resetText();
            MessageBox.Show("Cập nhật thành công");

            loadConnectionString();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            if (File.Exists("dic.txt"))
            {
                loadConnectionString();
            }
        }
    }
}
