using ParsingExpression;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TrapIntegration.App
{
    public class App
    {
        public delegate double FuncDelegate(double x);

        private Dictionary<string, FuncDelegate> _functions = new Dictionary<string, FuncDelegate>() {
            { "x * x", x => x * x }
        };
        
        public App()
        {
            var tuples = _functions.Select(kv => Tuple.Create(kv.Key, kv.Value)).ToArray();

        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var app = new App();
            var menu = new Menu(
                    new MenuItem("Integrate")
                    {
                        new MenuItem("Input function", app.PerformIntegrationForCustomFunction),
                        new MenuItem("Select function") {
                            new MenuItem("x*x", () => app.PerformIntegrationForFunction(x => x*x)),
                            new MenuItem("3*x*x*x-8*x+4", () => app.PerformIntegrationForFunction(x => 3*x*x*x-8*x+4)),
                            new MenuItem("cos(x)", () => app.PerformIntegrationForFunction(x => Math.Cos(x)))
                        }
                    }
                 );

            menu.RunMenu();
        }

        private void PerformIntegrationForFunction(Func<double, double> f)
        {
            string functionString = null;

            if (
                Dialogs.TryRequestValue("Input lower limit : ", Dialogs.ValidDouble, Double.Parse, out double a) &&
                Dialogs.TryRequestValue("Input upper limit : ", Dialogs.ValidDouble, Double.Parse, out double b) &&
                Dialogs.TryRequestValue("Input accuracy : ", Dialogs.ValidDouble, Double.Parse, out double accuracy))
            {
                var integrator = new NumericIntegrator(-1, a, b, accuracy, x => f(x));

                Dialogs.PrintInpuData(functionString, a, b, accuracy);

                integrator.PerformIntegration();

                Dialogs.OutputSolve(integrator.Result, integrator.StepsCount, integrator.Error);
            }
            else
            {
                Dialogs.PrintCancellation();
            }
        }

        class NumExprToLinqExprTranslator : INumExprVisitor<Expression>
        {
            public ParameterExpression Arg { get; private set; }

            private NumExprToLinqExprTranslator()
            {
                this.Arg = Expression.Parameter(typeof(double), "x");
            }

            Expression INumExprVisitor<Expression>.VisitBinExpr(NumBinExpr numBinExpr)
            {
                var left = numBinExpr.Left.Apply(this);
                var right = numBinExpr.Right.Apply(this);

                switch (numBinExpr.Kind)
                {
                    case NumOp.Sum: return Expression.Add(left, right);
                    case NumOp.Sub: return Expression.Subtract(left, right);
                    case NumOp.Mul: return Expression.Multiply(left, right);
                    case NumOp.Div: return Expression.Divide(left, right);
                    default:
                        throw new ApplicationException("Unknown binary expression kind " + numBinExpr.Kind);
                }
            }

            Expression INumExprVisitor<Expression>.VisitConst(NumConstExpr numConstExpr)
            {
                return Expression.Constant(numConstExpr.Value, typeof(double));
            }

            Expression INumExprVisitor<Expression>.VisitVar(NumVarExpr numVarExpr)
            {
                if (numVarExpr.Name != "x")
                    throw new ApplicationException("Unknown variable name " + numVarExpr.Name);

                return this.Arg;
            }

            public static Expression<Func<double, double>> TranslateExprTree(NumExpr tree)
            {
                var translator = new NumExprToLinqExprTranslator();
                var body = tree.Apply(translator);
                var lambda = Expression.Lambda<Func<double, double>>(body, translator.Arg);
                return lambda;
            }
        }

        private Func<double, double> ParseFunction(string str)
        {
            var tree = NumExprParser.ExpToTree(str);
            var expr = NumExprToLinqExprTranslator.TranslateExprTree(tree);
            return expr.Compile();
        }

        private void PerformIntegrationForCustomFunction()
        {
            string functionString = null;

            if (
                Dialogs.TryRequestValue("Input function : ", s => true, s => {
                    var tree = NumExprParser.ExpToTree(s);
                    var expr = NumExprToLinqExprTranslator.TranslateExprTree(tree);
                    functionString = tree.Apply(new NumExprStringCollector());
                    return expr.Compile();
                }, out var f) &&
                Dialogs.TryRequestValue("Input lower limit : ", Dialogs.ValidDouble, Double.Parse, out double a) &&
                Dialogs.TryRequestValue("Input upper limit : ", Dialogs.ValidDouble, Double.Parse, out double b) &&
                Dialogs.TryRequestValue("Input accuracy : ", Dialogs.ValidDouble, Double.Parse, out double accuracy))
            {
                var integrator = new NumericIntegrator(-1, a, b, accuracy, x => f(x));

                Dialogs.PrintInpuData(functionString, a, b, accuracy);

                integrator.PerformIntegration();

                Dialogs.OutputSolve(integrator.Result, integrator.StepsCount, integrator.Error);
            }
            else
            {
                Dialogs.PrintCancellation();
            }   
        }

    }

}
