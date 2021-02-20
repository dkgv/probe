using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Probe
{
    public class SourceReplacer
    {
        public SourceReplacer(IDependencyExtractor dependencyExtractor, IMethodExtractor methodExtractor, IEmitStrategy emitStrategy)
        {
            DependencyExtractor = dependencyExtractor;
            MethodExtractor = methodExtractor;
            EmitStrategy = emitStrategy;
        }

        public IDependencyExtractor DependencyExtractor { get; }

        public IMethodExtractor MethodExtractor { get; }

        public IEmitStrategy EmitStrategy { get; }

        public void Replace(Code code)
        {
            var notImplementedEmitStrategy = new NotImplementedEmitStrategy();

            var processed = new HashSet<string>();

            MethodDefinition FindUnprocessed()
            {
                // We need to continuously extract methods if a modification changes indexing
                var definitions = MethodExtractor.ExtractMethods(code);
                return definitions.FirstOrDefault(definition => !processed.Contains(definition.Signature));
            }

            MethodDefinition next;
            while ((next = FindUnprocessed()) != default)
            {
                var bodyLineStart = next.MethodBody.LineStartIndex;
                var bodyLineEnd = next.MethodBody.LineEndIndex;

                switch (next.Declaration.Variant)
                {
                    case MethodVariant.FullMethod:
                        if (next.MethodBody.NumLines == 0)
                        {
                            continue;
                        }

                        var lines = new List<string>(next.MethodBody.NumLines);
                        for (var i = 0; i < next.MethodBody.NumLines; i++)
                        {
                            lines.Add(EmitStrategy.Emit);
                        }

                        lines[^1] = notImplementedEmitStrategy.Emit;

                        if (next.MethodBody.NumLines == 1 && !next.FullMethod.Content.Contains("}"))
                        {
                            lines.Add("}");
                            code.Lines.Insert(bodyLineEnd, "");
                        }

                        code.Replace(bodyLineStart, bodyLineEnd, lines.ToArray());
                        break;

                    case MethodVariant.InlineMethod:
                        var inline = next.FullMethod.Content.Replace(next.MethodBody.Content,
                            notImplementedEmitStrategy.Emit);
                        code.Lines[bodyLineStart] = inline;
                        break;
                }

                processed.Add(next.Signature);
            }
        }
    }
}