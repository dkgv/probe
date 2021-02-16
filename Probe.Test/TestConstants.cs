namespace Probe.Test
{
    public class TestConstants
    {
        public const string NotImplementedException = "throw new NotImplementedException();";
        
        public const string TestMethodSignature = "public void Test()";
        public const string TestMethod = TestMethodSignature + "\n{\n}\n";
        public const string TestStaticMethod = "public static long Test(SomeType xyz, string param)\n{\n}";
        public const string TestMethodWithNestedMethod = "\n" + TestMethodSignature + "\n{\n" + TestMethod + "}\n";
        public const string TestMethodSpaceBody = TestMethodSignature + "\n{\n\n}\n";
        public const string TestMethodPrintBody = TestMethodSignature + "\n{\n" + PrintStatement + "\n}\n";

        public const string PrintStatement = "Console.WriteLine(\"Test\");";
        public const string TestInlineMethod = "int Do() => 1;\n";
        public const string TestNotInlineMethod = "return X.Select(y => y == 1);\n";

        public const string TestClassWithSingleMethod = "public class TestClass {\n"
            + TestMethodPrintBody
            + "}";

        public const string TestNamespaceAndClassWithSingleMethod = "namespace Test\n{\npublic class TestClass {\n"
            + TestMethodPrintBody
            + "}\n}\n";
    }
}