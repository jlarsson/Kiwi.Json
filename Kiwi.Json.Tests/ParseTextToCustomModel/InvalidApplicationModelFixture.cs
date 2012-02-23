using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    public class InvalidApplicationModelFixture
    {
        [Test]
        public void NullsAreAlwaysValid()
        {
            Assert.That(JSON.ToObject<ClassWithNoDefaultConstructor>("null"), Is.Null);
        }

        [Test]
        public void DeserializedClassesMustHaveDefaultConstructor()
        {
            Assert.Throws<MissingDefaultConstructorException>(() => JSON.ToObject<ClassWithNoDefaultConstructor>("{}"));
        }

        [Test]
        public void DeserializedClassesMustHavePublicDefaultConstructor()
        {
            Assert.Throws<MissingDefaultConstructorException>(
                () => JSON.ToObject<ClassWithProtectedDefaultConstructor>("{}"));
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