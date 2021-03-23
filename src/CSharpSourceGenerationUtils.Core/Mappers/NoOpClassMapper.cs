using System;
using System.Collections.Generic;
using System.Linq;
using CSharp.SourceGenerationUtils.Core.Mappers;
using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerationUtils.Core
{
    /// <summary>
    /// A Mapper that will produce a 
    /// </summary>
    public class NoOpClassMapper : IFinderToClassMapper
    {
        /// <summary>
        /// Set of Types that are contained in required assemblies. e.g. <see cref="string"/> is included by default so a `using System;` directive is added to produced <see cref="ClassCodeGenerator"/>
        /// </summary>
        public virtual List<Type> TypeNamespaces { get; } = new() { typeof(string) };
        public virtual string GetClassName(INamedTypeSymbol t) => $"{t.Name}";
        public virtual string GetFileName(INamedTypeSymbol t) => $"{t.Name}.Generated.cs";
        public virtual ClassCodeGenerator ProcessFields(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IFieldSymbol> fields) => gen;
        public virtual ClassCodeGenerator ProcessMethods(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IMethodSymbol> methos) => gen;
        public virtual ClassCodeGenerator ProcessProperties(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IPropertySymbol> properties) => gen;

        public virtual ClassCodeGenerator GetGenerator(INamedTypeSymbol cls) => new ClassCodeGenerator()
        {
            ClassNamespace = cls.ContainingNamespace.ToDisplayString(),
            ClassName = GetClassName(cls),
            FileName = GetFileName(cls),
            ClassModifiers = "public partial",
        }.AddUsingDirectives(TypeNamespaces.AsEnumerable())

        ;
    }
}
