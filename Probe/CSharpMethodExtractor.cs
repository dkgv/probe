using System.Collections.Generic;
using System.Diagnostics;

namespace Probe
{
    public class CSharpMethodExtractor : IMethodExtractor
    {
        public CSharpMethodExtractor(IMethodDeclarationIdentifier methodDeclarationIdentifier)
        {
            MethodDeclarationIdentifier = methodDeclarationIdentifier;
        }
        
        public IMethodDeclarationIdentifier MethodDeclarationIdentifier { get; }

        public IEnumerable<MethodDefinition> ExtractMethods(RawCode code)
        {
            var numOpenBrackets = 0;
            MethodDefinition currMethodDef = null;
            
            for (var i = 0; i < code.Lines.Count; i++)
            {
                var currLine = code.Lines[i].Trim();

                if (currMethodDef == null)
                {
                    var declaration = MethodDeclarationIdentifier.TryFind(i, code);
                    if (declaration == null)
                    {
                        continue;
                    }
                    
                    // Found new method definition
                    var signatureLineEndIndex = declaration.Signature.LineEndIndex;
                    var methodBody = declaration.Variant == MethodVariant.FullMethod 
                        ? new CodeSegment
                        {
                            LineStartIndex = signatureLineEndIndex + 2,
                            LineEndIndex = signatureLineEndIndex + 1
                        }
                        : new CodeSegment{LineStartIndex = signatureLineEndIndex, LineEndIndex = signatureLineEndIndex };

                    currMethodDef = new MethodDefinition
                    {
                        Signature = currLine,
                        FullMethod = new CodeSegment
                        {
                            LineStartIndex = i,
                            LineEndIndex = i
                        },
                        MethodBody = methodBody,
                        Declaration = declaration
                    };

                    if (declaration.Variant == MethodVariant.InlineMethod)
                    {
                        currMethodDef.MethodBody.LineStartIndex = currMethodDef.MethodBody.LineEndIndex;

                        currMethodDef.FullMethod.Content = currLine;
                        currMethodDef.MethodBody.Content = currLine.Split("=>")[1].Trim();

                        yield return currMethodDef;
                        currMethodDef = null;
                    }
                }
                else
                {
                    // Check if } was reached, potentially end of method
                    if (currLine.Contains("}"))
                    {
                        numOpenBrackets--;
                    }
                    if (currLine.Contains("{"))
                    {
                        numOpenBrackets++;
                    }

                    currMethodDef.FullMethod.LineEndIndex++;

                    if (numOpenBrackets > 0)
                    {
                        // Expand method body length if an orphan { still exists
                        currMethodDef.MethodBody.LineEndIndex++;
                    }
                    else
                    {
                        // Set full method content from line start to line end
                        currMethodDef.FullMethod.Content = code.Join(currMethodDef.FullMethod);

                        // Set method body content
                        if (currMethodDef.MethodBody.NumLines == 0)
                        {
                            currMethodDef.MethodBody.Content = string.Empty;
                        }
                        else
                        {
                            currMethodDef.MethodBody.Content = code.Join(currMethodDef.MethodBody);

                            if (currMethodDef.MethodBody.Content.Trim().Equals("}"))
                            {
                                currMethodDef.MethodBody.Content = string.Empty;
                            }
                        }

                        yield return currMethodDef;
                        i = currMethodDef.FullMethod.LineEndIndex;

                        // Reset method definition
                        currMethodDef = null;
                    }
                }
            }
        }
    }
}