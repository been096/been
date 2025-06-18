using Microsoft.VisualBasic.FileIO;

//namespace Practice
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello, World!");
//            //CritcalAttackMain();
//        }

//        //플레이어가 공격을 할 때 일정확률로 크리티컬이 터진다.
//        //플레이어가 가져야 하는 것. 공격력, 크리티컬 확률, 크리티컬 증가 데미지
//        //몬스터가 가져야 하는 것. 체력
//        //변수 : 공격력, 크리티컬 확률, 크리티컬 증가 데미지, 체력
//        //알고리즘 : 플레이어의 공격력이 크리티컬이 터지면 몬스터의 체력을 공격력*크리티컬수치만큼 깍는다(체력 - (공격력 * 크리티컬 수치)
//        //          플레이어의 공격력이 크리티컬이 터지지 않으면 공격력만큼 몬스터의 체력을 깎는다.(체력 - 공격력)
//        //변수 값을 설정해준다.
//        static void CritcalAttackMain()
//        {
//            int nPlayerAttck = 10;
//            int nMonsterHP = 100;
//            Random cRandom = new Random();
//            int nRandom = cRandom.Next(1, 3);

//            if (nRandom == 1)
//            {
//                nPlayerAttck = (int)(nPlayerAttck * 1.5f);
//                Console.WriteLine("크리티컬데미지!" + nPlayerAttck);
//            }
//            else
//            {
//                nPlayerAttck = 10;
//                Console.WriteLine("데미지!" + nPlayerAttck);
//            }

//            nMonsterHP = nMonsterHP - nPlayerAttck;

//            Console.WriteLine("남은 체력" + nMonsterHP);
//            Console.WriteLine("랜덤값" + nRandom);
//        }
//    }
//}

