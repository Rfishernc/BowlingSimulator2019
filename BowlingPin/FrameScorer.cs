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

        public void ScoreBowl()
        {
            var game = new FrameScorer();

            while (game.CurrentFrame < 10)
            {
                game.Bowl(game);
                game.StrikeAdder(game);
                game.SpareAdder(game);
                game.CurrentFrame += 1;
                game.RemainingPins = 10;
                game.FinalFramer(game);
            }
            game.ScoreDisplayer(game);
        }

        public void Bowl(FrameScorer game)
        {
            var bowl1 = game.GenerateBowl(game.RemainingPins);
            game.RemainingPins -= bowl1;
            var frame = new FrameScore() { Bowl1 = bowl1 };
            if (game.RemainingPins > 0)
            {
                var bowl2 = game.GenerateBowl(game.RemainingPins);
                frame.Bowl2 = bowl2;
                game.RemainingPins -= bowl2;
                if (game.RemainingPins == 0)
                {
                    frame.Spare = true;
                }
                else
                {
                    frame.TotalScore = 10 - game.RemainingPins;
                }
            }
            else
            {
                frame.Strike = true;
            }
            game.FrameList.Add(frame);
        }

        public void StrikeAdder(FrameScorer game)
        {
            if (game.FrameList.Count > 1 && game.FrameList[game.CurrentFrame - 1].Strike && !game.FrameList[game.CurrentFrame].Strike)
            {
                game.FrameList[game.CurrentFrame - 1].TotalScore = 20 - game.RemainingPins;
            }
            if (game.FrameList.Count > 2 && game.FrameList[game.CurrentFrame - 2].Strike && game.FrameList[game.CurrentFrame - 1].Strike)
            {
                game.FrameList[game.CurrentFrame - 2].TotalScore = 30 - game.RemainingPins;
            }
        }

        public void SpareAdder(FrameScorer game)
        {
            if (game.FrameList.Count > 1 && game.FrameList[game.CurrentFrame - 1].Spare)
            {
                game.FrameList[game.CurrentFrame - 1].TotalScore = 10 + game.FrameList[game.CurrentFrame].Bowl1;
            }
        }

        public void FinalFramer(FrameScorer game)
        {
            if (game.CurrentFrame == 10)
            {
                if (game.FrameList[9].Spare)
                {
                    game.FrameList[9].TotalScore = 20 - game.GenerateBowl(game.RemainingPins);
                }
                if (game.FrameList[9].Strike)
                {
                    var finalRolls = game.GenerateBowl(game.RemainingPins);
                    game.RemainingPins -= finalRolls;
                    if (game.RemainingPins > 0)
                    {
                        finalRolls += game.GenerateBowl(game.RemainingPins);
                    }
                    else
                    {
                        finalRolls += game.GenerateBowl(10);
                    }
                    game.FrameList[9].TotalScore = 10 + finalRolls;
                }
            }
        }

        public int Totalizer(FrameScorer game)
        {
            var total = 0;
            foreach(FrameScore frame in game.FrameList)
            {
                total += frame.TotalScore;
            }
            return total;
        }

        public void ScoreDisplayer(FrameScorer game)
        {
            var frameCounter = 0;
            foreach (FrameScore frame in game.FrameList)
            {
                frameCounter++;
                Console.WriteLine($"Frame {frameCounter}: {frame.TotalScore}--{frame.Bowl1}--" +
                    $"{(frame.Strike || frame.Spare ? (frame.Strike ? "X" : "/") : $"{frame.Bowl2}")}");
            }
            Console.WriteLine($"Total game score: {game.Totalizer(game)}");
        }
    }
}
