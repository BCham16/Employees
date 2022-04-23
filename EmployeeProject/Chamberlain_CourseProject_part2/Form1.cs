using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chamberlain_CourseProject_part2
{
    public partial class MainForm : Form
    {
        // form level refernces
        const string FILENAME = "Employees.dat";

        public MainForm()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // add item to the employee listbox
            InputForm frmInput = new InputForm();

            using (frmInput)
            {
                DialogResult result = frmInput.ShowDialog();

                // see if input form was cancelled
                if (result == DialogResult.Cancel)
                    return;

                // get user's input and create Employee object
                string fName = frmInput.FirstNameTextBox.Text;
                string lName = frmInput.LastNameTextBox.Text;
                string ssn = frmInput.SSNTextBox.Text;
                string date = frmInput.HireDateTextBox.Text;
                DateTime hireDate = DateTime.Parse(date);
                string healthIns = frmInput.HealthInsTextBox.Text;
                double lifeIns = Double.Parse( frmInput.LifeInsTextBox.Text );
                int vacation = Int32.Parse( frmInput.VacationTextBox.Text );

                Benefits benefits = new Benefits(healthIns, lifeIns, vacation);
                Employee emp;
                
                if( frmInput.HourlyRadioButton.Checked)
                {
                    float hourlyRate = float.Parse(frmInput.Pay1TextBox.Text);
                    float hoursWorked = float.Parse(frmInput.Pay2TextBox.Text);

                    emp = new Hourly(fName, lName, ssn, hireDate, benefits, hourlyRate, hoursWorked);
                } 
                else if (frmInput.SalaryRadioButton.Checked)
                {
                    double salary = Double.Parse(frmInput.Pay1TextBox.Text);

                    emp = new Salary(fName, lName, ssn, hireDate, benefits, salary);
                }
                else
                {
                    MessageBox.Show("Error. Invalid Employee type.");
                    return;
                }
                    

                // add the Employee object to the employees listbox
                EmployeesListBox.Items.Add(emp);

                // write Employee objects to a file
                WriteEmpsToFile();               
            }

        }

        private void WriteEmpsToFile()
        {

            // convert the listbox to a generic list
            List<Employee> empList = new List<Employee>();

            foreach (Employee emp in EmployeesListBox.Items)
            {
                empList.Add(emp);
            }

            // open a pipe to the file and create a translator
            FileStream fs = new FileStream(FILENAME, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            // write the generic list to the file
            formatter.Serialize(fs, empList);

            // close the pipe
            fs.Close();


            // written record confirmation
            MessageBox.Show("Employee successfully saved");
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int itemNumber = EmployeesListBox.SelectedIndex;
            if (itemNumber == -1)
            {
                MessageBox.Show("Please select an employee to remove");
            }
            else
            {
                EmployeesListBox.Items.RemoveAt(itemNumber);
                
                // write Employee objects to a file
                WriteEmpsToFile();
            }
        }

        private void DisplayButton_Click(object sender, EventArgs e)
        {
            // clear the listbox
            EmployeesListBox.Items.Clear();

            // read all employee objects from the file
            ReadEmpsFromFile();
        }

        private void ReadEmpsFromFile()
        {
            // check if the file exists & read from file
            if(File.Exists(FILENAME) && new FileInfo(FILENAME).Length > 0)
            {
                FileStream fs = new FileStream(FILENAME, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();

                List<Employee> list = (List<Employee>)formatter.Deserialize(fs);

                fs.Close();

                // copy the Employee objects into our listbox
                foreach (Employee emp in list)
                    EmployeesListBox.Items.Add(emp);
            }

        }

        private void PrintPaychecksButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Printing Paychecks for all employees...");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // load employees previously entered from CSV file
            ReadEmpsFromFile();
        }

        private void EmployeesListBox_DoubleClick(object sender, EventArgs e)
        {
            // edit selected employee in the list box
            InputForm frmUpdate = new InputForm();

            using (frmUpdate)
            {
                frmUpdate.Text = "Employee Update Form";
                frmUpdate.SubmitButton.Text = "Update";

                int itemNumber = EmployeesListBox.SelectedIndex;

                if (itemNumber < 0)
                {
                    MessageBox.Show("Error. Invalid Employee.");
                    return;
                }

                Employee emp = (Employee)EmployeesListBox.Items[itemNumber];

                frmUpdate.FirstNameTextBox.Text = emp.FirstName;
                frmUpdate.LastNameTextBox.Text = emp.LastName;
                frmUpdate.SSNTextBox.Text = emp.SSN;
                frmUpdate.HireDateTextBox.Text = emp.HireDate.ToShortDateString();
                frmUpdate.HealthInsTextBox.Text = emp.BenefitsPackage.HealthInsurance;
                frmUpdate.LifeInsTextBox.Text = emp.BenefitsPackage.LifeInsurance.ToString("C2");
                frmUpdate.VacationTextBox.Text = emp.BenefitsPackage.Vacation.ToString();

                if (emp is Hourly)
                {
                    Hourly hrly = (Hourly)emp;
                    frmUpdate.HourlyRadioButton.Checked = true;
                    frmUpdate.Pay1TextBox.Text = hrly.HourlyRate.ToString("N2");
                    frmUpdate.Pay2TextBox.Text = hrly.HoursWorked.ToString("N1");
                }
                else if (emp is Salary)
                {
                    Salary sal = (Salary)emp;
                    frmUpdate.SalaryRadioButton.Checked = true;
                    frmUpdate.Pay1TextBox.Text = sal.AnnualSalary.ToString("N2");
                }

                DialogResult result = frmUpdate.ShowDialog();

                if (result == DialogResult.Cancel)
                    return;

                EmployeesListBox.Items.RemoveAt(itemNumber);

                // get updated input and create new employee object
                string fName = frmUpdate.FirstNameTextBox.Text;
                string lName = frmUpdate.LastNameTextBox.Text;
                string ssn = frmUpdate.SSNTextBox.Text;
                string date = frmUpdate.HireDateTextBox.Text;
                DateTime hireDate = DateTime.Parse(date);
                string healthIns = frmUpdate.HealthInsTextBox.Text;
                // string that does not include the $ symbol
                string lifeInsString = frmUpdate.LifeInsTextBox.Text;
                lifeInsString = lifeInsString.Substring(1);
                double lifeIns = Double.Parse(lifeInsString);
                int vacationDays = Int32.Parse(frmUpdate.VacationTextBox.Text);

                Benefits benefits = new Benefits(healthIns, lifeIns, vacationDays);

                if (frmUpdate.HourlyRadioButton.Checked)
                {
                    float rate = float.Parse(frmUpdate.Pay1TextBox.Text);
                    float hours = float.Parse(frmUpdate.Pay2TextBox.Text);
                    emp = new Hourly(fName, lName, ssn, hireDate, benefits, rate, hours);
                }
                else if (frmUpdate.SalaryRadioButton.Checked)
                {
                    double salary = double.Parse(frmUpdate.Pay1TextBox.Text);
                    emp = new Salary(fName, lName, ssn, hireDate, benefits, salary);
                }


                // add updated Employee object to the employees listbox
                EmployeesListBox.Items.Add(emp);

                // write all of the updated Employee objects to the file
                WriteEmpsToFile();
            }
        }
    }
}

