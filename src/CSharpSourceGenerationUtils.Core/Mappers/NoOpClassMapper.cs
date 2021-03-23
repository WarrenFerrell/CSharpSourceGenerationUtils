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
        /// Set of Types that are contained in required assemblies. e.g. <see cref="string"/> is included by default so a `using System;` directive is added to produced <see cref="ClassCodeBuilder"/>
        /// </summary>
        public virtual List<Type> UsingTypeNamespaces { get; } = new() { typeof(string) };
        
        ///<inheritdoc/>
        public virtual string GetClassName(INamedTypeSymbol t) => $"{t.Name}";

        ///<inheritdoc/>
        public virtual string GetFileName(INamedTypeSymbol t) => $"{t.Name}.Generated.cs";

        ///<inheritdoc/>
        public virtual ClassCodeBuilder ProcessFields(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IFieldSymbol> fields) => generator;

        ///<inheritdoc/>
        public virtual ClassCodeBuilder ProcessMethods(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IMethodSymbol> methos) => generator;

        ///<inheritdoc/>
        public virtual ClassCodeBuilder ProcessProperties(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IPropertySymbol> properties) => generator;

        ///<inheritdoc/>
        public virtual ClassCodeBuilder GetGenerator(INamedTypeSymbol cls) => new ClassCodeBuilder()
        {
            ClassNamespace = cls.ContainingNamespace.ToDisplayString(),
            ClassName = GetClassName(cls),
            FileName = GetFileName(cls),
            ClassModifiers = "public partial",
        }.AddUsingDirectives(UsingTypeNamespaces.AsEnumerable())
        ;
    }
}
