using System;
using System.Linq;
using NUnit.Framework;

namespace Probe.Test
{
    public class SourceReplacerTests
    {
        private SourceReplacer _replacer;
        private IMethodExtractor _methodExtractor;
        private IImportExtractor _importExtractor;

        [SetUp]
        public void SetUp()
        {
            var dependencyExtractor = new CSharpDependencyExtractor(); 
            _methodExtractor = new CSharpMethodExtractor(new CSharpMethodDeclarationIdentifier());
            _importExtractor = new CSharpImportExtractor();
            _replacer = new SourceReplacer(dependencyExtractor, _methodExtractor, new RemovedCommentEmitStrategy(), _importExtractor);
        }

        [TestCase(TestConstants.TestMethodPrintBody, "Console.WriteLine(\"Test\");", TestConstants.NotImplementedException)]
        [TestCase(TestConstants.TestMethodWithNestedMethod, TestConstants.TestMethodEmptyBody, TestConstants.NotImplementedException)]
        [TestCase(TestConstants.TestInlineMethod, "1;", TestConstants.NotImplementedException)]
        [TestCase(TestConstants.TestClassWithSingleMethod, TestConstants.PrintStatement, TestConstants.NotImplementedException)]
        public void TestReplaceSingleMethod(string source, string bodyBefore, string expectedAfter)
        {
            var code = new RawCode(source.Split("\n"));

            _replacer.Replace(code);

            Assert.False(code.GetContent().Contains(bodyBefore));
            Assert.True(code.GetContent().Contains(expectedAfter));
        }

        [TestCase(TestConstants.TestProperty1, TestConstants.TestPropertyBody1)]
        [TestCase(TestConstants.TestProperty2, TestConstants.TestPropertyBody2)]
        public void TestDontReplaceIndividualProperty(string source, string expectedBody)
        {
            var code = new RawCode(source.Split("\n"));
            _replacer.Replace(code);
            Assert.True(code.GetContent().Contains(expectedBody));
        }

        [TestCase(TestConstants.TestConstructor1, "", TestConstants.NotImplementedException)]
        [TestCase(TestConstants.TestConstructor2, TestConstants.TestConstructor2Body, TestConstants.NotImplementedException)]
        [TestCase(TestConstants.TestConstructor3, "", TestConstants.NotImplementedException)]
        public void TestReplaceConstructor(string source, string bodyBefore, string expectedAfter)
        {
            var code = new RawCode(source.Split("\n"));

            _replacer.Replace(code);

            if (!string.IsNullOrEmpty(bodyBefore))
            {
                Assert.False(code.GetContent().Contains(bodyBefore));
            }

            Assert.True(code.GetContent().Contains(expectedAfter));
        }

        [Test]
        public void TestReplaceClassWithTwoConstructors()
        {
            var lines = TestConstants.TestClassWithConstructor1And2.Split("\n");
            var code = new RawCode(lines);

            _replacer.Replace(code);

            Assert.False(code.GetContent().Contains(TestConstants.TestConstructor1));
            Assert.False(code.GetContent().Contains(TestConstants.TestConstructor2));
            Assert.True(code.GetContent().Contains(TestConstants.NotImplementedException));
        }

        [Test]
        public void TestDontReplaceClassWithTwoProperties()
        {
            var lines = TestConstants.TestClassWithTwoProperties.Split("\n");
            var code = new RawCode(lines);

            _replacer.Replace(code);

            Assert.True(code.GetContent().Contains(TestConstants.TestProperty1));
            Assert.True(code.GetContent().Contains(TestConstants.TestProperty2));
        }

        [TestCase(TestConstants.TestClassShiftLineIndexing1, 3)]
        [TestCase(TestConstants.TestClassShiftLineIndexing2, 4)]
        [TestCase(TestConstants.TestClassShiftLineIndexing3, 4)]
        public void TestClassConstructorAndMethodLineShifting(string source, int expectedBracketCount)
        {
            var lines = source.Split("\n");
            var code = new RawCode(lines);

            _replacer.Replace(code);

            Assert.AreEqual(expectedBracketCount, code.GetContent().Count(ch => ch == '}'));
            Assert.AreEqual(expectedBracketCount, code.GetContent().Count(ch => ch == '{'));
        }

        [Test]
        public void TestReplaceWithinNamespace()
        {
            var lines = TestConstants.TestNamespaceAndClassWithSingleMethod.Split("\n");
            var code = new RawCode(lines);

            _replacer.Replace(code);

            Assert.True(code.GetContent().Contains(TestConstants.NotImplementedException));
            Assert.False(code.GetContent().Contains(TestConstants.PrintStatement));
            Assert.True(code.Lines[0].Equals(new NotImplementedEmitStrategy().Imports.First()));
        }
    }
}
