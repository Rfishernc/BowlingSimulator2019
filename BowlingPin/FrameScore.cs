using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingPin
{
    public class FrameScore
    {
        public int Bowl1 { get; set; }
        public int Bowl2 { get; set; }
        public int Bowl3 { get; set; }
        public int TotalScore { get; set; }
        public bool Strike { get; set; } = false;
        public bool Spare { get; set; } = false;
    }
}
