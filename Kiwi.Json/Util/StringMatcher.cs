namespace Kiwi.Json.Util
{
    public abstract class AbstractStringMatcher : AbstractTextMatcher
    {
        private readonly string _source;
        private int _sourcePosition;

        protected AbstractStringMatcher(string source)
        {
            _source = source;
        }

        public override bool EndOfInput { get { return _sourcePosition >= Source.Length; } }

        public string Source
        {
            get { return _source; }
        }

        protected override int Read()
        {
            return _sourcePosition < Source.Length ? Source[_sourcePosition++] : char.MinValue;
        }

        public override int PeekNextChar()
        {
            return _sourcePosition < Source.Length ? Source[_sourcePosition] : char.MinValue;
        }
    }
}