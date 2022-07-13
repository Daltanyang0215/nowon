using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example06_MyLinkedList
{



    internal class MyLinkedList<T>
    {
        // inner class : 클래스 내에 클래스 타입정의
        public class Node<K>
        {
            public K value;
            public Node<K> prev;
            public Node<K> next;

            public Node(K value)
            {
                this.value = value;
            }

        }

        Node<T> first, last, tmp1, tmp2;
        
        public int Count
        {
            get {
                int tmpCount = 0;
                  tmp1 = first;
                while (tmp1 != null)
                {
                    tmp1 = tmp1.next;
                    tmpCount++;
                }
                return tmpCount;
            }
        }

        public Node<T> First { get => first;  }
        public Node<T> Last { get => last;  }



        public T this[int index]
        {
            get { return this[index]; }
            set { this[index] = value; }
        }

        public void AddFirst(T value)
        {
            tmp1 = new Node<T>(value);
            if(first != null)
            {
                tmp1.next = first;
                first.prev = tmp1;
            }
            if(last == null)
                last = tmp1;
            first = tmp1;
        }

        public void AddLast(T value)
        {
            tmp1 = new Node<T>(value);
            if (last != null)
            {
                tmp1.prev = last;
                last.next = tmp1;
            }
            if (first == null)
                first = tmp1;
            last = tmp1;
        }

        public void AddBefore(Node<T> node, T value)
        {
            tmp1 = new Node<T>(value);
            node.prev = tmp1; 
            tmp1.next = node;
            if ( node == first)
            {
                first = tmp1;
            }
        }

        public void AddBefore(T target, T value)
        {
            tmp1 = first;
            while (tmp1 != null)
            {
                if (Comparer<T>.Default.Compare(tmp1.value, target) == 0)
                {
                    tmp2 = new Node<T>(value);
                    if (tmp1.prev != null)
                        tmp1.prev.next = tmp2;
                    tmp1.prev = tmp2;
                    tmp2.next = tmp1;
                    tmp2.prev = tmp1.prev;
                    if (tmp1 == first)
                        first = tmp2;

                    return;
                }
                tmp1 = tmp1.next;
            }
        }

        public void AddAfter(Node<T> node, T value)
        {
            tmp1 = new Node<T>(value);

            node.next = tmp1;
            tmp1.prev = node;

            if (node == last)
            {
                last = tmp1;
            }
        }

        public void AddAfter(T target, T value)
        {
            tmp1 = last;
            while (tmp1 != null)
            {
                if (Comparer<T>.Default.Compare(tmp1.value, target) == 0)
                {
                    tmp2 = new Node<T>(value);

                    if (tmp1.next != null)
                        tmp1.next.prev = tmp2;
                    tmp1.next = tmp2;
                    tmp2.prev = tmp1;
                    tmp2.next = tmp1.next;
                    if (tmp1 == last)
                        last = tmp2;

                    return;
                }
                tmp1 = tmp1.prev;
            }
        }

        public Node<T> Find(T value)
        {
            tmp1 = first;
            while (tmp1 != null)
            {
                if (Comparer<T>.Default.Compare(tmp1.value, value) == 0)
                {
                    return tmp1;
                }
                tmp1 = tmp1.next;
            }
            return null;
        }

        public Node<T> FindLast(T value)
        {
            tmp1 = last;
            while (tmp1 != null)
            {
                if (Comparer<T>.Default.Compare(tmp1.value, value) == 0)
                {
                    return tmp1;
                }
                tmp1 = tmp1.prev;    
            }
            return null;
        }

        public bool Remove(T value)
        {
            bool isRemoved = false;

            tmp1 = Find(value);
            if (tmp1 != null)
            {
                if (tmp1.prev != null)
                    tmp1.prev.next = tmp1.next;
                if (tmp1.next != null)
                    tmp1.next.prev = tmp1.prev;
             
                tmp1.next = null;
                tmp1.prev = null;
                tmp1 = null;
                isRemoved = true;
            }
            return isRemoved;
        }
        public bool RemoveLast(T value)
        {
            bool isRemoved = false;

            tmp1 = FindLast(value);
            if (tmp1 != null)
            {
                if (tmp1.prev != null)
                    tmp1.prev.next = tmp1.next;
                if (tmp1.next != null)
                    tmp1.next.prev = tmp1.prev;

                tmp1.next = null;
                tmp1.prev = null;
                tmp1 = null;
                isRemoved = true;
            }
            return isRemoved;
        }

        public Node<T>[] GetAllNodes()
        {
            Node<T>[] nodes = new Node<T>[Count];
            tmp1 = first;
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = tmp1;
                tmp1 = tmp1.next;
            }
            return nodes;
        }
    }
}
