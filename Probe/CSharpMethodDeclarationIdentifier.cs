using System.Linq;

namespace Probe
{
    public class CSharpMethodDeclarationIdentifier : IMethodDeclarationIdentifier
    {
        private static readonly string[] Modifiers =
        {
            "public", "private", "protected", "internal"
        };

        private static readonly string[] ClassTypes =
        {
            "class", "interface", "enum", "record"
        };
        
        public MethodVariant Find(string line, string nextLine)
        {
            // Check inline format `int x(int y) => y * 2;`
            if (line.Contains("=>") && line.Contains(";") && !line.Contains("{") && !line.Contains("}"))
            {
                return MethodVariant.InlineMethod;
            }

            // Ensure method begins with modifier
            var parts = line.Split(" ");
            if (!Modifiers.Contains(parts[0]))
            {
                return MethodVariant.None;
            }

            // Ensure we aren't looking at a class
            if (parts.Any(p => ClassTypes.Contains(p)))
            {
                return MethodVariant.None;
            }

            var numOpenParenthesis = line.Count(c => c == '('); 
            var numCloseParenthesis = line.Count(c => c == ')'); 
            if (numOpenParenthesis == 0 || numCloseParenthesis == 0 || numOpenParenthesis != numCloseParenthesis)
            {
                return MethodVariant.None;
            }

            if (nextLine.EndsWith("{"))
            {
                return MethodVariant.FullMethod;
            }

            return MethodVariant.None;
        }
    }
}