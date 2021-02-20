namespace Probe.Test
{
    public class TestConstants
    {
        public const string NotImplementedException = "throw new NotImplementedException();";
        
        public const string TestMethodSignature = "public void Test()";
        public const string TestMethodEmptyBody = TestMethodSignature + "\n{\n}\n";
        public const string TestStaticMethod = "public static long Test(SomeType xyz, string param)\n{\n}";
        public const string TestMethodWithNestedMethod = "\n" + TestMethodSignature + "\n{\n" + TestMethodEmptyBody + "}\n";
        public const string TestMethodSpaceBody = TestMethodSignature + "\n{\n\n}\n";
        public const string TestMethodPrintBody = TestMethodSignature + "\n{\n" + PrintStatement + "}\n";
        public const string TestConstructor1 = "private Constructor(int x) : base(x)\n{\n}";
        public const string TestConstructor2Body = "foreach (var (a, b) in data)\n{\nforeach (var c in a)\n{\n}\n}\n";
        public const string TestConstructor2 = "public Constructor(List<Tuple<int[], int>> ints, int name = null) : this(name)\n{\n" + TestConstructor2Body + "}";
        public const string TestConstructor3 = "protected Constructor() : this(1)\n{\n}";
        public const string TestProperty = "public bool Enabled\n{\nget => true;\nset => x = value;\n}";

        public const string PrintStatement = "Console.WriteLine(\"Test\");\n";

        public const string TestInlineMethod = "int Do() => 1;\n";
        public const string TestNotInlineMethod1 = "return X.Select(y => y == 1);\n";
        public const string TestNotInlineMethod2 = "public int _x = -1;\nX.Select(y => y == 1);\n";

        public const string TestClassWithSingleMethod = "public class TestClass {\n"
            + TestMethodPrintBody
            + "}";

        public const string TestClassWithConstructor1And2 = "public class Constructor {\n"
            + TestConstructor1
            + "\n"
            + TestConstructor2
            + "\n"
            + "}";

        public const string TestNamespaceAndClassWithSingleMethod = "namespace Test\n{\npublic class TestClass {\n"
            + TestMethodPrintBody
            + "}\n}\n";
    }
}