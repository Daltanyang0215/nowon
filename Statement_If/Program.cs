using System;

namespace Statement_If
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool condition1 = false;
            bool condition2 = false;
            bool condition3 = true;

            if (condition1)
            {
                // 조건1이 참일때 실행할 내용
                Console.WriteLine("조건 1은 참이여");
            }
            else if (condition2)
            {
                // 조건2가 참일때 실행할 내용
                Console.WriteLine("조건 1은 거짓이고, 조건 2은 참이여");
            }
            else if (condition3)
            {
                // 조건3이 참일때 실행할 내용
                Console.WriteLine("조건 1,2가 거짓이고, 조건 3은 참이여");
            }
            else
            {
                // 조건이 거짓일 때 실행할 내용
                Console.WriteLine("조건 1,2,3이 모두 거짓이여");
            }
        }
    }
}
