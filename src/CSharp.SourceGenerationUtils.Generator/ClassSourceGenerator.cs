using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharp.SourceGenerationUtils.Core;
using CSharp.SourceGenerationUtils.Core.Extensions;
using CSharp.SourceGenerationUtils.Core.Mappers;
using CSharp.SourceGenerationUtils.Core.Receivers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
#pragma warning disable IDE0022 // Use expression body for methods

namespace CSharp.SourceGenerationUtils.Generator
{
    public static class ClassSourceGeneratorFactory
    {

        public static ClassSourceGenerator<TFinder, TMapper> Create<TFinder, TMapper>(TFinder _, TMapper __)
            where TFinder : ClassFinder, new()
            where TMapper : IFinderToClassMapper, new() => new ClassSourceGenerator<TFinder, TMapper>();
    }

    [Generator]
    public class ClassSourceGenerator<TFinder, TMapper> : ISourceGenerator
        where TFinder: ClassFinder, new()
        where TMapper: IFinderToClassMapper, new()
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TFinder());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is TFinder finder)) 
                return;

            var classGenerators = finder.GetClassGenerators(context, new TMapper());
            foreach (var generator in classGenerators)
            {
                context.AddFile(generator);
            }
        }
    }
}
