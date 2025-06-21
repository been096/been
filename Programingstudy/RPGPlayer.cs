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
        public static void ClassPlayerbattleMain()
        {
            Player player = new Player("Player", 20, 10, 20, 1);
            Player monster = new Player("Monster", 20, 10, 20, 1);

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

    }
    class Player
    {
        public string Name { get; set; }
        int nAtk;
        int nHP;
        int nLevel;
        int nEXP;
        int nMAXEXP;

        public Player(string name, int hp = 100, int atk = 10, int exp = 20, int level = 1, int mxp = 30)
        {
            Name = name;
            nAtk = atk;
            nHP = hp;
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
            if (this.nEXP > 30)
            {
                this.nLevel += 1;
                this.nEXP -= 30;
                Console.WriteLine("LEVEL UP!");
            }
        }
    }
}
