using System.Text;
using CSharpSourceGenerationUtils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PrimaryConstructor
{
    public static class GeneratorExecutionContextExtensions
    {
        public static GeneratorExecutionContext AddFile(this GeneratorExecutionContext context, string fileName, ICodeGenerator generator)
        {
            context.AddSource(fileName, SourceText.From(generator.GetSource(), Encoding.UTF8));
            return context;
        }
    }
}
