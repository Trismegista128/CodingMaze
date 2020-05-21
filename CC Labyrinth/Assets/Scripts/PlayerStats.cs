using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class PlayerStats
    {
        public PlayerStats()
        {
            LevelsStats = new List<StatsInLevel>();
        }

        public List<StatsInLevel> LevelsStats;
        public int TotalSteps => LevelsStats.Sum(x => x.StepsDone);
    }
    public class StatsInLevel
    {
        public int Placement = 0;
        public int StepsDone = 0;
        public bool HasFinished = false;
        public ErrorType ErrorType = ErrorType.None;
    }
}
