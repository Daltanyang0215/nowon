using System;
using System.Collections.Generic;

namespace DiecGame
{
    internal class Program
    {
        static int totalTileCount = 20;
        static int totalDice = 20;
        static int currentStart = 0;
        static int currentTlineIndex = 1;
        static int previousTlineIndex = 1;

        static Random random = new Random();

        static void Main(string[] args)
        {
            TileMap tileMap = new TileMap();
            tileMap.setTlieMap(totalTileCount);

            int currentdice = totalDice;
            while(currentdice > 0)
            {
                int dicedata = RollDice();
                currentdice--;

                previousTlineIndex = currentTlineIndex;
                currentTlineIndex += dicedata;

                if(currentTlineIndex / 5 > previousTlineIndex / 5)
                {
                    for (int i= previousTlineIndex / 5  ; i < currentTlineIndex / 5; i++)
                    {
                        int index = i;
                        switch (index)
                        {
                            case 4:
                                index = 0;
                                break;
                            case 5:
                                index = 5;
                                break;
                            default:
                                index = i * 5;
                                break;
                        }
                        TileInfo passedStarTileInfo = tileMap.tiles.GetValueOrDefault(index); // 지나온 샛별칸의 TileInfo 가져오기
                        TileInfo_Star passedStarTileInfo_Star = passedStarTileInfo as TileInfo_Star; // TileInfo 타입을 TileInfo_Star 로 인식하겠다.
                        if (passedStarTileInfo_Star != null) // 샛별칸의 TileInfo 정보를 가져오는데 성공했으면
                        {
                            currentStart += passedStarTileInfo_Star.getStars; // 샛별점수 누적
                        }
                    }
                }

                if (currentTlineIndex >= totalTileCount) // 현재칸이 최대칸을 넘어가 버렸을때
                {
                    currentTlineIndex -= totalTileCount; // 현재칸에다가 최대칸 을 뺀다.
                }

                tileMap.tiles.GetValueOrDefault(currentTlineIndex).TileEvent();
                Console.WriteLine($"현재 총 별 수 : {currentStart}");
                Console.WriteLine($"현재 남은 주사위 수 : {currentdice}");

            }

            Console.WriteLine("게임 종료");
            Console.WriteLine("모든 주사위를 사용하셨습니다");
            Console.WriteLine($"총 별 개수 : {currentStart}");

        }

        static public int RollDice()
        {
            string userInput = "Default";
            while (userInput != "")
            {
                Console.WriteLine("엔터를 눌러 주사위를 굴리세요");
                userInput = Console.ReadLine();
            }
            int dicevaule = random.Next(1, 6 + 1);
            
            DisplayDice(dicevaule);
            return dicevaule;
        }

        static void DisplayDice(int diceValue)
        {
            switch (diceValue)
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
                    Console.WriteLine("│ ●        │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│         ●│");
                    Console.WriteLine("└───────────┘");
                    break;
                case 3:
                    Console.WriteLine("┌───────────┐");
                    Console.WriteLine("│ ●        │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│     ●    │");
                    Console.WriteLine("│           │");
                    Console.WriteLine("│         ●│");
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
