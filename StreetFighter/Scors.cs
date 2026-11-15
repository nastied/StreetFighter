using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace StreetFighter
{
    public partial class Scors : Form
    {
        int cnt = 0;
        Image chunLi, ryu;

        int rwin = 0, rlose = 0, cwin = 0, close = 0;
        int hs1 = 0, hs2 = 0;
        public Scors()
        {
            InitializeComponent();
            SetUpForm();

            Get1();
            Get2();
            GetHS1();
            GetHS2();

            GetNr();
        }

        private void GetNr()
        {
            PictureBox[] win1 = new PictureBox[] { n1, n2, n3 };
            PictureBox[] lose1 = new PictureBox[] { n4, n5, n6 };
            PictureBox[] his1 = new PictureBox[] { n7, n8, n9 };
            PictureBox[] win2 = new PictureBox[] { n10, n11, n12 };
            PictureBox[] lose2 = new PictureBox[] { n13, n14, n15 };
            PictureBox[] his2 = new PictureBox[] { n16, n17, n18 };

            PictureBox[][] all = new PictureBox[][] { win1, lose1, his1, win2, lose2, his2};

            int[] values = new int[] { rwin, rlose, hs1, cwin, close, hs2 };

            for (int j=0; j<=5; j++)
            {
                int i = 2;
                int cop = values[j];
                while (i >= 0)
                {
                    int cif = cop % 10;
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\writing\" + cif + ".png";
                    all[j][i].Image = Image.FromFile(path);
                    i--; cop /= 10;
                }
            }
            
        }

        private void Get1()
        {
            string connstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataAtestat.mdf;Integrated Security=True;Connect Timeout=30;";
            SqlConnection conn = new SqlConnection(connstring);
            try
            {
                conn.Open();
                
                string select = "SELECT * FROM AtestatUser WHERE p1 = '"+ 1 +"'";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rwin++;
                    close++;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Get2()
        {
            string connstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataAtestat.mdf;Integrated Security=True;Connect Timeout=30;";
            SqlConnection conn = new SqlConnection(connstring);
            try
            {
                conn.Open();

                string select = "SELECT * FROM AtestatUser WHERE p1 = '" + 0 + "'";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rlose++;
                    cwin++;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void GetHS1()
        {
            string connstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataAtestat.mdf;Integrated Security=True;Connect Timeout=30;";
            SqlConnection conn = new SqlConnection(connstring);
            try
            {
                conn.Open();
                string select = "SELECT MAX(scor1) FROM AtestatUser;";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hs1 = Convert.ToInt32(reader[0]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(hs1.ToString());
        }

        private void GetHS2()
        {
            string connstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataAtestat.mdf;Integrated Security=True;Connect Timeout=30;";
            SqlConnection conn = new SqlConnection(connstring);
            try
            {
                conn.Open();
                string select = "SELECT MAX(scor2) FROM AtestatUser;";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hs2 = Convert.ToInt32(reader[0]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(hs2.ToString());
        }

        private void SetUpForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            chunLi = Image.FromFile("controls\\chunLi.png");
            ryu = Image.FromFile("controls\\ryu.png");
        }

        private void Scors_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ryu, 12, 75, 460, 300);
            e.Graphics.DrawImage(chunLi, 509, 75, 460, 300);
        }

        private void Scors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (cnt % 2 == 0)
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

        private void Scors_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // fixed size border
            this.MaximizeBox = false; // remove maximize button
            this.MinimizeBox = true;  // optional: keep or hide minimize
        }
    }
}
