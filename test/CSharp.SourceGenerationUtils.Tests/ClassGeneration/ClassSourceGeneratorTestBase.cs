using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace CSharp.SourceGenerationUtils.Tests.ClassGeneration
{
    public class ClassSourceGeneratorTestBase
    {
        protected const string SimplePartialClassCode = @"
using System;
namespace CodeNamespace.Classes
{
    public partial class Simple
    {
        public int IntProp { get; set; }
        public string StringProp { get; set; }
    }
}";

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
