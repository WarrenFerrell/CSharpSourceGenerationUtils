using System;
using System.Linq;
using System.Text;

namespace CSharp.SourceGenerationUtils.Core
{
    public partial class ClassCodeBuilder
    {
        public virtual ClassCodeBuilder StartMethod(string modifiers, string type, string name, string parameters, bool writeParentheses = true) =>
            AddIndentedLine($"{modifiers} {type} {name}({parameters})")
                .ApplyIf(() => writeParentheses, x => x
                    .AddIndentedLine("{")
                    .IncrementIndent()
                )
            ;

        public virtual ClassCodeBuilder EndMethod(bool writeParentheses = true) =>
            ApplyIf(() => writeParentheses, x => x
                .DecrementIndent()
                .AddIndentedLine("}")
                )
            ;
    }
}
