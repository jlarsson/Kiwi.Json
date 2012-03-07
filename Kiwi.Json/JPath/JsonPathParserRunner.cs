using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Kiwi.Json.JPath.Visitors;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.JPath
{
    public class JsonPathParserRunner
    {
        private readonly StringMatcher _matcher;

        public JsonPathParserRunner(string path)
        {
            _matcher = new StringMatcher(path);
        }

        private IEnumerable<IJsonValue> EvalCurrent(IEnumerable<IJsonValue> values)
        {
            return values;
        }

        private Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>> EvalAllMembersRecursive()
        {
            var valueVisitor = new AllSimpleValuesVisitor();
            return values => from value in values
                             from v in value.Visit(valueVisitor)
                             select v;
        }

        private Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>> EvalAllObjectsRecursive()
        {
            var valueVisitor = new AllObjectValuesVisitor();
            return values => from value in values
                             from v in value.Visit(valueVisitor)
                             select v;
        }


        private Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>> AllMembers()
        {
            return values => values;
        }

        private Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>> Member(string name)
        {
            var visitor = new MemberVisitor(name);
            return values => from value in values select value.Visit(visitor);
        }

        private Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>> Member(int index)
        {
            var visitor = new IndexedMemberVisitor(index);
            return values => from value in values select value.Visit(visitor);
        }

        public Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>>[] Parse()
        {
            _matcher.Match('$');

            var evaluators = new List<Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>>>
                                 {
                                     EvalCurrent
                                 };
            while (!_matcher.EndOfInput)
            {
                if (_matcher.TryMatch(".."))
                {
                    if (_matcher.TryMatch('*'))
                    {
                        evaluators.Add(EvalAllMembersRecursive());
                        continue;
                    }
                    var member = _matcher.Peek() == '"' ? _matcher.MatchString() : _matcher.MatchIdent();
                    if (member != null)
                    {
                        evaluators.Add(EvalAllObjectsRecursive());
                        evaluators.Add(Member(member));
                        continue;
                    }

                    throw _matcher.CreateExpectedException("member");
                }

                if (_matcher.TryMatch('.'))
                {
                    if (_matcher.TryMatch('*'))
                    {
                        evaluators.Add(AllMembers());
                        continue;
                    }
                    var member = _matcher.Peek() == '"' ? _matcher.MatchString() : _matcher.MatchIdent();
                    if (member != null)
                    {
                        evaluators.Add(Member(member));
                        continue;
                    }
                    throw _matcher.CreateExpectedException("member");
                }

                if (_matcher.TryMatch('['))
                {
                    var c = (char) _matcher.Peek();
                    if (char.IsDigit(c) || (c == ':'))
                    {
                        var expr = _matcher.MatchUntil(']');

                        var indexedEvaluator = CreateEvaluatorFromIndexExpression(expr);
                        if (indexedEvaluator == null)
                        {
                            throw _matcher.CreateExpectedException("index");
                        }
                        evaluators.Add(indexedEvaluator);
                        _matcher.Match(']');
                        continue;
                    }


                    var member = _matcher.Peek() == '"' ? _matcher.MatchString() : _matcher.MatchIdent();
                    if (member != null)
                    {
                        evaluators.Add(Member(member));
                        _matcher.Match(']');
                        continue;
                    }
                }
            }
            return evaluators.ToArray();
        }

        private Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>> CreateEvaluatorFromIndexExpression(string expr)
        {
            var m = Regex.Match(expr, @"^\d+$", RegexOptions.Multiline);
            if (m.Success)
            {
                return Member(int.Parse(m.Value));
            }
            return null;
        }
    }
}