using System;

namespace Assets.Scripts.Goals.Fuzzy.Set
{
    public abstract class Set : Term.Term
    {
        private float _dom;

        protected Set(float representativeValue)
        {
            RepresentativeValue = representativeValue;
        }

        public float RepresentativeValue { get; private set; }
        public abstract float Min { get; }
        public abstract float Max { get; }
        public abstract float CalculateDOM(float value);

        #region Overrides of Term

        public override float DOM
        {
            get { return _dom; }
        }

        public void SetDOM(float value)
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException("value", value, "Must be within range 0.0 - 1.0");

            _dom = value;
        }

        public override void ClearDOM()
        {
            SetDOM(0);
        }

        public override void OrWithDOM(float value)
        {
            if (value > _dom) SetDOM(value);
        }

        #endregion
    }
}
