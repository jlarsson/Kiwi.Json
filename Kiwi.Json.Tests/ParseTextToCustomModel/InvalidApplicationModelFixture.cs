using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    public class InvalidApplicationModelFixture
    {
        [Test]
        public void NullsAreAlwaysValid()
        {
            Assert.That(JsonConvert.Parse<ClassWithNoDefaultConstructor>("null"), Is.Null);
        }

        [Test]
        public void DeserializedClassesMustHaveDefaultConstructor()
        {
            Assert.Throws<InvalidClassForDeserializationException>(() => JsonConvert.Parse<ClassWithNoDefaultConstructor>("{}"));
        }

        [Test]
        public void DeserializedClassesMustHavePublicDefaultConstructor()
        {
            Assert.Throws<InvalidClassForDeserializationException>(
                () => JsonConvert.Parse<ClassWithProtectedDefaultConstructor>("{}"));
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