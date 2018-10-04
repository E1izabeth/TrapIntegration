using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrapIntegration.App
{
    internal class Dialogs
    {       
        public int ReadN(ref int succ, int maxnumber)
        {
            var n = Console.ReadLine();
            if (ValidNumber(n) && Int32.Parse(n) <= maxnumber)
            {
                succ = 1;
                return Int32.Parse(n);
            }
            succ = this.AskTryAgain($"Invalid number. It should be in [1, {maxnumber}]. Try again? (y/n)");
            return -1;
        }

        public double ReadLimit(ref int succ, bool isUpperLimit)
        {
            if (isUpperLimit)
            {
                Console.WriteLine("Please, input the upper limit");
            }
            else
            {
                Console.WriteLine("Please, input the lower limit");
            }
            var n = Console.ReadLine();
            if (ValidDouble(n))
            {
                succ = 1;
                return Double.Parse(n);
            }
            succ = this.AskTryAgain($"Invalid limit. Try again? (y/n)");
            return -1;
        }

        internal void OutputSolve(double answer, int n, double error)
        {
            Console.WriteLine($"Number of splits:  {n}");
            Console.WriteLine($"Answer:    {answer.ToString("F10")}");
            Console.WriteLine($"Accuracy:  {error.ToString("F10")}");
        }

        public double ReadAccuracy(ref int succ)
        {
            Console.WriteLine("Please, input accuracy");
            return this.ReadDouble(ref succ);
        }

        private double ReadDouble(ref int succ)
        {
            var d = Console.ReadLine();
            if (ValidDouble(d))
            {
                succ = 1;
                return Double.Parse(d);
            }
            succ = this.AskTryAgain("Invalid double. Try again? (y/n)");
            return -1;
        }

        internal void PrintInpuData(string function, double a, double b, double accuracy)
        {
            Console.WriteLine("\n\n Input data:");
            Console.WriteLine($"\ty = : {function}");  
            Console.WriteLine($"\taccuracy: {accuracy}");
            Console.WriteLine($"\tUpper limit: {a}");
            Console.WriteLine($"\tLower limit: {b}");
        }

        internal void ShowFunctions()
        {
            Console.WriteLine("1)     x^2");
            Console.WriteLine("2)     2*x-4");
            Console.WriteLine("3)     cos(x)");
        }
        
        private int AskTryAgain(string massage)
        {
            Console.WriteLine(massage);
            var s = Console.ReadLine();
            if (s == "y")
                return 2;
            else return 0;
        }

        public static bool ValidDouble(string number)
        {
            Regex regex = new Regex("^[+-]?[0-9]*[,]?[0-9]+$");
            if (!regex.IsMatch(number))
            {
                return false;
            }
            return true;
        }

        public static bool ValidNumber(string number)
        {
            Regex regex = new Regex("^[0-9]+$");
            if (!regex.IsMatch(number))
            {
                return false;
            }
            return true;
        }
    }
}
