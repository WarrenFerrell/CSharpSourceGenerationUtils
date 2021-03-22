using System;
using System.Linq;
using System.Text;

namespace CSharpSourceGenerationUtils
{
    public partial class ClassCodeGenerator
    {
        public virtual ClassCodeGenerator AddProperty(string modifiers, string type, string name, string accessors) =>
            AddIndentedLine($"{modifiers} {type} {name} {accessors}");

        public virtual ClassCodeGenerator AddAutoProperty(string modifiers, string type, string name, string accessors) =>
            AddProperty("public", type, name, "{ get; set; }");
    }
}
