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

            foreach (var methodDefinition in MethodExtractor.ExtractMethods(code))
            {
                var lines = new string[methodDefinition.MethodBody.NumLines];
                for (var i = 0; i < methodDefinition.MethodBody.NumLines - 1; i++)
                {
                    lines[i] = EmitStrategy.Emit;
                }
                lines[^1] = notImplementedEmitStrategy.Emit;

                code.Replace(methodDefinition.MethodBody.LineStart, methodDefinition.MethodBody.LineEnd, lines);
            }
        }
    }
}