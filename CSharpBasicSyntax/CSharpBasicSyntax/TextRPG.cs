using System.Numerics;

namespace CSharpBasicSyntax
{
    internal class TextRPG
    {
        enum ClassType {
            None = 0,
            Knight,
            Archer,
            Mage
        }

        enum MonsterType {
            None = 0,
            Slime,
            Orc,
            Skeleton
        }

        struct Player
        {
            public int hp;
            public int attack;
        }

        struct Monster
        {
            public int hp;
            public int attack;
        }

        static ClassType ChooseClass()
        {
            Console.WriteLine("직업을 선택하세요.");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 궁수");
            Console.WriteLine("[3] 법사");

            string input = Console.ReadLine();
            ClassType choice = ClassType.None;

            switch (input)
            {
                case "1":
                    choice = ClassType.Knight;
                    break;
                case "2":
                    choice = ClassType.Archer;
                    break;
                case "3":
                    choice = ClassType.Mage;
                    break;
            }

            return choice;
        }

        static void CreatePlayer(ClassType choice, out Player player)
        {
            switch (choice)
            {
                case ClassType.Knight:
                    player.hp = 100;
                    player.attack = 10;
                    break;
                case ClassType.Archer:
                    player.hp = 75;
                    player.attack = 12;
                    break;
                case ClassType.Mage:
                    player.hp = 50;
                    player.attack = 15;
                    break;
                default:
                    player.hp = 0;
                    player.attack = 0;
                    break;
            }
        }

        static void CreateRandomMonster(out Monster monster)
        {
            Random random = new Random();
            int randMonster = random.Next(1, 4);

            switch (randMonster)
            {
                case (int)MonsterType.Slime:
                    Console.WriteLine("슬라임이 스폰되었습니다.");
                    monster.hp = 20;
                    monster.attack = 2;
                    break;
                case (int)MonsterType.Orc:
                    Console.WriteLine("오크가 스폰되었습니다.");
                    monster.hp = 40;
                    monster.attack = 4;
                    break;
                case (int)MonsterType.Skeleton:
                    Console.WriteLine("스켈레톤이 스폰되었습니다.");
                    monster.hp = 30;
                    monster.attack = 3;
                    break;
                default:
                    monster.hp = 0;
                    monster.attack = 0;
                    break;
            }
        }

        static void Fight(ref Player player, ref Monster monster)
        {
            while(true)
            {
                monster.hp -= player.attack;
                if(monster.hp <= 0)
                {
                    Console.WriteLine("승리했습니다.");
                    break;
                }

                player.hp -= monster.attack;
                if(player.hp <= 0)
                {
                    Console.WriteLine("패배했습니다.");
                    break;
                }
            }
        }

        static void EnterField(ref Player player)
        {
            while(true)
            {
                Console.WriteLine("필드에 접속했습니다.");

                Monster monster;
                CreateRandomMonster(out monster);

                Console.WriteLine("[1] 전투 모드로 돌입.");
                Console.WriteLine("[2] 일정 확률로 마을로 도망.");

                string input = Console.ReadLine();
                if (input == "1")
                    Fight(ref player, ref monster);
                else if (input == "2")
                {
                    Random random = new Random();
                    int randValue = random.Next(0, 100);
                    if (randValue <= 33)
                    {
                        Console.WriteLine("도망치는데 성공했습니다!");
                        break;
                    }
                    else
                        Fight(ref player, ref monster);
                }
            }
        }

        static void EnterGame(ref Player player)
        {
            while(true)
            {
                Console.WriteLine("게임에 접속했습니다!");
                Console.WriteLine("[1] 필드로 간다.");
                Console.WriteLine("[2] 로비로 돌아가기.");

                string input = Console.ReadLine();

                if (input == "1")
                    EnterField(ref player);
                else if (input == "2")
                    break;
            }
        }

        static void Main(string[] args)
        {
            ClassType choice = ClassType.None;

            while(true)
            {
                choice = ChooseClass();

                if (choice != ClassType.None)
                {
                    //캐릭터 생성 기사(100/10) 궁수(75/12) 법사(50/15)
                    Player player;
                    CreatePlayer(choice, out player);

                    EnterGame(ref player);
                }
            }

        }
    }
}
