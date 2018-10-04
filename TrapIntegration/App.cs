using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrapIntegration.App
{
    public class App
    {
        public int MaxSize { get; private set; }
        private readonly Dialogs user = new Dialogs();
        private Data data;

        public App(int maxsize)
        {
            this.MaxSize = maxsize;
        }

        static void Main(string[] args)
        {

            var app = new App(20);

            var menu = new Menu(
                    new MenuItem("Integrate") 
                    {
                        new MenuItem("Select function", app.GetFromKeyboard)
                    }
                 );

            menu.RunMenu();
        }

        private void HandData()
        {
            data.Solve();
            user.OutputSolve(this.data.Answer, this.data.N, this.data.GetError());
        }
        
        private void GetFromKeyboard()
        {
            user.ShowFunctions();
            var succ = 0;
            var n = this.GetN(ref succ);
            if (succ == 0) return;
            var a = this.GetLimit(ref succ, false);
            if (succ == 0) return;
            var b = this.GetLimit(ref succ, true);
            if (succ == 0) return;
            var accuracy = this.GetAccuracy(ref succ);
            if (succ == 0) return;
            
            //here!!!

            this.data = new Data(n,a, b, accuracy, (x)=> { return Math.Cos(x); });
            
            
            //here!!!
            user.PrintInpuData("x * x", a, b, accuracy);
            this.HandData();
        }
        

        private int GetN(ref int succ)
        {
            var n = user.ReadN(ref succ, this.MaxSize);
            while (succ == 2)
            {
                n = user.ReadN(ref succ, this.MaxSize);
            } 
            return n;
        }

        private double GetLimit(ref int succ, bool isUpperLimit)
        {
            var n = user.ReadLimit(ref succ, isUpperLimit);
            while (succ == 2)
            {
                n = user.ReadLimit(ref succ, isUpperLimit);
            }
            return n;
        }


        private double GetAccuracy(ref int succ)
        {
            var p = user.ReadAccuracy(ref succ);
            while (succ == 2)
            {
                p = user.ReadAccuracy(ref succ);
            };
            return p;
        }
    }

}
