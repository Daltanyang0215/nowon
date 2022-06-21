using System;

// Function : 특정 연산에 대한 기능
// Method : Function 중에서 객체를 통해서 호출되는 Function (멤버함수)

namespace Methods
{
    internal class Program
    {
        static int b = 3;

        // Program 클래스 의 멤버함수
        static void Main(string[] args)
        {
            // 함수호출의 형태
            // 함수이름(인자);
            PrintHelloWorld();
            PrintNum(3);
            PrintSumResult(1, 2);

            // 지역변수 
            // 함수 안에서 선언된 변수
            // 지역변수가 할당되는 공간은 어떤 값이 들어있는지 알 수 없으므로
            // 초기화를 해주어야한다.
            int a = 2;
            
            Console.WriteLine(Sum(a, b));
            Console.WriteLine(5);

        }

       
        // 함수정의의 형태
        // 반환형 함수이름 (매개변수자료형 매개변수이름)
        // 반환 : 제어권을 넘겨준다
        // void : 반환시에 특별한 값을 넘기지 않는다.
        static void PrintHelloWorld()
        {
            Console.WriteLine("Hello World!");
        }

        static void PrintNum(int num)
        {
            Console.WriteLine(num);
        }

        static void PrintSumResult(int a, int b)
        {
            Console.WriteLine(a + b);
            return;
        }

        static int Sum(int a, int b)
        {
            return a + b;
        }
    }

    
}
