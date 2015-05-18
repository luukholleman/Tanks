using Assets.Scripts.Goals.Fuzzy.Operator;

namespace Assets.Scripts.Goals.Fuzzy.Set
{
    public class LeftShoulder : Set
    {
        private readonly float _left;
        private readonly float _peak;
        private readonly float _right;

        public LeftShoulder(float left, float middle, float right)
            : base((middle - left) / 2 + left)
        {
            _peak = middle;
            _left = left;
            _right = right;
        }

        #region Overrides of Set

        public override float Min
        {
            get { return _left; }
        }

        public override float Max
        {
            get { return _right; }
        }

        public override float CalculateDOM(float value)
        {
            if (value < _left || value > _right) return 0;

            if (value > _peak)
                return _peak == _right ? 1 : (_right - value) / (_right - _peak);

            return 1;
        }

        #endregion
    }
}
