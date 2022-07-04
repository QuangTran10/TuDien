using Gma.System.MouseKeyHook;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;



namespace TuDien
{
    public partial class Main : Form
    {
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(128, 128, 255);

        ManageDictionary manage;
        MySqlConnection conn;

        private IKeyboardMouseEvents m_Events;
        private IKeyboardMouseEvents hotkey_Events;
        string data;
        ControlData ctrl;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4
        }
        public Main()
        {
            System.Diagnostics.Process myProcess = System.Diagnostics.Process.GetCurrentProcess();
            myProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notify.Visible = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.panel1.BackColor = borderColor;
            this.BackColor = borderColor;

            if (File.Exists("hotkey.txt") == false)
            {
                string noidung = "Shift+F";

                File.WriteAllText("hotkey.txt", noidung);
            }

            manage = new ManageDictionary();

            conn = ConnectDB.Connect();

            ctrl = new ControlData(m_Events, data, txtSearch);
        }


        public void resetConnection()
        {
            conn = ConnectDB.Connect();
        }
        public void resetHotKey()
        {
            string[] hot;
            string key;
            this.hotkey_Events = Hook.GlobalEvents();
            if (File.Exists("hotkey.txt"))
            {
                hot = File.ReadAllLines("hotkey.txt");
                key = hot[0];
            }
            else
            {
                key = "Shift+F";
            }
            var subhotkey = Combination.FromString(key);

            Action action = SubcribeHotKey;

            var assignment = new Dictionary<Combination, Action>
            {
                {subhotkey, action}
            };

            this.hotkey_Events.OnCombination(assignment);
        }

        private void SubcribeHotKey()
        {
            string keyword = Clipboard.GetText().Trim();
            if (keyword.Equals(""))
                return;
            try
            {
                conn.Open();
                ArrayList re;
                re = manage.findWord(conn, keyword);

                if (re.Count == 0)
                {
                    Dictionary a = new Dictionary(0, "Không tìm thấy kết quả", "", "", "", "");
                    re.Add(a);
                    Notification noti = new Notification(re, keyword);
                    noti.Show();
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

        private void btnFind_Click(object sender, EventArgs e)
        {
            string content = txtSearch.Text.Trim();
            if (content.Equals(""))
                return;
            ArrayList re = new ArrayList();
            try
            {
                conn.Open();
                re = manage.findWord(conn, content);

                if (re.Count == 0)
                {
                    Dictionary a = new Dictionary(0, "Không tìm thấy kết quả", "", "", "", "");
                    re.Add(a);
                    Notification noti = new Notification(re, content);
                    noti.TopMost = true;
                    noti.Show();
                }
                else
                {
                    Notification noti = new Notification(re, content);
                    noti.TopMost = true;
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

            if (this.WindowState == FormWindowState.Minimized && MousePointerNotOnTaskbar)
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
            if (MessageBox.Show("Bạn có chắc là muốn thoát không?", "IT Dictionary", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            if (MessageBox.Show("Bạn có chắc là muốn thoát không?", "IT Dictionary", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            int id = 0;
            resetHotKey();
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserManual u = new UserManual();
            u.Show();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            Setting s = new Setting();
            s.Button_Clicked += new EventHandler(btnReset_Click);
            s.Button2_Clicked += new EventHandler(btnResetHotKeys_Click);
            s.Show();
        }

        private void càiĐặtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting s = new Setting();
            s.Button_Clicked += new EventHandler(btnReset_Click);
            s.Button2_Clicked += new EventHandler(btnResetHotKeys_Click);
            s.Show();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetConnection();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ctrl.SubscribeGlobal();
            resetHotKey();
        }

        private void btnResetHotKeys_Click(object sender, EventArgs e)
        {
            resetHotKey();
        }

        /*protected override void WndProc(ref Message m)
        {
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
                            Dictionary a = new Dictionary(0, "Không tìm thấy kết quả", "", "", "", "");
                            re.Add(a);
                            Notification noti = new Notification(re, keyword);
                            noti.TopMost = true;
                            noti.Show();
                        }
                        else
                        {
                            Notification noti = new Notification(re, keyword);
                            noti.TopMost = true;
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

            base.WndProc(ref m);
        }*/
    }
}