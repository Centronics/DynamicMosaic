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
        static void CheckReflexValue(DynamicReflex actual, IEnumerable<Processor> pcExpected, int width, int height)
        {
            List<Processor> listActual = actual.Processors.ToList();

            foreach (Processor pExpected in pcExpected)
            {
                Assert.AreNotEqual(null, pExpected);

                Processor pFinded = listActual.Find(pActual =>
                {
                    Assert.AreNotEqual(null, pActual);

                    if (pActual.Tag[0] != pExpected.Tag[0])
                        return false;
                    if (width != pActual.Width)
                        return false;
                    if (height != pActual.Height)
                        return false;
                    if (width != pExpected.Width)
                        return false;
                    if (height != pExpected.Height)
                        return false;

                    for (int y = 0; y < pExpected.Height; y++)
                        for (int x = 0; x < pExpected.Width; x++)
                            if (pExpected[x, y] != pActual[x, y])
                                return false;

                    return true;
                });

                Assert.AreNotEqual(null, pFinded);
                Assert.AreEqual(true, listActual.Remove(pFinded));
            }

            Assert.AreEqual(0, listActual.Count);
        }

        [TestMethod]
        public void ReflexMultilineTest()
        {
            void Scenario(Processor p, string q, bool desiredResult)//добавить функцию сохранения Reflex и дописать негативные сценарии из теста ниже
            {
                IEnumerable<Processor> Procs()
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

                DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(Procs().ToArray()));

                CheckReflexValue(reflex, Procs(), 2, 2);

                Assert.AreEqual(desiredResult, reflex.FindRelation((p, q)));
            }

            Processor pMain = null;

            for (int k = 0; k < 4; k++)
            {
                Processor Main()
                {
                    Processor GetProcessor() => new Processor(new SignValue[1, 1], "main");

                    if (k > -1 && k < 2)
                        return GetProcessor();

                    if (k > 1 && k < 4)
                        return pMain ?? (pMain = GetProcessor());

                    throw new Exception($@"Variable ""{nameof(k)}"" is unknown value: {k}");
                }

                Scenario(Main(), "A", false);
                Scenario(Main(), "B", false);
                Scenario(Main(), "C", false);
                Scenario(Main(), "D", false);
                Scenario(Main(), "E", false);
                Scenario(Main(), "W", false);
                Scenario(Main(), null, false);
                Scenario(Main(), string.Empty, false);
                Scenario(null, "W", false);
            }

            pMain = null;

            for (int k = 0; k < 4; k++)
            {
                Processor Main()
                {
                    Processor GetProcessor()
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

                    if (k > -1 && k < 2)
                        return GetProcessor();

                    if (k > 1 && k < 4)
                        return pMain ?? (pMain = GetProcessor());

                    throw new Exception($@"Variable ""{nameof(k)}"" is unknown value: {k}");
                }

                Scenario(Main(), "A", true);
                Scenario(Main(), "B", true);
                Scenario(Main(), "C", true);
                Scenario(Main(), "D", true);
                Scenario(Main(), "E", false);
                Scenario(Main(), "W", false);

                Scenario(Main(), "AA", true);
                Scenario(Main(), "AB", true);
                Scenario(Main(), "BA", true);
                Scenario(Main(), "AC", true);
                Scenario(Main(), "CA", true);
                Scenario(Main(), "AD", true);
                Scenario(Main(), "DA", true);
                Scenario(Main(), "AE", false);

                Scenario(Main(), "BA", true);
                Scenario(Main(), "BB", true);
                Scenario(Main(), "BC", true);
                Scenario(Main(), "BD", true);
                Scenario(Main(), "BE", false);
                Scenario(Main(), "AB", true);
                Scenario(Main(), "BB", true);
                Scenario(Main(), "CB", true);
                Scenario(Main(), "DB", true);
                Scenario(Main(), "EB", false);

                Scenario(Main(), "CA", true);
                Scenario(Main(), "CB", true);
                Scenario(Main(), "CC", true);
                Scenario(Main(), "CD", true);
                Scenario(Main(), "CE", false);
                Scenario(Main(), "AC", true);
                Scenario(Main(), "BC", true);
                Scenario(Main(), "CC", true);
                Scenario(Main(), "DC", true);
                Scenario(Main(), "EC", false);

                Scenario(Main(), "DA", true);
                Scenario(Main(), "DB", true);
                Scenario(Main(), "DC", true);
                Scenario(Main(), "DD", true);
                Scenario(Main(), "DE", false);
                Scenario(Main(), "AD", true);
                Scenario(Main(), "BD", true);
                Scenario(Main(), "CD", true);
                Scenario(Main(), "DD", true);
                Scenario(Main(), "ED", false);

                Scenario(Main(), "EA", false);
                Scenario(Main(), "EB", false);
                Scenario(Main(), "EC", false);
                Scenario(Main(), "ED", false);
                Scenario(Main(), "EE", false);
                Scenario(Main(), "AE", false);
                Scenario(Main(), "BE", false);
                Scenario(Main(), "CE", false);
                Scenario(Main(), "DE", false);
                Scenario(Main(), "EE", false);
            }
        }

        [TestMethod]
        public void ReflexQueryTest()
        {
            SignValue[,] main = new SignValue[1, 1];
            main[0, 0] = new SignValue(10);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(6);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(7);

            void Scenario(string a, string b, string c, string nameA, string nameB, params Processor[] processors)
            {
                DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(processors));

                for (int k = 0; k < 3; k++)
                {
                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainA"), a)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainA"), a)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainC"), c)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainC"), c)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "mainB"), b)));

                    processors[1] = new Processor(main, nameB);
                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "mainB"), b)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(mapB, "mainX"), b)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(mapB, "mainX"), b)));

                    CheckReflexValue(reflex, processors, 1, 1);
                }

                main[0, 0] = new SignValue(4);

                for (int k = 0; k < 3; k++)
                {
                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainB"), b)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainB"), b)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainC"), c)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(main, "mainC"), c)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "mainA"), a)));

                    processors[0] = new Processor(main, nameA);
                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(main, "mainA"), a)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, "mainX"), a)));

                    CheckReflexValue(reflex, processors, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, "mainX"), a)));

                    CheckReflexValue(reflex, processors, 1, 1);
                }
            }

            Scenario("A", "B", "C", "A1", "B1", new Processor(mapA, "A"), new Processor(mapB, "B"));
            Scenario("aaa", "Bb", "ccc", "A2", "B2", new Processor(mapA, "A1"), new Processor(mapB, "B1"));
        }
        //объединить нижний тест с верхним
        [TestMethod]
        public void ReflexTestNegative()
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

            Processor pQ = new Processor(new[] { new SignValue(10) }, "Q");

            Processor pA = new Processor(new[] { new SignValue(2), new SignValue(62) }, "A");
            Processor pB = new Processor(new[] { new SignValue(12), new SignValue(9) }, "B");

            reflex = new DynamicReflex(new ProcessorContainer(pA, pB));

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
        public void ReflexTestChangePC()
        {
            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(3);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(5);
            SignValue[,] mapC = new SignValue[1, 1];
            mapC[0, 0] = new SignValue(8);
            SignValue[,] mapD = new SignValue[1, 1];
            mapD[0, 0] = new SignValue(9);

            Processor pA = new Processor(mapA, "A");
            Processor pB = new Processor(mapB, "B");
            Processor pC = new Processor(mapC, "C");
            Processor pD = new Processor(mapD, "D");

            ProcessorContainer pc = new ProcessorContainer(pA, pB);
            DynamicReflex reflex = new DynamicReflex(pc);

            void CheckReflexState() => CheckReflexValue(reflex, new[] { pA, pB }, 1, 1);

            void ResetReflex(int k)
            {
                SignValue[,] m5 = new SignValue[2, 1];
                m5[0, 0] = new SignValue(3);
                m5[1, 0] = new SignValue(5);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m5, "m5"), @"AB")));

                CheckReflexState();

                Assert.AreEqual(true,
                    k % 2 == 0
                        ? reflex.FindRelation((new Processor(mapA, "A"), @"A"), (new Processor(mapB, "B"), @"B"))
                        : reflex.FindRelation((pA, @"A"), (pB, @"B")));

                CheckReflexState();

                Assert.AreEqual(true,
                    k % 2 == 0
                        ? reflex.FindRelation((new Processor(mapA, "A"), @"A"), (new Processor(mapB, "B"), @"B"))
                        : reflex.FindRelation((pA, @"A"), (pB, @"B")));

                CheckReflexState();

                Assert.AreEqual(true,
                    k % 2 == 0
                        ? reflex.FindRelation((pA, @"A"), (pB, @"B"))
                        : reflex.FindRelation((new Processor(mapA, "A"), @"A"), (new Processor(mapB, "B"), @"B")));

                CheckReflexState();

                Assert.AreEqual(true,
                    k % 2 == 0
                        ? reflex.FindRelation((pA, @"A"), (pB, @"B"))
                        : reflex.FindRelation((new Processor(mapA, "A"), @"A"), (new Processor(mapB, "B"), @"B")));

                CheckReflexState();
            }

            for (int k = 0; k < 5; k++)
            {
                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pC, "C")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((new Processor(mapC, "C"), "C")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pD, "D")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((new Processor(mapD, "D"), "D")));

                pc.Add(k == 0 ? pC : new Processor(mapC, $@"C{k}"));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pC, "C")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((new Processor(mapC, "C"), "C")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((new Processor(mapD, "D"), "D")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pD, "D")));

                pc.Add(k == 0 ? pD : new Processor(mapD, $@"D{k}"));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((new Processor(mapC, "C"), "C")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pC, "C")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pD, "D")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((new Processor(mapD, "D"), "D")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((null, "D")));

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pC, null)));

                ResetReflex(k);

                ResetReflex(k);

                CheckReflexState();

                Assert.AreEqual(false, reflex.FindRelation((pC, string.Empty)));

                CheckReflexState();

                ResetReflex(k);

                ResetReflex(k);
            }
        }

        [TestMethod]
        public void ReflexPositionalTest()
        {
            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(3);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(5);

            void Scenario(string a, string b)
            {
                DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(new Processor(mapA, a), new Processor(mapB, b)));

                ReflexRecognizeTest(reflex, mapA, mapB, a, b);

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(new[] { new SignValue(4), new SignValue(4) }, "reset"), $@"{a}{b}")));

                CheckReflexValue(reflex,
                    new[]
                    {
                        new Processor(new[] { new SignValue(4) }, a), new Processor(new[] { new SignValue(4) }, b)
                    }, 1, 1);

                ReflexRecognizeTest(reflex, mapA, mapB, b, a);
            }

            Scenario("A", "B");
            Scenario("a", "b");
        }

        static void ReflexRecognizeTest(DynamicReflex reflex, SignValue[,] mapA, SignValue[,] mapB, string a, string b)
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

            void ResetReflex()
            {
                SignValue[,] m5 = new SignValue[2, 1];
                m5[0, 0] = new SignValue(3);
                m5[1, 0] = new SignValue(5);

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m5, "m5"), $@"{a}{b}")));

                CheckReflexValue(reflex, new[] { new Processor(mapA, a), new Processor(mapB, b) }, 1, 1);
            }

            ResetReflex();

            Assert.AreEqual(false, reflex.FindRelation((null, $@"{a}{b}"), (new Processor(m0, "mm"), string.Empty), (new Processor(m0, "mm"), null), (null, null), (new Processor(m0, "mm"), null), (null, string.Empty)));

            CheckReflexValue(reflex, new[] { new Processor(mapA, a), new Processor(mapB, b) }, 1, 1);

            #region Test1
            //дописать тест, когда один пиксель распознаётся картами с одинаковыми значениями
            {

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}")));

                IEnumerable<Processor> GetProcs1()
                {
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                }

                CheckReflexValue(reflex, GetProcs1(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m1, "m1"), b)));

                IEnumerable<Processor> GetProcs2()
                {
                    yield return new Processor(new[] { new SignValue(3) }, a);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                }

                CheckReflexValue(reflex, GetProcs2(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m2, "m2"), b)));

                IEnumerable<Processor> GetProcs3()
                {
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                }

                CheckReflexValue(reflex, GetProcs3(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m3, "m3"), $@"{a}{b}")));

                IEnumerable<Processor> GetProcs4()
                {
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(3) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                    yield return new Processor(new[] { new SignValue(13) }, b);
                }

                CheckReflexValue(reflex, GetProcs4(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true, reflex.FindRelation((new Processor(m4, "m4"), a)));

                IEnumerable<Processor> GetProcs5()
                {
                    yield return new Processor(new[] { new SignValue(1) }, a);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                }

                CheckReflexValue(reflex, GetProcs5(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region Test2

            {

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(10) }, b);
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(11) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m1, "m1"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"),
                            (new Processor(m0, "m0"), $@"{a}{b}"),
                            (new Processor(m1, "m1"), b), (new Processor(m1, "m1"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), b), (new Processor(m0, "m0"), a),
                            (new Processor(m1, "m1"), b), (new Processor(m1, "m1"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(1) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(10) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m2, "m2"), b),
                            (new Processor(m2, "m2"), $@"{a}{b}"), (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m0, "m0"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(3) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(10) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{b}{b}"), (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{b}{b}"), (new Processor(m2, "m2"), b),
                            (new Processor(m2, "m2"), $@"{b}{b}"), (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m0, "m0"), $@"{b}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(1) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{a}"), (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m0, "m0"), $@"{a}{a}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(10) }, b);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m0, "m0"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(11) }, b);
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(3) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m3, "m3"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), $@"{a}{b}"), (new Processor(m1, "m1"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(10) }, b);
                        yield return new Processor(new[] { new SignValue(2) }, a);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b),
                            (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m1, "m1"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m1, "m1"), b),
                            (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(10) }, b);
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(3) }, a);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                            (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m2, "m2"), b),
                            (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                            (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m2, "m2"), b),
                            (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), b),
                            (new Processor(m2, "m2"), a), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m2, "m2"), a),
                            (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), b),
                            (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(3) }, a);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), a),
                            (new Processor(m2, "m2"), a), (new Processor(m3, "m3"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m2, "m2"), a),
                            (new Processor(m3, "m3"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), a),
                            (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), a), (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                        yield return new Processor(new[] { new SignValue(3) }, a);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                            (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m2, "m2"), b),
                            (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                            (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(3) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $@"{a}{b}"),
                            (new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $@"{b}{a}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m2, "m2"), a),
                            (new Processor(m3, "m3"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $@"{b}{a}"),
                            (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), $@"{a}{b}"), (new Processor(m2, "m2"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(3) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $@"{a}{b}"),
                            (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $@"{b}{a}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m2, "m2"), b),
                            (new Processor(m3, "m3"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $@"{b}{a}"),
                            (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), $@"{a}{b}"), (new Processor(m2, "m2"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(5) }, b);
                        yield return new Processor(new[] { new SignValue(13) }, b);
                        yield return new Processor(new[] { new SignValue(1) }, a);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a),
                            (new Processor(m4, "m4"), a), (new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a),
                            (new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a),
                            (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m3, "m3"), b)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

                #region SubTest_2

                {

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(1) }, a);
                        yield return new Processor(new[] { new SignValue(2) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, a);
                        yield return new Processor(new[] { new SignValue(4) }, b);
                        yield return new Processor(new[] { new SignValue(10) }, b);
                    }

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $@"{a}{b}")));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $@"{a}{b}"),
                            (new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m4, "m4"), a),
                            (new Processor(m4, "m4"), a),
                            (new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m0, "m0"), $@"{a}{b}"),
                            (new Processor(m4, "m4"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m4, "m4"), a)));

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion

            }

            #endregion

            #region Test3

            {

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m1, "m1"), b),
                        (new Processor(m2, "m2"), b)));

                IEnumerable<Processor> GetProcs10()
                {
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                }

                CheckReflexValue(reflex, GetProcs10(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b),
                        (new Processor(m3, "m3"), b)));

                IEnumerable<Processor> GetProcs11()
                {
                    yield return new Processor(new[] { new SignValue(11) }, b);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(13) }, b);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                }

                CheckReflexValue(reflex, GetProcs11(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                        (new Processor(m4, "m4"), a)));

                IEnumerable<Processor> GetProcs12()
                {
                    yield return new Processor(new[] { new SignValue(1) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(13) }, b);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                }

                CheckReflexValue(reflex, GetProcs12(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $@"{a}{b}"),
                        (new Processor(m1, "m1"), b)));

                IEnumerable<Processor> GetProcs13()
                {
                    yield return new Processor(new[] { new SignValue(1) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                }

                CheckReflexValue(reflex, GetProcs13(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region Test4

            {

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m1, "m1"), b),
                        (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b)));

                IEnumerable<Processor> GetProcs14()
                {
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                    yield return new Processor(new[] { new SignValue(13) }, b);
                }

                CheckReflexValue(reflex, GetProcs14(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $@"{a}{b}"),
                        (new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b)));

                IEnumerable<Processor> GetProcs15()
                {
                    yield return new Processor(new[] { new SignValue(1) }, a);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                }

                CheckReflexValue(reflex, GetProcs15(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a),
                        (new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m1, "m1"), b)));

                IEnumerable<Processor> GetProcs16()
                {
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                    yield return new Processor(new[] { new SignValue(13) }, b);
                    yield return new Processor(new[] { new SignValue(1) }, a);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                }

                CheckReflexValue(reflex, GetProcs16(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                        (new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $@"{a}{b}")));

                IEnumerable<Processor> GetProcs17()
                {
                    yield return new Processor(new[] { new SignValue(1) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(13) }, b);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                }

                CheckReflexValue(reflex, GetProcs17(), 1, 1);

                ResetReflex();

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b),
                        (new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a)));

                IEnumerable<Processor> GetProcs18()
                {
                    yield return new Processor(new[] { new SignValue(1) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(5) }, b);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                    yield return new Processor(new[] { new SignValue(13) }, b);
                }

                CheckReflexValue(reflex, GetProcs18(), 1, 1);

                ResetReflex();

            }

            #endregion

            #region Test5

            {

                Assert.AreEqual(true,
                    reflex.FindRelation((new Processor(m0, "m0"), $@"{a}{b}"), (new Processor(m1, "m1"), b),
                        (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a)));

                IEnumerable<Processor> GetProcs19()
                {
                    yield return new Processor(new[] { new SignValue(4) }, a);
                    yield return new Processor(new[] { new SignValue(4) }, b);
                    yield return new Processor(new[] { new SignValue(10) }, b);
                    yield return new Processor(new[] { new SignValue(2) }, a);
                    yield return new Processor(new[] { new SignValue(11) }, b);
                }

                CheckReflexValue(reflex, GetProcs19(), 1, 1);

                ResetReflex();

            }

            #endregion
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflexArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new DynamicReflex(null);
        }
    }
}