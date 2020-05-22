namespace Assets.Scripts
{
    public class LevelStats
    {
        public int Placement = 0;
        public int StepsDone = 0;
        public bool HasFinished = false;
        public ErrorType ErrorType = ErrorType.None;

        public int FinishValue => HasFinished ? 1 : 0;

        public int ErrorValue =>
                      ErrorType == ErrorType.None ? 10 :
                      ErrorType == ErrorType.Looped ? 5 :
                      ErrorType == ErrorType.Bug ? 0 : 3;
    }
}
