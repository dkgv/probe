using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Probe
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootDirectory = args[1];
            if (!Directory.Exists(rootDirectory))
            {
                return;
            }

            var dependencyExtractor = new CSharpDependencyExtractor();
            var methodExtractor = new CSharpMethodExtractor(new CSharpMethodDeclarationIdentifier());
            var replacer = new SourceReplacer(dependencyExtractor, methodExtractor, new RemovedCommentEmitStrategy());

            var projectProcessor = new ProjectProcessor(replacer);
            projectProcessor.Process(rootDirectory);
        }
    }
}