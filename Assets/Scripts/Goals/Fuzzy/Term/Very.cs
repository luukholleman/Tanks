using System;

namespace Assets.Scripts.Goals.Fuzzy.Term
{
    public class Very : Term
    {
        private readonly Term _set;

        public Very(Term set)
        {
            if (set == null) throw new ArgumentNullException("set");
            _set = set;
        }

        #region Implementation of Term

        public override float DOM
        {
            get { return _set * _set; }
        }

        public override void ClearDOM()
        {
            _set.ClearDOM();
        }

        public override void OrWithDOM(float value)
        {
            _set.OrWithDOM(value * value);
        }

        #endregion
    }
}
