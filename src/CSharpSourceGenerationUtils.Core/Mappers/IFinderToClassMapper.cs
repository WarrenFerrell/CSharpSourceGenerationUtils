using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerationUtils.Core.Mappers
{
    public interface IFinderToClassMapper
    {
        string GetClassName(INamedTypeSymbol t);
        string GetFileName(INamedTypeSymbol t);

        ClassCodeBuilder GetGenerator(INamedTypeSymbol t);
        ClassCodeBuilder ProcessFields(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IFieldSymbol> fields);
        ClassCodeBuilder ProcessProperties(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IPropertySymbol> properties);
        ClassCodeBuilder ProcessMethods(INamedTypeSymbol t, ClassCodeBuilder generator, IEnumerable<IMethodSymbol> methos);
    }
}
