using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class PlayerStats
    {
        public PlayerStats()
        {
            Levels = new Dictionary<int, LevelStats>();
        }

        public Dictionary<int, LevelStats> Levels;
        public int TotalSteps => Levels.Sum(x => x.Value.StepsDone);
    }
}
