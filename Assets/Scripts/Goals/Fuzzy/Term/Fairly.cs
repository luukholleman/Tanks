using System;

namespace Assets.Scripts.Goals.Fuzzy.Term
{
    public class Fairly : Term
    {
        private readonly Term _set;

        public Fairly(Term set)
        {
            if (set == null) throw new ArgumentNullException("set");
            _set = set;
        }

        #region Implementation of Term

        public override float DOM
        {
            get { return (float)Math.Sqrt(_set); }
        }

        public override void ClearDOM()
        {
            _set.ClearDOM();
        }

        public override void OrWithDOM(float value)
        {
            _set.OrWithDOM((float)Math.Sqrt(value));
        }

        #endregion
    }
}
