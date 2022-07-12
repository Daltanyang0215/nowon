using System;
using System.Collections.Generic;

namespace Example04_Dicegame
{
    internal class Program
    {
        public static int tileMapSize=20;
        public static int TotalDice=20;
        public static int currentIndex=1;
        public static int previousTileIndex=1;
        public static int currentStar=0;

        static void Main(string[] args)
        {
            TilemMap tilemMap = new TilemMap(tileMapSize);

            int currentDice = TotalDice;

            while (currentDice > 0)
            {
                int diceNun = RollDice();

                previousTileIndex = currentIndex;
                currentIndex += diceNun;

                int passNum = currentIndex / 5 - previousTileIndex / 5;
                for (int i = 0; i < passNum ; i++)
                {

                    int starTileINdex = (currentIndex / 5 - i) * 5;
                    starTileINdex = starTileINdex > tileMapSize ? starTileINdex - tileMapSize : starTileINdex;

                    TileInfo ifstar = tilemMap.TileMap.GetValueOrDefault(starTileINdex);
                    TileInfo_Star info_Star = ifstar as TileInfo_Star;
                    if (info_Star != null)
                    {
                        currentStar += info_Star.starValue;
                    }
                }


                currentIndex = currentIndex > tileMapSize ? currentIndex - tileMapSize : currentIndex;

                tilemMap.TileMap.GetValueOrDefault(currentIndex).OnTileEvent();
                Console.WriteLine($"현재 샛별의 개수 : {currentStar}");
                currentDice--;
                Console.WriteLine($"남은 주사위의 개수 : {currentDice}");
            }

            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("주사위를 전부사용하여 게임이 종료되었습니다");
            Console.WriteLine($"획득한 샛별 총 개수 : {currentStar}");

        }

        public static int RollDice()
        {
            Random random = new Random();
            string input = null;
            while (input != "")
            {
                Console.WriteLine("엔터를 눌러 주사위를 굴립니다");
                input = Console.ReadLine();
            }
            int dicecount = random.Next(1, 6 + 1);
            //int dicecount = 6;
            DisplayDice(dicecount);
            Console.WriteLine($"주사위를 굴려 눈금은 {dicecount}가 나왔습니다.");

            return dicecount;
        }

        public static void DisplayDice(int diceNun)
        {

            switch (diceNun)
            {
                case 1:
                    Console.WriteLine("┌───────────┐");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│     ●    │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("└───────────┘");
                    break;
                case 2:
                    Console.WriteLine("┌───────────┐");
                    Console.WriteLine("│         ●│");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│ ●        │");
                    Console.WriteLine("└───────────┘");
                    break;

                case 3:
                    Console.WriteLine("┌───────────┐");
                    Console.WriteLine("│         ●│");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│     ●    │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│ ●        │");
                    Console.WriteLine("└───────────┘");
                    break;

                case 4:
                    Console.WriteLine("┌───────────┐");
                    Console.WriteLine("│ ●      ●│");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│ ●      ●│");
                    Console.WriteLine("└───────────┘");
                    break;
                case 5:
                    Console.WriteLine("┌───────────┐");
                    Console.WriteLine("│ ●      ●│");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│     ●    │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│ ●      ●│");
                    Console.WriteLine("└───────────┘");
                    break;
                case 6:
                    Console.WriteLine("┌───────────┐");
                    Console.WriteLine("│ ●      ●│");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│ ●      ●│");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│ ●      ●│");
                    Console.WriteLine("└───────────┘");
                    break;
                default:
                    break;
            }

        }
    }
}
