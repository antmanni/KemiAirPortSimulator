using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KemiAirportSimulator
{

    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer path0, path1, takeoffInterval, landingInterval, landingQue, waitingQue;
        int takeoffInt, landingInt, takeoffQue, landingPlane, timeLeft, points;
        string picture0, picture1;
        bool lane1Token, lane2Token;
        Random rand;

        public Form1()
        {
            //Initializing timers
            path0 = new System.Windows.Forms.Timer();
            path1 = new System.Windows.Forms.Timer();
            takeoffInterval = new System.Windows.Forms.Timer();
            landingInterval = new System.Windows.Forms.Timer();
            landingQue = new System.Windows.Forms.Timer();
            waitingQue = new System.Windows.Forms.Timer();
            landingQue.Interval = 10000;
            waitingQue.Interval = 20000;
            path0.Interval = 2000;
            path1.Interval = 2000;
            path0.Stop();
            path1.Stop();
            path0.Tick += new EventHandler(ChangePicture1);
            path1.Tick += new EventHandler(ChangePicture2);
            waitingQue.Tick += new EventHandler(TiredOfWaiting);
            takeoffInterval.Start();
            landingInterval.Start();

            //Used for checking if lane is used 
            lane1Token = false;
            lane2Token = false;

            rand = new Random();
            picture0 = "takeoff0";
            picture1 = "takeoff0";
            takeoffQue = 0;
            points = 0;
            //DisableLandingButtons();
            InitializeComponent();
            richTextBox1.AppendText("Welcome to the Kemi Airport Simulator. Press New Game to begin.\r\n");
            richTextBox2.AppendText(String.Format("Takeoff que: {0}", takeoffQue));
            pictureBox1.Image = Properties.Resources.takeoff0;
            pictureBox2.Image = Properties.Resources.takeoff0;
        }


        //Updates the score on the scoreboards
        private void ChangePoints()
        {
            textBox2.Clear();
            textBox2.AppendText("Points: " + points);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //Updates the image on lane 1
        private void ChangePicture1(object sender, EventArgs e)
        {
            Debug.Assert(picture0 != null, "Error on ChangePicture1, picture0 null");

            switch (picture0)
            {
                case "takeoff1":
                    {                           
                        pictureBox1.Image = Properties.Resources.takeoff2;
                        picture0 = "takeoff2";
                        break;
                    }
                case "takeoff2":
                    {
                        pictureBox1.Image = Properties.Resources.takeoff3;
                        picture0 = "takeoff3";
                        break;
                    }
                case "takeoff3":
                    {
                        pictureBox1.Image = Properties.Resources.takeoff0;
                        picture0 = "takeoff0";
                        break;
                    }

                case "landing0":
                    {
                        pictureBox1.Image = Properties.Resources.landing2;
                        picture0 = "landing2";
                        break;
                    }
                case "landing2":
                    {
                        pictureBox1.Image = Properties.Resources.landing3;
                        picture0 = "landing3";
                        break;
                    }
                case "landing3":
                    {
                        pictureBox1.Image = Properties.Resources.takeoff0;
                        picture0 = "takeoff0";
                        break;
                    }
                //Update necessary information when plane has left the lane    
                case "takeoff0":
                    {
                        points++;
                        ChangePoints();
                        button1.Enabled = true;
                        lane1Token = false;
                        path0.Stop();
                        if (!landingInterval.Enabled)
                        {
                            landingInterval.Start();
                        }
                        Debug.Assert(landingInterval.Enabled, "Error on ChangePicture1, landingInterval false");
                        break;
                    }
                default:
                    {
                        Debug.Assert(picture0 == null, "Error on ChangePicture1, default case");
                        break;
                    }
            }
        }

        //Updates the image on lane 2
        private void ChangePicture2(object sender, EventArgs e)
        {
            Debug.Assert(picture1 != null, "Error on ChangePicture2, picture1 null");

            switch (picture1)
            {
                case "takeoff1":
                    {
                        pictureBox2.Image = Properties.Resources.takeoff2;
                        picture1 = "takeoff2";
                        break;
                    }
                case "takeoff2":
                    {
                        pictureBox2.Image = Properties.Resources.takeoff3;
                        picture1 = "takeoff3";
                        break;
                    }
                case "takeoff3":
                    {
                        pictureBox2.Image = Properties.Resources.takeoff0;
                        picture1 = "takeoff0";
                        break;
                    }
                case "landing0":
                    {
                        pictureBox2.Image = Properties.Resources.landing2;
                        picture1 = "landing2";
                        break;
                    }
                case "landing2":
                    {
                        pictureBox2.Image = Properties.Resources.landing3;
                        picture1 = "landing3";
                        break;
                    }
                case "landing3":
                    {
                        pictureBox2.Image = Properties.Resources.takeoff0;
                        picture1 = "takeoff0";
                        break;
                    }

                case "takeoff0":
                    {
                        points++;
                        ChangePoints();
                        button2.Enabled = true;
                        lane2Token = false;
                        path1.Stop();
                        if (!landingInterval.Enabled)
                        {
                            landingInterval.Start();
                        }
                        Debug.Assert(landingInterval.Enabled, "Error on ChangePicture2, landingInterval false");

                        break;
                    }
                default:
                    {
                        Debug.Assert(picture1 == null, "Error on ChangePicture2, default case");
                        break;
                    }
            }
        }

        //Displays instruction
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kemi Airport Simulator\r\n Quide the incoming traffic to lanes.\r\n"
                + "Get point if the landing or takeoff is succesful. Get minus points if takeoff"
                + "que hasn't moved or by cancelling the landing.\r\n Buttons: \r\n"
                + "Takeoff: quides the plane from the takeoff que to lane.\r\n" 
                + "Landing: quides the landing plane to the plane\r\n"
                + "Cycle: Landing plane does a 360 in air, thus buying time for 10 seconds"
                + "Cancels the landing");
        }

        //Takeoff 1
        private void button1_Click(object sender, EventArgs e)
        {
            Debug.Assert(takeoffQue >= 0, "Error on button1_Click, takeoffQue negative number.");
            if (takeoffQue > 0 && !lane1Token)
            {
                Debug.Assert(!lane1Token, "Error on button1_click, lane1Token true");
                button1.Enabled = false;
                lane1Token = true;
                richTextBox1.AppendText("Sending a takeoff plane to lane 1.\r\n");
                path0.Start();
                waitingQue.Stop();
                takeoffQue--;
                pictureBox1.Image = Properties.Resources.takeoff1;
                picture0 = "takeoff1";
            }
            else if (lane1Token)
            {
                Debug.Assert(lane1Token, "Error on button1_click, lane1Token false");
                GenerateForm2();
            }
            else
            {
                MessageBox.Show("You don't have any planes on the takeoff que!");

            }
        }

     
        //takeoff 2
        private void button2_Click(object sender, EventArgs e)
        {
            Debug.Assert(takeoffQue >= 0, "Error on button2_Click, takeoffQue negative number.");
            if (takeoffQue > 0 && !lane2Token)
            {
                Debug.Assert(!lane2Token, "Error on button2_click, lane2Token true");
                button2.Enabled = false;
                lane2Token = true;
                richTextBox1.AppendText("Sending a takeoff plane to lane 2.\r\n");
                path1.Start();
                waitingQue.Stop();
                takeoffQue--;
                pictureBox2.Image = Properties.Resources.takeoff1;
                picture1 = "takeoff1";
            }
            else if (lane2Token)
            {
                Debug.Assert(lane2Token, "Error on button2_click, lane2Token false");
                GenerateForm2();
            }
            else
            {
                MessageBox.Show("You don't have any planes on the takeoff que!");

            }
        }

        //landing 1
        private void button3_Click(object sender, EventArgs e)
        {
            if (!lane1Token)
            {
                Debug.Assert(!lane1Token, "Error on button3_click, lane1Token true");
                if (timer1.Enabled)
                {
                    timer1.Stop();
                }
                //DisableLandingButtons();
                lane1Token = true;
                richTextBox1.AppendText("Sending a landing plane to lane 1.\r\n");
                path0.Start();
                pictureBox1.Image = Properties.Resources.landing0;
                picture0 = "landing0";
            }
            else
            {
                Debug.Assert(lane1Token, "Error on button3_click, lane1Token false");
                GenerateForm2();
            }
        }

      
        //landing 2
        private void button4_Click(object sender, EventArgs e)
        {
            if (!lane2Token)
            {
                Debug.Assert(!lane2Token, "Error on button3_click, lane2Token true");
                if (timer1.Enabled)
                {
                    timer1.Stop();
                }
                //DisableLandingButtons();
                lane2Token = true;
                richTextBox1.AppendText("Sending a landing plane to lane 2.\r\n");
                path1.Start();
                pictureBox2.Image = Properties.Resources.landing0;
                picture1 = "landing0";
            }
            else
            {
                Debug.Assert(lane1Token, "Error on button4_click, lane2Token false");
                GenerateForm2();
            }
        }

        //Quits the program
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //New Game
        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            //DisableLandingButtons();
            BeginNewGame();
        }

        //Cycle
        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            timeLeft += 10;
        }

        //Cancel
        private void button7_Click(object sender, EventArgs e)
        {
            //DisableLandingButtons();
            textBox1.Clear();
            textBox1.AppendText("Flight cancelled. -5 points.\r\n");
            points += -5;
            ChangePoints();
            timer1.Stop();
        }

        //Used to countdown the landing plane
        private void timer1_Tick(object sender, EventArgs e)
        {


            if (timeLeft > 0)
            {
                timeLeft--;
                textBox1.Clear();
                textBox1.AppendText("Landing Plane will land on lane " + landingPlane + " in " + timeLeft + " seconds.\r\n");
            }
            else
            {
                Debug.Assert(timeLeft == 0, "Error on timer1_Tick, timeLeft bigger than 0.");
                timer1.Stop();
                textBox1.Clear();
                textBox1.AppendText("Landing on " + landingPlane);
                //Chooses which lane to land
                switch (landingPlane)
                {
                    case 1:
                        {
                            if (!lane1Token)
                            {
                                Debug.Assert(!lane1Token, "Error on timer1_Tick, lane1Token true");
                                //DisableLandingButtons();
                                lane1Token = true;
                                path0.Start();
                                pictureBox1.Image = Properties.Resources.landing0;
                                picture0 = "landing0";
                                break;
                            }
                            else
                            {
                                Debug.Assert(lane1Token, "Error on timer1_Tick, laneTtoken false");
                                GenerateForm2();
                                break;
                            }
                            
                        }
                    case 2:
                        {
                            if (!lane2Token)
                            {
                                Debug.Assert(!lane2Token, "Error on timer1_Tick, lane2Token true");
                                //DisableLandingButtons();
                                lane2Token = true;
                                path1.Start();
                                pictureBox2.Image = Properties.Resources.landing0;
                                picture1 = "landing0";
                                break;
                            }
                            else
                            {
                                Debug.Assert(lane2Token, "Error on timer1_Tick, lane2Token false");
                                GenerateForm2();
                                break;
                            }
                        }

                }
            }
        }

        //Disables landing buttons
        
        private void DisableLandingButtons()
        {
            Debug.Assert(button3.Enabled, "Error on DisableLandingButtons, buttons already disabled.");
            button3.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            Debug.Assert(!button3.Enabled, "Error on DisableLandingButtons, buttons enabled.");
        }

        //About menu
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kemi Airport Simulator by Antti Männikkö.\r\n"
                + "Made for the Software Construction course \"Defensive Programming\""
                + "module.");
        }

        //Initializing new game
        private void BeginNewGame()
        {
            ChangePoints();
            takeoffInt = rand.Next(3000, 5000);
            landingInt = rand.Next(7000, 10000);
            takeoffInterval.Interval = takeoffInt;
            takeoffInterval.Tick += new EventHandler(NewPlane);
            landingInterval.Interval = landingInt;
            landingInterval.Tick += new EventHandler(NewLanding);
            

        }

        //Automatic scroll down for the richTExtBox1
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        //Puts new takeoff plane to the que
        private void NewPlane(object sender, EventArgs e)
        {
           
            richTextBox1.AppendText("A new plane has entered to the takeoff que.\r\n");
            if (!waitingQue.Enabled)
            {
                Debug.Assert(!waitingQue.Enabled, "Error on NewPlane, waitingQue already enabled");
                waitingQue.Start();
            }
            richTextBox2.Clear();
            takeoffQue++;
            richTextBox2.AppendText(String.Format("Takeoff que: {0}", takeoffQue));                       
        }
        private void NewLanding(object sender, EventArgs e)
        {
            richTextBox1.AppendText("A new plane has entered to the landing.\r\n");
            button3.Enabled = true;
            button4.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            timeLeft = 10;
            landingInterval.Stop();
            switch (rand.Next(0, 2))
            {
                case 0:
                    {
                        Debug.Assert(button3.Enabled, "Error on NewLanding case0 , buttons still disabled");
                        richTextBox1.AppendText("The landing plane has chosen lane 1 to landing.");
                        landingPlane = 1;
                        timer1.Start();
                        break;
                    }
                case 1:
                    {
                        Debug.Assert(button3.Enabled, "Error on NewLanding case1 , buttons still disabled");
                        richTextBox1.AppendText("The landing plane has chosen lane 2 to landing.");
                        landingPlane = 2;
                        timer1.Start();
                        break;
                    }


            }
        }

        //Generates the losing screen
        private void GenerateForm2()
        {
            Form2 frm2 = new Form2(points);
            DialogResult dia = frm2.ShowDialog();
            
            if (dia == DialogResult.Retry)
            {
                Form1 NewForm = new Form1();
                NewForm.Show();
                this.Dispose(false);
            }
            else if (dia == DialogResult.Cancel)
            {
                Environment.Exit(0);
            }
        }

        //Decreases points when the takeoff que hasn't moved in certain time 
        private void TiredOfWaiting(object sender, EventArgs e)
        {
            richTextBox1.AppendText("The takeoff plane is tired of waiting! -5 points.\r\n");
            points -= 5;
            ChangePoints();
        }
    }
}
