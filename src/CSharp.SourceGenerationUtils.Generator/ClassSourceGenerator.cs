using CSharp.SourceGenerationUtils.Core;
using CSharp.SourceGenerationUtils.Core.Extensions;
using CSharp.SourceGenerationUtils.Core.Mappers;
using Microsoft.CodeAnalysis;
#pragma warning disable IDE0022 // Use expression body for methods

namespace CSharp.SourceGenerationUtils.Generator
{
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
