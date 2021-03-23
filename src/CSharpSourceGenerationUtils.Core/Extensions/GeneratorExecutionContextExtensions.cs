using System.Text;
using CSharp.SourceGenerationUtils.Core.CodeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharp.SourceGenerationUtils.Core.Extensions
{
    public static class GeneratorExecutionContextExtensions
    {
        public static GeneratorExecutionContext AddFile(this GeneratorExecutionContext context, ICodeGenerator generator)
        {
            context.AddSource(generator.GetFileName(), SourceText.From(generator.GetSource(), Encoding.UTF8));
            return context;
        }
    }
}
