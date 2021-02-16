using NUnit.Framework;

namespace Probe.Test
{
    public class SourceReplacerTests
    {
        private SourceReplacer _replacer;
        private IMethodExtractor _methodExtractor;

        [SetUp]
        public void SetUp()
        {
            var dependencyExtractor = new CSharpDependencyExtractor(); 
            _methodExtractor = new CSharpMethodExtractor(new CSharpMethodDeclarationIdentifier());
            _replacer = new SourceReplacer(dependencyExtractor, _methodExtractor, new RemovedCommentEmitStrategy());
        }

        [TestCase(TestConstants.TestMethodPrintBody, "Console.WriteLine(\"Test\");", TestConstants.NotImplementedException)]
        [TestCase(TestConstants.TestMethodWithNestedMethod, TestConstants.TestMethod, TestConstants.NotImplementedException)]
        [TestCase(TestConstants.TestInlineMethod, "1;", TestConstants.NotImplementedException)]
        public void TestReplaceSingleMethod(string source, string bodyBefore, string after)
        {
            var lines = source.Split("\n");
            var code = new Code {Lines = lines};

            _replacer.Replace(code);

            Assert.False(code.GetContent().Contains(bodyBefore));
            Assert.True(code.GetContent().Contains(after));
        }

        [Test]
        public void TestReplaceWithinClass()
        {
            var lines = TestConstants.TestClassWithSingleMethod.Split("\n");
            var code = new Code {Lines = lines};

            _replacer.Replace(code);

            Assert.True(code.GetContent().Contains(TestConstants.NotImplementedException));
            Assert.False(code.GetContent().Contains(TestConstants.PrintStatement));
        }

        [Test]
        public void TestReplaceWithinNamespace()
        {
            var lines = TestConstants.TestNamespaceAndClassWithSingleMethod.Split("\n");
            var code = new Code {Lines = lines};

            _replacer.Replace(code);

            Assert.True(code.GetContent().Contains(TestConstants.NotImplementedException));
            Assert.False(code.GetContent().Contains(TestConstants.PrintStatement));
        }
    }
}
