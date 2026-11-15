using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace StreetFighter
{
    public partial class Controls : Form
    {
        int cnt = 0;
        Image chunLi, ryu;
        public Controls()
        {
            InitializeComponent();
            SetUpForm();
        }

        private void SetUpForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            chunLi = Image.FromFile("controls\\chunLi.png");
            ryu = Image.FromFile("controls\\ryu.png");
        }

        private void Controls_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ryu, 12, 75, 460, 300);
            e.Graphics.DrawImage(chunLi, 509, 75, 460, 300);
            //e.Graphics.DrawImage(chunLi, new Point(500, 75));
            //e.Graphics.DrawImage(ryu, new Point(100, 75));
        }

        private void Controls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (cnt%2 == 0)
                {
                    panel1.Visible = true;
                    panel1.BringToFront();
                }
                else
                {
                    panel1.Visible = false;
                }
                cnt++;
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            cnt++;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
            StartForm startForm = new StartForm();
            startForm.Show();
        }

        private void pictureBox7_MouseHover(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.BackColor = Color.Silver;
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Controls_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // fixed size border
            this.MaximizeBox = false; // remove maximize button
            this.MinimizeBox = true;  // optional: keep or hide minimize
        }
    }
}
