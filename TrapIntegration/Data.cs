using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrapIntegration
{
    public class Data
    {
        public delegate double FuncDelegate(double X);
        FuncDelegate func;
        private double prevAnswer;
        public double Answer;

        public int FuncNumber { get; private set; }
        public double A { get; private set; }
        public double B { get; private set; }
        public int N { get; private set; }
        public double Accuracy { get; private set; }

        public Data(int funcNumber, double a, double b, double precision, FuncDelegate func)
        {
            this.FuncNumber = funcNumber;
            this.A = a;
            this.B = b;
            this.Accuracy = precision;
            this.func = func;
        }

        public double GetError()
        {
            return Math.Abs(this.Answer - prevAnswer)/3;
        }

        public void Solve()
        {
            double h;
            int n = 10;

            while (true)
            {
                h = (this.B - this.A) / n;
                for (int i = 0; i < n; i++)
                {
                    if (i == 0)
                    {
                        prevAnswer = func(this.A + i * h);
                    }
                    else if (i < n - 1)
                    {
                        prevAnswer += 2 * func(this.A + i * h);
                    }
                    else
                    {
                        prevAnswer += (func(this.A + i * h));
                    }
                }
                prevAnswer = h / 2 * prevAnswer;
                n = n * 2;
                h = (this.B - this.A) / n;
                for (int i = 0; i < n; i++)
                {
                    if (i == 0)
                    {
                        Answer = func(this.A + i * h);
                    }
                    else if (i < n - 1)
                    {
                        Answer += 2 * func(this.A + i * h);
                    }
                    else
                    {
                        Answer += (func(this.A + i * h));
                    }
                }
                Answer = h / 2 * Answer;
                if (Math.Abs(Answer - prevAnswer) / 3 < this.Accuracy)
                {
                    this.N = n;
                    break;
                }
                else n = n * 2;
            }
        }
    }
}