namespace Programingstudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World! 박원빈");
            //Console.WriteLine("Add:" + Add(10, 20));
            //ValMain();
            //PlayerAttackMain();
            //CritcalAttackMain();//함수의 호출(사용)
            //StageMain();
            //AttackWhile();
            //MonsterListMain();
            PlayerbattleMain();
        }

        static int Add(int a, int b)
        {
            int c = a + b;
            return c;
        }

        static void ValMain()
        {
            int nScore = 0;
            float fRat = 1.0f / 4.0f;
            Console.WriteLine("score:" + nScore);
            Console.WriteLine("Rat:" + fRat);
        }
        //몬스터가 플레이어를 공격한다. ㅡ>몬스터가 공격했을 때 (플레이어의 피)가 깍인다.
        //몬스터는 공격력 < 플레이어는 체력 : 공격력으로 피를 깍는 행위
        //데이터 : 바뀌는 것. 몬스터의 공격력, 플레이어의 체력
        //알고리즘 : 몬스터의 공격력으로 플레이어의 체력을 깍는다.(체력 - 공격력)
        //값을 설정하지 않아서 작동하지 않는다. 각 값을 공격력 10, 체력 100으로 설정한다.
        // 남은 수치가 잘 까졌는지를 확인하기 위해서 이전수치를 보여주는 것도 좋다.
        static void PlayerAttackMain()
        {
            int nPlayerHP = 10;
            int nMonsterAttack = 100;
            Console.WriteLine("남은 hp" + nPlayerHP);
            nPlayerHP = nPlayerHP - nMonsterAttack;
            Console.WriteLine("남은 hp" + nPlayerHP);
        }

        //내가 작성한 것
        //플레이어가 공격을 할 때 일정확률로 크리티컬이 터진다.
        //플레이어가 가져야 하는 것. 공격력, 크리티컬 확률, 크리티컬 증가 데미지
        //몬스터가 가져야 하는 것. 체력
        //데이터 : 공격력, 크리티컬 확률, 크리티컬 증가 데미지, 체력
        //알고리즘 : 플레이어의 공격력이 크리티컬이 터지면 몬스터의 체력을 공격력*크리티컬수치만큼 깍는다(체력 - (공격력 * 크리티컬 수치)
        //          플레이어의 공격력이 크리티컬이 터지지 않으면 공격력만큼 몬스터의 체력을 깎는다.(체력 - 공격력)
        //변수 값을 설정해준다.

        //강사님이 작성한것
        //플레이어가 (몬스터를) 공격을 할 때 일정확률로 크리티컬이 터진다.
        //플레이어가 공격 -> 플레이어의 공격력, 몬스터의 체력이 필요하다.
        //데이터 : 플레이어의 공격력, 몬스터의 체력
        //알고리즘 : 플레이어가 몬스터를 공격하는데, 일정확률로 크리컬이 발생한다.
        //일정확률? -> 플레이어가 몬스터를 공격한다. -> 때릴때 일정확률로 크리티컬이 발생하고, 데미지가 1.5배가 된다.
        static void CritcalAttackMain()
        {
            Console.WriteLine("CritcalAttackMain");
            int nPlayerAtk = 10;
            int nMonsterHP = 100;

            Random cRandom = new Random();
            //int nRandom = cRandom.Next(1,3);//1/3의 확률로 랜덤값이 나온다. -> 엄밀히 따지면 1~2의 값이 나온다. -> 1~2의 값이 나온다. 1/2
            int nRandom = cRandom.Next(0, 3);//1/3의 확률로 랜덤값이 나온다. -> 엄밀히 따지면 1~3의 값이 나온다. -> 0,1,2의 값이 나온다 1/3
            Console.WriteLine("플레이어의 공격력:" + nPlayerAtk + "남은 hp:" + nMonsterHP);
            if (nRandom == 1)// =은 대입, 값을 때려넣는거고 ==은 
            {
                Console.WriteLine("크티티컬 어택!");
                nMonsterHP = nMonsterHP - (int)((float)(nPlayerAtk * 1.5f));
            }
            else
                nMonsterHP = nMonsterHP - nPlayerAtk;
            Console.WriteLine("플레이어의 공격력:" + nPlayerAtk + "남은 hp:" + nMonsterHP);
            Console.WriteLine("랜덤값:" + nRandom);
        }
        //마을, 상점, 필드 중에서 이동장소를 입력하면 그 장소의 이름이 나오는 프로그램 작성.
        //데이터 : 마을, 상점, 필드 입력값
        //알고리즘 :  입력값 안내를 표시하는 메세지를 먼저 출력하고, 입력값이 마을이면, 마을입니다. 라고 출력하고 필드면... 상점...

        static void StageMain()
        {
            Console.WriteLine("StageMain");
            string strTown = "마을";
            string strField = "필드";
            string strStore = "상점";
            Console.WriteLine("가고싶은곳을 입력하세요! (마을, 상점, 필드)");
            string strStage = Console.ReadLine();

            switch (strStage)
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

            ////if(strStage == strTown)
            //{
            //    Console.WriteLine("마을 입니다.");
            //}

            //else if (strStage == strField)
            //{
            //    Console.WriteLine("필드 입니다.");
            //}
            //else if (strStage == strStore)
            //{
            //    Console.WriteLine("상점 입니다.");
            //}
        }

        //플레이어가 몬스터를 (죽을때까지 : 몬스터의 hp가 0이 될 때) 공격한다.
        //
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
            for (int i = 0; i < listMonster.Count; i++)
                Console.WriteLine(String.Format("monster[{0}]: {1}", i, listMonster[i]));
        }

        static void PlayerbattleMain()
        {
            int nPlayerAtk = 10;
            int nPlayerHP = 50;
            int nMonsterAtk = 5;
            int nMonsterHP = 100;
            Random cRandom = new Random();
            int nRandom = cRandom.Next(0, 3);

           

            Console.WriteLine("플레이어 데미지" + nPlayerAtk +  "\n남은 몬스터 체력" + nMonsterHP);
            nMonsterHP -= nPlayerAtk;
            Console.WriteLine("플레이어 데미지" + nPlayerAtk + "\n남은 몬스터 체력" + nMonsterHP);

            if (nRandom == 1)
            {
                Console.WriteLine("크티티컬 어택!" + (nPlayerAtk * 1.5));
                nMonsterHP -= (int)(nPlayerAtk * 1.5);
                Console.WriteLine("남은 몬스터 체력" + nMonsterHP);
            }
            else if (nRandom == 2)
            {
                Console.WriteLine("미스!");
            }
            else
            {
                nMonsterHP -= nPlayerAtk;
                Console.WriteLine("플레이어 데미지:" + nPlayerAtk + "\n남은 몬스터 체력:" + nMonsterHP);
            }
            Console.WriteLine("랜덤값:" + nRandom);

            Console.WriteLine("몬스터 데미지" + nMonsterAtk +  "\n남은 플레이어 체력" + nPlayerHP);
            nPlayerHP -= nMonsterAtk;
            Console.WriteLine("몬스터 데미지" + nMonsterAtk + "\n남은 플레이어 체력" + nPlayerHP);
        }

    }
}
