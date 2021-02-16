using System;
using System.Linq;

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

            var methodDefinitions = MethodExtractor.ExtractMethods(code).ToList();
            Console.WriteLine($"\t-> Processing {methodDefinitions.Count} methods");
            foreach (var methodDef in methodDefinitions)
            {
                var bodyLineStart = methodDef.MethodBody.LineStartIndex;
                var bodyLineEnd = methodDef.MethodBody.LineEndIndex;

                switch (methodDef.Declaration.Variant)
                {
                    case MethodVariant.FullMethod:
                        if (methodDef.MethodBody.NumLines == 0)
                        {
                            continue;
                        }

                        var lines = new string[methodDef.MethodBody.NumLines];
                        for (var i = 0; i < methodDef.MethodBody.NumLines; i++)
                        {
                            lines[i] = EmitStrategy.Emit;
                        }
                        lines[^1] = notImplementedEmitStrategy.Emit;
                        code.Replace(bodyLineStart, bodyLineEnd, lines);
                        break;

                    case MethodVariant.InlineMethod:
                        var inline = methodDef.FullMethod.Content.Replace(methodDef.MethodBody.Content, notImplementedEmitStrategy.Emit);
                        code.Lines[bodyLineStart] = inline;
                        break;
                }
            }
        }
    }
}