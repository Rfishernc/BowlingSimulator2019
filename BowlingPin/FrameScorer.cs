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
                FrameList.Add(manual ? ManualBowl() : AutoBowl());
                StrikeAdder();
                SpareAdder();
                CurrentFrame += 1;
                RemainingPins = 10;      
            }
            FrameList[9].TotalScore = manual ? ManualFinalFramer(): AutoFinalFramer();
            Console.WriteLine(FinalScoreDisplayer());
        }

        public FrameScore ManualBowl()
        {
            Console.WriteLine($"Enter your first bowl score for frame {CurrentFrame + 1}");
            var bowl1 = OptionChooser(Console.ReadLine());
            RemainingPins -= bowl1;
            var frame = new FrameScore() { Bowl1 = bowl1 };
            if (RemainingPins > 0)
            {
                Console.WriteLine($"Enter your second bowl score for {CurrentFrame + 1}");
                var bowl2 = OptionChooser(Console.ReadLine());
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
            return frame;
        }

        public FrameScore AutoBowl()
        {
            var bowl1 = GenerateBowl(RemainingPins);
            RemainingPins -= bowl1;
            var frame = new FrameScore() { Bowl1 = bowl1 };
            if (RemainingPins > 0)
            {
                var bowl2 = GenerateBowl(RemainingPins);
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
            return frame;
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

        public int AutoFinalFramer()
        {
            var finalFrame = FrameList[9];
            if (finalFrame.Spare)
            {
                finalFrame.Bowl3 = GenerateBowl(RemainingPins);
                return 10 + finalFrame.Bowl3;
            }
            if (finalFrame.Strike)
            {
                finalFrame.Bowl2 = GenerateBowl(RemainingPins);
                RemainingPins -= finalFrame.Bowl2;
                if (RemainingPins > 0)
                {
                    finalFrame.Bowl3 = GenerateBowl(RemainingPins);
                }
                else
                {
                    finalFrame.Bowl3 = GenerateBowl(10);
                }
                return 10 + finalFrame.Bowl2 + finalFrame.Bowl3;
            }
            return finalFrame.TotalScore;
        }

        public int ManualFinalFramer()
        {
            var finalFrame = FrameList[9];
            if (finalFrame.Spare)
            {
                Console.WriteLine("Enter your third bowl for frame 10.");
                finalFrame.Bowl3 = OptionChooser(Console.ReadLine());
                return 10 + finalFrame.Bowl3;
            }
            if (finalFrame.Strike)
            {
                Console.WriteLine("Enter your second bowl for frame 10.");
                finalFrame.Bowl2 = OptionChooser(Console.ReadLine());
                Console.WriteLine("Enter your third bowl for frame 10.");
                finalFrame.Bowl3 += OptionChooser(Console.ReadLine());
                return 10 + finalFrame.Bowl2 + finalFrame.Bowl3;
            }
            return finalFrame.TotalScore;
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

        public int OptionChooser(string input)
        {
            if (input == "s" || input == "S")
            {
                Console.WriteLine(ScoreDisplayer());
                return EnterScores(Console.ReadLine());
            } else
            {
                return EnterScores(input);
            }
        }

        public string ScoreDisplayer()
        {
            var message = "";
            var frameCounter = 0;
            message += Environment.NewLine + "Your current frame scores:";
            foreach (FrameScore frame in FrameList)
            {
                frameCounter++;
                message += Environment.NewLine + $"Frame {frameCounter}: {frame.TotalScore}--{frame.Bowl1}--" +
                    $"{(frame.Strike || frame.Spare ? (frame.Strike ? "X" : "/") : $"{frame.Bowl2}")}";
            }
            message += Environment.NewLine + $"Total  score: {Totalizer()}";
            message += Environment.NewLine + Environment.NewLine + "Please enter your next score.";
            return message;
        }

        public string FinalScoreDisplayer()
        {
            var finalScore = Environment.NewLine + "";
            var frameCounter = 0;
            foreach (FrameScore frame in FrameList)
            {
                frameCounter++;
                finalScore += ($"Frame {frameCounter}: {frame.TotalScore}--{frame.Bowl1}--" +
                    $"{(frame.Strike || frame.Spare ? (frame.Strike ? "X" : "/") : $"{frame.Bowl2}")}");
                finalScore += Environment.NewLine;
            }
            finalScore += Environment.NewLine + ($"Total  score: {Totalizer()}");
            return finalScore;
        }
    }
}
