using System;

namespace CSharp.SourceGenerationUtils.Core.CodeGenerators
{
    public interface ICodeGenerator
    {
        string GetSource();
        string GetFileName();
    }
}
