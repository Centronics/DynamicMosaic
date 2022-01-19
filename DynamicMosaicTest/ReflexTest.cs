using System;
using System.Collections.Generic;
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
        static void CheckReflexValue(DynamicReflex actual, IEnumerable<Processor> pcExpected, int width, int height)//не учитываются номера карт (после первой буквы)
        {
            Dictionary<string, Processor> dicActual = actual.Processors.ToDictionary(p => p.Tag);

            foreach (Processor pExpected in pcExpected)
            {
                Assert.AreNotEqual(null, pExpected);
                Processor pActual = dicActual[pExpected.Tag];
                Assert.AreNotEqual(null, pActual);
                dicActual.Remove(pExpected.Tag);
                Assert.AreEqual(pActual.Tag, pExpected.Tag);

                Assert.AreEqual(width, pActual.Width);
                Assert.AreEqual(height, pActual.Height);
                Assert.AreEqual(width, pExpected.Width);
                Assert.AreEqual(height, pExpected.Height);

                for (int y = 0; y < pExpected.Height; y++)
                    for (int x = 0; x < pExpected.Width; x++)
                        if (pExpected[x, y] != pActual[x, y])
                            throw new ArgumentException();
            }

            Assert.AreEqual(0, dicActual.Count);
        }

        static Processor MainProcessorForTest1
        {
            get
            {
                SignValue[,] map = new SignValue[4, 4];
                map[0, 0] = SignValue.MaxValue;
                map[2, 0] = SignValue.MaxValue;
                map[1, 1] = SignValue.MaxValue;
                map[2, 1] = SignValue.MaxValue;
                map[0, 2] = SignValue.MaxValue;
                map[2, 2] = SignValue.MaxValue;
                map[3, 3] = SignValue.MaxValue;

                return new Processor(map, "main");
            }
        }

        static IEnumerable<Processor> ProcessorsForTest1
        {
            get
            {
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

                yield return new Processor(mapA, "A");
                yield return new Processor(mapB, "B");
                yield return new Processor(mapC, "C");
                yield return new Processor(mapD, "D");
                yield return new Processor(mapE, "E");
            }
        }

        [TestMethod]
        public void ReflexTest1()
        {
            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(ProcessorsForTest1.ToArray()));

            CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);

            Processor minProcessor = new Processor(new SignValue[1, 1], "main");

            Assert.AreEqual(false, reflex.FindRelation((minProcessor, "A")));
            Assert.AreEqual(false, reflex.FindRelation((minProcessor, "B")));
            Assert.AreEqual(false, reflex.FindRelation((minProcessor, "C")));
            Assert.AreEqual(false, reflex.FindRelation((minProcessor, "D")));
            Assert.AreEqual(false, reflex.FindRelation((minProcessor, "E")));
            Assert.AreEqual(false, reflex.FindRelation((minProcessor, "W")));
            Assert.AreEqual(false, reflex.FindRelation((minProcessor, null)));
            Assert.AreEqual(false, reflex.FindRelation((null, "W")));
            Assert.AreEqual(false, reflex.FindRelation((minProcessor, string.Empty)));

            CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);

            Processor main = MainProcessorForTest1;

            Assert.AreNotEqual(false, reflex.FindRelation((main, "A")));
            Assert.AreNotEqual(false, reflex.FindRelation((main, "B")));
            Assert.AreNotEqual(false, reflex.FindRelation((main, "C")));
            Assert.AreNotEqual(false, reflex.FindRelation((main, "D")));
            Assert.AreEqual(false, reflex.FindRelation((main, "E")));
            Assert.AreEqual(false, reflex.FindRelation((main, "W")));

            CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);

            for (int k = 0; k < 50; k++)
            {
                Assert.AreNotEqual(false, reflex.FindRelation((main, "AA")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "AB")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "BA")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "AC")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "CA")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "AD")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "DA")));
                Assert.AreEqual(false, reflex.FindRelation((main, "AE")));

                CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);

                Assert.AreNotEqual(false, reflex.FindRelation((main, "BA")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "BB")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "BC")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "BD")));
                Assert.AreEqual(false, reflex.FindRelation((main, "BE")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "AB")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "BB")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "CB")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "DB")));
                Assert.AreEqual(false, reflex.FindRelation((main, "EB")));

                CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);

                Assert.AreNotEqual(false, reflex.FindRelation((main, "CA")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "CB")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "CC")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "CD")));
                Assert.AreEqual(false, reflex.FindRelation((main, "CE")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "AC")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "BC")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "CC")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "DC")));
                Assert.AreEqual(false, reflex.FindRelation((main, "EC")));

                CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);

                Assert.AreNotEqual(false, reflex.FindRelation((main, "DA")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "DB")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "DC")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "DD")));
                Assert.AreEqual(false, reflex.FindRelation((main, "DE")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "AD")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "BD")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "CD")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "DD")));
                Assert.AreEqual(false, reflex.FindRelation((main, "ED")));

                CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);

                Assert.AreEqual(false, reflex.FindRelation((main, "EA")));
                Assert.AreEqual(false, reflex.FindRelation((main, "EB")));
                Assert.AreEqual(false, reflex.FindRelation((main, "EC")));
                Assert.AreEqual(false, reflex.FindRelation((main, "ED")));
                Assert.AreEqual(false, reflex.FindRelation((main, "EE")));
                Assert.AreEqual(false, reflex.FindRelation((main, "AE")));
                Assert.AreEqual(false, reflex.FindRelation((main, "BE")));
                Assert.AreEqual(false, reflex.FindRelation((main, "CE")));
                Assert.AreEqual(false, reflex.FindRelation((main, "DE")));
                Assert.AreEqual(false, reflex.FindRelation((main, "EE")));

                CheckReflexValue(reflex, ProcessorsForTest1, 2, 2);
            }
        }

        [TestMethod]
        public void ReflexTest1_1()
        {
            SignValue[,] main = new SignValue[1, 1];
            main[0, 0] = new SignValue(10);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(6);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(7);

            Processor[] processors = { new Processor(mapA, "a"), new Processor(mapB, "b") };

            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(processors));

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "A")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "B")));

                processors[1] = new Processor(main, "B");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapB, "X"), "B")));
                processors[1] = new Processor(mapB, "B");

                CheckReflexValue(reflex, processors, 1, 1);
            }

            main[0, 0] = new SignValue(4);

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "B")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "A")));

                processors[0] = new Processor(main, "A");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, "X"), "A")));
                processors[0] = new Processor(mapA, "A");

                CheckReflexValue(reflex, processors, 1, 1);
            }
        }

        [TestMethod]
        public void ReflexTest1_2()
        {
            SignValue[,] main = new SignValue[1, 1];
            main[0, 0] = new SignValue(10);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(6);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(7);

            Processor[] processors = { new Processor(mapA, "A"), new Processor(mapB, "B") };

            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(processors));

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "A")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "B")));

                processors[1] = new Processor(main, "B");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapB, "X"), "B")));
                processors[1] = new Processor(mapB, "B");

                CheckReflexValue(reflex, processors, 1, 1);
            }

            main[0, 0] = new SignValue(4);

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "B")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "A")));

                processors[0] = new Processor(main, "A");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, "X"), "A")));
                processors[0] = new Processor(mapA, "A");

                CheckReflexValue(reflex, processors, 1, 1);
            }
        }

        [TestMethod]
        public void ReflexTest1_3()
        {
            SignValue[,] main = new SignValue[1, 1];
            main[0, 0] = new SignValue(10);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(6);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(7);

            Processor[] processors = { new Processor(mapA, "A1"), new Processor(mapB, "B1") };

            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(processors));

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "aaa")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "ccc")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "Bb")));

                processors[1] = new Processor(main, "B1");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapB, "x"), "bB")));
                processors[1] = new Processor(mapB, "B1");

                CheckReflexValue(reflex, processors, 1, 1);
            }

            main[0, 0] = new SignValue(4);

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "bbbb")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "cccc")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "aaA")));

                processors[0] = new Processor(main, "A2");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, "x"), "Aa")));
                processors[0] = new Processor(mapA, "A2");

                CheckReflexValue(reflex, processors, 1, 1);
            }
        }

        [TestMethod]
        public void ReflexTest1_4()
        {
            SignValue[,] main = new SignValue[1, 1];
            main[0, 0] = new SignValue(10);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(5);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(5);

            Test1_X(main, mapA, mapB, new[] { new Processor(mapA, "A"), new Processor(mapB, "B") });
            Test1_X(main, mapA, mapB, new[] { new Processor(mapA, "a"), new Processor(mapB, "b") });
        }

        static void Test1_X(SignValue[,] main, SignValue[,] mapA, SignValue[,] mapB, Processor[] processors)
        {
            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(processors));

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "A")));

                processors[0] = new Processor(main, "A");

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "B")));

                processors[1] = new Processor(main, "B");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, "X"), "A")));
                processors[0] = new Processor(mapA, "A");

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapB, "X"), "B")));
                processors[1] = new Processor(mapB, "B");

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);
            }

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "B")));

                processors[1] = new Processor(main, "B");

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "main"), "A")));

                processors[0] = new Processor(main, "A");
                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapB, "X"), "B")));
                processors[1] = new Processor(mapB, "B");

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, "X"), "A")));
                processors[0] = new Processor(mapA, "A");

                CheckReflexValue(reflex, processors, 1, 1);

                Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "C")));

                CheckReflexValue(reflex, processors, 1, 1);
            }
        }

        [TestMethod]
        public void ReflexTest1_5()
        {
            SignValue[,] main = new SignValue[1, 1];
            main[0, 0] = new SignValue(10);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(5);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(5);

            Processor[] processors = { new Processor(mapA, "A"), new Processor(mapB, "B") };

            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(processors));

            Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "AB")));

            CheckReflexValue(reflex, processors, 1, 1);

            Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "BA")));

            CheckReflexValue(reflex, processors, 1, 1);

            Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "Abb")));

            CheckReflexValue(reflex, processors, 1, 1);

            Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "aaB")));

            CheckReflexValue(reflex, processors, 1, 1);

            Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "cab")));

            CheckReflexValue(reflex, processors, 1, 1);

            Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "main"), "c")));

            CheckReflexValue(reflex, processors, 1, 1);
        }

        [TestMethod]
        public void ReflexTest1_6()
        {
            SignValue[,] m0 = new SignValue[3, 1];
            m0[0, 0] = new SignValue(10);
            m0[1, 0] = new SignValue(2);
            m0[2, 0] = new SignValue(4);

            SignValue[,] m1 = new SignValue[1, 1];
            m1[0, 0] = new SignValue(11);

            SignValue[,] m2 = new SignValue[1, 1];
            m2[0, 0] = new SignValue(4);

            SignValue[,] m3 = new SignValue[7, 1];
            m3[0, 0] = new SignValue(2);
            m3[1, 0] = new SignValue(3);
            m3[2, 0] = new SignValue(4);
            m3[3, 0] = new SignValue(4);
            m3[4, 0] = new SignValue(4);
            m3[5, 0] = new SignValue(5);
            m3[6, 0] = new SignValue(13);

            SignValue[,] m4 = new SignValue[1, 1];
            m4[0, 0] = new SignValue(1);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(3);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(5);

            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B")));

            void ResetReflex()
            {
                SignValue[,] m5 = new SignValue[2, 1];
                m5[0, 0] = new SignValue(3);
                m5[1, 0] = new SignValue(5);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m5, "m5"), "AB")));

                CheckReflexValue(reflex, new[] { new Processor(mapA, "A"), new Processor(mapB, "B") }, 1, 1);
            }

            Assert.AreEqual(false, reflex.FindRelation((null, "AB"), (new Processor(m0, "mm"), string.Empty), (new Processor(m0, "mm"), null), (null, null), (new Processor(m0, "mm"), null), (null, string.Empty)));

            CheckReflexValue(reflex, new[] { new Processor(mapA, "A"), new Processor(mapB, "B") }, 1, 1);

            #region Test1

            {

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AB")));

                IEnumerable<Processor> GetProcs1()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                }

                CheckReflexValue(reflex, GetProcs1(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m1, "m1"), "B")));

                IEnumerable<Processor> GetProcs2()
                {
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(11) }, "B");
                }

                CheckReflexValue(reflex, GetProcs2(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m2, "m2"), "B")));

                IEnumerable<Processor> GetProcs3()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                }

                CheckReflexValue(reflex, GetProcs3(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m3, "m3"), "AB")));

                IEnumerable<Processor> GetProcs4()
                {
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                }

                CheckReflexValue(reflex, GetProcs4(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m4, "m4"), "A")));

                IEnumerable<Processor> GetProcs5()
                {
                    yield return new Processor(new[] { new SignValue(1) }, "A");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                }

                CheckReflexValue(reflex, GetProcs5(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region Test2

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs6()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(11) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m1, "m1"), "B")));

                CheckReflexValue(reflex, GetProcs6(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m0, "m0"), "AB"),
                        (new Processor(m1, "m1"), "B"), (new Processor(m1, "m1"), "A")));

                CheckReflexValue(reflex, GetProcs6(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m0, "m0"), "B"), (new Processor(m0, "m0"), "A"),
                        (new Processor(m1, "m1"), "B"), (new Processor(m1, "m1"), "A")));

                CheckReflexValue(reflex, GetProcs6(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(1) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                }

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m2, "m2"), "B"), (new Processor(m2, "m2"), "AB"), (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m0, "m0"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                }

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "BB"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "BB"), (new Processor(m2, "m2"), "B"), (new Processor(m2, "m2"), "BB"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m0, "m0"), "BB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(1) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                }

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AA"), (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m0, "m0"), "AA")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                }

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m3, "m3"), "B"), (new Processor(m0, "m0"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(11) }, "B");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                }

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m1, "m1"), "B"), (new Processor(m3, "m3"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m3, "m3"), "AB"), (new Processor(m1, "m1"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B"),
                        (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m1, "m1"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"),
                        (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m2, "m2"), "B"),
                        (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"),
                        (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "B"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "B"),
                        (new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m2, "m2"), "A"),
                        (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "B"),
                        (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "B"), (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "A"),
                        (new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m2, "m2"), "A"),
                        (new Processor(m3, "m3"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "A"),
                        (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "A"), (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"),
                        (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m2, "m2"), "B"),
                        (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"),
                        (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "B"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2
            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "AB"),
                        (new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "BA")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m2, "m2"), "A"),
                        (new Processor(m3, "m3"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "A"), (new Processor(m3, "m3"), "BA"),
                        (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "AB"), (new Processor(m2, "m2"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(3) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "AB"),
                        (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "BA")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m2, "m2"), "B"),
                        (new Processor(m3, "m3"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "BA"),
                        (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "AB"), (new Processor(m2, "m2"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(5) }, "B");
                    yield return new Processor(new[] { new SignValue(13) }, "B");
                    yield return new Processor(new[] { new SignValue(1) }, "A");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A"),
                        (new Processor(m4, "m4"), "A"), (new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A"),
                        (new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A"),
                        (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m4, "m4"), "A"), (new Processor(m3, "m3"), "B")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region SubTest_2

            {

                IEnumerable<Processor> GetProcs()
                {
                    yield return new Processor(new[] { new SignValue(1) }, "A");
                    yield return new Processor(new[] { new SignValue(2) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "A");
                    yield return new Processor(new[] { new SignValue(4) }, "B");
                    yield return new Processor(new[] { new SignValue(10) }, "B");
                }

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m4, "m4"), "A"), (new Processor(m0, "m0"), "AB")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m4, "m4"), "A"), (new Processor(m0, "m0"), "AB"),
                        (new Processor(m0, "m0"), "AB"), (new Processor(m4, "m4"), "A"), (new Processor(m4, "m4"), "A"),
                        (new Processor(m0, "m0"), "AB"), (new Processor(m0, "m0"), "AB"),
                        (new Processor(m4, "m4"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m4, "m4"), "A")));

                CheckReflexValue(reflex, GetProcs(), 1, 1);

                ResetReflex();

            }
            #endregion

            //есть ли тест, где все знаки 100% совпадают? Есть ли позитивный тест с поиском нескольких карт большей, чем 1 разрядности? Надо включить ещё повторение одного и того же запроса. Добавить тест с изменением РС, от которого создаётся Reflex (можно дополнить существующие тесты).

            /*SignValue[,] bmap = new SignValue[4, 4];
            bmap[0, 0] = SignValue.MaxValue;
            bmap[2, 0] = SignValue.MaxValue;
            bmap[1, 1] = SignValue.MaxValue;
            bmap[2, 1] = SignValue.MaxValue;
            bmap[0, 2] = SignValue.MaxValue;
            bmap[2, 2] = SignValue.MaxValue;
            bmap[3, 3] = SignValue.MaxValue;

            SignValue[,] mapA = new SignValue[2, 2];
            mapA[0, 0] = SignValue.MaxValue;
            mapA[1, 0] = SignValue.MaxValue;

            SignValue[,] mapB = new SignValue[2, 2];
            mapB[1, 1] = SignValue.MaxValue;

            ProcessorContainer pc = new ProcessorContainer(new Processor(mapB, "B"), new Processor(mapB, "C"));

            Processor main = new Processor(bmap, "bigmain");

            Reflex reflex = new Reflex(pc);

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));

            pc.Add(new Processor(mapA, "A"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));*/

            /* SignValue[,] map = new SignValue[4, 4];
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A1"), new Processor(mapB, "A2"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "A3")));

            Processor main = new Processor(map, "main");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WW"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ADC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CAD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DCA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ADCE"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "EA"));
            
             
             [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflexArgumentNullException3()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflex((ProcessorContainer)null);
        }
             */

            #endregion

            #region Test3

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B")));

            IEnumerable<Processor> GetProcs10()
            {
                yield return new Processor(new[] { new SignValue(4) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(10) }, "B");
                yield return new Processor(new[] { new SignValue(2) }, "A");
                yield return new Processor(new[] { new SignValue(11) }, "B");
            }

            CheckReflexValue(reflex, GetProcs10(), 1, 1);

            ResetReflex();

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B")));

            IEnumerable<Processor> GetProcs11()
            {
                yield return new Processor(new[] { new SignValue(11) }, "B");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(13) }, "B");
                yield return new Processor(new[] { new SignValue(5) }, "B");
            }

            CheckReflexValue(reflex, GetProcs11(), 1, 1);

            ResetReflex();

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A")));

            IEnumerable<Processor> GetProcs12()
            {
                yield return new Processor(new[] { new SignValue(1) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(13) }, "B");
                yield return new Processor(new[] { new SignValue(5) }, "B");
            }

            CheckReflexValue(reflex, GetProcs12(), 1, 1);

            ResetReflex();

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m4, "m4"), "A"), (new Processor(m0, "m0"), "AB"), (new Processor(m1, "m1"), "B")));

            IEnumerable<Processor> GetProcs13()
            {
                yield return new Processor(new[] { new SignValue(1) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(10) }, "B");
                yield return new Processor(new[] { new SignValue(2) }, "A");
                yield return new Processor(new[] { new SignValue(11) }, "B");
            }

            CheckReflexValue(reflex, GetProcs13(), 1, 1);

            ResetReflex();

            #endregion

            #region Test4

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B")));

            IEnumerable<Processor> GetProcs14()
            {
                yield return new Processor(new[] { new SignValue(4) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(10) }, "B");
                yield return new Processor(new[] { new SignValue(2) }, "A");
                yield return new Processor(new[] { new SignValue(11) }, "B");
                yield return new Processor(new[] { new SignValue(5) }, "B");
                yield return new Processor(new[] { new SignValue(13) }, "B");
            }

            CheckReflexValue(reflex, GetProcs14(), 1, 1);

            ResetReflex();

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m4, "m4"), "A"), (new Processor(m0, "m0"), "AB"), (new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B")));

            IEnumerable<Processor> GetProcs15()
            {
                yield return new Processor(new[] { new SignValue(1) }, "A");
                yield return new Processor(new[] { new SignValue(2) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(4) }, "A");
                yield return new Processor(new[] { new SignValue(10) }, "B");
                yield return new Processor(new[] { new SignValue(11) }, "B");
            }

            CheckReflexValue(reflex, GetProcs15(), 1, 1);

            ResetReflex();

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A"), (new Processor(m0, "m0"), "AB"), (new Processor(m1, "m1"), "B")));

            IEnumerable<Processor> GetProcs16()
            {
                yield return new Processor(new[] { new SignValue(2) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(5) }, "B");
                yield return new Processor(new[] { new SignValue(13) }, "B");
                yield return new Processor(new[] { new SignValue(1) }, "A");
                yield return new Processor(new[] { new SignValue(10) }, "B");
                yield return new Processor(new[] { new SignValue(11) }, "B");
            }

            CheckReflexValue(reflex, GetProcs16(), 1, 1);

            ResetReflex();

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A"), (new Processor(m0, "m0"), "AB")));

            IEnumerable<Processor> GetProcs17()
            {
                yield return new Processor(new[] { new SignValue(1) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(13) }, "B");
                yield return new Processor(new[] { new SignValue(5) }, "B");
                yield return new Processor(new[] { new SignValue(10) }, "B");
                yield return new Processor(new[] { new SignValue(2) }, "A");
            }

            CheckReflexValue(reflex, GetProcs17(), 1, 1);

            ResetReflex();

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A")));

            IEnumerable<Processor> GetProcs18()
            {
                yield return new Processor(new[] { new SignValue(1) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(5) }, "B");
                yield return new Processor(new[] { new SignValue(2) }, "A");
                yield return new Processor(new[] { new SignValue(11) }, "B");
                yield return new Processor(new[] { new SignValue(13) }, "B");
            }

            CheckReflexValue(reflex, GetProcs18(), 1, 1);

            ResetReflex();

            #endregion

            #region Test5

            Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), "AB"), (new Processor(m1, "m1"), "B"), (new Processor(m2, "m2"), "B"), (new Processor(m3, "m3"), "B"), (new Processor(m4, "m4"), "A")));

            IEnumerable<Processor> GetProcs19()
            {
                yield return new Processor(new[] { new SignValue(4) }, "A");
                yield return new Processor(new[] { new SignValue(4) }, "B");
                yield return new Processor(new[] { new SignValue(10) }, "B");
                yield return new Processor(new[] { new SignValue(2) }, "A");
                yield return new Processor(new[] { new SignValue(11) }, "B");
            }

            CheckReflexValue(reflex, GetProcs19(), 1, 1);

            ResetReflex();

            #endregion

            //перевернуть карты и провести "обратный" тест
        }

        [TestMethod]
        public void ReflexTest1_7()
        {//дописать негативные тесты
            Processor pQ = new Processor(new[] { new SignValue(10) }, "Q");

            Processor pA = new Processor(new[] { new SignValue(2), new SignValue(62) }, "A");
            Processor pB = new Processor(new[] { new SignValue(12), new SignValue(9) }, "B");

            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(pA, pB));

            Processor pQ1 = new Processor(new[] { new SignValue(1), new SignValue(10), new SignValue(81) }, "Q1");

            Processor pA1 = new Processor(new[] { new SignValue(2), new SignValue(10) }, "A1");
            Processor pB1 = new Processor(new[] { new SignValue(10), new SignValue(90) }, "B1");

            SignValue[,] svQ2 = new SignValue[1, 3];

            svQ2[0, 0] = new SignValue(1);
            svQ2[0, 1] = new SignValue(10);
            svQ2[0, 2] = new SignValue(81);

            Processor pQ2 = new Processor(svQ2, "Q2");

            SignValue[,] svQ3 = new SignValue[1, 2];
            svQ3[0, 0] = new SignValue(2);
            svQ3[0, 1] = new SignValue(10);

            SignValue[,] svQ4 = new SignValue[1, 2];
            svQ4[0, 0] = new SignValue(10);
            svQ4[0, 1] = new SignValue(90);

            Processor pA2 = new Processor(svQ3, "A2");
            Processor pB2 = new Processor(svQ4, "B2");

            /*Assert.AreEqual(false, reflex.FindRelation((null, "AB"), (new Processor(m0, "mm"), string.Empty), (new Processor(m0, "mm"), null), (null,null), (new Processor(m0, "mm"), null), (null, string.Empty)));

            CheckReflexValue(reflex, processors, 1,1);*/
        }

        [TestMethod]
        public void ReflexTest2()
        {
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

            DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            {
                SignValue[,] map = new SignValue[4, 4];
                map[0, 0] = SignValue.MaxValue;
                map[2, 0] = SignValue.MaxValue;
                map[1, 1] = SignValue.MaxValue;
                map[2, 1] = SignValue.MaxValue;
                map[0, 2] = SignValue.MaxValue;
                map[2, 2] = SignValue.MaxValue;
                map[3, 3] = SignValue.MaxValue;

                Processor main = new Processor(map, "main");

                Assert.AreNotEqual(false, reflex.FindRelation((main, "E")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "C")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "A")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "B")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "D")));
                Assert.AreEqual(false, reflex.FindRelation((main, "W")));

                for (int k = 0; k < 50; k++)
                {
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BA")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AC")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CA")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AD")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "DA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AE")));

                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BB")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "BC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BB")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EB")));

                    Assert.AreEqual(false, reflex.FindRelation((main, "CA")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CC")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CD")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CE")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AC")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "BC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CC")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "DC")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "EC")));

                    Assert.AreEqual(false, reflex.FindRelation((main, "DA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DB")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "DC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DE")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BD")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "ED")));

                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EB")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "EC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "ED")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BE")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EE")));
                }
            }

            {
                SignValue[,] map = new SignValue[4, 4];
                map[0, 0] = SignValue.MaxValue;
                map[1, 0] = SignValue.MaxValue;
                map[0, 1] = SignValue.MaxValue;
                map[1, 1] = SignValue.MaxValue;
                map[2, 1] = SignValue.MaxValue;
                map[3, 1] = SignValue.MaxValue;
                map[3, 2] = SignValue.MaxValue;
                map[0, 3] = SignValue.MaxValue;
                map[1, 3] = SignValue.MaxValue;

                Processor main = new Processor(map, "main");

                Assert.AreNotEqual(false, reflex.FindRelation((main, "E")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "C")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "A")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "B")));
                Assert.AreNotEqual(false, reflex.FindRelation((main, "D")));
                Assert.AreEqual(false, reflex.FindRelation((main, "W")));

                for (int k = 0; k < 50; k++)
                {
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BA")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AC")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "CA")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AD")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "DA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AE")));

                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EB")));

                    Assert.AreEqual(false, reflex.FindRelation((main, "CA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CE")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EC")));

                    Assert.AreEqual(false, reflex.FindRelation((main, "DA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DE")));
                    Assert.AreEqual(false, reflex.FindRelation((main, "AD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DD")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "ED")));

                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EA")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EB")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EC")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "ED")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "AE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "BE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "CE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "DE")));
                    Assert.AreNotEqual(false, reflex.FindRelation((main, "EE")));
                }
            }
        }

        [TestMethod]
        public void ReflexTest6()
        {
            SignValue[,] map = new SignValue[2, 2];
            map[0, 0] = SignValue.MaxValue;
            map[1, 1] = SignValue.MaxValue;
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            Processor main = new Processor(map, "main");

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
            Assert.AreEqual(null, reflex.FindRelation(main, "C"));
            Assert.AreEqual(null, reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AE"));
        }

        [TestMethod]
        public void ReflexTest7()
        {
            SignValue[,] m1 = new SignValue[1, 1];
            m1[0, 0] = SignValue.MaxValue;
            SignValue[,] m2 = new SignValue[1, 1];

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(m1, "1"), new Processor(m2, "2")));

            SignValue[,] p1 = new SignValue[1, 1];
            p1[0, 0] = new SignValue(SignValue.MaxValue.Value / 2);

            Assert.AreEqual(null, reflex.FindRelation(new Processor(p1, "p1"), "1"));

            SignValue[,] p2 = new SignValue[1, 1];
            p2[0, 0] = new SignValue(p1[0, 0].Value - 10000);

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(new Processor(p2, "p2"), "2"));
            Assert.AreEqual(null, reflex.FindRelation(new Processor(p2, "p2"), "1"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(new Processor(p2, "p2"), "2"));
            Assert.AreEqual(null, reflex.FindRelation(new Processor(p2, "p2"), "1"));
        }

        [TestMethod]
        public void ReflexTest7_1()
        {
            SignValue[,] m1 = new SignValue[1, 1];
            m1[0, 0] = SignValue.MaxValue;
            SignValue[,] m2 = new SignValue[1, 1];

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(m1, "1"), new Processor(m2, "2")));

            SignValue[,] p1 = new SignValue[1, 1];
            p1[0, 0] = new SignValue(SignValue.MaxValue.Value / 2);

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(new Processor(p1, "p1"), "2"));

            SignValue[,] p2 = new SignValue[1, 1];
            p2[0, 0] = new SignValue(p1[0, 0].Value + 10000);

            Assert.AreEqual(null, reflex.FindRelation(new Processor(p2, "p2"), "1"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(new Processor(p2, "p2"), "2"));

            Assert.AreEqual(null, reflex.FindRelation(new Processor(p2, "p2"), "1"));
            Assert.AreNotEqual(null, reflex.FindRelation(new Processor(p2, "p2"), "2"));
        }

        [TestMethod]
        public void ReflexTest8()
        {
            SignValue average = new SignValue(SignValue.MaxValue.Value / 2);
            ProcessorContainer pc = new ProcessorContainer(new Processor(new SignValue[1], "a"), new Processor(new[] { average }, "b"));

            Reflex reflex = new Reflex(pc);
            Processor p = new Processor(new[] { average }, "c");

            Assert.AreEqual(null, reflex.FindRelation(p, "a"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(p, "b"));
            Assert.AreEqual(null, reflex.FindRelation(p, "f"));

            Processor pf = new Processor(new[] { average }, "f");
            pc.Add(pf);

            Assert.AreEqual(null, reflex.FindRelation(p, "a"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(p, "b"));
            Assert.AreEqual(null, reflex.FindRelation(p, "f"));
        }

        [TestMethod]
        public void ReflexTest8_1()
        {
            SignValue average = new SignValue(SignValue.MaxValue.Value / 2);
            ProcessorContainer pc = new ProcessorContainer(new Processor(new SignValue[1], "a"), new Processor(new[] { average }, "b"),
                new Processor(new[] { average }, "f"));

            Reflex reflex = new Reflex(pc);
            Processor p = new Processor(new[] { average }, "c");

            Assert.AreEqual(null, reflex.FindRelation(p, "a"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(p, "b"));
            Assert.AreNotEqual(null, reflex.FindRelation(p, "f"));
        }

        [TestMethod]
        public void ReflexTest9()
        {
            SignValue[,] minmap = new SignValue[1, 1];
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            {
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                int c = 0;
                try
                {
                    reflex.FindRelation(minProcessor, null);
                }
                catch (ArgumentNullException)
                {
                    c++;
                }

                try
                {
                    reflex.FindRelation(null, "W");
                }
                catch (ArgumentNullException)
                {
                    c++;
                }

                try
                {
                    reflex.FindRelation(minProcessor, string.Empty);
                }
                catch (ArgumentException)
                {
                    c++;
                }

                Assert.AreEqual(3, c);
            }

            Processor main = new Processor(map, "main");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            {
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                int c = 0;
                try
                {
                    reflex.FindRelation(minProcessor, null);
                }
                catch (ArgumentNullException)
                {
                    c++;
                }

                try
                {
                    reflex.FindRelation(null, "W");
                }
                catch (ArgumentNullException)
                {
                    c++;
                }

                try
                {
                    reflex.FindRelation(minProcessor, string.Empty);
                }
                catch (ArgumentException)
                {
                    c++;
                }

                Assert.AreEqual(3, c);
            }

            for (int k = 0; k < 50; k++)
            {
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AE"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    int c = 0;
                    try
                    {
                        reflex.FindRelation(minProcessor, null);
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(null, "W");
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(minProcessor, string.Empty);
                    }
                    catch (ArgumentException)
                    {
                        c++;
                    }

                    Assert.AreEqual(3, c);
                }

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EB"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    int c = 0;
                    try
                    {
                        reflex.FindRelation(minProcessor, null);
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(null, "W");
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(minProcessor, string.Empty);
                    }
                    catch (ArgumentException)
                    {
                        c++;
                    }

                    Assert.AreEqual(3, c);
                }

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EC"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    int c = 0;
                    try
                    {
                        reflex.FindRelation(minProcessor, null);
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(null, "W");
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(minProcessor, string.Empty);
                    }
                    catch (ArgumentException)
                    {
                        c++;
                    }

                    Assert.AreEqual(3, c);
                }

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ED"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    int c = 0;
                    try
                    {
                        reflex.FindRelation(minProcessor, null);
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(null, "W");
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(minProcessor, string.Empty);
                    }
                    catch (ArgumentException)
                    {
                        c++;
                    }

                    Assert.AreEqual(3, c);
                }

                Assert.AreEqual(null, reflex.FindRelation(main, "EA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ED"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EE"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    int c = 0;
                    try
                    {
                        reflex.FindRelation(minProcessor, null);
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(null, "W");
                    }
                    catch (ArgumentNullException)
                    {
                        c++;
                    }

                    try
                    {
                        reflex.FindRelation(minProcessor, string.Empty);
                    }
                    catch (ArgumentException)
                    {
                        c++;
                    }

                    Assert.AreEqual(3, c);
                }
            }
        }

        [TestMethod]
        public void ReflexTest11()
        {
            SignValue[,] map = new SignValue[2, 2];
            map[0, 0] = SignValue.MaxValue;
            map[1, 0] = SignValue.MinValue;
            map[0, 1] = new SignValue(11000);
            map[1, 1] = new SignValue(116000);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = SignValue.MinValue;
            SignValue[,] mapC = new SignValue[1, 1];
            mapC[0, 0] = new SignValue(11000);
            SignValue[,] mapD = new SignValue[1, 1];
            mapD[0, 0] = new SignValue(116000);

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"), new Processor(mapD, "D")));

            Processor main = new Processor(map, "main");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ABCD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dADDDbbBBDDbBacccaCccccaaabbcDddd"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ABC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CAB"));
            Assert.AreNotEqual(null, reflex.FindRelation(main, "BCA"));
        }

        [TestMethod]
        public void ReflexTest16()
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A1"), new Processor(mapB, "A2"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "A3")));

            Processor main = new Processor(map, "main");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WW"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ADC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CAD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DCA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ADCE"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "EA"));
        }

        [TestMethod]
        public void ReflexCopyTest2()
        {
            SignValue[,] bmap = new SignValue[4, 4];
            bmap[0, 0] = SignValue.MaxValue;
            bmap[2, 0] = SignValue.MaxValue;
            bmap[1, 1] = SignValue.MaxValue;
            bmap[2, 1] = SignValue.MaxValue;
            bmap[0, 2] = SignValue.MaxValue;
            bmap[2, 2] = SignValue.MaxValue;
            bmap[3, 3] = SignValue.MaxValue;

            SignValue[,] mapA = new SignValue[2, 2];
            mapA[0, 0] = SignValue.MaxValue;
            mapA[1, 0] = SignValue.MaxValue;

            SignValue[,] mapB = new SignValue[2, 2];
            mapB[1, 1] = SignValue.MaxValue;

            ProcessorContainer pc = new ProcessorContainer(new Processor(mapB, "B"), new Processor(mapB, "C"));

            Processor main = new Processor(bmap, "bigmain");

            Reflex reflex = new Reflex(pc);

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));

            pc.Add(new Processor(mapA, "A"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflexArgumentException1()
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            Processor main = new Processor(map, "main");

            reflex.FindRelation(main, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflexArgumentException2()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflex(new ProcessorContainer(new Processor(new SignValue[1], "tag")));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflexArgumentNullException1()
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            Processor main = new Processor(map, "main");

            reflex.FindRelation(main, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflexArgumentNullException2()
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            reflex.FindRelation(null, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflexArgumentNullException3()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflex((ProcessorContainer)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflexArgumentNullException4()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflex((Reflex)null);
        }
    }
}