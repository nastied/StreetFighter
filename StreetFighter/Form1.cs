using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace StreetFighter
{
    public partial class Form1 : Form
    {
        Image ryuStand, ryuForwards, ryuBackwards, ryuPunch1, ryuPunch2, ryuFireballPose;
        Image chunStand, chunForwards, chunBackwards, chunPunch1, chunPunch2, chunKicks;
        Image fireballAnim;

        Image player;
        Image background;
        //Image drum;
        Image fireball;

        int playerX = 0;
        int playerY = 300;

        //int drumX = 450;
        //int drumY = 335;

        int fireballX, fireballY;

        //int drumMoveTime = 0;
        int actionStrenght = 0;
        int endFrame = 0;
        int backgroundPosition = 0;
        int totalFrame = 0;
        int bg_number = 0;

        float num;

        bool goLeft, goRight;
        bool directionPressed;
        bool playingAction = false;
        bool shotFireBall = false;

        List<string> background_images = new List<string>();

        //+player2
        Image player2;
        Image portraitChunLi;
        Image portraitRyu;
        Image vs;

        int player2X = 650;
        int player2Y = 320;

        int playerMoveTime = 0, player2MoveTime = 0;
        int actionStrenght2 = 0;
        int endFrame2 = 0;
        int totalFrame2 = 0;

        float num2;

        bool goLeft2, goRight2;
        bool directionPressed2;
        bool playingAction2;

        int scor1 = 100, scor2 = 100, cnt = 0;
        bool endGame = false;
        bool pause = false;

        public Form1()
        {
            InitializeComponent();
            SetUpForm();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!endGame)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (cnt % 2 == 0)
                    {
                        timer1.Stop();
                        timer2.Stop();
                        panel1.Visible = true;
                        pause = true;
                    }
                    else
                    {
                        timer1.Start();
                        timer2.Start();
                        panel1.Visible = false;
                        pause = false;
                    }
                    cnt++;
                }

                if (!pause)
                {
                    if (e.KeyCode == Keys.A && !directionPressed)
                    {
                        MovePlayerAnimation("left");
                    }
                    if (e.KeyCode == Keys.D && !directionPressed)
                    {
                        MovePlayerAnimation("right");
                    }

                    if (e.KeyCode == Keys.J && !directionPressed2)
                    {
                        MovePlayerAnimation2("left");
                    }
                    if (e.KeyCode == Keys.L && !directionPressed2)
                    {
                        MovePlayerAnimation2("right");
                    }
                }
            }
            else
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!endGame && !pause)
            {
                if (e.KeyCode == Keys.B)
                {
                    if (bg_number < background_images.Count - 1)
                    {
                        bg_number++;
                    }
                    else
                    {
                        bg_number = 0;
                    }

                    background = Image.FromFile(background_images[bg_number]);
                    backgroundPosition = 0;
                    playerMoveTime = 0; player2MoveTime = 0;
                }

                //plaayer1
                if (e.KeyCode == Keys.D || e.KeyCode == Keys.A)
                {
                    goLeft = false;
                    goRight = false;
                    directionPressed = false;
                    ResetPlayer();
                }

                if (e.KeyCode == Keys.Q && !playingAction && !goLeft && !goRight)
                {
                    SetPlayerAction(ryuPunch2, 2);
                }

                if (e.KeyCode == Keys.E && !playingAction && !goLeft && !goRight)
                {
                    SetPlayerAction(ryuPunch1, 5);
                }

                if (e.KeyCode == Keys.F && !playingAction && !goLeft && !goRight && !shotFireBall)
                {
                    SetPlayerAction(ryuFireballPose, 15);
                }

                //player2
                if (e.KeyCode == Keys.L || e.KeyCode == Keys.J)
                {
                    goLeft2 = false;
                    goRight2 = false;
                    directionPressed2 = false;
                    ResetPlayer2();
                }

                if (e.KeyCode == Keys.U && !playingAction2 && !goLeft2 && !goRight2)
                {
                    SetPlayerAction2(chunPunch1, 2);
                }

                if (e.KeyCode == Keys.O && !playingAction2 && !goLeft2 && !goRight2)
                {
                    SetPlayerAction2(chunPunch2, 5);
                }

                if (e.KeyCode == Keys.H && !playingAction2 && !goLeft2 && !goRight2)
                {
                    SetPlayerAction2(chunKicks, 15);
                }
            }
        }

        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(background, new Point(backgroundPosition, 0));
            e.Graphics.DrawImage(player, new Point(playerX, playerY));
            e.Graphics.DrawImage(player2, new Point(player2X, player2Y));
            //e.Graphics.DrawImage(drum, new Point(drumX, drumY));
            e.Graphics.DrawImage(vs, 315, 0, 100, 70);
            e.Graphics.DrawImage(portraitRyu, 0, 0, 50, 55);
            e.Graphics.DrawImage(portraitChunLi, this.ClientSize.Width - 40, 0, 50, 55);

            if (shotFireBall)
            {
                e.Graphics.DrawImage(fireball, new Point(fireballX, fireballY));
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            this.Invalidate();
            ImageAnimator.UpdateFrames();
            MovePlayerAndBackground();
            CheckPunchHit();
            if (playingAction)
            {
                if (num < totalFrame)
                {
                    num += 0.5f;
                }
            }
            if (num == totalFrame)
            {
                ResetPlayer();
            }
            if (shotFireBall)
            {
                fireballX += 10;
                CheckFireballHit();
            }
            if (fireballX > this.ClientSize.Width)
            {
                shotFireBall = false;
            }
            if (!shotFireBall && num > endFrame && player2MoveTime == 0 && actionStrenght == 15)
            {
                ShootFireball();
            }
            if (player2MoveTime > 0)
            {
                player2MoveTime--;
                player2X += 10;
            }
            else
            {
                player2MoveTime = 0;
            }
            if (player2X + actionStrenght > this.ClientSize.Width - player2.Width)
            {
                player2MoveTime = 0;
                player2X = this.ClientSize.Width - player2.Width;
            }
        }

        private void SetUpForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            background_images = Directory.GetFiles(Path.Combine(baseDir, "street_fighter_images", "background"), "*.jpg").ToList();
            background = Image.FromFile(background_images[bg_number]);

            ryuStand = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "standing.gif"));
            ryuForwards = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "forwards.gif"));
            ryuBackwards = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "backwards.gif"));
            ryuPunch1 = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "punch1.gif"));
            ryuPunch2 = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "punch2.gif"));
            ryuFireballPose = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "fireball.gif"));
            fireballAnim = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "FireBallFinal.gif"));

            chunStand = Image.FromFile(Path.Combine(baseDir, "chunLi2", "stand.gif"));
            chunForwards = Image.FromFile(Path.Combine(baseDir, "chunLi3", "forwards.gif"));
            chunBackwards = Image.FromFile(Path.Combine(baseDir, "chunLi3", "backwards.gif"));
            chunPunch1 = Image.FromFile(Path.Combine(baseDir, "chunLi3", "punch1.gif"));
            chunPunch2 = Image.FromFile(Path.Combine(baseDir, "chunLi3", "punch2.gif"));
            chunKicks = Image.FromFile(Path.Combine(baseDir, "chunLi3", "kicks.gif"));

            portraitChunLi = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "portraitChunLi.png"));
            portraitRyu = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "portraitRyu.png"));
            vs = Image.FromFile(Path.Combine(baseDir, "street_fighter_images", "vsLogo.png"));

            player = ryuStand;
            player2 = chunStand;

            SetUpAnimation();
            SetUpAnimation2();
        }

        private void SetUpAnimation()
        {
            ImageAnimator.Animate(player, this.OnFrameChangedHandler);
            FrameDimension dimensions = new FrameDimension(player.FrameDimensionsList[0]);
            totalFrame = player.GetFrameCount(dimensions);
            endFrame = totalFrame - 3;
        }

        private void SetUpAnimation2()
        {
            ImageAnimator.Animate(player2, this.OnFrameChangedHandler2);
            FrameDimension dimensions = new FrameDimension(player2.FrameDimensionsList[0]);
            totalFrame2 = player2.GetFrameCount(dimensions);
            endFrame2 = totalFrame2 - 2;
        }

        private void OnFrameChangedHandler2(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void OnFrameChangedHandler(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void MovePlayerAndBackground()
        {
            if (goLeft)
            {
                if (playerX > 0)
                {
                    playerX -= 6;
                }

                if (backgroundPosition < 0 && playerX < 100 && goLeft2)
                {
                    backgroundPosition += 5;
                }
                else if (backgroundPosition < 0 && playerX < 100 && player2X < this.ClientSize.Width - player2.Width)
                {
                    backgroundPosition += 5;
                    player2X += 5;
                }
            }
            if (goRight)
            {
                if (playerX + player.Width < this.ClientSize.Width)
                {
                    playerX += 6;
                }

                if (backgroundPosition + background.Width > this.ClientSize.Width + 5 && playerX > 614 && goRight2)
                {
                    backgroundPosition -= 5;
                }
                else if (backgroundPosition + background.Width > this.ClientSize.Width + 5 && playerX > 614 && player2X > 10)
                {
                    backgroundPosition -= 5;
                    player2X -= 5;
                }
            }
        }

        private void MovePlayerAndBackground2()
        {
            if (goLeft2)
            {
                if (player2X > 0)
                {
                    player2X -= 6;
                }

                if (backgroundPosition < 0 && player2X < 100 && goLeft)
                {
                    backgroundPosition += 5;
                }
                else if (backgroundPosition < 0 && player2X < 100 && playerX < this.ClientSize.Width - player.Width)
                {
                    backgroundPosition += 5;
                    playerX += 5;
                }
            }
            if (goRight2)
            {
                if (player2X + player2.Width < this.ClientSize.Width)
                {
                    player2X += 6;
                }

                if (backgroundPosition + background.Width > this.ClientSize.Width + 5 && player2X > 614 && goRight)
                {
                    backgroundPosition -= 5;
                }
                else if (backgroundPosition + background.Width > this.ClientSize.Width + 5 && player2X > 614 && playerX > 10)
                {
                    backgroundPosition -= 5;
                    playerX -= 5;
                }
            }
        }

        private void MovePlayerAnimation(string direction)
        {
            if (direction == "left")
            {
                goLeft = true;
                player = ryuBackwards;
            }
            if (direction == "right")
            {
                goRight = true;
                player = ryuForwards;
            }

            directionPressed = true;
            playingAction = false;
            SetUpAnimation();
        }

        private void MovePlayerAnimation2(string direction)
        {
            if (direction == "left")
            {
                goLeft2 = true;
                player2 = chunForwards;
            }
            if (direction == "right")
            {
                goRight2 = true;
                player2 = chunBackwards;
            }

            directionPressed2 = true;
            playingAction2 = false;
            SetUpAnimation2();
        }

        private void ResetPlayer()
        {
            player = ryuStand;
            SetUpAnimation();
            num = 0;
            playingAction = false;
        }

        private void ResetPlayer2()
        {
            player2 = chunStand;
            SetUpAnimation2();
            num2 = 0;
            playingAction2 = false;
        }

        private void SetPlayerAction(Image animation, int strenght)
        {
            player = animation;
            actionStrenght = strenght;
            SetUpAnimation();
            playingAction = true;
        }

        private void SetPlayerAction2(Image animation1, int strenght1)
        {
            player2 = animation1;
            actionStrenght2 = strenght1;
            SetUpAnimation2();
            playingAction2 = true;
        }

        private void GameTimer2Event(object sender, EventArgs e)
        {
            this.Invalidate();
            ImageAnimator.UpdateFrames();
            MovePlayerAndBackground2();

            CheckPunchHit2();

            if (playingAction2)
            {
                if (num2 < totalFrame2)
                {
                    num2 += 0.5f;
                }
            }
            if (num2 == totalFrame2)
            {
                ResetPlayer2();
            }

            if (playerMoveTime > 0 && playerX - actionStrenght2 > 0)
            {
                playerMoveTime--;
                playerX -= 10;
            }
            else if (playerMoveTime < 0 && playerX - actionStrenght2 < 0)
            {
                playerMoveTime = 0;
                playerX = 10;
            }
            else
            {
                playerMoveTime = 0;
            }
        }

        private void ShootFireball()
        {
            fireball = fireballAnim;
            ImageAnimator.Animate(fireball, this.OnFrameChangedHandler);
            fireballX = playerX + player.Width - 50;
            fireballY = playerY - 33;
            shotFireBall = true;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
            StartForm startForm = new StartForm();
            startForm.Show();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            cnt++;
            panel1.Visible = false;
            timer1.Start();
            timer2.Start();
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Controls controls = new Controls();
            controls.Show();
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void CheckPunchHit()
        {
            bool collision = DetectCollision(playerX, playerY, player.Width, player.Height, player2X, player2Y, player2.Width, player2.Height);
            if (collision && playingAction && num > endFrame && actionStrenght != 15)
            {
                player2MoveTime = actionStrenght;
                if (progressBar2.Value - actionStrenght > 0)
                {
                    progressBar2.Value -= actionStrenght;
                    scor2 -= actionStrenght;
                }
                else
                {
                    progressBar2.Value = 0;
                    timer1.Stop();
                    timer2.Stop();
                    panel1.Show();
                    ry.Visible = true;
                    pictureBox7.Visible = false;
                    pictureBox7.Enabled = false;
                    endGame = true;

                    string connstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataAtestat.mdf;Integrated Security=True;Connect Timeout=30;";

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connstring))
                        {
                            conn.Open();
                            string insert = String.Format("INSERT INTO AtestatUser (scor1, scor2, p1, p2) VALUES ({0}, {1}, {2}, {3});", scor1, scor2, 1, 0);
                            using (SqlCommand cmd = new SqlCommand(insert, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }

        private void CheckPunchHit2()
        {
            bool collision = DetectCollision(player2X, player2Y, player2.Width, player2.Height, playerX, playerY, player.Width, player.Height);
            if (collision && playingAction2 && num2 > endFrame2)
            {
                playerMoveTime = actionStrenght2;
                if (progressBar1.Value - actionStrenght2 > 0)
                {
                    progressBar1.Value -= actionStrenght2;
                    scor1 -= actionStrenght2;
                }
                else
                {
                    progressBar1.Value = 0;
                    timer1.Stop();
                    timer2.Stop();
                    panel1.Visible = true;
                    chu.Visible = true;
                    pictureBox7.Visible = false;
                    pictureBox7.Enabled = false;
                    endGame = true;

                    string connstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataAtestat.mdf;Integrated Security=True;Connect Timeout=30;";

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connstring))
                        {
                            conn.Open();
                            string insert = String.Format("INSERT INTO AtestatUser (scor1, scor2, p1, p2) VALUES ({0}, {1}, {2}, {3});", scor1, scor2, 0, 1);
                            using (SqlCommand cmd = new SqlCommand(insert, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void CheckFireballHit()
        {
            bool collision = DetectCollision(fireballX, fireballY, fireball.Width, fireball.Height, player2X, player2Y, player2.Width, player2.Height);
            if (collision)
            {
                if (progressBar2.Value + actionStrenght > 0)
                {
                    player2MoveTime = actionStrenght;
                    progressBar2.Value -= actionStrenght;
                    shotFireBall = false;
                }
                else
                {
                    player2MoveTime = actionStrenght;
                    progressBar2.Value = 0;
                    shotFireBall = false;
                }
            }
        }

        private bool DetectCollision(int object1X, int object1Y, int object1Width, int object1Height, int object2X, int object2Y, int object2Width, int object2Height)
        {
            if (object1X + object1Width <= object2X || object1X >= object2X + object2Width || object1Y + object1Height <= object2Y || object1Y >= object2Y + object2Height)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}