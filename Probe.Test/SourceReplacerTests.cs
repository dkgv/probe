using NUnit.Framework;

namespace Probe.Test
{
    public class SourceReplacerTests
    {
        private SourceReplacer _replacer;

        [SetUp]
        public void SetUp()
        {
            var dependencyExtractor = new CSharpDependencyExtractor();
            var methodExtractor = new CSharpMethodExtractor(new CSharpMethodDeclarationIdentifier());
            _replacer = new SourceReplacer(dependencyExtractor, methodExtractor, new RemovedCommentEmitStrategy());
        }

        [TestCase(TestConstants.TestMethodPrintBody, "Console.WriteLine(\"Test\");", "// removed")]
        public void TestReplace(string source, string before, string after)
        {
            var lines = source.Split("\n");
            var code = new Code
            {
                Lines = lines
            };

            var @out = _replacer.Replace(code);
            Assert.False(@out.GetContent().Contains(before));
            Assert.True(@out.GetContent().Contains(after));
        }
    }
}
