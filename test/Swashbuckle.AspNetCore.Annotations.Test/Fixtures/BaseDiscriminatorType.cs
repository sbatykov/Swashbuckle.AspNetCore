namespace Swashbuckle.AspNetCore.Annotations.Test.Fixtures
{
    public abstract class BaseType
    {
        public string BaseProperty { get; set; }
    }

    [SwaggerDiscriminator("type")]
    [SwaggerSubType(typeof(SubType1), DiscriminatorValue = "subType1")]
    [SwaggerSubType(typeof(SubType2), DiscriminatorValue = "subType2")]
    public abstract class BaseDiscriminatorType: BaseType
    {
        public BaseDiscriminatorType(string type)
        {
            Type = type;
        }

        public string Type { get; set; }
    }

    public sealed class SubType1: BaseDiscriminatorType
    {
        public SubType1(string property1): base("subType1")
        {
            Property1 = property1;
        }

        public string Property1 { get; set; }
    }

    public abstract class BaseSubType2 : BaseDiscriminatorType
    {
        public BaseSubType2(string type, string property2) : base(type)
        {
            Property2 = property2;
        }

        public string Property2 { get; set; }
    }

    public sealed class SubType2 : BaseSubType2
    {
        public SubType2(string property2) : base("subType2", property2)
        {
        }
    }
}
