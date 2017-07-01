using System;
using System.Linq;
using DynamicMosaic;
using DynamicParser;
using DynamicProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor = DynamicParser.Processor;

namespace DynamicMosaicTest
{
    [TestClass]
    public class ReflexTest
    {
        [TestMethod]
        // ReSharper disable once FunctionComplexityOverflow
        public void ReflexTest1()
        {
            SignValue[,] map = new SignValue[4, 4];
            map[0, 0] = SignValue.MaxValue;
            map[2, 0] = SignValue.MaxValue;
            map[1, 1] = SignValue.MaxValue;
            map[2, 1] = SignValue.MaxValue;
            map[0, 2] = SignValue.MaxValue;
            map[2, 2] = SignValue.MaxValue;
            map[3, 3] = SignValue.MaxValue;
            SignValue[,] mapA = new SignValue[2, 2];
            mapA[0, 0] = SignValue.MaxValue;
            mapA[0, 1] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[2, 2];
            mapB[1, 1] = SignValue.MaxValue;
            SignValue[,] mapC = new SignValue[2, 2];
            mapC[0, 0] = SignValue.MaxValue;
            mapC[1, 0] = SignValue.MaxValue;
            SignValue[,] mapD = new SignValue[2, 2];
            mapD[0, 0] = SignValue.MaxValue;
            mapD[0, 1] = SignValue.MaxValue;
            mapD[1, 0] = SignValue.MaxValue;
            mapD[1, 1] = SignValue.MaxValue;
            SignValue[,] mapE = new SignValue[2, 2];
            Processor main = new Processor(map, "main");
            Processor procA = new Processor(mapA, "A");
            Processor procB = new Processor(mapB, "B");
            Processor procC = new Processor(mapC, "C");
            Processor procD = new Processor(mapD, "D");
            Processor procE = new Processor(mapE, "E");
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procB, procC, procD, procE));

            TestingAllProperties(reflex);

            Reflex r = (Reflex)reflex.Clone();
            Assert.AreNotSame(r, reflex);
            TestingAllProperties(r);

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            TestingAllProperties((Reflex)reflex.Clone());

