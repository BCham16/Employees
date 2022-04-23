using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chamberlain_CourseProject_part2
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void SSNLabel_Click(object sender, EventArgs e)
        {

        }

        private void HourlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ShowControls();
        }

        private void SalaryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ShowControls();
        }

        private void ShowControls()
        {
            if (HourlyRadioButton.Checked)
            {
                Pay1Label.Text = "Hourly Rate:";
                Pay2Label.Text = "Hours Worked:";
                Pay2Label.Visible = true;
                Pay2TextBox.Visible = true;
            }
            else if (SalaryRadioButton.Checked)
            {
                Pay1Label.Text = "Annual Salary:";
                Pay2Label.Visible = false;
                Pay2TextBox.Visible = false;
            }
        }
    }
}
