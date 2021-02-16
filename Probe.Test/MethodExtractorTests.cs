using System.Linq;
using NUnit.Framework;

namespace Probe.Test
{
    public class MethodExtractorTests
    {
        private static readonly CSharpMethodExtractor Extractor = new(new CSharpMethodDeclarationIdentifier());
        
        [TestCase(TestConstants.TestInlineMethod, new[]{ "1;" })]
        public void TestInlineMethod(string source, string[] methodBodies)
        {
            var code = new Code
            {
                Lines = source.Split("\n")
            };

            Assert.AreEqual(MethodVariant.InlineMethod, Extractor.MethodDeclarationIdentifier.Find(0, code).Variant);

            var methods = Extractor.ExtractMethods(code).ToArray();

            Assert.AreEqual(methodBodies.Length, methods.Length);

            for (var i = 0; i < methodBodies.Length; i++)
            {
                Assert.AreEqual(methodBodies[i], methods[i].MethodBody.Content);
            }
        }

        [TestCase(TestConstants.TestMethod, 1, new[] {""})]
        [TestCase(TestConstants.TestMethodSpaceBody, 1, new[] {"\n"})]
        [TestCase(TestConstants.TestMethod + TestConstants.TestMethod, 2, new[] {"", ""})]
        [TestCase(TestConstants.TestMethodWithNestedMethod, 1, new[] { TestConstants.TestMethod })]
        public void TestExtractMethods(string source, int numExpectedMethods, string[] expectedMethodBodies)
        {
            var code = new Code
            {
                Lines = source.Split('\n')
            };
            var methods = Extractor.ExtractMethods(code).ToArray();
            
            Assert.AreEqual(numExpectedMethods, methods.Length);
            
            for (var i = methods.Length - 1; i >= 0; i--)
            {
                var methodDef = methods[i];
                var expectedMethodBody = expectedMethodBodies[i];
                Assert.AreEqual(expectedMethodBody, methodDef.MethodBody.Content);
            }
        }
    }
}