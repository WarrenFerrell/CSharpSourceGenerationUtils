using System;
using System.Linq;
using System.Text;

namespace CSharp.SourceGenerationUtils.Core
{
    public partial class ClassCodeBuilder
    {
        public virtual ClassCodeBuilder AddProperty(string modifiers, string type, string name, string accessors) =>
            AddIndentedLine($"{modifiers} {type} {name} {accessors}");

        public virtual ClassCodeBuilder AddAutoProperty(string type, string name) =>
            AddProperty("public", type, name, "{ get; set; }");
    }
}
