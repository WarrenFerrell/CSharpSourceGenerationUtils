using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CSharp.SourceGenerationUtils.Core.CodeGenerators;

namespace CSharp.SourceGenerationUtils.Core
{
    public partial class ClassCodeGenerator : ICodeGenerator
    {
        private const string Indent = "    ";

        public string ClassModifiers { get; set; } = "public";
        public string? Inheritance { get; set; }
        public string ClassNamespace { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string FileName { get; set; } = "";
        
        public bool GenerationComplete { get; private set; }
        public string? Source { get; private set; }

        public StringBuilder UsingStatements { get; } = new StringBuilder();
        public StringBuilder ClassBodyBuilder { get; } = new StringBuilder();

        public string GetFileName() => FileName;

        public string GetSource()
        {
            if (GenerationComplete) return Source!;
            var sourceBuilder = new StringBuilder()
                .Append(UsingStatements)
                .Append(
$@"namespace {ClassNamespace}
{{
    {ClassModifiers} class {ClassName} {(Inheritance != null ? $": {Inheritance}" : "")}
    {{
")
            .Append(ClassBodyBuilder)
            .Append(
@"    }
}
").ToString();
            Source = sourceBuilder.ToString();
            GenerationComplete = true;
            return Source;
        }

        public ClassCodeGenerator Append(string text)
        {
            ClassBodyBuilder.Append(text);
            return this;
        }

        public ClassCodeGenerator AddLine(string line)
        {
            ClassBodyBuilder.AppendLine(line);
            return this;
        }

        public ClassCodeGenerator BlankLine() => AddLine("");

        /// <summary>
        /// Add line that is indented by 12 spaces
        /// </summary>
        /// <param name="line"></param>
        public virtual ClassCodeGenerator AddIndentedLine(string line) =>
             Append("            ").AddLine(line);

        public virtual ClassCodeGenerator AddUsingDirective(Type type)
        {
            UsingStatements.AppendLine($"using {type.Namespace};" );
            return this;
        }

        public virtual ClassCodeGenerator AddUsingDirectives(IEnumerable<Type> types) => types.Aggregate(this, (a, t) => a.AddUsingDirective(t));

        public virtual ClassCodeGenerator Apply(Func<ClassCodeGenerator, ClassCodeGenerator> action) => action.Invoke(this);

        public static string GetIndent(int level) => string.Concat(Enumerable.Repeat(Indent, level));
    }
}
