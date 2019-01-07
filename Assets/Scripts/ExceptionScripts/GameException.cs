using System;

namespace ExceptionScripts
{
    /// <summary>
    ///     For exception which is unusual
    /// </summary>
    public class GameException : Exception
    {
        public GameException(string message) : base(message)
        {
        }

        public GameException()
        {
        }
    }
}