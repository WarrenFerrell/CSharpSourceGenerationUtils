using System;
using System.Collections.Generic;
using System.Linq;
using CSharp.SourceGenerationUtils.Core.Mappers;
using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerationUtils.Core
{
    /// <summary>
    /// A Mapper that will produce a partial class file named {className}.Generated.cs with no methods or properties.
    /// </summary>
    public class NoOpClassMapper : IFinderToClassMapper
    {
        /// <summary>
        /// Set of Types that are contained in required assemblies. e.g. <see cref="string"/> is included by default so a `using System;` directive is added to produced <see cref="ClassCodeGenerator"/>
        /// </summary>
        public virtual List<Type> TypeNamespaces { get; } = new() { typeof(string) };
        
        ///<inheritdoc/>
        public virtual string GetClassName(INamedTypeSymbol t) => $"{t.Name}";

        ///<inheritdoc/>
        public virtual string GetFileName(INamedTypeSymbol t) => $"{t.Name}.Generated.cs";

        ///<inheritdoc/>
        public virtual ClassCodeGenerator ProcessFields(INamedTypeSymbol t, ClassCodeGenerator generator, IEnumerable<IFieldSymbol> fields) => generator;

        ///<inheritdoc/>
        public virtual ClassCodeGenerator ProcessMethods(INamedTypeSymbol t, ClassCodeGenerator generator, IEnumerable<IMethodSymbol> methos) => generator;

        ///<inheritdoc/>
        public virtual ClassCodeGenerator ProcessProperties(INamedTypeSymbol t, ClassCodeGenerator generator, IEnumerable<IPropertySymbol> properties) => generator;

        ///<inheritdoc/>
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
