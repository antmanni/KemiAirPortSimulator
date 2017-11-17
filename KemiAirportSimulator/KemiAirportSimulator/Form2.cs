using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KemiAirportSimulator
{
    public partial class Form2 : Form
    {
        int points;
        public Form2(int newPoints)
        {
            points = newPoints;
            InitializeComponent();
            richTextBox1.AppendText("There has been a crash, game is over. You got: " + points + " points.\r\n");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
