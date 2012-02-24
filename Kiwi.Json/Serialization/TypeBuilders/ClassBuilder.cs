namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class ClassBuilder<TClass>: InstanceBuilderBase<TClass> where TClass : new()
    {
        public ClassBuilder(ITypeBuilderRegistry registry) : base(registry)
        {
        }

        public override object CreateNull()
        {
            return null;
        }

        protected override object CreateNewInstance()
        {
            return new TClass();
        }
    }
}