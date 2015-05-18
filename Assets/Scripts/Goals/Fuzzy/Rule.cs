using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Goals.Fuzzy.Term;

namespace Assets.Scripts.Goals.Fuzzy.Operator
{
    public class Rule
    {
        private readonly Term.Term _antecent;
        private readonly Term.Term _consequence;

        public Rule(Term.Term antecent, Term.Term consequence)
        {
            if (antecent == null) throw new ArgumentNullException("antecent");
            if (consequence == null) throw new ArgumentNullException("consequence");
            _antecent = antecent;
            _consequence = consequence;
        }

        public void SetConfidenceOfConsequentToZero()
        {
            _consequence.ClearDOM();
        }

        public void Calculate()
        {
            _consequence.OrWithDOM(_antecent);
        }
    }
}
