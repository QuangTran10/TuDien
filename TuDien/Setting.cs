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
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(128, 128, 255);

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public event EventHandler Button_Clicked;

        public event EventHandler Button2_Clicked;

        public Setting()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.panel2.BackColor = borderColor;
            this.BackColor = borderColor;
        }

        private void check(int keycode)
        {
            switch (keycode)
            {
                case 1:
                    this.ckAlt.Checked = true;
                    break;
                case 2:
                    this.ckCtrl.Checked = true;
                    break;
                case 3:
                    this.ckAlt.Checked = true;
                    this.ckCtrl.Checked = true;
                    break;
                case 4:
                    this.ckShift.Checked = true;
                    break;
                case 5:
                    this.ckAlt.Checked = true;
                    this.ckShift.Checked = true;
                    break;
                case 6:
                    this.ckCtrl.Checked = true;
                    this.ckShift.Checked = true;
                    break;
                case 7:
                    this.ckCtrl.Checked = true;
                    this.ckShift.Checked = true;
                    this.ckAlt.Checked = true;
                    break;
                default:
                    this.ckCtrl.Checked = false;
                    this.ckShift.Checked = false;
                    this.ckAlt.Checked = false;
                    break;
            }
        }

        private string printcheck(int keycode)
        {
            string result = "";
            switch (keycode)
            {
                case 1:
                    result = "Alt";
                    break;
                case 2:
                    result = "Ctrl";
                    break;
                case 3:
                    result = "Alt + Ctrl";
                    break;
                case 4:
                    result = "Shift";
                    break;
                case 5:
                    result = "Alt + Shift";
                    break;
                case 6:
                    result = "Ctrl + Shift";
                    break;
                case 7:
                    result = "Ctrl + Shift + Alt";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
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

        private void loadHotKey()
        {
            if (File.Exists("hotkey.txt"))
            {
                string[] b = File.ReadAllLines("hotkey.txt");
                int modi = Convert.ToInt32(b[0]);
                check(modi);

                this.txtKey.Text = KeyItem.find(Convert.ToInt32(b[1]));
                string shortcut = printcheck(modi) + " + " + KeyItem.find(Convert.ToInt32(b[1]));
                lblNotice.Text = shortcut;
            }
            else
            {
                string noidung = 4 + "\r\n" + 70 + "\r\n" + 66;

                File.WriteAllText("hotkey.txt", noidung);
            } 
            
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text;
            string port = txtPort.Text;
            string user = txtUser.Text;
            string pass = txtPass.Text;
            string database = txtDatabase.Text;

            string noidung = ip + "\r\n" + port + "\r\n" + user + "\r\n" + pass + "\r\n" + database;
            
            File.WriteAllText("dic.txt", noidung);
            resetText();
            if (this.Button_Clicked != null)
                this.Button_Clicked(sender, e);

            MessageBox.Show("Cập nhật thành công");

            loadConnectionString();
        }
        private void Setting_Load(object sender, EventArgs e)
        {
            if (File.Exists("dic.txt"))
            {
                loadConnectionString();
            }
            loadHotKey();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int modifier = 0; 
            if (ckCtrl.Checked)
            {
                modifier += 2;
            }
            if (ckShift.Checked)
            {
                modifier += 4;
            }
            if (ckAlt.Checked)
            {
                modifier += 1;
            }

            if(modifier != 0)
            {
                int key;

                if (txtKey.Text.Equals(""))
                {
                    key = Keys.F.GetHashCode();
                }
                else
                {
                    key = KeyItem.findCode(txtKey.Text.Trim());
                }

                string noidung = modifier + "\r\n" + key + "\r\n";

                File.WriteAllText("hotkey.txt", noidung);
                loadHotKey();
                MessageBox.Show("Cập nhật thành công");

                if (this.Button2_Clicked != null)
                    this.Button2_Clicked(sender, e);
            }
            else{
                MessageBox.Show("Chưa chọn modifiers");
                return;
            }
        }
    }
}
