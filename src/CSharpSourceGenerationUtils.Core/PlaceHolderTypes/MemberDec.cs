using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerationUtils.Core
{
    public class MemberDec : ParameterOrTypeDeclaration
    {
        private MemberDec(string? modifiers, string type, string name) : base(modifiers, type, name) { }

        //public static Member Of<TType>(string named) => new(null, typeof(TType).Name, named);
        //public static Member Of(ITypeSymbol type, string named) => new(null, type.ToDisplayString(), named);
        public static MemberDec WithDef<TType>(string modifiers, string named) => new(modifiers, typeof(TType).Name, named);
        public static MemberDec WithDef(string modifiers, ITypeSymbol type, string named) => new(modifiers, type.ToDisplayString(), named);
        public static MemberDec WithDef(string modifiers, string type, string named) => new(modifiers, type, named);

        public static implicit operator MemberDec((string mods, ITypeSymbol type, string name) x) => WithDef(x.mods, x.type, x.name);
    }
}
