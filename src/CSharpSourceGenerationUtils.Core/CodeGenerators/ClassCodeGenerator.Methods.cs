using System;
using System.Linq;
using System.Text;

namespace CSharp.SourceGenerationUtils.Core
{
    public partial class ClassCodeGenerator
    {
        public virtual ClassCodeGenerator StartMethod(string modifiers, string type, string name, string accessors) =>
            AddIndentedLine($"{modifiers} {type} {name} {accessors}");

        //public virtual ClassCodeGenerator AddAutoProperty(string type, string name) =>
        //    AddProperty("public", type, name, "{ get; set; }");
    }
}
