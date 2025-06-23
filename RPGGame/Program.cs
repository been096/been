using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame
{

    class Player
    {
        public string Name { get; set; }
        int nAtk;
        int nHP;

        public Player(string name, int hp = 100, int atk = 10)
        {
            Name = name;
            nAtk = atk;
            nHP = hp;
        }

        public void Attack(Player target)
        {
            target.nHP = target.nHP - this.nAtk;
            //target.nHP -= this.nAtk;
        }

        public bool Death()
        {
            if (this.nHP <= 0)
                return true;
            else
                return false;
        }
        //몬스터가 죽으면, 죽은몬스터로 부터 경험치를 가져온다.
        //데이터: 플레이어의 경험치, 몬스터의 경험치
        //알고리즘: 몬스터가 사망하면 플레어가 몬스터의 경험치를 가져온다.
        //렙업한다. 만약 경험치가 최대 경험치가되면 
        //데이터: 경험치,렙업,최대 경험치
        //알고리즘: 만약 경험치가 최대경험치보다 크다면, 레벨이 1 오른다.
        public void StillExp(Player target)
        {

        }

        public void Display(string msg = "")
        {
            Console.WriteLine(msg);
            Console.WriteLine(Name + " Atk/HP:" + this.nAtk + "/" + this.nHP);
        }
    }


    internal class Program
    {

        public static void BattleMain(Player player, Player monster)
        {
            //플레이어가 살아있을때(죽었을때가 아닐때), 그리고 몬스터가 살아있을때(죽었을떄가 아닐때)
            //플레이어가 살아있고, 몬스터가 살아있을때 -> 플레이어도 몬스터도 살아있다.
            //아래 while 코드를 봤을 때에 [플레이어도 몬스터도 살아있다] 라는 글이 떠오르는 것이 최종목표
            //플레이어도 몬스터도 살아있을 때 반복한다
            while (!player.Death() && !monster.Death())
            {
                //플레이어가 죽었을 때가 아닐 때 -> 플레이어가 살아있을때
                if (player.Death() == false)
                {
                    Console.WriteLine("=========Player Trun===========");
                    //플레이어가 몬스터를 공격하고, 플레이어와 몬스터의 능력치를 확인했다.
                    player.Attack(monster); // 플레이어가 몬스터를 공격한다.
                    player.Display(); // 플레이어의 능력치를 확인(출력)한다.
                    monster.Display(); // 몬스터의 능력치를확인(출력)한다.
                }
                else // 플레이어가 죽었을때가 아닐때가 아닐때 -> 플레이어가 죽었을 때
                    Console.WriteLine("Player Death!");
                //몬스터가 죽었을때가 아닐때 -> 몬스터가 살아있을 때
                if (monster.Death() == false)
                {
                    Console.WriteLine("=========Monster Trun===========");
                    //몬스터가 플레이어를 공격하고, 플레이어와 몬스터의 능력치를 확인한다.
                    monster.Attack(player); //몬스터가 플레이어를 공격한다.
                    player.Display(); // 플레이어의 능력치를 확인(출력)한다.
                    monster.Display(); // 몬스터의 능력치를 확인(출력)한다.

                }
                else // 몬스터가 죽었을 때가 아닐때가 아닐때 -> 몬스터가 죽었을 때
                    Console.WriteLine("Monster Death!");

                Console.WriteLine("==============================");
            }
        }

        public static void MonsterSelectMain()
        {
            Console.WriteLine("이동 할 장소를 입력하세요.(평원,무덤,던전,계곡)");

            string strInput = Console.ReadLine();

            int nMonsterAtk = 10;
            int nMonsterHP = 100;
            string strMonster = "none";

            switch (strInput)
            {
                case "평원":
                    Console.WriteLine("슬라임이 출연합니다.");
                    strMonster = "슬라임";
                    nMonsterAtk = 5;
                    nMonsterHP = 20;
                    break;
                case "무덤":
                    Console.WriteLine("스켈레톤 출연합니다.");
                    strMonster = "스켈레톤";
                    nMonsterAtk = 10;
                    nMonsterHP = 30;
                    break;
                case "던전":
                    Console.WriteLine("좀비 출연 합니다.");
                    strMonster = "좀비";
                    nMonsterAtk = 20;
                    nMonsterHP = 50;
                    break;
                case "계곡":
                    strMonster = "드래곤";
                    Console.WriteLine("드래곤이 출연 합니다.");
                    nMonsterAtk = 50;
                    nMonsterHP = 200;
                    break;
                default:
                    Console.WriteLine("장소를 잘못입력했습니다.");
                    break;
            }

            Player player = new Player("Player", 20, 10);
            Player monster = new Player(strMonster, nMonsterHP, nMonsterHP);

            BattleMain(player, monster);
        }

        static void Main(string[] args)
        {
        }
    }
}
