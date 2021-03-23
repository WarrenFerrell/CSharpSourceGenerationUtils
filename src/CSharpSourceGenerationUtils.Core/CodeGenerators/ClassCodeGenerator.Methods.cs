using System;
using System.Linq;
using System.Text;

namespace CSharp.SourceGenerationUtils.Core
{
    public partial class ClassCodeGenerator
    {
        public virtual ClassCodeGenerator StartMethod(string modifiers, string type, string name, string parameters, bool writeParentheses = true) =>
            AddIndentedLine($"{modifiers} {type} {name}({parameters})")
                .ApplyIf(() => writeParentheses, x => x
                    .AddIndentedLine("{")
                    .IncrementIndent()
                )
            ;

        public virtual ClassCodeGenerator EndMethod(bool writeParentheses = true) =>
            ApplyIf(() => writeParentheses, x => x
                .DecrementIndent()
                .AddIndentedLine("}")
                )
            ;
    }
}
