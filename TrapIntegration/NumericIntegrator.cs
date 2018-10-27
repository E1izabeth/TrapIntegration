using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrapIntegration
{
    public class NumericIntegrator
    {
        public delegate double FuncDelegate(double X);

        public double Result { get; private set; }

        public int FuncNumber { get; private set; }
        public double A { get; private set; }
        public double B { get; private set; }
        public int StepsCount { get; private set; }
        public double Accuracy { get; private set; }
        public double Error { get; private set; }

        FuncDelegate _func;
        private double _prevResult;

        public NumericIntegrator(int funcNumber, double a, double b, double precision, FuncDelegate func)
        {
            this.FuncNumber = funcNumber;
            this.A = a;
            this.B = b;
            this.Accuracy = precision;

            _func = func;
        }

        private bool IsAccuracySufficient(int n)
        {
            if (this.Error < this.Accuracy)
            {
                this.StepsCount = n;
                return true;
            }
            return false;
        }

        public bool PerformIntegration()
        {
            this.Result = 0;
            _prevResult = 0;

            try
            {

                double h;
                int n = 10;
                do
                {
                    h = (this.B - this.A) / n;
                    for (int i = 0; i < n; i++)
                    {
                        if (i == 0)
                        {
                            _prevResult = _func(this.A + i * h);
                        }
                        else if (i < n - 1)
                        {
                            _prevResult += 2 * _func(this.A + i * h);
                        }
                        else
                        {
                            _prevResult += _func(this.A + i * h);
                        }
                    }
                    _prevResult = h / 2 * _prevResult;
                    n = n * 2;
                    h = (this.B - this.A) / n;
                    for (int i = 0; i < n; i++)
                    {
                        if (i == 0)
                        {
                            this.Result = _func(this.A + i * h);
                        }
                        else if (i < n - 1)
                        {
                            this.Result += 2 * _func(this.A + i * h);
                        }
                        else
                        {
                            this.Result += _func(this.A + i * h);
                        }
                    }
                    this.Result = h / 2 * this.Result;
                    if (Double.IsInfinity(this.Result) || Double.IsNaN(this.Result))
                    {
                        return false;
                    }
                    
                    this.Error = Math.Abs(this.Result - _prevResult) / 3;

                } while (!this.IsAccuracySufficient(n));
            }
            catch(ArithmeticException ex)
            {
                return false;
            }
            return true;

        }

    }
}
