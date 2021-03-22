﻿using System.Collections.Generic;
using CSharpSourceGenerationUtils;
using Microsoft.CodeAnalysis;

namespace PrimaryConstructor
{
    public interface IFinderToClassMapper
    {
        string GetClassName(INamedTypeSymbol t);
        string GetFileName(INamedTypeSymbol t);

        ClassCodeGenerator GetGenerator(INamedTypeSymbol t);
        ClassCodeGenerator ProcessFields(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IFieldSymbol> fields);
        ClassCodeGenerator ProcessProperties(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IPropertySymbol> properties);
        ClassCodeGenerator ProcessMethods(INamedTypeSymbol t, ClassCodeGenerator gen, IEnumerable<IMethodSymbol> methos);
    }
}