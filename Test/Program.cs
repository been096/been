using static Test.Program;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            solution(10);


        }
        public static int solution(int n)
        {
            int answer = 0;
            n = 10;
            for (int i = 0; i <= n; i++)
            {
                if (i % 2 == 0)
                    answer += i;
            }
            Console.WriteLine(answer);

            return answer;
        }
    }

}
