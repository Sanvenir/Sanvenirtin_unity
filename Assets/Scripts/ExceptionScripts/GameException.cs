using System;

namespace ExceptionScripts
{
    public class GameException : Exception
    {
        public GameException(string message):base(message)
        {
            
        }
    }
}