namespace ExceptionScripts
{
    public class CoordOutOfWorldException: GameException
    {
        public CoordOutOfWorldException(string message) : base(message)
        {
        }
    }
}