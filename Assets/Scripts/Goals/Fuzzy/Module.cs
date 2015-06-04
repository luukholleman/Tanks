using System;
using System.Collections.Generic;
using Assets.Scripts.Goals.Fuzzy.Operator;

namespace Assets.Scripts.Goals.Fuzzy
{
    public class Module
    {
        private readonly List<Rule> _rules = new List<Rule>();
        private readonly Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();

        public Variable this[string name]
        {
            get { return _variables[name]; }
        }

        public Variable CreateFLV(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (_variables.ContainsKey(name)) throw new Exception();

            return _variables[name] = new Variable();
        }

        public void Add(Rule rule)
        {
            _rules.Add(rule);
        }

        public void Add(Term.Term antecent, Term.Term consequence)
        {
            _rules.Add(new Rule(antecent, consequence));
        }

        public float Defuzzify(string name)
        {
            foreach (var rule in _rules)
                rule.SetConfidenceOfConsequentToZero();

            foreach (var rule in _rules)
                rule.Calculate();

            return this[name].Defuzzify();
        }
    }
}
