using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrapIntegration.App
{
    internal static class Dialogs
    {
        public static bool TryRequestValue<T>(string message, Func<string, bool> validate, Func<string, T> parse, out T value)
        {
            Console.Write(message);

            string errMsg = null;

            do
            {

                try
                {
                    var str = Console.ReadLine();
                    if (validate(str))
                    {
                        value = parse(str);
                        errMsg = null;
                    }
                    else
                    {
                        errMsg = "Invalid value string";
                        value = default(T);
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    value = default(T);
                }

                if (errMsg != null)
                {
                    Console.WriteLine("Error: " + errMsg);
                    Console.WriteLine("Try again? (n to cancel)");

                    if (Console.ReadKey().Key == ConsoleKey.N)
                        break;
                }

            } while (errMsg != null);

            return errMsg == null;
        }

        public static bool ValidDouble(string number)
        {
            Regex regex = new Regex("^[+-]?[0-9]*[.]?[0-9]+$");
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

        internal static void PrintInpuData(string function, double a, double b, double accuracy)
        {
            Console.WriteLine("\n\n Input data:");
            Console.WriteLine($"\ty = : {function}");
            Console.WriteLine($"\taccuracy: {accuracy}");
            Console.WriteLine($"\tUpper limit: {a}");
            Console.WriteLine($"\tLower limit: {b}");
        }

        internal static void OutputSolve(double answer, int n, double error)
        {
            Console.WriteLine($"Number of splits:  {n}");
            Console.WriteLine($"Answer:    {answer.ToString("F10")}");
            Console.WriteLine($"Accuracy:  {error.ToString("F10")}");
        }

        internal static void PrintCancellation()
        {
            Console.WriteLine("\nCancelled");
        }
        
    }
}
