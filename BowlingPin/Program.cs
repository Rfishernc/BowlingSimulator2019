using System;

namespace BowlingPin
{
    class Program
    {
        static void Main(string[] args)
        {
            var newGame = new FrameScorer();
            Console.WriteLine("Do you want to manually enter your bowling scores or have them generated automatically?");
            Console.WriteLine("1. Manual");
            Console.WriteLine("2. Automatic");
            var input = Console.ReadLine();
            if (input == "1")
            {
                Console.WriteLine("Enter 'S' at any time to see your current scores.");
                newGame.ScoreBowl(true);
            }
            if (input == "2")
            {
                newGame.ScoreBowl(false);
            }
            Console.ReadLine();
        }
    }
}
