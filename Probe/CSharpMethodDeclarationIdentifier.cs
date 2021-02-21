using System.Linq;

namespace Probe
{
    public class CSharpMethodDeclarationIdentifier : IMethodDeclarationIdentifier
    {
        private static readonly string[] Primitives =
        {
            "int", "float", "short", "long", "bool", "string", "byte", "double", "object", "ulong", "ushort", "uint", "decimal"
        };

        private static readonly string[] AccessModifiers =
        {
            "public", "private", "protected", "internal"
        };

        private static readonly string[] ClassTypes =
        {
            "class", "interface", "enum", "record"
        };
        
        public MethodDeclaration TryFind(int lineIndex, RawCode code)
        {
            var currLine = code.Lines[lineIndex].Trim();
            if (string.IsNullOrEmpty(currLine))
            {
                return null;
            }

            // Ensure method begins with access modifier
            var parts = currLine.Split(" ");
            if (!AccessModifiers.Contains(parts[0]) && !Primitives.Contains(parts[0]))
            {
                return null;
            }

            // Check inline format `int x(int y) => y * 2;`
            if (currLine.Contains("=>") && currLine.Contains(";") && !currLine.Contains("{") && !currLine.Contains("}"))
            {
                return new MethodDeclaration
                {
                    Signature = new CodeSegment
                    {
                        LineStartIndex = lineIndex,
                        LineEndIndex = lineIndex,
                        Content = currLine.Split("=>")[0].Trim()
                    },
                    Variant = MethodVariant.InlineMethod
                };
            }

            // Ensure we aren't looking at a class
            if (parts.Any(p => ClassTypes.Contains(p)) || parts.Length < 2)
            {
                return null;
            }

            // We are looking at [accessModifier] [name]
            var numOpenParenthesis = currLine.Count(c => c == '(');
            var endIndex = -1;

            if (numOpenParenthesis > 0)
            {
                for (var i = lineIndex; i < code.Lines.Count; i++)
                {
                    var numCloseParenthesis = code.Lines[i].Count(ch => ch == ')');
                    if (numCloseParenthesis > 0)
                    {
                        numOpenParenthesis -= numCloseParenthesis;
                    }

                    if (numOpenParenthesis == 0)
                    {
                        endIndex = i;
                        break;
                    }
                }
            }

            if (endIndex == -1 || !code.Lines[endIndex + 1].EndsWith("{"))
            {
                return null;
            }

            var content = code.Join(lineIndex, endIndex);
            return new MethodDeclaration
            {
                Variant = MethodVariant.FullMethod,
                Signature = new CodeSegment
                {
                    Content = content,
                    LineStartIndex = lineIndex,
                    LineEndIndex = endIndex
                }
            };
        }
    }
}