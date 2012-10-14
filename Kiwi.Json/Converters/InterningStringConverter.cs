using System;
using System.Collections.Generic;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;

namespace Kiwi.Json.Converters
{
    public class InterningStringConverter: AbstractJsonConverter
    {
        readonly Dictionary<string, string> _interned = new Dictionary<string, string>();
        public override ITypeBuilder CreateTypeBuilder(Type type)
        {
            return TryCreateTypeBuilder<string, string>(type, s =>
                                                                  {
                                                                      if (s == null)
                                                                      {
                                                                          return null;
                                                                      }
                                                                      string interned;
                                                                      if (!_interned.TryGetValue(s,out interned))
                                                                      {
                                                                          interned = s;
                                                                          _interned[interned] = interned;
                                                                      }
                                                                      return interned;
                                                                  });
        }

        public override ITypeWriter CreateTypeWriter(Type type)
        {
            return null;
        }
    }
}