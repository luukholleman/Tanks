using System;
using Assets.Scripts.Goals.Fuzzy.Operator;

namespace Assets.Scripts.Goals.Fuzzy.Term
{
    public class Or : Term
    {
        private readonly Term _left;
        private readonly Term _right;

        public Or(Term left, Term right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            _left = left;
            _right = right;
        }

        #region Implementation of Term

        public override float DOM
        {
            get { return Math.Max(_left, _right); }
        }

        public override void ClearDOM()
        {
            throw new NotImplementedException("Invalid context");
        }

        public override void OrWithDOM(float value)
        {
            throw new NotImplementedException("Invalid context");
        }

        #endregion
    }
}
