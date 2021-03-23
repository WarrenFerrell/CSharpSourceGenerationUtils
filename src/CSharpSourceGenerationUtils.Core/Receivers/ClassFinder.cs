using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CSharp.SourceGenerationUtils.Core.Extensions;
using CSharp.SourceGenerationUtils.Core.Mappers;
using CSharp.SourceGenerationUtils.Core.Receivers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharp.SourceGenerationUtils.Core
{
    public class ClassFinder : CandidateReceiver<ClassDeclarationSyntax>
    {
        public List<Type> RequiredAttributes { get; } = new();

        public bool RequirePartial { get; set; }

        public override bool IsCandidate(ClassDeclarationSyntax candidate) =>
            candidate.AttributeLists.Count >= RequiredAttributes.Count;

        /// <summary>
        /// Retrieve the meta data from the <see cref="GeneratorExecutionContext"/> for the 
        /// </summary>
        /// <param name="context">The generator context used for code generation</param>
        /// <returns></returns>
        public IEnumerable<INamedTypeSymbol> GetClassSymbols(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            foreach (var classCandy in Candidates)
            {
                var model = compilation.GetSemanticModel(classCandy.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(classCandy);
                if (classSymbol is null)
                    continue;

                var classAttributes = classSymbol.GetAttributes();
                if (RequiredAttributes.All(ra => classAttributes.Any(ca => ca.AttributeClass?.EqualsType(ra) ?? false)))
                {
                    if (RequirePartial && !classCandy.Modifiers.Any(m => m.ValueText == "partial"))
                        throw new InvalidOperationException($"class {classSymbol.Name} must be marked partial");

                    yield return classSymbol;
                }
            }
        }

        /// <summary>
        /// Convert found classes into a set of <see cref="ClassCodeBuilder"/>s
        /// </summary>
        /// <param name="context">The generator context used for code generation</param>
        /// <param name="mapper">The <see cref="IFinderToClassMapper"/> used to map each matching class returned from <see cref="GetClassSymbols(GeneratorExecutionContext)"/> /param>
        /// <returns></returns>
        public IEnumerable<ClassCodeBuilder> GetClassGenerators(GeneratorExecutionContext context, IFinderToClassMapper mapper, ClassCodeBuilder? generator = null) =>
            GetClassSymbols(context).Select(cls => (generator ?? mapper.GetGenerator(cls))
            .Apply(gen => mapper.ProcessFields(cls, gen, cls.GetMembers().OfType<IFieldSymbol>()))
            .Apply(gen => mapper.ProcessProperties(cls, gen, cls.GetMembers().OfType<IPropertySymbol>()))
            .Apply(gen => mapper.ProcessMethods(cls, gen, cls.GetMembers().OfType<IMethodSymbol>()))
            );
    }
}
