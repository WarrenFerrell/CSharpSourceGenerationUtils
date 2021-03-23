using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CSharp.RuntimeBinder;
//using Microsoft.CodeAnalysis.CSharp.Test.Utilities;
//using Microsoft.CodeAnalysis.CSharp.UnitTests;
//using Microsoft.CodeAnalysis.Diagnostics;
//using Microsoft.CodeAnalysis.Test.Utilities;
//using Microsoft.CodeAnalysis.Text;
//using Roslyn.Test.Utilities;
//using Roslyn.Test.Utilities.TestGenerators;
//using Roslyn.Utilities;
using Xunit;
using CSharp.SourceGenerationUtils.Core;
using CSharp.SourceGenerationUtils.Generator;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace CSharp.SourceGenerationUtils.Tests
{
    public class ClassSourceGeneratorTests
    {
        private const string SimplePartialClassCode = @"
using System;
namespace CodeNamespace.Classes
{
    public partial class Simple
    {
        public int IntProp { get; set; }
        public string StringProp { get; set; }
    }
}";

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

        public class PropertyCopyMapper : NoOpClassMapper
        {
            public override ClassCodeGenerator ProcessProperties(INamedTypeSymbol t, ClassCodeGenerator generator, IEnumerable<IPropertySymbol> properties)
            {
                foreach(var prop in properties)
                {
                    generator.AddAutoProperty(prop.Type.ToDisplayString(), prop.Name + "Copy");
                }
                return generator;
            }
        }

        [Fact]
        public void PropertyCopyProducesMatchingProperties()
        {
            // Arrange
            Compilation inputCompilation = CreateCompilation(SimplePartialClassCode);

            // Act
            var newComp = RunGenerators(inputCompilation, out var generatorDiagnostics, new ClassSourceGenerator<ClassFinder, PropertyCopyMapper>());

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

        internal static void AssertSourceTextIsEqual(SourceText newSourceText, string expectedSource)
        {
            // TODO: Switch To Snapper Yaml comparison
            Assert.Equal(expectedSource.Trim().Replace("\r", ""), newSourceText.ToString().Trim().Replace("\r", ""));
        }

        internal static SourceText AssertSingleGeneratedClass(Compilation secondCompilation)
        {
            Assert.Equal(2, secondCompilation.SyntaxTrees.Count());
            var lastTree = secondCompilation.SyntaxTrees.Last();
            Assert.EndsWith("Simple.Generated.cs", lastTree.FilePath);
            Assert.True(lastTree.TryGetText(out var newSourceText));
            return newSourceText;
        }

        internal static IEnumerable<Diagnostic> AssertValidCompilation(IEnumerable<Diagnostic> generatorDiagnostics, Compilation secondCompilation)
        {
            Assert.Empty(generatorDiagnostics);
            var diagnostics = secondCompilation.GetDiagnostics();
            Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
            return diagnostics;
        }

        internal static Compilation CreateCompilation(string source) => CSharpCompilation.Create(
            assemblyName: "compilation",
            syntaxTrees: new[] { CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview)) },
            references: new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        internal static GeneratorDriver CreateDriver(Compilation compilation, params ISourceGenerator[] generators) => CSharpGeneratorDriver.Create(
            generators: ImmutableArray.Create(generators),
            additionalTexts: ImmutableArray<AdditionalText>.Empty,
            parseOptions: (CSharpParseOptions)compilation.SyntaxTrees.First().Options,
            optionsProvider: null
        );

        internal static Compilation RunGenerators(Compilation compilation, out ImmutableArray<Diagnostic> diagnostics, params ISourceGenerator[] generators)
        {
            CreateDriver(compilation, generators).RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation, out diagnostics);
            return updatedCompilation;
        }
    }
}
