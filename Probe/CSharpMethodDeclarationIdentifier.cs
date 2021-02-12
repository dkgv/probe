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
        
        public bool Matches(string line, string nextLine)
        {
            // Ensure method begins with modifier
            var parts = line.Split(" ");
            if (!Modifiers.Contains(parts[0]))
            {
                return false;
            }

            // Ensure we aren't looking at a class
            if (parts.Any(p => ClassTypes.Contains(p)))
            {
                return false;
            }

            var numOpenParenthesis = line.Count(c => c == '('); 
            var numCloseParenthesis = line.Count(c => c == ')'); 
            if (numOpenParenthesis == 0 || numCloseParenthesis == 0 || numOpenParenthesis != numCloseParenthesis)
            {
                return false;
            }

            return nextLine.EndsWith("{");
        }
    }
}