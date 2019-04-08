using BowlingPin;
using System;
using Xunit;

namespace BowlingPinTests
{
    public class BowlingTests
    {
        [Fact]
        public void entering_a_score_into_option_chooser_should_return_a_parsed_copy_of_that_score()
        {
            var game = new FrameScorer();
            var input = "8";
            var expectedResult = 8;

            var actualResult = game.OptionChooser(input);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void bowling_a_strike_2_frames_in_a_row_will_add_the_first_roll_of_the_next_frame_to_the_first_strikes_frame()
        {
            var game = new FrameScorer() { CurrentFrame = 2, RemainingPins = 5 };
            var frame1 = new FrameScore() { Bowl1 = 10, Strike = true };
            var frame2 = new FrameScore() { Bowl1 = 10, Strike = true };
            var frame3 = new FrameScore() { Bowl1 = 5, Bowl2 = 3 };
            var expectedResult = 25;

            game.FrameList.Add(frame1);
            game.FrameList.Add(frame2);
            game.FrameList.Add(frame3);
            game.StrikeAdder();
            var actualResult = frame1.TotalScore;

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void bowling_a_strike_followed_by_a_non_strike_will_add_both_bowls_from_the_second_frame_to_the_strike_frame()
        {
            var game = new FrameScorer() { CurrentFrame = 1, RemainingPins = 2 };
            var frame1 = new FrameScore() { Bowl1 = 10, Strike = true };
            var frame2 = new FrameScore() { Bowl1 = 5, Bowl2 = 3 };
            var expectedResult = 18;

            game.FrameList.Add(frame1);
            game.FrameList.Add(frame2);
            game.StrikeAdder();
            var actualResult = frame1.TotalScore;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void bowling_a_spare_adds_the_next_bowl_to_the_spare_frame()
        {
            var game = new FrameScorer() { CurrentFrame = 1, RemainingPins = 2 };
            var frame1 = new FrameScore() { Bowl1 = 4, Bowl2 = 6, Spare = true };
            var frame2 = new FrameScore() { Bowl1 = 5, Bowl2 = 3 };
            var expectedResult = 15;

            game.FrameList.Add(frame1);
            game.FrameList.Add(frame2);
            game.SpareAdder();
            var actualResult = frame1.TotalScore;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void totalizer_should_return_the_sum_of_all_frame_scores()
        {
            var game = new FrameScorer();
            var frame1 = new FrameScore() { TotalScore = 20 };
            var frame2 = new FrameScore() { TotalScore = 18 };
            var frame3 = new FrameScore() { TotalScore = 8 };
            var expectedResult = 46;

            game.FrameList.Add(frame1);
            game.FrameList.Add(frame2);
            game.FrameList.Add(frame3);
            var actualResult = game.Totalizer();

            Assert.Equal(expectedResult, actualResult);

        }

        [Fact]
        public void the_auto_final_frame_scorer_should_set_the_value_of_the_third_bowl_of_the_frame_if_it_is_a_spare()
        {
            var game = new FrameScorer();

            for (var i = 0; i < 10; i++)
            {
                game.FrameList.Add(new FrameScore());
            }
            game.FrameList[9].Bowl1 = 6;
            game.FrameList[9].Bowl2 = 4;
            game.FrameList[9].Spare = true;
            game.AutoFinalFramer();
            var actualResult = game.FrameList[9].Bowl3;

            Assert.InRange(actualResult, 0, 10);
        }

        [Fact]
        public void the_auto_final_frame_scorer_should_set_the_value_of_the_third_bowl_of_the_frame_if_it_is_a_strike()
        {
            var game = new FrameScorer();

            for (var i = 0; i < 10; i++)
            {
                game.FrameList.Add(new FrameScore());
            }
            game.FrameList[9].Bowl1 = 10;
            game.FrameList[9].Strike = true;
            game.AutoFinalFramer();
            var actualResult = game.FrameList[9].Bowl3;

            Assert.InRange(actualResult, 0, 10);
        }

        [Fact]
        public void the_auto_final_frame_scorer_should_set_the_value_of_the_second_bowl_of_the_frame_if_it_is_a_strike()
        {
            var game = new FrameScorer();

            for (var i = 0; i < 10; i++)
            {
                game.FrameList.Add(new FrameScore());
            }
            game.FrameList[9].Bowl1 = 10;
            game.FrameList[9].Strike = true;
            game.AutoFinalFramer();
            var actualResult = game.FrameList[9].Bowl2;

            Assert.InRange(actualResult, 0, 10);
        }
    }
}
