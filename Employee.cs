using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EData
{
    class Employee
    {
        private string _firstName;
        private string _lastName;

        public int ID { get; set; }
        public string FirstName { get => _firstName; set => CheckName(value); }
        public string LastName { get => _lastName; set => CheckName(value); }
        public float Salary { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return new StringBuilder(ID +" "+ FirstName + " " + LastName + " " + Salary + " " + Date.ToShortDateString()).ToString();
        }

        public void ParseFromString(string input)
        {

            var r = new Regex(@"\A\s*(\d+) ([a-zA-Z]+) ([a-zA-Z]+) (\d+) (\d{2}\/\d{2}\/\d{4})\s*\Z", RegexOptions.IgnoreCase); //10 John Doe 4000 15/12/2018;11 Ion Don 4000 11/10/2013;13 Jid Kot 5500 05/05/2018
            var match = r.Match(input);
            if (match.Success)
            {
                try
                {
                    ID = int.Parse(match.Groups[1].Value);
                    _firstName = match.Groups[2].Value;
                    _lastName = match.Groups[3].Value;
                    Salary = int.Parse(match.Groups[4].Value);
                    Date = DateTime.ParseExact(match.Groups[5].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid input: " + input);
            }
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
            public NameException(string name):base("Exception: Invalid Name input: "+ name) { }
        }
        class InputException : Exception
        {
            public InputException(string name) : base("Exception: Invalid input: " + name) { }
        }
    }
}
