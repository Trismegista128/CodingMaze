using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class PlayerStats
    {
        public PlayerStats()
        {
            Levels = new List<LevelStats>();
        }

        public List<LevelStats> Levels;
        public int TotalSteps => Levels.Sum(x => x.StepsDone);
    }
}
