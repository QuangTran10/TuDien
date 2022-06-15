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
                string modi = "";
                if (Convert.ToInt32(b[0]) == 1)
                {
                    this.rdAlt.Checked = true;
                    modi = "Alt";
                }else if (Convert.ToInt32(b[0]) == 2)
                {
                    this.rdCtrl.Checked = true;
                    modi = "Ctrl";
                }
                else
                {
                    this.rdShift.Checked = true;
                    modi = "Shift";
                }
                this.cmbKeys.SelectedIndex = Convert.ToInt32(b[2]);
                string shortcut = modi + " + " + KeyItem.find(Convert.ToInt32(b[1]));
                lblNotice.Text = shortcut;
            }
            else
            {
                string noidung = 4 + "\r\n" + 70 + "\r\n" + 66;

                File.WriteAllText("hotkey.txt", noidung);
                this.rdShift.Checked = true;
                this.cmbKeys.SelectedIndex = 66;
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

            cmbKeys.Items.Clear();
            cmbKeys.DisplayMember = nameof(KeyItem.Name);
            cmbKeys.ValueMember = nameof(KeyItem.KeyCode);
            cmbKeys.DataSource = KeyItem.List;
            loadHotKey();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int modifier = 4; //Mac dinh la nut Shift
            if (rdCtrl.Checked)
            {
                modifier = 2;
            }
            if (rdShift.Checked)
            {
                modifier = 4;
            }
            if (rdAlt.Checked)
            {
                modifier = 1;
            }

            int key=Keys.F.GetHashCode();

            key = (int)cmbKeys.SelectedValue;

            string noidung = modifier + "\r\n" + key + "\r\n" + cmbKeys.SelectedIndex;

            File.WriteAllText("hotkey.txt", noidung);
            loadHotKey();
            MessageBox.Show("Cập nhật thành công");

            if (this.Button2_Clicked != null)
                this.Button2_Clicked(sender, e);

        }
    }
}
