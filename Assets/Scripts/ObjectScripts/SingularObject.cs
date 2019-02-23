namespace ObjectScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Basic type of item, has a property of weight and a property of size; can not be split anymore(Which is the meaning
    ///     of basic)
    /// </summary>
    public abstract class SingularObject : BaseObject
    {
        public int Size;
        public int Weight;

        public override float GetSize()
        {
            return Size;
        }

        public override float GetWeight()
        {
            return Weight;
        }
    }
}