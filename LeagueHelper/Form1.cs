using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LeagueHelper
{
    public partial class LeagueHelper : Form
    {
        private Timer timer;
        private System.Media.SoundPlayer player;
        private int oldRed;

        private int recX;
        private int recY;
        private int recW;
        private int recH;

        private int accuracy;


        public LeagueHelper()
        {
            string fileName = "Settings\\properties.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            int interval = Convert.ToInt32(lines[1]);
            this.recX = Convert.ToInt32(lines[4]);
            this.recY = Convert.ToInt32(lines[5]);
            this.recW = Convert.ToInt32(lines[6]);
            this.recH = Convert.ToInt32(lines[7]);

            this.accuracy = Convert.ToInt32(lines[10]);



            this.timer = new Timer();
            this.timer.Interval = interval;
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(OnTimerEvent);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.player = new System.Media.SoundPlayer("Sounds\\alert.wav");

            this.oldRed = 0;

            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //player.Play();
        }

        public void OnTimerEvent(object source, EventArgs e)
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = new Size(recW, recH);
            Bitmap memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(recX, recY, 0, 0, s);

            this.oldRed = checkData(memoryImage, this.oldRed);

            pictureBox1.Image = memoryImage;
        }

        private int checkData(Bitmap memoryImage, int oldRed)
        {
            int championMin = this.accuracy;

            int newRed = getRedContent(memoryImage);

            if ((newRed - oldRed) > championMin)
            {
                alert();
            }
            return newRed;
        }

        private void alert()
        {
            player.Play();
        }

        private int getRedContent(Bitmap image)
        {
            Color pixel;
            int redCount = 0;
            int redMin = 234;
            int greMax = 20;
            int bluMax = 20;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    pixel = image.GetPixel(i, j);
                    if ((pixel.R > redMin) &&
                        (pixel.G < greMax) &&
                        (pixel.B < bluMax))
                    {
                        redCount++;
                    }
                }
            }
            return redCount;
        }
    }
}
