using System.IO;

namespace Probe
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = args[1];
            var code = new Code
            {
                Lines = File.ReadAllLines(filePath)
            };

            var dependencyExtractor = new CSharpDependencyExtractor();
            var methodExtractor = new CSharpMethodExtractor(new CSharpMethodDeclarationIdentifier());
            var replacer = new SourceReplacer(dependencyExtractor, methodExtractor, new NotImplementedEmitStrategy());
            
        }
    }
}