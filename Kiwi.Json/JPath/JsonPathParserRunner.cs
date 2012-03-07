using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Kiwi.Json.JPath.Visitors;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.JPath
{
    public class JsonPathParserRunner : AbstractStringMatcher
    {
        public JsonPathParserRunner(string path): base(path)
        {
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

        private Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>> AllObjectMembersOrArrayElements()
        {
            var visitor = new AllObjectMembersOrArrayElementsVisitor();
            return values => from value in values from v in value.Visit(visitor) select v;
        }


        public Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>>[] Parse()
        {
            Match('$');

            var evaluators = new List<Func<IEnumerable<IJsonValue>, IEnumerable<IJsonValue>>>
                                 {
                                     EvalCurrent
                                 };
            while (!EndOfInput)
            {
                if (TryMatch('.'))
                {
                    if (TryMatch('.'))
                    {
                        if (TryMatch('*'))
                        {
                            evaluators.Add(EvalAllMembersRecursive());
                            continue;
                        }
                        var m = PeekNextChar() == '"' ? MatchString() : MatchIdent("string or identifier");
                        if (m != null)
                        {
                            evaluators.Add(EvalAllObjectsRecursive());
                            evaluators.Add(Member(m));
                            continue;
                        }

                        throw CreateExpectedException("member");
                        
                    }
                    if (TryMatch('*'))
                    {
                        evaluators.Add(AllMembers());
                        continue;
                    }
                    var member = PeekNextChar() == '"' ? MatchString() : MatchIdent("string or identifier");
                    if (member != null)
                    {
                        evaluators.Add(Member(member));
                        continue;
                    }
                    throw CreateExpectedException("member");
                }

                if (TryMatch('['))
                {
                    var c = (char) PeekNextChar();
                    if (char.IsDigit(c) || (c == ':'))
                    {
                        var expr = MatchUntil(']');

                        var indexedEvaluator = CreateEvaluatorFromIndexExpression(expr);
                        if (indexedEvaluator == null)
                        {
                            throw CreateExpectedException("index");
                        }
                        evaluators.Add(indexedEvaluator);
                        Match(']');
                        continue;
                    }

                    if (c == '*')
                    {
                        MatchAnyChar();
                        evaluators.Add(AllObjectMembersOrArrayElements());
                        Match(']');
                        continue;
                    }

                    var member = PeekNextChar() == '"' ? MatchString() : MatchIdent("index");
                    if (member != null)
                    {
                        evaluators.Add(Member(member));
                        Match(']');
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

        public override Exception CreateExpectedException(object expectedWhat)
        {
            throw new JsonPathException(string.Format("Expected {0} at ({1},{2}) in {3}", expectedWhat, Line, Column, Source));
        }
    }
}