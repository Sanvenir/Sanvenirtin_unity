using System;

namespace UtilScripts
{
    [Serializable]
    public struct LimitValue
    {
        public float MaxValue;

        private float _value;

        public float Value
        {
            set { _value = value > MaxValue ? MaxValue : value; }
            get { return _value; }
        }

        public LimitValue(int value)
        {
            MaxValue = value;
            _value = value;
        }

        public float GetRemainRatio()
        {
            return _value / MaxValue;
        }
    }
}