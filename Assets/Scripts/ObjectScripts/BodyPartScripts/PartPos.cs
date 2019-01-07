namespace ObjectScripts.BodyPartScripts
{
    public enum PartPos
    {
        /// <summary>
        ///     Can not be attacked by normal attack
        /// </summary>
        Unreachable = -1,

        /// <summary>
        ///     Can attack or be attacked by any other pos
        /// </summary>
        Arbitrary = 0,

        High = 1,
        Middle = 2,
        Low = 3
    }
}