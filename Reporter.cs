using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EData
{
    class Reporter
    {
        public delegate void Del(string output);
        public event Del Report;
        Dictionary<int, Employee> Employees = new Dictionary<int, Employee>();
        string[] args;
        public Reporter(string[] args)
        {
            this.args = args;
            Report += new Del(FileReport);// == Event += new Del(FileReport); when: public delegate void Del(); public event Del Event;
            Report += new Del(ConsoleReport);
        }

        public string CreateReport()
        {
            //C:\Users\estratulat\source\repos\EData\EData\Report.txt
            Dictionary<int, List<Employee>> byYear = new Dictionary<int, List<Employee>>();
            if (Employees == null || Employees.Count < 1) {
                Console.WriteLine("Employee list is emty");
                return null;
            }
                
            foreach (var e in Employees)
            {
                if (!byYear.ContainsKey(e.Value.Date.Year))
                {
                    byYear.Add(e.Value.Date.Year, new List<Employee> { e.Value });
                }
                else
                    byYear[e.Value.Date.Year].Add(e.Value);
            }
            var orderedByYear = byYear.OrderBy(x => x.Key);
            StringBuilder sbo = new StringBuilder();
            foreach (var pair in orderedByYear)
            {
                sbo.AppendLine(pair.Key.ToString());
                float[] salaryByTrimesters = new float[3];
                foreach (var e in pair.Value)
                {
                    switch (e.Date.Month)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            salaryByTrimesters[0] += e.Salary;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            salaryByTrimesters[1] += e.Salary;
                            break;
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            salaryByTrimesters[2] += e.Salary;
                            break;
                    }
                }
                sbo.AppendLine("Trimester I:   " + ((salaryByTrimesters[0] > 0) ? salaryByTrimesters[0].ToString() : "No Data"));
                sbo.AppendLine("Trimester II:  " + ((salaryByTrimesters[1] > 0) ? salaryByTrimesters[1].ToString() : "No Data"));
                sbo.AppendLine("Trimester III: " + ((salaryByTrimesters[2] > 0) ? salaryByTrimesters[2].ToString() : "No Data"));
            }
            return sbo.ToString();
        }
        public void AddAndReport() {
            ChooseAdd(args);
            string report = CreateReport();
            if (String.IsNullOrEmpty(report))
                Console.WriteLine("Report is empty");
            else
                Report?.Invoke(report);
        }
        
        public void Add(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Console.WriteLine("No Input");
                return;
            }
            

            string[] stringEmployee = inputString.Split(";", StringSplitOptions.RemoveEmptyEntries);
            var r = new Regex(@"\A\s*(\d+) (\w+) (\w+) (\d+) (\d{2}\/\d{2}\/\d{4})\s*\Z", RegexOptions.IgnoreCase); //10 John Doe 4000 15/12/2018;11 Ion Don 4000 11/10/2013;13 Jid Kot 5500 05/05/2018


            foreach (var sE in stringEmployee)
            {

                var match = r.Match(sE);
                if (match.Success)
                {
                    try
                    {
                        var Employee = new Employee();

                        Employee.ID = int.Parse(match.Groups[1].Value);
                        Employee.FirstName = match.Groups[2].Value;
                        Employee.LastName = match.Groups[3].Value;
                        Employee.Salary = int.Parse(match.Groups[4].Value);
                        Employee.Date = DateTime.ParseExact(match.Groups[5].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (!Employees.ContainsKey(Employee.ID))
                        {
                            Employees.Add(Employee.ID, Employee);
                        }
                        else
                            Console.WriteLine("Employee ID duplicate");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                    Console.WriteLine("Invalid input: " + sE);

            }
        }

        public void ChooseAdd(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Enter Data:");
                Add(Console.ReadLine() );
            }
            else
            if (args.Length == 2 && args[0] == "l")
                Add(args[1]);
            else
            if (args[0] == "f")
            {
                string input;
                if (args.Length == 1)
                    input = System.IO.File.ReadAllText(@"C:\Users\estratulat\source\repos\EData\EData\EmployeesInput.txt");
                else
                    try
                    {
                        input = System.IO.File.ReadAllText(args[1]);
                        Add(input);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
            }
        }

        public void ConsoleReport(string outPut)
        {
            Console.WriteLine(outPut);
        }
        public void FileReport(string outPut)
        {
            System.IO.File.WriteAllText(@"C:\Users\estratulat\source\repos\EData\EData\Report.txt", outPut);
        }
    }
}
