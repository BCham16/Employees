using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamberlain_CourseProject_part2
{
    [Serializable]
    class Employee
    {
        //public string firstName { get; set; }
        //public string lastName { get; set; }
        //public string ssn { get; set; }
        //public DateTime hireDate { get; set; }

        // attributes
        protected string firstName;
        protected string lastName;
        protected string ssn;
        protected DateTime hireDate;
        protected Benefits benefits;

        //constructors
        public Employee()
        {
            firstName = "unknown";
            lastName = "unknown";
            ssn = "unknown";
            hireDate = DateTime.MinValue;
        }
        public Employee(string firstName, string lastName, string ssn, DateTime hireDate, Benefits benefits)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            SSN = ssn;
            HireDate = hireDate;
            BenefitsPackage = benefits;
        }

        //behaviors
        public override string ToString()
        {
            // John Smith, SSN: 123-45-6789, Hire Date: 01/01/1900
            return firstName + " " + lastName + ", SSN: " + ssn + ", Hire Date: " + hireDate.ToShortDateString();
        }

        //properties
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (value.Length > 0)
                    firstName = value;
                else
                    firstName = "unknown";
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (value.Length > 0)
                    lastName = value;
                else
                    lastName = "unknown";
            }
        }
        public string SSN
        {
            get
            {
                return ssn;
            }
            set
            {
                if (value.Length == 9 || value.Length == 11)
                    ssn = value;
                else
                    ssn = "unknown";
            }
        }
        public DateTime HireDate
        {
            get
            {
                return hireDate;
            }
            set
            {
                if (value.Year > 1950 && value.Year <= DateTime.Now.Year + 1)
                    hireDate = value;
                else
                    hireDate = DateTime.MinValue; 
            }
        }

        public virtual double CalculatePay()
        {
            return 0.0;
        }

        public Benefits BenefitsPackage
        {
            get
            {
                return benefits;
            }
            set
            {
                this.benefits = value;
            }
        }
    }
}
