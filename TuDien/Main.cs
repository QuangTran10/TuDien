using Gma.System.MouseKeyHook;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace TuDien
{
    public partial class Main : Form
    {
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(128, 128, 255);

        ManageDictionary manage;
        MySqlConnection conn;

        private IKeyboardMouseEvents m_Events;
        string data;
        ControlData ctrl;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }
        public Main()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notify.Visible = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.panel1.BackColor = borderColor;
            this.BackColor = borderColor;

            manage = new ManageDictionary();

            conn = ConnectDB.Connect();

            ctrl = new ControlData(m_Events, data, txtSearch);
        }
        public void resetConnection()
        {
            conn = ConnectDB.Connect();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            string content = txtSearch.Text;
            ArrayList re = new ArrayList();
            try
            {
                conn.Open();
                re = manage.findWord(conn, content);
            
                if(re.Count == 0)
                {
                    Message mess = new Message("Không tìm thấy kết quả");
                    mess.Show();
                }
                else{
                    Notification noti = new Notification(re, content);
                    noti.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Kết nối thất bại");
            }
            conn.Close();
        }
        
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            bool MousePointerNotOnTaskbar = Screen.GetWorkingArea(this).Contains(Cursor.Position);

            if(this.WindowState == FormWindowState.Minimized && MousePointerNotOnTaskbar)
            {
                notify.Icon = new Icon("logo.ico");
                this.ShowInTaskbar = false;
                notify.Visible = true;
            }
        }
        private void notify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                notify.Visible = false;
            }
        }
        private void xoáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc là muốn thoát không?", "Nhật - Việt | Dictionary", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void mởToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                notify.Visible = false;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            int id = 0; 
            RegisterHotKey(this.Handle, id, (int)KeyModifier.Shift, Keys.F.GetHashCode());
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void Main_Load(object sender, EventArgs e)
        {
            int id = 0;
            RegisterHotKey(this.Handle, id, (int)KeyModifier.Shift, Keys.F.GetHashCode());
            ctrl.SubscribeGlobal();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       
                int id = m.WParam.ToInt32();
                string keyword = txtSearch.Text.TrimEnd();
                if (keyword.Equals(""))
                    return;
                if (id == 0)
                {
                    try
                    {
                        conn.Open();
                        ArrayList re;
                        re = manage.findWord(conn, keyword); 

                        if (re.Count == 0)
                        {
                            Message mess = new Message("Không tìm thấy kết quả");
                            mess.Show();
                        }
                        else
                        {
                            Notification noti = new Notification(re, keyword);
                            noti.Show();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Kết nối thất bại");
                    }
                    conn.Close();
                }
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserManual u = new UserManual();
            u.Show();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            Setting s = new Setting();
            s.Show();
        }

        private void càiĐặtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting s = new Setting();
            s.Show();
        }
    }
}