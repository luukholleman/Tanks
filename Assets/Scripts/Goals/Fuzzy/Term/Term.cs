using System;

namespace Assets.Scripts.Goals.Fuzzy.Term
{
    public abstract class Term
    {
        public abstract float DOM { get; }
        public abstract void ClearDOM();
        public abstract void OrWithDOM(float value);

        public static implicit operator float(Term term)
        {
            if (term == null) throw new ArgumentNullException("term");
            return term.DOM;
        }

        public static Term operator &(Term left, Term right)
        {
            return new And(left, right);
        }

        public static Term operator |(Term left, Term right)
        {
            return new Or(left, right);
        }

        public Term Very()
        {
            return new Very(this);
        }

        public Term Fairly()
        {
            return new Fairly(this);
        }

        public static bool operator true(Term term)
        {
            return term != null;
        }

        public static bool operator false(Term term)
        {
            return term == null;
        }
    }
}
