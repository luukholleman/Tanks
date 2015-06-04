using System;

namespace Assets.Scripts.Goals.Fuzzy.Term
{
    public class And : Term
    {
        private readonly Term _left;
        private readonly Term _right;

        public And(Term left, Term right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            _left = left;
            _right = right;
        }

        #region Implementation of Term

        public override float DOM
        {
            get { return Math.Min(_left, _right); }
        }

        public override void ClearDOM()
        {
            _left.ClearDOM();
            _right.ClearDOM();
        }

        public override void OrWithDOM(float value)
        {
            _left.OrWithDOM(value);
            _right.OrWithDOM(value);
        }

        #endregion
    }
}
