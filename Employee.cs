using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EData
{
    class Employee
    {
        private string firstName;
        private string lastName;

        public int ID { get; set; }
        public string FirstName { get => firstName; set => CheckName(value); }
        public string LastName { get => lastName; set => CheckName(value); }
        public float Salary { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return new StringBuilder(ID +" "+ FirstName + " " + LastName + " " + Salary + " " + Date.ToShortDateString()).ToString();
        }

        string CheckName(string name)
        {
            var r = new Regex(@"\A[a-zA-z]+\Z");
            if (!r.Match(name).Success)
                throw new NameException(name);
            else return name;
        }
        class NameException : Exception
        {
            public NameException(string name):base("Invalid Name input: "+ name) { }
        }
    }
}
