using UnityEngine;

namespace ExceptionScripts
{
    public class CoordOccupiedException: GameException
    {
        public Collider2D Collider;

        public CoordOccupiedException(Collider2D collider)
        {
            Collider = collider;
        }
    }
}