            for (int k = 0; k < 50; k++)
            {
                //TestingAllCount(reflex);

                Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "ED"));

                Assert.AreEqual(false, reflex.FindRelation(main, "EA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "ED"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EE"));
            }
        }

        [TestMethod]
        public void ReflexTest2()
        {
            SignValue[,] map = new SignValue[4, 4];
            map[0, 0] = SignValue.MaxValue;
            map[2, 0] = SignValue.MaxValue;
            map[1, 1] = SignValue.MaxValue;
            map[2, 1] = SignValue.MaxValue;
            map[0, 2] = SignValue.MaxValue;
            map[2, 2] = SignValue.MaxValue;
            map[3, 3] = SignValue.MaxValue;
            SignValue[,] mapA = new SignValue[2, 2];
            mapA[0, 0] = SignValue.MaxValue;
            mapA[0, 1] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[2, 2];
            mapB[1, 1] = SignValue.MaxValue;
            SignValue[,] mapC = new SignValue[2, 2];
            mapC[0, 0] = SignValue.MaxValue;
            mapC[0, 1] = SignValue.MaxValue;
            Processor main = new Processor(map, "main");
            Processor procA = new Processor(mapA, "A11");
            Processor procB = new Processor(mapB, "B22a");
            Processor procC = new Processor(mapC, "C33b");

            Reflex reflex = new Reflex(new ProcessorContainer(procA, procB, procC));

            TestingAllMaps(reflex);

            Reflex r = (Reflex)reflex.Clone();
            Assert.AreNotSame(r, reflex);
            TestingAllMaps(r);

            Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BB"));

            Assert.AreEqual(true, reflex.FindRelation(main, "11"));
            Assert.AreEqual(true, reflex.FindRelation(main, "22"));
            Assert.AreEqual(true, reflex.FindRelation(main, "33"));
            Assert.AreEqual(true, reflex.FindRelation(main, "112233"));

            Assert.AreEqual(true, reflex.FindRelation(main, "1122"));
            Assert.AreEqual(true, reflex.FindRelation(main, "22a"));
            Assert.AreEqual(true, reflex.FindRelation(main, "33B22A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "33b22A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "33b22A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "33b"));

            Assert.AreEqual(true, reflex.FindRelation(main, "33B22a"));
            Assert.AreEqual(false, reflex.FindRelation(main, "33B233"));
            Assert.AreEqual(false, reflex.FindRelation(main, "223"));
            Assert.AreEqual(true, reflex.FindRelation(main, "33B"));

            Assert.AreEqual(true, reflex.FindRelation(main, "223311"));
            Assert.AreEqual(false, reflex.FindRelation(main, "121312"));
            Assert.AreEqual(true, reflex.FindRelation(main, "332211"));
            Assert.AreEqual(true, reflex.FindRelation(main, "112233"));

            Reflex r1 = (Reflex)reflex.Clone();
            Assert.AreNotSame(r1, reflex);
            TestingAllMaps(r1);

            //TestingAllCount1(reflex);
        }

        [TestMethod]
        public void SizeTesting()
        {
            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MaxValue, SignValue.MinValue }, "d")));
            Assert.AreEqual(2, reflex.MapSize.Width);
            Assert.AreEqual(1, reflex.MapSize.Height);
        }

        static void TestingAllMaps(Reflex reflex)
        {
            Assert.AreEqual(2, reflex.MapSize.Width);
            Assert.AreEqual(2, reflex.MapSize.Height);
        }

        static void TestingAllProperties(Reflex reflex)
        {
        }

        /*static void TestingAllCount(Reflex reflex)
        {
            int count = 0;
            try
            {
                // ReSharper disable once UnusedVariable
                Processor p = reflex[reflex.CountProcessors];
            }
            catch (ArgumentOutOfRangeException)
            {
                count++;
            }
            try
            {
                // ReSharper disable once UnusedVariable
                Processor p = reflex[reflex.CountProcessors - 1];
            }
            catch
            {
                count++;
            }
            try
            {
                // ReSharper disable once UnusedVariable
                Processor p = reflex[-1];
            }
            catch (ArgumentOutOfRangeException)
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }

        static void TestingAllCount1(Reflex reflex)
        {
            int count = 0;
            Assert.AreEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(7, reflex.CountProcessors);
            try
            {
                // ReSharper disable once UnusedVariable
                Processor p = reflex[reflex.CountProcessors];
            }
            catch (ArgumentOutOfRangeException)
            {
                count++;
            }
            try
            {
                // ReSharper disable once UnusedVariable
                Processor p = reflex[reflex.CountProcessors - 1];
            }
            catch
            {
                count++;
            }
            try
            {
                // ReSharper disable once UnusedVariable
                Processor p = reflex[-1];
            }
            catch (ArgumentOutOfRangeException)
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void GetMapTest()
        {
            SignValue[,] map = new SignValue[2, 2];
            Processor procA = new Processor(map, "A1");
            Processor procA1 = new Processor(map, "A12");
            Processor procB = new Processor(map, "B1");
            Processor procB1 = new Processor(map, "B13");
            Processor procB2 = new Processor(map, "B14");
            Processor procC = new Processor(map, "C1");
            Processor procC1 = new Processor(map, "C143a8");
            Processor procC2 = new Processor(map, "C153A8");
            Processor procD = new Processor(map, "D1");
            Processor procD1 = new Processor(map, "D12");
            Processor procD2 = new Processor(map, "D13");
            Processor procE = new Processor(map, "E1");
            Processor procE1 = new Processor(map, "E12");
            Processor procE2 = new Processor(map, "E13");
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procA1, procB, procB1, procB2, procC, procC1, procC2, procD, procD1, procD2, procE, procE1, procE2));

            Processor[] p1 = reflex.GetMap("A").ToArray();
            Assert.AreEqual(2, p1.Length);
            Assert.AreEqual(true, p1.Any(p => p.IsProcessorName("A1", 0)));
            Assert.AreEqual(true, p1.Any(p => p.IsProcessorName("A12", 0)));

            Processor[] p2 = reflex.GetMap("B").ToArray();
            Assert.AreEqual(3, p2.Length);
            Assert.AreEqual(true, p2.Any(p => p.IsProcessorName("B1", 0)));
            Assert.AreEqual(true, p2.Any(p => p.IsProcessorName("B13", 0)));
            Assert.AreEqual(true, p2.Any(p => p.IsProcessorName("B14", 0)));

            Processor[] p3 = reflex.GetMap("C").ToArray();
            Assert.AreEqual(3, p3.Length);
            Assert.AreEqual(true, p3.Any(p => p.IsProcessorName("C1", 0)));
            Assert.AreEqual(true, p3.Any(p => p.IsProcessorName("C143A8", 0)));
            Assert.AreEqual(true, p3.Any(p => p.IsProcessorName("C153a8", 0)));

            Processor[] p4 = reflex.GetMap("D").ToArray();
            Assert.AreEqual(3, p4.Length);
            Assert.AreEqual(true, p4.Any(p => p.IsProcessorName("D1", 0)));
            Assert.AreEqual(true, p4.Any(p => p.IsProcessorName("D12", 0)));
            Assert.AreEqual(true, p4.Any(p => p.IsProcessorName("D13", 0)));

            Processor[] p5 = reflex.GetMap("E").ToArray();
            Assert.AreEqual(3, p5.Length);
            Assert.AreEqual(true, p5.Any(p => p.IsProcessorName("E1", 0)));
            Assert.AreEqual(true, p5.Any(p => p.IsProcessorName("E12", 0)));
            Assert.AreEqual(true, p5.Any(p => p.IsProcessorName("E13", 0)));

            Processor[] p6 = reflex.GetMap("1", 1).ToArray();
            Assert.AreEqual(14, p6.Length);
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("A1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("A12", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("B1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("B13", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("B14", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("C1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("C143A8", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("C153a8", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("D1", 0)));
            Assert.AreEqual(false, p6.Any(p => p.IsProcessorName("D12 ", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("D12", 0)));
            Assert.AreEqual(false, p6.Any(p => p.IsProcessorName("D13 ", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("D13", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("E1", 0)));
            Assert.AreEqual(false, p6.Any(p => p.IsProcessorName("E12 ", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("E12", 0)));
            Assert.AreEqual(false, p6.Any(p => p.IsProcessorName("E13 ", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("E13", 0)));
            Assert.AreEqual(false, p6.Any(p => p.IsProcessorName(" E13", 0)));

            Processor[] p7 = reflex.GetMap("3a8", 3).ToArray();
            Assert.AreEqual(2, p7.Length);
            Assert.AreEqual(true, p7.Any(p => p.IsProcessorName("C143a8", 0)));
            Assert.AreEqual(true, p7.Any(p => p.IsProcessorName("C153A8", 0)));

            Processor[] p9 = reflex.GetMap("bb", 3).ToArray();
            Assert.AreEqual(0, p9.Length);

            Processor[] p10 = reflex.GetMap("bb", -3).ToArray();
            Assert.AreEqual(0, p10.Length);

            Processor[] p11 = reflex.GetMap(null).ToArray();
            Assert.AreEqual(0, p11.Length);

            Processor[] p12 = reflex.GetMap(" ").ToArray();
            Assert.AreEqual(0, p12.Length);

            Processor[] p13 = reflex.GetMap(" ", 3).ToArray();
            Assert.AreEqual(0, p13.Length);

            Processor[] p14 = reflex.GetMap("E13", -3).ToArray();
            Assert.AreEqual(0, p14.Length);

            Processor[] p15 = reflex.GetMap("e13", -3).ToArray();
            Assert.AreEqual(0, p15.Length);

            Processor[] p16 = reflex.GetMap("e13").ToArray();
            Assert.AreEqual(1, p16.Length);
            Assert.AreEqual(true, p16[0].IsProcessorName("E13", 0));
            Assert.AreEqual(false, p16[0].IsProcessorName(" E13", 0));
            Assert.AreEqual(false, p16[0].IsProcessorName("E13 ", 0));
            Assert.AreEqual(false, p16[0].IsProcessorName(" E13 ", 0));
            Assert.AreEqual(true, p16[0].IsProcessorName("e13", 0));
            Assert.AreEqual(false, p16[0].IsProcessorName(" e13", 0));
            Assert.AreEqual(false, p16[0].IsProcessorName("e13 ", 0));
            Assert.AreEqual(false, p16[0].IsProcessorName(" e13 ", 0));

            Processor[] p17 = reflex.GetMap("E13").ToArray();
            Assert.AreEqual(1, p17.Length);
            Assert.AreEqual(true, p17[0].IsProcessorName("E13", 0));
            Assert.AreEqual(false, p17[0].IsProcessorName(" E13", 0));
            Assert.AreEqual(false, p17[0].IsProcessorName("E13 ", 0));
            Assert.AreEqual(false, p17[0].IsProcessorName(" E13 ", 0));
            Assert.AreEqual(true, p17[0].IsProcessorName("e13", 0));
            Assert.AreEqual(false, p17[0].IsProcessorName(" e13", 0));
            Assert.AreEqual(false, p17[0].IsProcessorName("e13 ", 0));
            Assert.AreEqual(false, p17[0].IsProcessorName(" e13 ", 0));
        }*/

        [TestMethod]
        public void IsMapsWordTest()
        {
            SignValue[,] map = new SignValue[2, 2];
            Processor procA = new Processor(map, "A1");
            Processor procA1 = new Processor(map, "A12");
            Processor procB = new Processor(map, "B1");
            Processor procB1 = new Processor(map, "B13");
            Processor procB2 = new Processor(map, "B14");
            Processor procC = new Processor(map, " C1");
            Processor procC1 = new Processor(map, " C14388 ");
            Processor procC2 = new Processor(map, " C15388 ");
            Processor procD = new Processor(map, "  D1    ");
            Processor procD1 = new Processor(map, "D12 ");
            Processor procD2 = new Processor(map, "D13 ");
            Processor procE = new Processor(map, "E1");
            Processor procE1 = new Processor(map, " E12");
            Processor procE2 = new Processor(map, " E13");
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procA1, procB, procB1, procB2, procC, procC1, procC2, procD, procD1, procD2, procE, procE1, procE2));

            Assert.AreEqual(true, reflex.IsMapsWord("1"));
            Assert.AreEqual(true, reflex.IsMapsWord("ABCDE"));
            Assert.AreEqual(true, reflex.IsMapsWord("ABcde"));
            Assert.AreEqual(true, reflex.IsMapsWord("dacBe"));
            Assert.AreEqual(false, reflex.IsMapsWord("dacBe "));
            Assert.AreEqual(false, reflex.IsMapsWord(" dacBe"));
            Assert.AreEqual(false, reflex.IsMapsWord(" dacBe "));
            Assert.AreEqual(false, reflex.IsMapsWord("d acBe"));
            Assert.AreEqual(false, reflex.IsMapsWord("dac   Be"));
            Assert.AreEqual(false, reflex.IsMapsWord("  dacBe    "));
            Assert.AreEqual(false, reflex.IsMapsWord("dacBe    "));
            Assert.AreEqual(false, reflex.IsMapsWord("  dacBe"));
            Assert.AreEqual(true, reflex.IsMapsWord("AEdbC"));
            Assert.AreEqual(true, reflex.IsMapsWord("AB143"));
            Assert.AreEqual(false, reflex.IsMapsWord("ABCDEW"));
            Assert.AreEqual(false, reflex.IsMapsWord("ABCDE "));
            Assert.AreEqual(false, reflex.IsMapsWord(" ABCDEW"));
            Assert.AreEqual(false, reflex.IsMapsWord(" ABCDEW "));
            Assert.AreEqual(false, reflex.IsMapsWord("  ABCDE   "));
            Assert.AreEqual(false, reflex.IsMapsWord("ABCDEW    "));
            Assert.AreEqual(false, reflex.IsMapsWord("  ABCDEW"));
            Assert.AreEqual(false, reflex.IsMapsWord("F"));
            Assert.AreEqual(false, reflex.IsMapsWord(" "));
            Assert.AreEqual(false, reflex.IsMapsWord("  "));
            Assert.AreEqual(false, reflex.IsMapsWord(string.Empty));
            Assert.AreEqual(false, reflex.IsMapsWord(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflexArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflex(null);
        }
    }
}