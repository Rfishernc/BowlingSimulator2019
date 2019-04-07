using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingPin
{
    public class FrameScorer
    {
        public List<FrameScore> FrameList = new List<FrameScore>();
        public int CurrentFrame = 0;
        public int RemainingPins = 10;

        public int GenerateBowl(int remainingPins)
        {
            return new Random().Next(0, remainingPins + 1);
        }

        public int EnterScores(string score)
        {
            return Int32.Parse(score);
        }

        public void ScoreBowl(bool manual)
        {
            while (CurrentFrame < 10)
            {
                Bowl(manual);
                StrikeAdder();
                SpareAdder();
                CurrentFrame += 1;
                RemainingPins = 10;      
            }
            FinalFramer();
            FinalScoreDisplayer();
        }

        public void Bowl(bool manual)
        {
            if (manual)
            {
                Console.WriteLine($"Enter your first bowl score for frame {CurrentFrame + 1}");
            }
            var bowl1 = manual ? OptionChooser() : GenerateBowl(RemainingPins);
            RemainingPins -= bowl1;
            var frame = new FrameScore() { Bowl1 = bowl1 };
            if (RemainingPins > 0)
            {
                if (manual)
                {
                    Console.WriteLine($"Enter your second bowl score for {CurrentFrame + 1}");
                }
                var bowl2 = manual ? OptionChooser() : GenerateBowl(RemainingPins);
                frame.Bowl2 = bowl2;
                RemainingPins -= bowl2;
                if (RemainingPins == 0)
                {
                    frame.Spare = true;
                }
                else
                {
                    frame.TotalScore = 10 - RemainingPins;
                }
            }
            else
            {
                frame.Strike = true;
            }
            FrameList.Add(frame);
        }

        public void StrikeAdder()
        {
            if (FrameList.Count > 1 && FrameList[CurrentFrame - 1].Strike && !FrameList[CurrentFrame].Strike)
            {
                FrameList[CurrentFrame - 1].TotalScore = 20 - RemainingPins;
            }
            if (FrameList.Count > 2 && FrameList[CurrentFrame - 2].Strike && FrameList[CurrentFrame - 1].Strike)
            {
                FrameList[CurrentFrame - 2].TotalScore = 30 - RemainingPins;
            }
        }

        public void SpareAdder()
        {
            if (FrameList.Count > 1 && FrameList[CurrentFrame - 1].Spare)
            {
                FrameList[CurrentFrame - 1].TotalScore = 10 + FrameList[CurrentFrame].Bowl1;
            }
        }

        public void FinalFramer()
        {
            if (FrameList[9].Spare)
            {
                FrameList[9].TotalScore = 20 - GenerateBowl(RemainingPins);
            }
            if (FrameList[9].Strike)
            {
                var finalRolls = GenerateBowl(RemainingPins);
                RemainingPins -= finalRolls;
                if (RemainingPins > 0)
                {
                    finalRolls += GenerateBowl(RemainingPins);
                }
                else
                {
                    finalRolls += GenerateBowl(10);
                }
                FrameList[9].TotalScore = 10 + finalRolls;
            }
        }

        public int Totalizer()
        {
            var total = 0;
            foreach(FrameScore frame in FrameList)
            {
                total += frame.TotalScore;
            }
            return total;
        }

        public int OptionChooser()
        {
            var input = Console.ReadLine();
            if (input == "s" || input == "S")
            {
                ScoreDisplayer();
                return EnterScores(Console.ReadLine());
            } else
            {
                return EnterScores(input);
            }
        }

        public void ScoreDisplayer()
        {
            var frameCounter = 0;
            Console.WriteLine("Your current frame scores:");
            foreach (FrameScore frame in FrameList)
            {
                frameCounter++;
                Console.WriteLine($"Frame {frameCounter}: {frame.TotalScore}--{frame.Bowl1}--" +
                    $"{(frame.Strike || frame.Spare ? (frame.Strike ? "X" : "/") : $"{frame.Bowl2}")}");
            }
            Console.WriteLine($"Total  score: {Totalizer()}");
            Console.WriteLine("Please enter your next score.");
        }

        public void FinalScoreDisplayer()
        {
            var frameCounter = 0;
            foreach (FrameScore frame in FrameList)
            {
                frameCounter++;
                Console.WriteLine($"Frame {frameCounter}: {frame.TotalScore}--{frame.Bowl1}--" +
                    $"{(frame.Strike || frame.Spare ? (frame.Strike ? "X" : "/") : $"{frame.Bowl2}")}");
            }
            Console.WriteLine($"Total  score: {Totalizer()}");
        }
    }
}
