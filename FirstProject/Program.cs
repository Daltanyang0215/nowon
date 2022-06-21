// 파란색글자 :
// 키워드(예약어)란 ? 미리 정의되어있는 단어
// 이미 문법용도로 사용되고있기때문에 식별용으로 개발자가 쓸 수 없다.
//
// 흰글자 :
// 식별문자 (참조 있음, 명시적 표현)
//
// 청록색글자 :
// 클래스 타입 식별문자(이름)
//
// 노란색글자 :
// 함수 식별문자(이름)
//
// 하늘색글자 :
// 함수의 파라미터 (매개변수) 의 식별문자(이름)
//
// 주황색글자:
// 문자/ 문자열 상수
//
// 글자색이 어둡다면 :
// (참조 없음 / 생략 가능 == 암시적 표현)

// using 키워드 
// 특정 namespace 를 사용하기위한 키워드
// 형식 : using namespace이름 
using System;

// namespace 키워드
// 공간을 구분하기위한 키워드
// (내부식별자를 가지고 namespace로 묶인 변수,함수,클래스,인터페이스 등등을 구분함)
namespace FirstProject // Note: actual namespace depends on the project name.
{
    // internal 키워드
    // 동일 어셈블리에서만 접근가능한 키워드 

    // class 키워드 
    // 객체를 만들기위한 타입을 정의하는 키워드 
    // 형식 : class 클래스이름
    internal class Program
    {
        // static 키워드 
        // 정적 키워드, 메모리에 동적할당이 불가능한 속성을 부여함.

        // void 키워드
        // 아무것도 없음. (반환값이 없음)

        // Main 함수 
        // 실행파일 (.exe) 을 실행했을떄 가장 먼저 실행되는 함수
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            Console.WriteLine("Hello World!");
        }
    }
}