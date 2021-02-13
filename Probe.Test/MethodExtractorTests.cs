using System.Linq;
using NUnit.Framework;

namespace Probe.Test
{
    public class MethodExtractorTests
    {
        private static readonly CSharpMethodExtractor Extractor = new(new CSharpMethodDeclarationIdentifier());

        [TestCase(TestConstants.TestMethodSignature, "    {")]
        [TestCase("public void Test(int a, int b)", "    {")]
        public void TestIsMethodDeclarationSuccess(string currLine, string nextLine)
        {
            Assert.AreEqual(MethodVariant.FullMethod, Extractor.MethodDeclarationIdentifier.Find(currLine, nextLine));
        }

        [TestCase(TestConstants.TestInlineMethod, new[]{ "1;" })]
        public void TestInlineMethod(string source, string[] methodBodies)
        {
            Assert.AreEqual(MethodVariant.InlineMethod, Extractor.MethodDeclarationIdentifier.Find(source, string.Empty));

            var code = new Code
            {
                Lines = source.Split("\n")
            };
            var methods = Extractor.ExtractMethods(code).ToArray();

            Assert.AreEqual(methodBodies.Length, methods.Length);

            for (var i = 0; i < methodBodies.Length; i++)
            {
                Assert.AreEqual(methodBodies[i], methods[i].MethodBody.Content);
            }
        }

        [TestCase(TestConstants.TestMethodSignature, "")]
        [TestCase("public void Test(", "    {")]
        [TestCase("public void Test)", "    {")]
        public void TestIsMethodDeclarationFailure(string currLine, string nextLine)
        {
            Assert.AreEqual(MethodVariant.None, Extractor.MethodDeclarationIdentifier.Find(currLine, nextLine));
        }

        [TestCase(TestConstants.TestMethod, 1,  new []{0}, new[] {""})]
        [TestCase(TestConstants.TestMethodSpaceBody, 1, new[]{1}, new[] {"\n"})]
        [TestCase(TestConstants.TestMethod + TestConstants.TestMethod, 2, new []{0, 0}, new[] {"", ""})]
        [TestCase(TestConstants.TestMethodWithNestedMethod, 1, new[]{3}, new[] { TestConstants.TestMethod })]
        public void TestExtractMethods(string source, int numExpectedMethods, int[] numExpectedBodyLines, string[] expectedMethodBodies)
        {
            var code = new Code
            {
                Lines = source.Split('\n')
            };
            var methods = Extractor.ExtractMethods(code).ToArray();
            
            Assert.AreEqual(numExpectedMethods, methods.Length);
            
            for (var i = methods.Length - 1; i >= 0; i--)
            {
                Assert.AreEqual(numExpectedBodyLines[i], methods[i].MethodBody.LineEnd - methods[i].MethodBody.LineStart);
                Assert.AreEqual(expectedMethodBodies[i], methods[i].MethodBody.Content);
            }
        }
    }
}