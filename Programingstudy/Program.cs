namespace Programingstudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World! 박원빈");
            Console.WriteLine("Add:" + Add(10, 20));
            ValMain();
            PlayerAttackMain();
            //CritcalAttackMain();//함수의 호출(사용)
            //StageMain();
            //AttackWhile();
            //MonsterListMain();
        }

        static int Add(int a, int b)
        {
            int c = a + b;
            return c;
        }

        static void ValMain ()
        {
            int nScore = 0;
            float fRat = 1.0f / 4.0f;
            Console.WriteLine("score:" + nScore);
            Console.WriteLine("Rat:" + fRat);
        }
        //몬스터가 플레이어를 공격한다. ㅡ>몬스터가 공격했을 때 (플레이어의 피)가 깍인다.
        //몬스터는 공격력 < 플레이어는 체력 : 공격력으로 피를 깍는 행위
        //변수 : 바뀌는 것. 몬스터의 공격력, 플레이어의 체력
        //알고리즘 : 몬스터의 공격력으로 플레이어의 체력을 깍는다.(체력 - 공격력)
        //값을 설정하지 않아서 작동하지 않는다. 각 값을 공격력 10, 체력 100으로 설정한다.
        // 남은 수치가 잘 까졌는지를 확인하기 위해서 이전수치를 보여주는 것도 좋다.
        static void PlayerAttackMain()
        {
            int nPlayerHP = 10;
            int nMOnsterAttack = 100;
            Console.WriteLine("남은 hp" + nPlayerHP);
            nPlayerHP = nPlayerHP - nMOnsterAttack;
            Console.WriteLine("남은 hp"+ nPlayerHP);
        }

        //플레이어가 공격을 할 때 일정확률로 크리티컬이 터진다.
        //플레이어가 가져야 하는 것. 공격력, 크리티컬 확률, 크리티컬 증가 데미지
        //몬스터가 가져야 하는 것. 체력
        //변수 : 공격력, 크리티컬 확률, 크리티컬 증가 데미지, 체력
        //알고리즘 : 플레이어의 공격력이 크리티컬이 터지면 몬스터의 체력을 공격력*크리티컬수치만큼 깍는다(체력 - (공격력 * 크리티컬 수치)
        //          플레이어의 공격력이 크리티컬이 터지지 않으면 공격력만큼 몬스터의 체력을 깎는다.(체력 - 공격력)
        //변수 값을 설정해준다.
        static void CritcalAttackMain()
        {
            Console.WriteLine("CritcalAttackMain");
            int nPlayerAttack = 10;
            int nMonsterHP = 100;
            Random cRandom = new Random();
            int nRandom = cRandom.Next(1, 3);

            if (nRandom == 1)
            {
                nPlayerAttack = (int)(nPlayerAttack * 1.5);
                Console.WriteLine("크리티컬데미지!" + nPlayerAttack);
            }
            else
                Console.WriteLine("데미지:" + nPlayerAttack);

            nMonsterHP = nMonsterHP - nPlayerAttack;

            Console.WriteLine("몬스터의 HP" + nMonsterHP);
            Console.WriteLine("랜덤값" + nRandom);
        }

        static void StageMain()
        {
            Console.WriteLine("StageMain");
            Console.WriteLine("가고싶은곳을 입력하세요! (마을, 상점, 필드)");
            string strStage = Console.ReadLine();

            switch(strStage)
            {
                case "마을":
                    Console.WriteLine("마을 입니다.");
                    break;
                case "상점":
                    Console.WriteLine("상점 입니다.");
                    break;
                case "필드":
                    Console.WriteLine("필드 입니다.");
                    break;
            }
        }

        static void AttackWhile()
        {
            Console.WriteLine("AttackWhile");
            int nPlayerDemage = 10;
            int nMonsterHP = 100000;

            while (nMonsterHP > 0)
            {
                nMonsterHP = nMonsterHP - nPlayerDemage;
                string msg = string.Format("몬스터가 데미지{0}를 HP가 {1}되었다.", nPlayerDemage, nMonsterHP);
                Console.WriteLine(msg);
            }
        }

        static void MonsterListMain()
        {
            Console.WriteLine("MonsterListMain");
            List<string> listMonster = new List<string>();

            listMonster.Add("슬라임");
            listMonster.Add("스켈레톤");
            listMonster.Add("좀비");
            listMonster.Add("드래곤");
            //첫번째 값은 [0]으로 접근하고, 마지막값은 몬스터수-1이 마지막 값이다.
            Console.WriteLine(listMonster[0]);
            Console.WriteLine(listMonster[3]);
            //console.WriteLine(listMonster[4]); //잘못된 접근을 하면 프로그램이 죽는다.

            //for문을 이용해 반복해서 출력한다.
            for (int i = 0;  i < listMonster.Count; i++)
                Console.WriteLine(String.Format("monster[{0}]: {1}", i, listMonster[i]));
        }
    }
}
