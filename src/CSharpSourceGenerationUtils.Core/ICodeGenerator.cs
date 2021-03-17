using System;

namespace CSharpSourceGenerationUtils
{
    public interface ICodeGenerator
    {
        string GetSource();
        string GetFileName();
    }
}
