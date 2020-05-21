namespace Assets.Scripts
{
    public interface IPlayerAI
    {
        DirectionType RequestMove(DirectionType[] direction);
    }
}
