using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerationUtils.Core
{
    public partial class ClassCodeBuilder
    {
        public virtual ClassCodeBuilder AddProperty(MemberDec member, string accessors) =>
            AddIndentedLine($"{member} {accessors}");

        public virtual ClassCodeBuilder AddAutoProperty<TType>( string name, string accessors = "{ get; set; }") =>
            AddProperty(MemberDec.WithDef<TType>("public", name), accessors);

        public virtual ClassCodeBuilder AddAutoProperty(string type, string name, string accessors = "{ get; set; }") =>
            AddProperty(MemberDec.WithDef("public", type, name), accessors);

        public virtual ClassCodeBuilder AddAutoProperty(ITypeSymbol type, string name, string accessors = "{ get; set; }") =>
            AddProperty(MemberDec.WithDef("public", type, name), accessors);
    }
}
