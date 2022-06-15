using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TuDien
{
    public partial class Notification : Form
    {
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(128, 128, 255);
        private ArrayList dic;

        public string convert(string data)
        {
            var windows1252 = Encoding.GetEncoding(1252);
            var utf8Bytes = windows1252.GetBytes(data);
            var correct = Encoding.UTF8.GetString(utf8Bytes);
            return correct;
        }

        public Notification()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.panel2.BackColor = borderColor;
            this.BackColor = borderColor;
        }

        public Notification(ArrayList a, string keywords) : this()
        {
            this.lblKey.Text = keywords;
            var listItems = new ListItem[a.Count+1];
            flowLayoutPanel1.Controls.Clear();
            int i = 0;
            dic = a;
            keywords = keywords.Trim();
            string replacekey = "<mark>" + keywords + "</mark>";
            foreach (Dictionary item in dic)
            {

                listItems[i] = new ListItem();

                string japanese_trans = convert(item.Tm_japanese_translate);
                string japan_hiragana = convert(item.Tm_japanese_hiragana);
                string vietnamese = convert(item.Tm_vietnamese_tranlate);
                string english = convert(item.Tm_english_tranlate);
                string example = convert(item.Tm_example);

                japanese_trans = japanese_trans.Replace(keywords, replacekey, StringComparison.OrdinalIgnoreCase);
                japan_hiragana = japan_hiragana.Replace(keywords, replacekey, StringComparison.OrdinalIgnoreCase);
                vietnamese = vietnamese.Replace(keywords, replacekey, StringComparison.OrdinalIgnoreCase);
                english = english.Replace(keywords, replacekey, StringComparison.OrdinalIgnoreCase);
                if(example == "" )
                {
                    example = "";
                }
                else
                {
                    example = "<br><b>Ví dụ:</b><br>" + example.Replace(keywords, replacekey, StringComparison.OrdinalIgnoreCase).Replace("\n","<br>");
                }


                listItems[i].Content = "<html>" +
                "<style> mark{background-color:#CE96F8;} p{padding : 0;margin: 0;line-height:20px;font-size:12px;text-align:justify }</style>" +
                "<body>" +
                "<p style='font-size: 10px; line-height:0'>" + japan_hiragana + "</p>" +
                "<p style= 'color:red; font-size:14px'>" + japanese_trans + "</p>" +
                "<p>" + japan_hiragana + "</p>" +
                "<p>" + vietnamese + "</p>" +
                "<p>" + english + example + "</p></body></html>";

                flowLayoutPanel1.Controls.Add(listItems[i]);
                i++;
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
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

        private void Notification_Load(object sender, EventArgs e)
        {

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblKey.Text.Trim());
        }
    }
}
