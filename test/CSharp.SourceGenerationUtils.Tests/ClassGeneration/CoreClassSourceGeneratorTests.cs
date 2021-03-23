using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Xunit;
using CSharp.SourceGenerationUtils.Core;
using CSharp.SourceGenerationUtils.Generator;
using System.Collections.Generic;

namespace CSharp.SourceGenerationUtils.Tests.ClassGeneration
{
    public class CoreClassSourceGeneratorTests : ClassSourceGeneratorTestBase
    {

        [Fact]
        public void NoOpMapperProducesSimpleClassWithNoBody()
        {
            // Arrange
            Compilation inputCompilation = CreateCompilation(SimplePartialClassCode);

            // Act
            var newComp = RunGenerators(inputCompilation, out var generatorDiagnostics, new ClassSourceGenerator<ClassFinder, NoOpClassMapper>());

            // Assert
            _ = AssertValidCompilation(generatorDiagnostics, newComp);
            var newSourceText = AssertSingleGeneratedClass(newComp);
            AssertSourceTextIsEqual(newSourceText, @"
using System;
namespace CodeNamespace.Classes
{
    public partial class Simple
    {
    }
}");
        }

        private class PropertyCopier : NoOpClassMapper
        {
            public override ClassCodeBuilder ProcessProperties(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IPropertySymbol> properties) =>
                properties.Aggregate(generator, (a, x) => a.AddAutoProperty(x.Type.ToDisplayString(), x.Name + "Copy"));
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

        private class NewUsingsMapper : NoOpClassMapper
        {
            public NewUsingsMapper()
            {
                UsingTypeNamespaces.Add(typeof(List<string>));
                UsingTypeNamespaces.Add(typeof(Encoding));
            }
        }

        [Fact]
        public void MapperIsAbleToAddNewUsing()
        {
            // Arrange
            Compilation inputCompilation = CreateCompilation(SimplePartialClassCode);

            // Act
            var newComp = RunGenerators(inputCompilation, out var generatorDiagnostics, new ClassSourceGenerator<ClassFinder, NewUsingsMapper>());

            // Assert
            _ = AssertValidCompilation(generatorDiagnostics, newComp);
            var newSourceText = AssertSingleGeneratedClass(newComp);
            AssertSourceTextIsEqual(newSourceText, @"
using System;
using System.Collections.Generic;
using System.Text;
namespace CodeNamespace.Classes
{
    public partial class Simple
    {
    }
}");
        }
    }
}
