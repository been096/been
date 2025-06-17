namespace Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            CritcalAttackMain();
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
            int nPlayerAttck = 10;
            int nMonsterHP = 100;
            Random cRandom =  new Random();
            int nRandom = cRandom.Next(1, 3);

            if (nRandom == 1)
            {
                nPlayerAttck = (int)(nPlayerAttck * 1.5);
                Console.WriteLine("크리티컬데미지!" + nPlayerAttck);
            }
            else
            {
                nPlayerAttck = 10;
                Console.WriteLine("데미지!" + nPlayerAttck);
            }

            nMonsterHP = nMonsterHP - nPlayerAttck;

            Console.WriteLine("남은 체력" + nMonsterHP);
            Console.WriteLine("랜덤값" + nRandom);

        }
    }
}
