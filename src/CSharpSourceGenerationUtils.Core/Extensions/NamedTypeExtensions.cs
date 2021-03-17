using System;
using Microsoft.CodeAnalysis;

namespace CSharpSourceGenerationUtils
{
    public static class NamedTypeExtensions
    {
        public static bool EqualsType(this INamedTypeSymbol x, Type y) =>
            x.ContainingNamespace?.Name == y.Namespace && x.Name == y.Name;
    }
}
