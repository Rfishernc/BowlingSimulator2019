using System;

namespace BowlingPin
{
    class Program
    {
        static void Main(string[] args)
        {
            var newGame = new FrameScorer();
            newGame.ScoreBowl();
            Console.ReadLine();
        }
    }
}
