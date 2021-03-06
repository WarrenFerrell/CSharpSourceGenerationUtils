using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;
using CSharp.SourceGenerationUtils.Core;
using CSharp.SourceGenerationUtils.Generator;
using System.Collections.Generic;

namespace CSharp.SourceGenerationUtils.Tests.ClassGeneration
{

    public class MethodGenerationTests : ClassSourceGeneratorTestBase
    {
        private class PropertyGetterGenerator1 : NoOpClassMapper
        {
            public override ClassCodeBuilder ProcessProperties(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IPropertySymbol> properties) =>
                properties.Aggregate(generator, (a, x) => a.AddIndentedLine($"public {x.Type.ToDisplayString()} Get{x.Name}() => {x.Name};"));
        }

        [Fact]
        public void PropertyGetter1ProducesMatchingMethods() => PropertyGetterProducesMatchingMethods<PropertyGetterGenerator1>();

        private class PropertyGetterGenerator2 : NoOpClassMapper
        {
            public override ClassCodeBuilder ProcessProperties(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IPropertySymbol> properties) =>
                properties.Aggregate(generator, (a, x) => a.StartMethod(("public", x.Type, $"Get{x.Name}")) {x.Name};"));
        }
        
        [Fact]
        public void PropertyGetter2ProducesMatchingMethods() => PropertyGetterProducesMatchingMethods<PropertyGetterGenerator2>();

        private static void PropertyGetterProducesMatchingMethods<IMapper>()
            where IMapper: NoOpClassMapper, new()
        { 
            // Arrange
            Compilation inputCompilation = CreateCompilation(SimplePartialClassCode);

            // Act
            var newComp = RunGenerators(inputCompilation, out var generatorDiagnostics, new ClassSourceGenerator<ClassFinder, IMapper>());
            // Assert
            _ = AssertValidCompilation(generatorDiagnostics, newComp);
            var newSourceText = AssertSingleGeneratedClass(newComp);
            AssertSourceTextIsEqual(newSourceText, @"
using System;
namespace CodeNamespace.Classes
{
    public partial class Simple
    {
        public int GetIntProp() => IntProp;
        public string GetStringProp => StringProp;
    }
}");
        }
    }
    public class PropertyGenerationTests : ClassSourceGeneratorTestBase
    {
        private class PropertyCopier : NoOpClassMapper
        {
            public override ClassCodeBuilder ProcessProperties(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IPropertySymbol> properties) =>
                properties.Aggregate(generator, (a, x) => a.AddAutoProperty(x.Type, $"{x.Name}Copy"));
        }

        [Fact]
        public void PropertyCopyProducesMatchingProperties()
        {
            // Arrange
            Compilation inputCompilation = CreateCompilation(SimplePartialClassCode);

            // Act
            var newComp = RunGenerators(inputCompilation, out var generatorDiagnostics, new ClassSourceGenerator<ClassFinder, PropertyCopier>());

            // Assert
            _ = AssertValidCompilation(generatorDiagnostics, newComp);
            var newSourceText = AssertSingleGeneratedClass(newComp);
            AssertSourceTextIsEqual(newSourceText, @"
using System;
namespace CodeNamespace.Classes
{
    public partial class Simple
    {
        public int IntPropCopy { get; set; }
        public string StringPropCopy { get; set; }
    }
}");
        }
    }
}
