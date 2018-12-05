namespace UtilScripts
{
    public struct LimitValue
    {
        public int MaxValue;

        private int _value;

        public int Value
        {
            set { _value = value > MaxValue ? MaxValue : value; }
            get { return _value; }
        }

        public LimitValue(int value)
        {
            MaxValue = value;
            _value = value;
        }

    }
}