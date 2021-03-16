using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PrimaryConstructor
{
    public partial class ClassGenerator : ISourceGenerator
    {
        public string Modifiers { get; set; } = "public";
        public string? Inheritance { get; set; }
        public string Namespace { get; set; } = "";
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
$@"namespace {Namespace}
{{
    {Modifiers} class {ClassName} {(Inheritance != null ? $": {Inheritance}" : "")}
    {{
")
            .Append(ClassBodyBuilder)
            .Append(@"
        }
    }
}
").ToString();
            Source = sourceBuilder.ToString();
            GenerationComplete = true;
            return Source;
        }

        public ClassGenerator Append(string text)
        {
            ClassBodyBuilder.Append(text);
            return this;
        }

        public ClassGenerator AddLine(string line)
        {
            ClassBodyBuilder.AppendLine(line);
            return this;
        }

        public ClassGenerator BlankLine() => AddLine("");

        /// <summary>
        /// Add line that is indented by 12 spaces
        /// </summary>
        /// <param name="line"></param>
        public virtual ClassGenerator AddIndentedLine(string line) =>
             Append("            ").AddLine(line);

        public virtual ClassGenerator AddUsingDirective(Type type)
        {
            UsingStatements.AppendLine($"using {type.Namespace};" );
            return this;
        }

        public virtual ClassGenerator Apply(Func<ClassGenerator, ClassGenerator> action) => action.Invoke(this);
    }
}
