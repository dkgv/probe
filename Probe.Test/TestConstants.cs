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

        public const string TestPropertyBody1 = "get => true;\nset => x = value;";
        public const string TestProperty1 = "public bool Enabled\n{\n" + TestPropertyBody1 + "\n}";
        public const string TestPropertyBody2 = "get => _enabled;\nset\n{\nif (_canBeDisabled || value)\n{\n_enabled = value;\n}\n}";
        public const string TestProperty2 = "public bool Enabled\n{\n" + TestPropertyBody2 + "\n}";

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
        public const string TestClassWithTwoProperties = "public class Constructor {\n"
                                                            + TestProperty1
                                                            + "\n"
                                                            + TestProperty2
                                                            + "\n"
                                                            + "}";
        public const string TestClassShiftLineIndexing1 = "public class Constructor {\n"
                                                            + TestConstructor3
                                                            + "\n"
                                                            + TestMethodWithNestedMethod
                                                            + "\n"
                                                            + "}";        
        public const string TestClassShiftLineIndexing2 = "namespace Test\n"
                                                          + "{\n" 
                                                          + "public class Constructor {\n"
                                                            + TestConstructor3
                                                            + "\n"
                                                            + TestMethodWithNestedMethod
                                                            + "\n"
                                                            + "}\n"
                                                            + "}";

        public const string TestClassShiftLineIndexing3 = @"
            using System.Collections.Generic;

            namespace Test
            {
                public class Constructor
                {
                    public readonly List<int> Ints = new List<int>();

                    public void DoSomething()
                    {
                        Ints.ForEach(i =>
                        {
                            X(i);
                        });
                    }

                    public void DoSomethingElse()
                    {
                        Ints.ForEach(i => Y(i));
                        Ints.Clear();
                    }
                }
            }";
        
        public const string TestNamespaceAndClassWithSingleMethod = "namespace Test\n{\npublic class TestClass {\n"
            + TestMethodPrintBody
            + "}\n}\n";
    }
}