using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
  [Serializable]
  public class Condition
  {
    [SerializeField] Disjunction[] _and;
    public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
    {
      foreach (var disjunction in _and)
        if (!disjunction.Check(evaluators))
          return false;
      return true;
    }
    [Serializable]
    class Disjunction
    {
      [SerializeField] Predicate[] _or;
      public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
      {
        foreach (var predicate in _or)
          if (predicate.Check(evaluators))
            return true;
        return false;
      }
    }
    [Serializable]
    class Predicate
    {
      [SerializeField] string _predicate;
      [SerializeField] string[] _parameters;
      [SerializeField] bool _negate;
      public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
      {
        foreach (var evaluator in evaluators)
          if (evaluator.Evaluate(_predicate, _parameters) is bool val)
            if (val == _negate) return false;
        return true;
      }
    }
  }

}