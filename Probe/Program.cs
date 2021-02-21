using System.IO;

namespace Probe
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootDirectory = args[0];
            if (!Directory.Exists(rootDirectory))
            {
                return;
            }

            var importExtractor = new CSharpImportExtractor();
            var dependencyExtractor = new CSharpDependencyExtractor();
            var methodExtractor = new CSharpMethodExtractor(new CSharpMethodDeclarationIdentifier());
            var removedCommentEmitStrategy = new RemovedCommentEmitStrategy();
            var replacer = new SourceReplacer(dependencyExtractor, methodExtractor, removedCommentEmitStrategy, importExtractor);

            var projectProcessor = new ProjectProcessor(replacer);
            projectProcessor.Process(rootDirectory);
        }
    }
}