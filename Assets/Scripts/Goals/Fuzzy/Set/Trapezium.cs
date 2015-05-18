namespace Assets.Scripts.Goals.Fuzzy.Set
{
    public class Trapezium : Set
    {
        private readonly float _left;
        private readonly float _middleleft;
        private readonly float _middleright;
        private readonly float _right;

        public Trapezium(float left, float middleleft, float middleright, float right)
            : base((middleright - middleleft) / 2 + middleleft)
        {
            _left = left;
            _middleleft = middleleft;
            _middleright = middleright;
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

            if (value < _middleleft)
                return _middleleft == _left ? 1 : (value - _left) / (_middleleft - _left);
            if (value > _middleright)
                return _middleright == _right ? 1 : (_right - value) / (_right - _middleright);

            return 1;
        }

        #endregion
    }
}
