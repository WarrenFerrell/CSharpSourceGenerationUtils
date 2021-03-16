using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PrimaryConstructor
{
    public class PartialClassReceiver : ClassReceiver
    {
        public override bool IsCandidate(ClassDeclarationSyntax candidate) => 
            candidate.Modifiers.Any(m => m.Text == "partial") &&
            base.IsCandidate(candidate);
    }
}
