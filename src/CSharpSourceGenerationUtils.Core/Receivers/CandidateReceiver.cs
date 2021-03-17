using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpSourceGenerationUtils
{
    public class CandidateReceiver<T> : ISyntaxReceiver where T : CSharpSyntaxNode
    {
        public List<T> Candidates { get; } = new List<T>();

        /// <summary>
        /// Called for every syntax node in the compilation, 
        /// we inspect the nodes to find Candidate classes
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is T candidate && IsCandidate(candidate))
            {
                Candidates.Add(candidate);
            }
        }

        public virtual bool IsCandidate(T candidate) => true;

    }
}
