using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.SourceGenerationUtils.Core
{
    public partial class ClassCodeBuilder
    {
        public virtual ClassCodeBuilder StartMethod(MemberDec signature, IEnumerable<Param> parameters, string body = "") =>
            AddIndentedLine($"{signature}({string.Join(",", parameters)}){body.IfSomeAppendSpace()}");

        //public virtual ClassCodeBuilder StartBlockMethod(string modifiers, string type, string name, string parameters, bool writeBrackets = true) =>
        //    AddIndentedLine($"{modifiers} {type} {name}({parameters})")
        //        .ApplyIf(() => writeBrackets, x => x
        //            .AddIndentedLine("{")
        //            .IncrementIndent()
        //        )
        //    ;

        public virtual ClassCodeBuilder StartBlockMethod(MemberDec signature, IEnumerable<Param> parameters, bool writeBrackets = true) =>
            StartMethod(signature, parameters)
                .ApplyIf(() => writeBrackets, x => x
                    .AddIndentedLine("{")
                    .IncrementIndent()
                )
            ;

        public virtual ClassCodeBuilder EndBlockMethod(bool writeBrackets = true) =>
            ApplyIf(() => writeBrackets, x => x
                .DecrementIndent()
                .AddIndentedLine("}")
                )
            ;
    }
}
