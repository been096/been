using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programingstudy
{
    internal class MonsterSelectMain
    {
       public static void Monsterselect()
        {
            Console.WriteLine("이동할 장소를 입력하세요.(평원, 무덤, 던전, 계곡");

            string strInput = Console.ReadLine();

            int nMonsterAtk = 10;
            int nMonsterHP = 100;
            string strmonster = "none";

            switch (strInput)
            {
                case "평원":
                    Console.WriteLine("슬라임이 출연합니다.");
                    strmonster = "슬라임";
                    nMonsterAtk = 5;
                    nMonsterHP = 20;
                    break;
                case "무덤":
                    Console.WriteLine("스켈레톤이 출연합니다.");
                    strmonster = "스켈레톤";
                    nMonsterAtk = 10;
                    nMonsterHP = 30;
                    break;
                case "던전":
                    Console.WriteLine("좀비가 출연합니다.");
                    strmonster = "좀비";
                    nMonsterAtk = 20;
                    nMonsterHP = 50;
                    break;
                case "계곡":
                    Console.WriteLine("드래곤이 출연합니다.");
                    strmonster = "드래곤";
                    nMonsterAtk = 50;
                    nMonsterHP = 200;
                    break;
                default:
                    Console.WriteLine("장소를 잘못 입력했습니다.");
                    break;
            }
        }
    }
}
