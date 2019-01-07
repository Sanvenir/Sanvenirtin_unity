namespace ExceptionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     If given coord is out of the Earth Map, throw this exception
    /// </summary>
    public class CoordOutOfWorldException : GameException
    {
        public CoordOutOfWorldException(string message) : base(message)
        {
        }
    }
}