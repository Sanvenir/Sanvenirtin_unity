using UnityEngine;

namespace ExceptionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     If a world coord is occupied by a collider, throw this Exception
    /// </summary>
    public class CoordOccupiedException : GameException
    {
        public readonly Collider2D Collider;

        public CoordOccupiedException(Collider2D collider)
        {
            Collider = collider;
        }
    }
}