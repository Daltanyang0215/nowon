using System;

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

            foreach (var sub in list.GetAllNodes())
            {
                Console.WriteLine(sub.value);
            }
        }
    }
}
