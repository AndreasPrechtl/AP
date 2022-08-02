using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    static class Program
    {
        public static void Main()
        {
            var eft = new UnitTestProject1.EFTest();

            eft.Setup();

            eft.ClearShit();
            eft.TestEFCreation();

        }
    }
}
