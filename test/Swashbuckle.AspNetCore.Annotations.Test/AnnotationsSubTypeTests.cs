using System.Linq;
using Microsoft.OpenApi.Models;
using Xunit;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.TestSupport;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations.Test.Fixtures;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle.AspNetCore.Annotations.Test
{
    public class AnnotationsSubTypeTests
    {
        [Fact]
        public void GenerateSchema_SubTypeAttributes()
        {
            var subject = Subject(configureGenerator: c =>
            {
                c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: false);
            });

            var schemaRepository = new SchemaRepository();

            var schema = subject.GenerateSchema(typeof(BaseDiscriminatorType), schemaRepository);

            // The base type schema
            Assert.NotNull(schema.Reference);
            var baseSchema = schemaRepository.Schemas[schema.Reference.Id];
            Assert.Equal("object", baseSchema.Type);
            Assert.Equal(new[] { "type", "baseProperty" }, baseSchema.Properties.Keys);

            // The first sub type schema
            var subType1Schema = schemaRepository.Schemas["SubType1"];
            Assert.Equal("object", subType1Schema.Type);
            Assert.NotNull(subType1Schema.AllOf);
            Assert.Equal(1, subType1Schema.AllOf.Count);
            Assert.NotNull(subType1Schema.AllOf[0].Reference);
            Assert.Equal(schema.Reference.Id, subType1Schema.AllOf[0].Reference.Id);
            Assert.Equal(new[] { "property1" }, subType1Schema.Properties.Keys);

            // The second sub type schema
            var subType2Schema = schemaRepository.Schemas["SubType2"];
            Assert.Equal("object", subType2Schema.Type);
            Assert.NotNull(subType2Schema.AllOf);
            Assert.Equal(1, subType2Schema.AllOf.Count);
            Assert.NotNull(subType2Schema.AllOf[0].Reference);
            Assert.Equal(schema.Reference.Id, subType2Schema.AllOf[0].Reference.Id);
            Assert.Equal(new[] { "property2" }, subType2Schema.Properties.Keys);
        }

        private SchemaGenerator Subject(
            Action<SwaggerGenOptions> configureGenerator = null)
        {
            var options = new SwaggerGenOptions();
            configureGenerator?.Invoke(options);


            var jsonOptions = new JsonOptions();

            return new SchemaGenerator(options.SchemaGeneratorOptions, new JsonSerializerDataContractResolver(jsonOptions.JsonSerializerOptions));
        }
    }
}
