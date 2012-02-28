using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kiwi.Json.Conversion;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    [DebuggerDisplay("JsonPath: {Path}")]
    public class JsonPath : IJsonPath
    {
        private readonly Func<IJsonValue, IJsonValue>[] _traversers;

        public JsonPath(string path)
        {
            Path = path;
            _traversers = CreateTraversers().ToArray();
        }

        public bool Strict { get; set; }

        #region IJsonPath Members

        public string Path { get; private set; }

        public IJsonValue GetValue(IJsonValue obj)
        {
            return _traversers.Aggregate(obj, (o, t) => t(o));
        }

        #endregion

        private IEnumerable<Func<IJsonValue, IJsonValue>> CreateTraversers()
        {
            var tokens = new JsonStringReader(Path).Tokenize();

            var state = State.ExpectMember | State.ExpectArrayBegin | State.ExpectEnd;

            var pathSoFar = "";
            foreach (var token in tokens)
            {
                pathSoFar += token;

                if (state.HasFlag(State.ExpectArrayBegin) && (token == "["))
                {
                    state = State.ExpectArrayIndex;
                    continue;
                }
                if (state.HasFlag(State.ExpectArrayEnd) && (token == "]"))
                {
                    state = State.ExpectArrayBegin | State.ExpectDot | State.ExpectEnd;
                    continue;
                }
                if (state.HasFlag(State.ExpectDot) && (token == "."))
                {
                    state = State.ExpectMember;
                    continue;
                }

                if (state.HasFlag(State.ExpectArrayIndex))
                {
                    var pathHint = pathSoFar + "]";
                    state = State.ExpectArrayEnd;
                    int index;
                    if (int.TryParse(token, out index))
                    {
                        yield return obj => GetIndexedValue(obj, index, pathHint);

                        continue;
                    }
                    var member = token;
                    yield return obj => GetMember(obj, member, pathHint);
                    continue;
                }
                if (state.HasFlag(State.ExpectMember))
                {
                    state = State.ExpectDot | State.ExpectArrayBegin | State.ExpectEnd;
                    var member = token;
                    yield return obj => GetMember(obj, member, pathSoFar);
                    continue;
                }

                throw new JsonPathException(string.Format("Illegal JPath expression: {0}", Path));
            }
            if (!state.HasFlag(State.ExpectEnd))
            {
                throw new JsonPathException(string.Format("Illegal JPath expression: {0}", Path));
            }
        }

        private IJsonValue GetIndexedValue(IJsonValue obj, int index, string pathHint)
        {
            var a = CastTo<IJsonArray>(obj, pathHint);
            return VerifyReturnValue(pathHint, (a == null) || (index >= a.Count) ? null : a[index]);
        }

        private IJsonValue GetMember(IJsonValue obj, string member, string pathHint)
        {
            var o = CastTo<IJsonObject>(obj, pathHint);

            IJsonValue value;
            return VerifyReturnValue(pathHint, (o != null) && o.TryGetValue(member, out value) ? value : null);
        }

        private T CastTo<T>(IJsonValue jsonValue, string pathHint) where T : class, IJsonValue
        {
            var cast = jsonValue as T;
            if (Strict && (cast == null))
            {
                throw new JsonPathException(string.Format("Actual types does not apply to operands in expression {0}",
                                                          pathHint));
            }
            return cast;
        }

        private IJsonValue VerifyReturnValue(string pathHint, IJsonValue jsonValue)
        {
            if (Strict && (jsonValue == null))
            {
                throw new JsonPathException(string.Format("Unable to evaluate expression {0}", pathHint));
            }
            return jsonValue;
        }

        #region Nested type: State

        [Flags]
        private enum State
        {
            ExpectMember = 0x01,
            ExpectArrayBegin = 0x02,
            ExpectArrayIndex = 0x04,
            ExpectArrayEnd = 0x08,

            ExpectDot = 0x10,

            ExpectEnd = 0x20
        }

        #endregion
    }
}