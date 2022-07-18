using System;
using System.Collections;
using System.Collections.Generic;

namespace Example06_MyLinkedList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyLinkedList<int> list = new MyLinkedList<int>();

            list.AddFirst(1);
            list.AddFirst(2);
            list.AddBefore(1, 3);

            //foreach (var sub in list)
            //{
            //    Console.WriteLine(sub.);
            //}
            
            foreach (var sub in list.GetAllNodes())
            {
                Console.WriteLine(sub.value);
            }
            foreach (var sub in list)
            {
                Console.WriteLine(sub);
            }

                Console.WriteLine("test");
            EnumeratorTest testInstance = new EnumeratorTest();
            foreach (var item in testInstance.PrintFactorial(5))
            {
                Console.WriteLine(item);
            }
        }
    }

    public class EnumeratorTest
    {
        // FSM(finite State Machine : 유한상태머신) 을 작성할때도 유용함
        public IEnumerable<int> PrintFactorial(int num)
        {
            int tmpResult = 1;
            for (int i = 1; i <= num; i++)
            {
                tmpResult *= i;
                yield return tmpResult;
            }
                yield return 1;
                yield return 2;
        }

        public int GetFactorial(int num)
        {
            int tmpResult = 1;
            for (int i = 1; i <= num; i++)
            {
                tmpResult *= i;
            }
            return tmpResult;
        }
    }
}
