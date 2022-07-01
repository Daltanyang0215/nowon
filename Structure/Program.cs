using System;

// Structure : 구조체
// 변수 + 함수들을 멥버로 가지는 사용자 정의 자료형

// class 와의 차이점은, 구조방식이 값 차이로 되어있음. 인자 전달시 값으로서 파라미터가 들어감.
// 클래스틑 주소 방식이기에 인자 데이터 수정시 원본에 데이터가 수정될 수 있다.
// 구조체는 값 방식으로 인자 데이터 수정시 원본에 다른 데이터로서 원본의 데이터를 수정 할 수 없다

// .net 버전에 다라  상의하나 한번에 복사할수 있는 값의 크기는 16b
// 그래서 그이상의 크기를 복사하게되는 경우 2번 이상에 나눠서 복사가 이루어짐.
// 16b 미만이면 값 복사가 빈번한 경우에 구조체 사용을 권장함

struct Stats
{
    public int STR;
    public int DEX;
    public int INT;
    public int LUK;

    public Stats(int STR,int DEX, int INT, int LUK)
    {
        this.STR = STR;
        this.DEX = DEX;
        this.INT = INT;
        this.LUK = LUK;
    }

    public int GetCombatPower()
    {
        return STR + DEX + INT + LUK;
    }
}

class Stats_C
{
    public int STR;
    public int DEX;
    public int INT;
    public int LUK;

    public Stats_C(int STR, int DEX, int INT, int LUK)
    {
        this.STR = STR;
        this.DEX = DEX;
        this.INT = INT;
        this.LUK = LUK;
    }

    public int GetCombatPower()
    {
        return STR + DEX + INT + LUK;
    }
}


namespace Structure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stats stats1 = new Stats(10,30,50,20);

            Stats_C stats_C = new Stats_C(40, 20, 20, 10);




        }
    }
}
