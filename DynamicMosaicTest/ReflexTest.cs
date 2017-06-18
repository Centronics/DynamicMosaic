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
                TestingAllCount(reflex);

                Processor[] procsA = reflex.GetMap("A").ToArray();

                Assert.AreEqual(3, procsA.Length);

                Assert.AreEqual(true, procsA.Any(p => p.Tag == "A"));
                Assert.AreEqual(true, procsA.Any(p => p.Tag == "A0"));
                Assert.AreEqual(true, procsA.Any(p => p.Tag == "A00"));

                Assert.AreEqual(true, procsA.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MaxValue)
                        return false;
                    if (p[0, 1] != SignValue.MaxValue)
                        return false;
                    if (p[1, 0] != SignValue.MinValue)
                        return false;
                    return p[1, 1] == SignValue.MinValue;
                }));

                Assert.AreEqual(true, procsA.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MinValue)
                        return false;
                    if (p[0, 1] != SignValue.MaxValue)
                        return false;
                    if (p[1, 0] != SignValue.MaxValue)
                        return false;
                    return p[1, 1] == SignValue.MinValue;
                }));

                Assert.AreEqual(true, procsA.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MaxValue)
                        return false;
                    if (p[0, 1] != SignValue.MinValue)
                        return false;
                    if (p[1, 0] != SignValue.MinValue)
                        return false;
                    return p[1, 1] == SignValue.MinValue;
                }));

                Processor[] procsB = reflex.GetMap("B").ToArray();

                Assert.AreEqual(2, procsB.Length);

                Assert.AreEqual(true, procsB.Any(p => p.Tag == "B"));
                Assert.AreEqual(true, procsB.Any(p => p.Tag == "B0"));

                Assert.AreEqual(true, procsB.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MinValue)
                        return false;
                    if (p[0, 1] != SignValue.MinValue)
                        return false;
                    if (p[1, 0] != SignValue.MinValue)
                        return false;
                    return p[1, 1] == SignValue.MaxValue;
                }));

                Assert.AreEqual(true, procsB.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MaxValue)
                        return false;
                    if (p[0, 1] != SignValue.MinValue)
                        return false;
                    if (p[1, 0] != SignValue.MinValue)
                        return false;
                    return p[1, 1] == SignValue.MaxValue;
                }));

                Processor[] procsC = reflex.GetMap("C").ToArray();

                Assert.AreEqual(3, procsC.Length);

                Assert.AreEqual(true, procsC.Any(p => p.Tag == "C"));
                Assert.AreEqual(true, procsC.Any(p => p.Tag == "C0"));
                Assert.AreEqual(true, procsC.Any(p => p.Tag == "C00"));

                Assert.AreEqual(true, procsC.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MaxValue)
                        return false;
                    if (p[0, 1] != SignValue.MinValue)
                        return false;
                    if (p[1, 0] != SignValue.MaxValue)
                        return false;
                    return p[1, 1] == SignValue.MinValue;
                }));

                Assert.AreEqual(true, procsC.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MinValue)
                        return false;
                    if (p[0, 1] != SignValue.MinValue)
                        return false;
                    if (p[1, 0] != SignValue.MaxValue)
                        return false;
                    return p[1, 1] == SignValue.MinValue;
                }));

                Assert.AreEqual(true, procsC.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MaxValue)
                        return false;
                    if (p[0, 1] != SignValue.MinValue)
                        return false;
                    if (p[1, 0] != SignValue.MaxValue)
                        return false;
                    return p[1, 1] == SignValue.MaxValue;
                }));

                Processor[] procsD = reflex.GetMap("D").ToArray();

                Assert.AreEqual(2, procsD.Length);

                Assert.AreEqual(true, procsD.Any(p => p.Tag == "D"));
                Assert.AreEqual(true, procsD.Any(p => p.Tag == "D0"));

                Assert.AreEqual(true, procsD.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MinValue)
                        return false;
                    if (p[0, 1] != SignValue.MaxValue)
                        return false;
                    if (p[1, 0] != SignValue.MaxValue)
                        return false;
                    return p[1, 1] == SignValue.MaxValue;
                }));

                Assert.AreEqual(true, procsD.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MaxValue)
                        return false;
                    if (p[0, 1] != SignValue.MaxValue)
                        return false;
                    if (p[1, 0] != SignValue.MaxValue)
                        return false;
                    return p[1, 1] == SignValue.MaxValue;
                }));

                Processor[] procsE = reflex.GetMap("E").ToArray();

                Assert.AreEqual(1, procsE.Length);

                Assert.AreEqual(true, procsE.Any(p => p.Tag == "E"));

                Assert.AreEqual(true, procsE.Any(p =>
                {
                    Assert.AreNotEqual(null, p);
                    if (p[0, 0] != SignValue.MinValue)
                        return false;
                    if (p[0, 1] != SignValue.MinValue)
                        return false;
                    if (p[1, 0] != SignValue.MinValue)
                        return false;
                    return p[1, 1] == SignValue.MinValue;
                }));

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

            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessors);

            TestingAllMaps(reflex);

            Assert.AreEqual(2, reflex.MapSize.Width);
            Assert.AreEqual(2, reflex.MapSize.Height);

            Reflex r = (Reflex)reflex.Clone();
            Assert.AreNotSame(r, reflex);

            Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BB"));

            Assert.AreEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(11, reflex.CountProcessors);

            Assert.AreEqual(true, reflex.FindRelation(main, "11", 1, 2));
            Assert.AreEqual(true, reflex.FindRelation(main, "22", 1, 2));
            Assert.AreEqual(true, reflex.FindRelation(main, "33", 1, 2));
            Assert.AreEqual(true, reflex.FindRelation(main, "112233", 1, 2));

            Assert.AreEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(11, reflex.CountProcessors);

            Assert.AreEqual(true, reflex.FindRelation(main, "1122", 1, 2));
            Assert.AreEqual(true, reflex.FindRelation(main, "22a", 1, 3));
            Assert.AreEqual(true, reflex.FindRelation(main, "33B22a33b", 1, 3));
            Assert.AreEqual(true, reflex.FindRelation(main, "33b22A", 1, 3));
            Assert.AreEqual(false, reflex.FindRelation(main, "33b22A11", 1, 3));
            Assert.AreEqual(false, reflex.FindRelation(main, "33b11", 1, 3));

            Assert.AreEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(11, reflex.CountProcessors);

            Assert.AreEqual(true, reflex.FindRelation(main, "33B22a33", 1, 3));
            Assert.AreEqual(false, reflex.FindRelation(main, "33B233b", 1, 3));
            Assert.AreEqual(false, reflex.FindRelation(main, "223B", 1, 3));
            Assert.AreEqual(false, reflex.FindRelation(main, "33BF", 1, 3));

            Assert.AreEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(11, reflex.CountProcessors);

            Assert.AreEqual(true, reflex.FindRelation(main, "223311112233", 1, 2));
            Assert.AreEqual(false, reflex.FindRelation(main, "1213123", 1, 2));
            Assert.AreEqual(true, reflex.FindRelation(main, "332211", 1, 2));
            Assert.AreEqual(true, reflex.FindRelation(main, "112233", 1, 2));

            Assert.AreEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(11, reflex.CountProcessors);

            TestingAllMaps((Reflex)reflex.Clone());

            TestingAllCount1(reflex);
        }

        [TestMethod]
        public void ReflexTest3()
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
            SignValue[,] mapD = new SignValue[2, 2];
            mapD[0, 0] = SignValue.MaxValue;
            mapD[0, 1] = SignValue.MaxValue;
            mapD[1, 0] = SignValue.MaxValue;
            mapD[1, 1] = SignValue.MaxValue;
            SignValue[,] mapE = new SignValue[2, 2];
            Processor main = new Processor(map, "main");
            Processor procA = new Processor(mapA, "A11");
            Processor procB = new Processor(mapB, "B22a");
            Processor procC = new Processor(mapC, "C33b");
            Processor procD = new Processor(mapD, "D12");
            Processor procE = new Processor(mapE, "E13");

            Reflex r1 = GetReflex1(procA, procB, procC, procD, procE, main),
                r2 = GetReflex2(procA, procB, procC, procD, procE, main),
                r3 = GetReflex3(procA, procB, procC, procD, procE, main);

            //Assert.AreEqual(true, MapsDifference(r1.Processors, r2.Processors.ToArray()));
            //Assert.AreEqual(true, MapsDifference(r1.Processors, r2.Processors.ToArray()));

            Processor[] p1 = r1.Processors.ToArray();
            Processor[] p2 = r2.Processors.ToArray();
            Processor[] p3 = r3.Processors.ToArray();
        }

        static Reflex GetReflex1(Processor procA, Processor procB, Processor procC, Processor procD, Processor procE, Processor main)
        {
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procB, procC, procD, procE));

            Assert.AreEqual(reflex.CountProcessors, reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessors);

            TestingAllMaps(reflex);
            TestingAllMaps((Reflex)reflex.Clone());

            Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
            return reflex;
        }

        static Reflex GetReflex2(Processor procA, Processor procB, Processor procC, Processor procD, Processor procE, Processor main)
        {
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procB, procC, procD, procE));

            Assert.AreEqual(5, reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessors);

            Assert.AreEqual(2, reflex.MapSize.Width);
            Assert.AreEqual(2, reflex.MapSize.Height);

            Reflex r = (Reflex)reflex.Clone();
            Assert.AreNotSame(r, reflex);

            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
            return reflex;
        }

        static Reflex GetReflex3(Processor procA, Processor procB, Processor procC, Processor procD, Processor procE, Processor main)
        {
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procB, procC, procD, procE));

            Assert.AreEqual(5, reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessors);

            TestingAllMaps(reflex);

            Assert.AreEqual(2, reflex.MapSize.Width);
            Assert.AreEqual(2, reflex.MapSize.Height);

            Reflex r = (Reflex)reflex.Clone();
            Assert.AreNotSame(r, reflex);

            Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
            return reflex;
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

            Assert.AreEqual(5, reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessors);

            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "A11"));
            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "B22a"));
            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "C33b"));

            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "A11"));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "B22a"));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "C33b"));

            Processor[] processors = { reflex[0], reflex[1], reflex[2] };
            Assert.AreEqual(true, processors.Any(p => p.Tag == "A11"));
            Assert.AreEqual(true, processors.Any(p => p.Tag == "B22a"));
            Assert.AreEqual(true, processors.Any(p => p.Tag == "C33b"));
        }

        static void TestingAllProperties(Reflex reflex)
        {
            Assert.AreEqual(5, reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessors);

            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "A"));
            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "B"));
            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "C"));
            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "D"));
            Assert.AreEqual(true, reflex.Processors.Any(p => p.Tag == "E"));

            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "A"));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "B"));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "C"));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "D"));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => p.Tag == "E"));

            Processor[] processors = { reflex[0], reflex[1], reflex[2], reflex[3], reflex[4] };
            Assert.AreEqual(true, processors.Any(p => p.Tag == "A"));
            Assert.AreEqual(true, processors.Any(p => p.Tag == "B"));
            Assert.AreEqual(true, processors.Any(p => p.Tag == "C"));
            Assert.AreEqual(true, processors.Any(p => p.Tag == "D"));
            Assert.AreEqual(true, processors.Any(p => p.Tag == "E"));
        }

        static void TestingAllCount(Reflex reflex)
        {
            int count = 0;
            Assert.AreEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessorsBase);
            Assert.AreEqual(11, reflex.CountProcessors);
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
            Assert.AreNotEqual(true, reflex.CountProcessors > reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessorsBase);
            Assert.AreEqual(3, reflex.CountProcessors);
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
        }

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