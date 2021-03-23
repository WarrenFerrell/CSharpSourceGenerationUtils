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
        private const string IndentSize = "    ";

        public string ClassModifiers { get; set; } = "public partial";

        /// <summary>
        /// Any inherited class or implemented interfaces (DO NOT specify a colon). Is set back to null when the generator is Reset,
        /// otherwise it is semantically equivalent to adding " : {...}" to the end of <see cref="ClassName"/>.
        /// </summary>
        public string? Inheritance { get; set; }
        public string ClassNamespace { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string FileName { get; set; } = "";

        public bool GenerationComplete { get; private set; }
        public string? Source { get; private set; }

        public int CurrentIndentLevel { get; private set; } = 2;
        public string CurrentIndent => GetIndent(CurrentIndentLevel);
        public StringBuilder UsingStatements { get; } = new StringBuilder();
        public StringBuilder ClassBodyBuilder { get; private set; } = new StringBuilder();

        public string GetFileName() => FileName;

        public string GetSource()
        {
            if (GenerationComplete)
                return Source!;
            var sourceBuilder = new StringBuilder()
                .Append(UsingStatements)
                .Append(
$@"namespace {ClassNamespace}
{{
    {ClassModifiers} class {ClassName}{(Inheritance != null ? $" : {Inheritance}" : "")}
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

        public void ResetBody()
        {
            ClassBodyBuilder = new StringBuilder();
            Inheritance = null;
            GenerationComplete = false;
            Source = null;
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

        public ClassCodeGenerator IncrementIndent() => AdjustIndent(1);
        public ClassCodeGenerator DecrementIndent() => AdjustIndent(-1);
        public ClassCodeGenerator Indent(int level = 1) => Append(GetIndent(level));
        public ClassCodeGenerator BlankLine() => AddLine("");

        /// <summary>
        /// Add line that is indented <see cref="CurrentIndentLevel"/> times.
        /// </summary>
        /// <param name="line">Text that follows the indention.</param>
        public virtual ClassCodeGenerator AddIndentedLine(string line) =>
             Indent(CurrentIndentLevel).AddLine(line);

        /// <summary>
        /// Add a block of text, indenting each line by <see cref="CurrentIndentLevel"/>.
        /// </summary>
        /// <remarks>Useful for specifying a block of text via a literal string to write without having to indent each line individually</remarks>
        /// <param name="line">Text block.</param>
        public virtual ClassCodeGenerator AddIndentedText(string text) =>
             Append(text.Replace("\n", $"\n{CurrentIndent}");

        public virtual ClassCodeGenerator AddUsingDirective(Type type)
        {
            UsingStatements.AppendLine($"using {type.Namespace};");
            return this;
        }

        public virtual ClassCodeGenerator AddUsingDirectives(IEnumerable<Type> types) => types.Aggregate(this, (a, t) => a.AddUsingDirective(t));

        public virtual ClassCodeGenerator Apply(Func<ClassCodeGenerator, ClassCodeGenerator> action) => action.Invoke(this);

        public virtual ClassCodeGenerator ApplyIf(Func<bool> condition, Func<ClassCodeGenerator, ClassCodeGenerator> action) => condition.Invoke() ? action.Invoke(this) : this;

        protected ClassCodeGenerator AdjustIndent(int deltaLevels)
        {
            CurrentIndentLevel += deltaLevels;
            return this;
        }

        public static string GetIndent(int level) => string.Concat(Enumerable.Repeat(IndentSize, level));
    }
}
