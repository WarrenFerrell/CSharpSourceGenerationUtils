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
        public int IntProp { get; set;}
        public string StringProp { get; set;}
    }
}
";

        [Fact]
        public void NoOpMapperProducesSimpleClassWithNoBody()
        {
            // Arrange
            Compilation inputCompilation = CreateCompilation(SimplePartialClassCode);

            // Act
            var newComp = RunGenerators(inputCompilation, out var generatorDiagnostics, new ClassSourceGenerator<ClassFinder, NoOpClassMapper>());

            // Assert
            AssertValidCompilation(generatorDiagnostics, newComp);
            Assert.Equal(2, newComp.SyntaxTrees.Count());
            var lastTree = newComp.SyntaxTrees.Last();
            Assert.EndsWith("Simple.Generated.cs", lastTree.FilePath);
            Assert.True(lastTree.TryGetText(out var newSourceText));
            Assert.Equal(@"
using System;
namespace CodeNamespace.Classes
{
    public partial class Simple 
    {
    }
}
".Trim(), newSourceText.ToString().Trim().Replace("\r", ""));

        }

        internal static void AssertValidCompilation(IEnumerable<Diagnostic> generatorDiagnostics, Compilation newComp)
        {
            Assert.Empty(generatorDiagnostics);
            var diagnostics = newComp.GetDiagnostics();
            Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
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
