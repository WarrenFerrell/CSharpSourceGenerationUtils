using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PrimaryConstructor
{
    public class ClassReceiver : CandidateReceiver<ClassDeclarationSyntax>
    {
        public List<Type> RequiredAttributes { get; } = new List<Type>();

        public bool RequirePartial { get; set; }

        public override bool IsCandidate(ClassDeclarationSyntax candidate) =>
            candidate.AttributeLists.Count >= RequiredAttributes.Count;

        public IEnumerable<INamedTypeSymbol> GetClassSymbols(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            foreach (var @class in Candidates)
            {
                var model = compilation.GetSemanticModel(@class.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(@class);
                if (classSymbol is null) continue;

                var classAttributes = classSymbol.GetAttributes();
                if (RequiredAttributes.All(ra => classAttributes.Any(ca => ca.AttributeClass?.EqualsType(ra) ?? false)))
                {
                    yield return classSymbol;
                }
            }
        }

        public IEnumerable<ClassGenerator> GetClassGenerators(GeneratorExecutionContext context, ReceiverToClassGeneratorConfiguration mapper)
        {
            return GetClassSymbols(context).Select(cls => GetBaseGenerator(cls, mapper)
            .Apply(GetMemberProcessor(cls, mapper.ProcessField))
            .Apply(GetMemberProcessor(cls, mapper.ProcessProperty))
            .Apply(GetMemberProcessor(cls, mapper.ProcessMethod))
            );
        }

        protected virtual ClassGenerator GetBaseGenerator(INamedTypeSymbol cls, ReceiverToClassGeneratorConfiguration mapper) => new()
        {
            Namespace = cls.ContainingNamespace.ToDisplayString(),
            ClassName = mapper.GetClassName(cls),
            FileName = mapper.GetFileName(cls),
            Modifiers = "public",
        };

        public Func<ClassGenerator, ClassGenerator> GetMemberProcessor<TMemberSymbol>
            (INamedTypeSymbol cls, Func<TMemberSymbol, ClassGenerator, ClassGenerator>? action) => generator =>
            {
                var memberList = cls.GetMembers().OfType<TMemberSymbol>();
                memberList.ForEach(f => action?.Invoke(f, generator));
                return generator;
            };
    }

    public class ReceiverToClassGeneratorConfiguration
    {
        public Func<INamedTypeSymbol, string> GetClassName { get; set; } = t => $"{t.Name}";
        public Func<INamedTypeSymbol, string> GetFileName { get; set; } = t => $"{t.Name}.Generated.cs";
        //public Action<IFieldSymbol, ClassGenerator>? ProcessField { get; set; }
        //public Action<IPropertySymbol, ClassGenerator>? ProcessProperty { get; set; }
        //public Action<IMethodSymbol, ClassGenerator>? ProcessMethod { get; set; }
        public Func<IFieldSymbol, ClassGenerator, ClassGenerator>? ProcessField { get; set; }
        public Func<IPropertySymbol, ClassGenerator, ClassGenerator>? ProcessProperty { get; set; }
        public Func<IMethodSymbol, ClassGenerator, ClassGenerator>? ProcessMethod { get; set; }
        public List<Assembly> UsingAssemblies { get; } = new();
    }
}
