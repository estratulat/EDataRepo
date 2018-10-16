using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EData
{
    class Program
    {
        static void Main(string[] args)
        {
            new Reporter(args).AddAndReport();
            Console.Read();
        }
        
        
    }
}
