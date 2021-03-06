using System.Linq;
using NUnit.Framework;

namespace Probe.Test
{
    public class MethodExtractorTests
    {
        private static readonly CSharpMethodExtractor Extractor = new CSharpMethodExtractor(new CSharpMethodDeclarationIdentifier());
        
        [TestCase(TestConstants.TestInlineMethod, new[]{ "1;" })]
        public void TestInlineMethod(string source, string[] methodBodies)
        {
            var code = new RawCode(source.Split("\n"));

            Assert.AreEqual(MethodVariant.InlineMethod, Extractor.MethodDeclarationIdentifier.TryFind(0, code).Variant);

            var methods = Extractor.ExtractMethods(code).ToArray();

            Assert.AreEqual(methodBodies.Length, methods.Length);

            for (var i = 0; i < methodBodies.Length; i++)
            {
                Assert.AreEqual(methodBodies[i], methods[i].MethodBody.Content);
            }
        }

        [TestCase(TestConstants.TestNotInlineMethod1)]
        [TestCase(TestConstants.TestNotInlineMethod2)]
        public void TestNotInlineMethod(string source)
        {
            var code = new RawCode(source.Split("\n"));

            var methodDeclaration = Extractor.MethodDeclarationIdentifier.TryFind(0, code);
            Assert.AreEqual(null, methodDeclaration);
        }

        [TestCase(TestConstants.TestProperty1)]
        [TestCase(TestConstants.TestProperty2)]
        public void TestEnsureMethodNotProperty(string source)
        {
            var code = new RawCode(source.Split("\n"));

            var methodDeclaration = Extractor.MethodDeclarationIdentifier.TryFind(0, code);
            Assert.AreEqual(null, methodDeclaration);
        }

        [TestCase(TestConstants.TestMethodEmptyBody, 1, new[]{1}, new[] {""})]
        [TestCase(TestConstants.TestMethodSpaceBody, 1, new[]{1}, new[] {"\n"})]
        [TestCase(TestConstants.TestMethodEmptyBody + TestConstants.TestMethodEmptyBody, 2, new[]{1, 1}, new[] {"", ""})]
        [TestCase(TestConstants.TestMethodWithNestedMethod, 1, new[]{3}, new[] { TestConstants.TestMethodEmptyBody })]
        [TestCase(TestConstants.TestStaticMethod, 1, new[]{1}, new[] {""})]
        [TestCase(TestConstants.TestConstructor1, 1, new[]{1}, new[] {""})]
        [TestCase(TestConstants.TestConstructor2, 1, new[]{6}, new[] {TestConstants.TestConstructor2Body})]
        [TestCase(TestConstants.TestConstructor3, 1, new[]{1}, new[] {""})]
        [TestCase(TestConstants.TestClassWithSingleMethod, 1, new[]{1}, new[] {TestConstants.PrintStatement})]
        [TestCase(TestConstants.TestClassWithConstructor1And2, 2, new[]{1,6}, new[] {"", TestConstants.TestConstructor2Body })]
        public void TestExtractMethods(string source, int numExpectedMethods, int[] expectedMethodBodyLengths, string[] expectedMethodBodies)
        {
            var code = new RawCode(source.Split('\n'));
            var methods = Extractor.ExtractMethods(code).ToArray();
            
            Assert.AreEqual(numExpectedMethods, methods.Length);
            
            for (var i = methods.Length - 1; i >= 0; i--)
            {
                var methodDef = methods[i];
                Assert.AreEqual(expectedMethodBodyLengths[i], methodDef.MethodBody.NumLines);

                var expectedMethodBody = expectedMethodBodies[i];
                Assert.AreEqual(expectedMethodBody, methodDef.MethodBody.Content);
            }
        }
    }
}