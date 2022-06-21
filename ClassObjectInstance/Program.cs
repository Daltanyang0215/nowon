using System;

namespace ClassObjectInstance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 값형식 , 참조형식
            // 값형식 : 값을 직접 읽고 / 쓰는 형태의 형식
            // 참조형식 : 주소를 읽고 / 주소위치에 값을 쓰는 형태의 형식

            // int : 정수 타입
            // a : 정수 타입 변수
            int a = 0;
            // Human : 클래스 타입
            // human1 : 객체
            Human human1 = new Human(); // new 키워드 : 동적할당하는 키워드
            // 객체화 : new Human(); 클래스로 객체를 생성하는 과정
            // 인스턴스화 : 객체에 실제 데이터를 초기화해서 사용하게되는 과정 
            human1.height = 100f;
            Console.WriteLine(human1.height);
            Human.age = 3;
            Console.WriteLine(Human.age);
            Console.WriteLine(human1.weight);
            // .연산자 : 멤버에 접근할떄 쓰는 연산자
            Human human2 = new Human();
            human2.height = 200f;
            Console.WriteLine(human2.height);
        }
    }

    class Human
    {
        public static int age; 
        public float height; 
        public double weight; 
        private bool isResting;
        private char genderChar;
        private string name; 

        // 생성자
        public Human()
        {
            height = 20f;
            weight = 30;
        }
    }
}
