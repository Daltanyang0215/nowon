using System;

namespace Delegate
{
    internal class Program
    {
        enum OP
        {
            SUM,
            SUB,
            MUL,
            DIV,
            MOD
        }
        
        static void Main(string[] args)
        {
            DoOP(OP.SUM, 5, 3);
        }

        static int DoOP(OP op,int a, int b)
        {
            int result = 0;
            switch (op)
            {
                case OP.SUM:
                    result = Sum(a,b);
                    break;
                case OP.SUB:
                    result = Sub(a,b);
                    break;
                case OP.MUL:
                    result = Mul(a,b);
                    break;
                case OP.DIV:
                    result = Div(a,b);
                    break;
                case OP.MOD:
                    result = Mod(a,b);
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
