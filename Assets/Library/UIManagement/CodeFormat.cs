namespace Library.UIManagement
{
    public static class CodeFormat
    {
        private static readonly string CodeFormatString =
            @"
namespace Library.UIManagement
{{
    public enum UiType
        {{
            {0}
        }}
}}
";

        public static string FormatCode(string codeBuilder)
        {
            return string.Format(CodeFormatString, codeBuilder);
        }
    }
}