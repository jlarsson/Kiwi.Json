using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Kiwi.Json.JPath.Evaluators;
using Kiwi.Json.Util;

namespace Kiwi.Json.JPath
{
    public class JsonPathParserRunner : AbstractStringMatcher
    {
        public JsonPathParserRunner(string path) : base(path)
        {
        }

        public IJsonPathPartEvaluator[] Parse()
        {
            Match('$');

            var evaluators = new List<IJsonPathPartEvaluator> {IdentityEvaluator.Default};
            while (!EndOfInput)
            {
                if (TryMatch('.'))
                {
                    if (TryMatch('.'))
                    {
                        if (TryMatch('*'))
                        {
                            evaluators.Add(MatchAllRecursiveEvaluator.Default);
                            continue;
                        }
                        var m = PeekNextChar() == '"' ? MatchString() : MatchIdent("string or identifier");
                        if (m != null)
                        {
                            evaluators.Add(MatchObjectRecursiveEvaluator.Default);
                            evaluators.Add(new NamedMemberEvaluator(m));
                            continue;
                        }

                        throw CreateExpectedException("member");
                    }
                    if (TryMatch('*'))
                    {
                        evaluators.Add(AnyMemberEvaluator.Default);
                        continue;
                    }
                    var member = PeekNextChar() == '"' ? MatchString() : MatchIdent("string or identifier");
                    if (member != null)
                    {
                        evaluators.Add(new NamedMemberEvaluator(member));
                        continue;
                    }
                    throw CreateExpectedException("member");
                }

                if (TryMatch('['))
                {
                    var c = (char) PeekNextChar();
                    if (char.IsDigit(c) || (c == ':') || (c == '-'))
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
                        evaluators.Add(MatchObjectMembersOrArrayElementsEvaluator.Default);
                        Match(']');
                        continue;
                    }

                    var member = PeekNextChar() == '"' ? MatchString() : MatchIdent("index");
                    if (member != null)
                    {
                        evaluators.Add(new NamedMemberEvaluator(member));
                        Match(']');
                        continue;
                    }
                }
            }
            return evaluators.ToArray();
        }

        private IJsonPathPartEvaluator CreateEvaluatorFromIndexExpression(string expr)
        {
            var m = Regex.Match(expr, @"^\d+$", RegexOptions.Multiline);
            if (m.Success)
            {
                return new IndexedElementEvaluator(int.Parse(m.Value));
            }

            var start_end_step = TryParseNullableInts(expr.Split(':'));
            if (start_end_step != null)
            {
                if (start_end_step.Length > 3)
                {
                    return null;
                }
                return new ArraySliceEvaluator(
                    start_end_step.Length > 0 ? start_end_step[0] : null,
                    start_end_step.Length > 1 ? start_end_step[1] : null,
                    start_end_step.Length > 2 ? start_end_step[2] : null);
            }
            var indexes = TryParseInts(expr.Split(','));
            if (indexes != null)
            {
                return new IndexedElementsEvaluator(indexes);
            }
            return null;
        }

        private int?[] TryParseNullableInts(string[] ints)
        {
            var result = new int?[ints.Length];
            for (var i = 0; i < ints.Length; ++i)
            {
                if (!string.IsNullOrEmpty(ints[i]))
                {
                    int d;
                    if (!int.TryParse(ints[i], out d))
                    {
                        return null;
                    }
                    result[i] = d;
                }
            }
            return result;
        }

        private int[] TryParseInts(string[] ints)
        {
            var result = new int[ints.Length];
            for (var i = 0; i < ints.Length; ++i)
            {
                int d;
                if (!int.TryParse(ints[i], out d))
                {
                    return null;
                }
                result[i] = d;
            }
            return result;
        }

        public override Exception CreateExpectedException(object expectedWhat)
        {
            throw new JsonPathException(string.Format("Expected {0} at ({1},{2}) in {3}", expectedWhat, Line, Column,
                                                      Source));
        }
    }
}