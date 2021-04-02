using System;

namespace CSharp.SourceGenerationUtils.Core
{
    internal static class StringExtensions
    {
        public static string IfSome(this string? src, Func<string, string> transformation, string ifNone = "") => $"{(src != null ? transformation : ifNone)}";
        public static string IfSomeAppendSpace(this string? src, string ifNone = "") => src.IfSome(x => $"{x} ", ifNone);
        public static string IfSomePrependSpace(this string? src, string ifNone = "") => src.IfSome(x => $" {x}", ifNone);
    }
}
