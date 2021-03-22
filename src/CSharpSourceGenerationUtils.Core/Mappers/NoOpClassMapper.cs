using System;
using System.Collections.Generic;
using CSharpSourceGenerationUtils;
using Microsoft.CodeAnalysis;

namespace PrimaryConstructor
{
    public class NoOpClassMapper : IFinderToClassMapper
    {
        //public virtual List<Assembly> UsingAssemblies { get; } = new() { typeof(string).Assembly };
        public virtual List<Type> TypeNamespaces { get; } = new() { typeof(string) };
        public virtual string GetClassName(INamedTypeSymbol t) => $"{t.Name}";
        public virtual string GetFileName(INamedTypeSymbol t) => $"{t.Name}.Generated.cs";
        public virtual ClassCodeGenerator ProcessFields(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IFieldSymbol> fields) => gen;
        public virtual ClassCodeGenerator ProcessMethods(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IMethodSymbol> methos) => gen;
        public virtual ClassCodeGenerator ProcessProperties(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IPropertySymbol> properties) => gen;

        public virtual ClassCodeGenerator GetGenerator(INamedTypeSymbol cls) => new()
        {
            ClassNamespace = cls.ContainingNamespace.ToDisplayString(),
            ClassName = GetClassName(cls),
            FileName = GetFileName(cls),
            ClassModifiers = "public partial",
            UsingStatements = 
        };
    }
}
