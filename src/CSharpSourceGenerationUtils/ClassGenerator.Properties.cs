using System;
using System.Linq;
using System.Text;

namespace PrimaryConstructor
{
    public partial class ClassGenerator : ISourceGenerator
    {
        public virtual ClassGenerator AddProperty(string modifiers, string type, string name, string accessors) =>
            AddIndentedLine($"{modifiers} {type} {name} {accessors}");

        public virtual ClassGenerator AddAutoProperty(string modifiers, string type, string name, string accessors) =>
            AddProperty("public", type, name, "{ get; set; }");
    }
}
