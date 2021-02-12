namespace Probe.Test
{
    public class TestConstants
    {
        public const string TestMethodSignature = "public void Test()";
        public const string TestMethod = TestMethodSignature + "\n{\n}\n";
        public const string TestMethodSpaceBody = TestMethodSignature + "\n{\n\n}\n";
        public const string TestMethodWithNestedMethod = "\n" + TestMethodSignature + "\n{\n" + TestMethod + "}\n";
        public const string TestMethodPrintBody = TestMethodSignature + "\n{\nConsole.WriteLine(\"Test\");\n}";
    }
}