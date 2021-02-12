using System.IO;

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

        public Code Replace(Code code)
        {
            var methods = MethodExtractor.ExtractMethods(code);

            foreach (var methodDefinition in methods)
            {
                code.Replace(methodDefinition.MethodBody.LineStart, methodDefinition.MethodBody.LineEnd, EmitStrategy.Emit);
            }

            return code;
        }
    }
}