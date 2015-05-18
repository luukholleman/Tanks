using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Goals.Fuzzy.Set;

namespace Assets.Scripts.Goals.Fuzzy.Operator
{
    public class Variable
    {
        private readonly Dictionary<string, Set.Set> _sets = new Dictionary<string, Set.Set>();
        private float _maxRange;
        private float _minRange;

        public Set.Set this[string name]
        {
            get { return _sets[name]; }
        }

        public Set.Set Add(string name, Set.Set set)
        {
            _sets[name] = set;
            AdjustRangeToFit(set.Min, set.Max);

            return set;
        }

        public void Fuzzify(float value)
        {
            if (value < _minRange || value > _maxRange)
                throw new ArgumentOutOfRangeException("value", value, "out of range.");

            foreach (var set in _sets)
            {
                set.Value.SetDOM(set.Value.CalculateDOM(value));
            }
        }

        public float Defuzzify()
        {
            //MaxAv

            float bottom = 0f;
            float top = 0f;

            foreach (var set in _sets)
            {
                bottom += set.Value.DOM;
                top += set.Value.RepresentativeValue * set.Value.DOM;
            }

            if (bottom == 0) return 0;

            return top / bottom;
        }

        private void AdjustRangeToFit(float min, float max)
        {
            _minRange = Math.Min(_minRange, min);
            _maxRange = Math.Max(_maxRange, max);
        }
    }
}
