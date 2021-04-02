using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerationUtils.Core
{
    public class Param : ParameterOrTypeDeclaration
    {
        private Param(string? modifiers, string type, string name) : base(modifiers, type, name) { }

        public static Param Of<TType>(string named) => new(null, typeof(TType).Name, named);
        public static Param Of(ITypeSymbol type, string named) => new(null, type.ToDisplayString(), named);
    }
}
