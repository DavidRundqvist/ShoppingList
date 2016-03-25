using System;

using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace Test {
    public class Program
    {
        public static void Main(string[] args) {
//#if DNX451
//            new AutoRun().Execute(args);
//#else
            var runner = new AutoRun(typeof (Program).GetTypeInfo().Assembly);

            var writer = new NUnit.Common.ExtendedTextWrapper(Console.Out);
            runner.Execute(args, writer, Console.In);
//#endif
        }
    }
}