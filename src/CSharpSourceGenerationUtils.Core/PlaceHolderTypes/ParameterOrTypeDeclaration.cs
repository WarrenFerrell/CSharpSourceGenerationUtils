namespace CSharp.SourceGenerationUtils.Core
{
    public abstract class ParameterOrTypeDeclaration
    {
        protected ParameterOrTypeDeclaration(string type, string name) : this(null, type, name) { }

        protected ParameterOrTypeDeclaration(string? modifiers, string type, string name)
        {
            Modifiers = modifiers;
            Type = type;
            Name = name;
        }

        public string? Modifiers { get; protected set; }
        public virtual string Type { get; protected set; }
        public string Name { get; protected set; }

        public override bool Equals(object obj)
        {
            return obj is ParameterOrTypeDeclaration y
                ? Modifiers == y.Modifiers &&
                    Type == y.Type &&
                    Name == y.Name
                : false;
        }

        public override int GetHashCode() => Type.GetHashCode();
        public override string ToString() => $"{Modifiers!.IfSomeAppendSpace()}{Type} {Name}";
    }
}
