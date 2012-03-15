using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    public class InvalidApplicationModelFixture
    {
        [Test]
        public void NullsAreAlwaysValid()
        {
            Assert.That(JsonConvert.Read<ClassWithNoDefaultConstructor>("null"), Is.Null);
        }

        [Test]
        public void DeserializedClassesMustHaveDefaultConstructor()
        {
            Assert.Throws<InvalidClassForDeserializationException>(() => JsonConvert.Read<ClassWithNoDefaultConstructor>("{}"));
        }

        [Test]
        public void DeserializedClassesMustHavePublicDefaultConstructor()
        {
            Assert.Throws<InvalidClassForDeserializationException>(
                () => JsonConvert.Read<ClassWithProtectedDefaultConstructor>("{}"));
        }

        #region Nested type: ClassWithNoDefaultConstructor

        public class ClassWithNoDefaultConstructor
        {
            public ClassWithNoDefaultConstructor(string message)
            {
            }
        }

        #endregion

        #region Nested type: ClassWithProtectedDefaultConstructor

        public class ClassWithProtectedDefaultConstructor
        {
            protected ClassWithProtectedDefaultConstructor()
            {
            }
        }

        #endregion
    }
}