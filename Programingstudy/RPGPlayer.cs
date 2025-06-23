using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Programingstudy
{
    internal class RPGPlayer
    {
        //플레이어가 몬스터를 잡으면 경험치를 얻는다. 경험치가 가득 차면 레벨업을 한다
        //데이터 : 플레이어의 경험치, 몬스터의 경험치, 플레이어의 레벨
        //알고리즘 : 플레이어가 몬스터를 잡으면 경험치를 얻는다 -> 몬스터에게 경험치가 있어야 한다 -> 몬스터를 죽이면 몬스터의 경험치를 가져온다. -> 몬스터의 경험치를 가져온다 -> 지정된 대상(몬스터)의 경험치를 플레이어가 가져온다.
        //          가져온 경험치가 가득 차면 레벨업을 한다. 
        public static void BattleMain(Player player, Player monster)
        {
            
            while (!player.Death() && !monster.Death())
            {
                if (player.Death() == false)
                {
                    Console.WriteLine("=========Player Trun===========");
                    player.Attack(monster);
                    player.Display();
                    monster.Display();
                }
                else
                    Console.WriteLine("Player Death!");

                if (monster.Death() == false)
                {
                    Console.WriteLine("========Monster Trun===========");
                    monster.Attack(player);
                    player.Display();
                    monster.Display();
                }
                else
                {
                    Console.WriteLine("Monster Death!");
                    player.stillEXP(monster);
                    player.LevelUP();
                    player.Display();
                }


                Console.WriteLine("==============================");

               
            }
        }

        public static void ClassPlayerBattleMain()
        {
            Player player = new Player("Player", 20, 10);
            Player monster = new Player("Monster", 20, 10);

            while (!player.Death() && !monster.Death())
            {
                if (player.Death() == false)
                {
                    Console.WriteLine("=========Player Trun===========");
                    player.Attack(monster);
                    player.Display();
                    monster.Display();
                }
                else
                    Console.WriteLine("Player Death!");

                if (monster.Death() == false)
                {
                    Console.WriteLine("=========Monster Trun===========");
                    monster.Attack(player);
                    player.Display();
                    monster.Display();

                }
                else
                {
                    Console.WriteLine("Monster Death!");
                    player.stillEXP(monster);
                    player.LevelUP();
                    player.Display();

                }
                
                Console.WriteLine("==============================");
            }
        }

        public static void MonsterSelectMain()
        {
            Console.WriteLine("이동할 장소를 입력하세요.(평원, 무덤, 던전, 계곡");

            string strInput = Console.ReadLine();

            int nMonsterAtk = 10;
            int nMonsterHP = 100;
            string strMonster = "none";

            switch (strInput)
            {
                case "평원":
                    Console.WriteLine("슬라임이 출연합니다");
                    strMonster = "슬라임";
                    nMonsterAtk = 5;
                    nMonsterHP = 20;
                    break;
                case "무덤":
                    Console.WriteLine("스켈레톤이 출연합니다");
                    strMonster = "스켈레톤";
                    nMonsterAtk = 10;
                    nMonsterHP = 30;
                    break;
                case "던전":
                    Console.WriteLine("좀비가 출연합니다");
                    strMonster = "좀비";
                    nMonsterAtk = 20;
                    nMonsterHP = 50;
                    break;
                case "계곡":
                    Console.WriteLine("드래곤이 출연합니다");
                    strMonster = "드래곤";
                    nMonsterAtk = 50;
                    nMonsterHP = 200;
                    break;
                default:
                    Console.WriteLine("장소를 잘못입력하였습니다");
                    break;
            }

            Player player = new Player("Player", 20, 10);
            Player monster = new Player(strMonster, nMonsterHP, nMonsterAtk);

            BattleMain(player, monster);
        }

        public static void SelectFieldBattleMain()
        {
            List<Player> listMonster = new List<Player>();

            listMonster.Add(new Player("Slime", 5, 20, 20));
            listMonster.Add(new Player("Skeleton", 10, 30));
            listMonster.Add(new Player("Zombie", 20, 50));
            listMonster.Add(new Player("Dragon", 50, 200));
            
            int nSelectIdx = -1;

            Player player = new Player("Player", 20, 10, 20, 1, 30);


            while (true)
            {
                Console.WriteLine("이동할 장소를 입력하세요.(평원, 무덤, 던전, 계곡)");

                string strInput = Console.ReadLine();

                switch (strInput)
                {
                    case "평원":
                        Console.WriteLine("슬라임이 출연합니다.");
                        nSelectIdx = 0;
                        break;
                    case "무덤":
                        Console.WriteLine("스켈레톤 출연합니다.");
                        nSelectIdx = 1;
                        break;
                    case "던전":
                        Console.WriteLine("좀비 출연 합니다.");
                        nSelectIdx = 2;
                        break;
                    case "계곡":
                        Console.WriteLine("드래곤이 출연합니다");
                        nSelectIdx = 3;
                        break;
                    default:
                        Console.WriteLine("장소를 잘못입력했습니다.");
                        break;
                }

                Player monster = listMonster[nSelectIdx];
                monster.Health();
                BattleMain(player, monster);

                if(player.Death())
                {
                    Console.WriteLine("Game Over!");
                    break;
                }

                if (listMonster[3].Death())
                {
                    Console.WriteLine("The End!");
                    break;
                }
            }
        }

    }
    class Player
    {
        public string Name { get; set; }
        int nAtk;
        int nHP;
        int nMaxHP;
        int nLevel;
        int nEXP;
        int nMAXEXP;

        public Player(string name, int hp = 100, int atk = 10, int exp = 20, int level = 1, int mxp = 30)
        {
            Name = name;
            nAtk = atk;
            nHP = hp;
            nMaxHP = hp;
            nEXP = exp;
            nLevel = level;
            nMAXEXP = mxp;
        }

        public void Attack(Player target)
        {
            target.nHP = target.nHP - this.nAtk;
            //target.nHP -= this.nAtk; 윗줄과 의미가 같다
        }

        public bool Death()
        {
            if (this.nHP <= 0)
                return true;
            else
                return false;
        }

        public void Display(string msg = "")
        {
            Console.WriteLine(msg);
            Console.WriteLine(Name + "Atk/HP" + this.nAtk + "/" + this.nHP + "EXP:" + this.nEXP + "Lv" + this.nLevel);
        }

        public void stillEXP(Player target)
        {
            this.nEXP = this.nEXP + target.nEXP;
            Console.WriteLine("stillEXP");
        }

        public void LevelUP()
        {
            if (this.nEXP >= 30)
            {
                this.nLevel += 1;
                this.nEXP -= 30;
                this.nHP += 20;
                this.nAtk += 20;
                Console.WriteLine("LEVEL UP!");
            }
        }

        public void Health()
        {
            if (nHP <= 0)
            {
                if (Name == "Slime")
                    nHP = 5;
                else if (Name == "Skeleton")
                    nHP = 30;
                else if (Name == "Zombie")
                    nHP = 50;
                else if (Name == "Dragon")
                    nHP = 200;
            }
        }
    }
    //플레이어가 레벨업한 것이 초기화됐던 이유 ; while문 밑에 플레이어 선언이 있었기 때문 -> 플레이어 선언을 while문 위에 두는 것으로 해결. while문 안에 있으면 평원에 몬스터를 잡고 무덤으로 가면 매번 플레이어가 새로 호출됨
    //몬스터가 죽고 다시 출연하지 않는 문제 : 몬스터가 죽어있기 때문에 출연하지 않음 -> 몬스터가 죽었다는 의미 -> 몬스터의 HP가 0보다 작거나 같을 때 죽음 -> 몬스터의 피를 복구(초기화)시켜야함
    //해결방법 1. switch문 안에 피를 복구시키는 함수를 넣는다 -> 몬스터 HP 선언 함수를 Public화 시키고 HP를 넣어준다.
    //해결방법 2. 몬스터 HP를 복구하는(몬스터를 초기화시키는) 함수를 새로 생성한다.
}