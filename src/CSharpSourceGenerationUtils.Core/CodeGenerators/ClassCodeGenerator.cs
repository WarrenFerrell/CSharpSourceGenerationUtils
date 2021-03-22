using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharpSourceGenerationUtils
{
    public partial class ClassCodeGenerator : ICodeGenerator
    {
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
            .Append(@"
        }
    }
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

        public virtual ClassCodeGenerator Apply(Func<ClassCodeGenerator, ClassCodeGenerator> action) => action.Invoke(this);
    }
}
