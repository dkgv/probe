using System.Collections.Generic;

namespace Probe
{
    public class CSharpMethodExtractor : IMethodExtractor
    {
        public CSharpMethodExtractor(IMethodDeclarationIdentifier methodDeclarationIdentifier)
        {
            MethodDeclarationIdentifier = methodDeclarationIdentifier;
        }
        
        public IMethodDeclarationIdentifier MethodDeclarationIdentifier { get; }

        public IEnumerable<MethodDefinition> ExtractMethods(Code code)
        {
            var numOpenBrackets = 0;
            MethodDefinition currMethodDef = null;
            
            for (var line = 1; line < code.Lines.Length; line++)
            {
                var currLine = code.Lines[line - 1].Trim();
                var nextLine = code.Lines[line].Trim();

                if (currMethodDef == null)
                {
                    if (!MethodDeclarationIdentifier.Matches(currLine, nextLine))
                    {
                        continue;
                    }
                    
                    // Found new method definition
                    currMethodDef = new MethodDefinition
                    {
                        Signature = currLine,
                        FullMethod = new CodeSegment
                        {
                            LineStart = line - 1,
                            LineEnd = line
                        },
                        MethodBody = new CodeSegment
                        {
                            LineStart = line + 1,
                            LineEnd = line
                        }
                    };
                }
                else
                {
                    // Check if } was reached, potentially end of method
                    if (currLine.EndsWith("}"))
                    {
                        numOpenBrackets--;
                    }
                    else if (currLine.EndsWith("{"))
                    {
                        numOpenBrackets++;
                    }
                    
                    currMethodDef.FullMethod.LineEnd++;
                    
                    // Expand method body length if an orphan { still exists
                    if (numOpenBrackets > 0)
                    {
                        currMethodDef.MethodBody.LineEnd++;
                    }
                    else
                    {
                        // Set method content from line start to line end
                        currMethodDef.FullMethod.Content = code.Join(currMethodDef.FullMethod);

                        // Remove }
                        currMethodDef.MethodBody.Content = code.Join(currMethodDef.MethodBody);
                        
                        yield return currMethodDef;
                        
                        // Reset method definition
                        currMethodDef = null;
                    }
                }
            }
        }
    }
}