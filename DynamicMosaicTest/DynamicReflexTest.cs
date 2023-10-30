using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicMosaic;
using DynamicParser;
using DynamicProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor = DynamicParser.Processor;

namespace DynamicMosaicTest
{
    [TestClass]
    public class DynamicReflexTest
    {
        static void SetMinMaxPoolThreads()
        {
            ThreadPool.GetMinThreads(out _, out int comPortMin);
            Assert.AreEqual(true, ThreadPool.SetMinThreads(Environment.ProcessorCount * 3, comPortMin));
            ThreadPool.GetMaxThreads(out _, out int comPortMax);
            Assert.AreEqual(true, ThreadPool.SetMaxThreads(Environment.ProcessorCount * 15, comPortMax));
        }

        static SignValue[,] InitValues(SignValue[,] values)
        {
            Assert.AreNotEqual(null, values);

            Assert.AreEqual(true, values.LongLength > 0);

            Parallel.For(0, values.GetLength(1), (y, yState) =>
            {
                if (yState.ShouldExitCurrentIteration)
                    return;

                Parallel.For(0, values.GetLength(0), (x, xState) =>
                {
                    if (xState.ShouldExitCurrentIteration || yState.ShouldExitCurrentIteration)
                        return;

                    values[x, y] = new SignValue(10);
                });
            });

            return values;
        }

        static void CheckReflexValue(DynamicReflex actual, IEnumerable<Processor> pcExpected, int widthExpected, int heightExpected)
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
                    if (widthExpected != pActual.Width)
                        return false;
                    if (heightExpected != pActual.Height)
                        return false;
                    if (widthExpected != pExpected.Width)
                        return false;
                    if (heightExpected != pExpected.Height)
                        return false;

                    ParallelLoopResult state = Parallel.For(0, pExpected.Height, (y, yState) =>
                    {
                        if (yState.ShouldExitCurrentIteration)
                            return;

                        Parallel.For(0, pExpected.Width, (x, xState) =>
                        {
                            if (xState.ShouldExitCurrentIteration || yState.ShouldExitCurrentIteration)
                                return;

                            if (pExpected[x, y] == pActual[x, y])
                                return;

                            xState.Stop();
                            yState.Stop();
                        });
                    });

                    return state.IsCompleted;
                });

                Assert.AreNotEqual(null, pFinded);
                Assert.AreEqual(true, listActual.Remove(pFinded));
            }

            Assert.AreEqual(0, listActual.Count);
        }

        static void ArgumentNullExceptionTest()
        {
            bool TestFunc(Action act)
            {
                try
                {
                    act();
                }
                catch (ArgumentNullException)
                {
                    return true;
                }

                return false;
            }

            Assert.AreEqual(true, TestFunc(() => new DynamicReflex(null)));

            Assert.AreEqual(true, TestFunc(() => new DynamicReflex(new ProcessorContainer(new Processor(new[] { new SignValue(3) }, "A"), new Processor(new[] { new SignValue(5) }, "B"))).FindRelation(null)));
        }

        static void ArgumentExceptionTest()
        {
            bool TestFunc(Action act)
            {
                try
                {
                    act();
                }
                catch (ArgumentException)
                {
                    return true;
                }

                return false;
            }

            ArgumentNullExceptionTest();

            Processor p1 = new Processor(new[] { new SignValue(3) }, "A");

            DynamicReflex dr = new DynamicReflex(new ProcessorContainer(p1));

            Assert.AreEqual(true, TestFunc(() => dr.FindRelation()));

            Assert.AreEqual(false, new DynamicReflex(new ProcessorContainer(p1)).FindRelation((p1, " ")));

            Assert.AreEqual(false, new DynamicReflex(new ProcessorContainer(p1)).FindRelation((p1, "  ")));

            Assert.AreEqual(false, new DynamicReflex(new ProcessorContainer(p1)).FindRelation((p1, "\t")));

            Assert.AreEqual(false, new DynamicReflex(new ProcessorContainer(p1)).FindRelation((p1, string.Empty)));

            Assert.AreEqual(false, new DynamicReflex(new ProcessorContainer(p1)).FindRelation((p1, null)));
        }

        [TestMethod]
        public void ReflexMultilineTest()
        {
            SetMinMaxPoolThreads();

            #region Иллюстрация

            /*

                MAIN:
                1 0 1 0
                0 1 1 0
                1 0 1 0
                0 0 0 1

                A:
                1 0
                1 0

                B:
                0 0
                0 1

                C:
                1 1
                0 0

                D:
                1 1
                1 1

                E:
                0 0
                0 0
            */

            #endregion

            #region Требуемые результаты

            SignValue[,] checkA = new SignValue[2, 2];
            checkA[0, 0] = SignValue.MaxValue;
            checkA[0, 1] = SignValue.MaxValue;

            Processor checkProcA = new Processor(checkA, "A");

            SignValue[,] checkA1 = new SignValue[2, 2];
            checkA1[0, 1] = SignValue.MaxValue;
            checkA1[1, 0] = SignValue.MaxValue;

            Processor checkProcA1 = new Processor(checkA1, "A1");

            SignValue[,] checkA2 = new SignValue[2, 2];
            checkA2[0, 0] = SignValue.MaxValue;

            Processor checkProcA2 = new Processor(checkA2, "A2");

            SignValue[,] checkB = new SignValue[2, 2];
            checkB[0, 0] = SignValue.MaxValue;
            checkB[1, 1] = SignValue.MaxValue;

            Processor checkProcB = new Processor(checkB, "B");

            SignValue[,] checkC = new SignValue[2, 2];
            checkC[0, 0] = SignValue.MaxValue;

            Processor checkProcC = new Processor(checkC, "C");

            SignValue[,] checkC1 = new SignValue[2, 2];
            checkC1[0, 1] = SignValue.MaxValue;
            checkC1[1, 0] = SignValue.MaxValue;

            Processor checkProcC1 = new Processor(checkC1, "C1");

            SignValue[,] checkC2 = new SignValue[2, 2];
            checkC2[0, 0] = SignValue.MaxValue;
            checkC2[1, 0] = SignValue.MaxValue;
            checkC2[1, 1] = SignValue.MaxValue;

            Processor checkProcC2 = new Processor(checkC2, "C2");

            SignValue[,] checkC3 = new SignValue[2, 2];
            checkC3[1, 0] = SignValue.MaxValue;

            Processor checkProcC3 = new Processor(checkC3, "C3");

            SignValue[,] checkD = new SignValue[2, 2];
            checkD[0, 1] = SignValue.MaxValue;
            checkD[1, 0] = SignValue.MaxValue;
            checkD[1, 1] = SignValue.MaxValue;

            Processor checkProcD = new Processor(checkD, "D");

            SignValue[,] checkD1 = new SignValue[2, 2];
            checkD1[0, 1] = SignValue.MaxValue;
            checkD1[1, 0] = SignValue.MaxValue;

            Processor checkProcD1 = new Processor(checkD1, "D1");

            SignValue[,] checkD2 = new SignValue[2, 2];
            checkD2[0, 0] = SignValue.MaxValue;
            checkD2[1, 0] = SignValue.MaxValue;
            checkD2[1, 1] = SignValue.MaxValue;

            Processor checkProcD2 = new Processor(checkD2, "D2");

            SignValue[,] checkE = new SignValue[2, 2];
            checkE[0, 1] = SignValue.MaxValue;
            checkE[1, 0] = SignValue.MaxValue;

            Processor checkProcE = new Processor(checkE, "E");

            SignValue[,] checkE1 = new SignValue[2, 2];
            checkE1[0, 0] = SignValue.MaxValue;

            Processor checkProcE1 = new Processor(checkE1, "E1");

            SignValue[,] checkE2 = new SignValue[2, 2];
            checkE2[1, 0] = SignValue.MaxValue;

            Processor checkProcE2 = new Processor(checkE2, "E2");

            Processor[] pA = GetExpectedProcessors(checkProcA, checkProcA1, checkProcA2);
            Processor[] pB = GetExpectedProcessors(checkProcB);
            Processor[] pC = GetExpectedProcessors(checkProcC, checkProcC1, checkProcC2, checkProcC3);
            Processor[] pD = GetExpectedProcessors(checkProcD, checkProcD1, checkProcD2);
            Processor[] pE = GetExpectedProcessors(checkProcE, checkProcE1, checkProcE2);

            Processor[] pAB = GetExpectedProcessors(checkProcA, checkProcA1, checkProcA2, checkProcB);
            Processor[] pAC = GetExpectedProcessors(checkProcA, checkProcC, checkProcC1, checkProcC3);
            Processor[] pAD = GetExpectedProcessors(checkProcA, checkProcA2, checkProcD, checkProcD1);
            Processor[] pAE = GetExpectedProcessors(checkProcA, checkProcE, checkProcE1, checkProcE2);

            Processor[] pBC = GetExpectedProcessors(checkProcB, checkProcC, checkProcC1, checkProcC3);
            Processor[] pBD = GetExpectedProcessors(checkProcB, checkProcD, checkProcD1);
            Processor[] pBE = GetExpectedProcessors(checkProcB, checkProcE, checkProcE1, checkProcE2);

            Processor[] pCD = GetExpectedProcessors(checkProcC, checkProcC3, checkProcD);

            Processor[] pDE = GetExpectedProcessors(checkProcD, checkProcE1, checkProcE2);

            #endregion

            #region Тестовые сценарии

            DynamicReflex stReflex = null;

            Scenario("A", pA);
            Scenario("B", pB);
            Scenario("C", pC);
            Scenario("D", pD);
            Scenario("E", pE);

            Scenario("AA", pA);
            Scenario("AB", pAB);
            Scenario("AC", pAC);
            Scenario("AD", pAD);
            Scenario("AE", pAE);

            Scenario("AA", pA);
            Scenario("BA", pAB);
            Scenario("CA", pAC);
            Scenario("DA", pAD);
            Scenario("EA", pAE);

            Scenario("BA", pAB);
            Scenario("BB", pB);
            Scenario("BC", pBC);
            Scenario("BD", pBD);
            Scenario("BE", pBE);

            Scenario("AB", pAB);
            Scenario("BB", pB);
            Scenario("CB", pBC);
            Scenario("DB", pBD);
            Scenario("EB", pBE);

            Scenario("CA", pAC);
            Scenario("CB", pBC);
            Scenario("CC", pC);
            Scenario("CD", pCD);
            Scenario();

            Scenario("AC", pAC);
            Scenario("BC", pBC);
            Scenario("CC", pC);
            Scenario("DC", pCD);
            Scenario("EC");

            Scenario("DA", pAD);
            Scenario("DB", pBD);
            Scenario("DC", pCD);
            Scenario("DD", pD);
            Scenario("DE", pDE);

            Scenario("AD", pAD);
            Scenario("BD", pBD);
            Scenario("CD", pCD);
            Scenario("DD", pD);
            Scenario("ED", pDE);

            Scenario("EA", pAE);
            Scenario("EB", pBE);
            Scenario("EC");
            Scenario("ED", pDE);
            Scenario("EE", pE);

            Scenario("AE", pAE);
            Scenario("BE", pBE);
            Scenario();
            Scenario("DE", pDE);
            Scenario("EE", pE);

            Scenario();

            #endregion

            return;

            #region Тестовая процедура

            void Scenario(string q = "CE", params Processor[] desiredResult)
            {
                for (int k = 0; k < 6; k++)
                {
                    TestBody(k, q);
                    TestBody(k, string.Empty);
                    TestBody(k);
                }

                return;

                void TestBody(int k, string qx = null)
                {
                    if (stReflex == null)
                        stReflex = new DynamicReflex(new ProcessorContainer(GetTestProcessors().ToArray()));

                    TestProc(new DynamicReflex(new ProcessorContainer(GetTestProcessors().ToArray())));
                    TestProc(stReflex);
                    return;

                    void TestProc(DynamicReflex reflex)
                    {
                        CheckReflexValue(reflex, GetTestProcessors(), 2, 2);

                        bool isNormal = k == 0 && !string.IsNullOrEmpty(qx);

                        Assert.AreEqual(isNormal && desiredResult.Length > 0, reflex?.FindRelation((GetProcessorByIndex(k), qx)));

                        CheckReflexValue(reflex, isNormal && desiredResult.Length > 0 ? desiredResult : GetExpectedProcessors(), 2, 2);

                        Assert.AreEqual(true, reflex?.FindRelation(GetTestProcessors().Select(p => (p, p.Tag[0].ToString())).ToArray()));

                        CheckReflexValue(reflex, GetTestProcessors(), 2, 2);
                    }
                }
            }

            #endregion

            #region Вспомогательные методы

            Processor[] GetExpectedProcessors(params Processor[] procs)
            {
                List<Processor> result = procs.ToList();
                HashSet<char> chs = new HashSet<char>(procs.Select(p => char.ToUpper(p.Tag[0])));

                result.AddRange(GetTestProcessors().Where(p => !chs.Contains(char.ToUpper(p.Tag[0]))));

                return result.ToArray();
            }

            IEnumerable<Processor> GetTestProcessors()
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

            Processor GetProcessorByIndex(int index)
            {
                switch (index)
                {
                    case 0:
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
                    case 1:
                        return new Processor(new SignValue[1, 1], "main");
                    case 2:
                        return new Processor(new SignValue[1, 2], "main");
                    case 3:
                        return new Processor(new SignValue[1, 4], "main");
                    case 4:
                        return new Processor(new SignValue[2, 1], "main");
                    case 5:
                        return new Processor(new SignValue[4, 1], "main");
                }

                throw new ArgumentException($"{nameof(index)} value = {index}", nameof(index));
            }

            #endregion
        }

        [TestMethod]
        public void ReflexSamePointTest()
        {
            SetMinMaxPoolThreads();

            SignValue svDefault = new SignValue(6);
            SignValue svMidl = new SignValue(8);

            SignValue[] svsMax = { new SignValue(10) };
            SignValue[] svsDefault = { svDefault, svDefault };
            SignValue[] svsDeflt = { svDefault };
            SignValue[] svsMidl = { svMidl, svMidl };
            SignValue[] svsMdl = { svMidl };
            SignValue[] svsDefaultA = { svDefault };
            SignValue[] svsDefaultB = { svDefault };

            Processor p1 = new Processor(svsMax, "main"), p11 = new Processor(svsMax, "main");
            Processor p2 = new Processor(svsDefault, "default"), p22 = new Processor(svsDefault, "default");
            Processor p3 = new Processor(svsDeflt, "deflt"), p33 = new Processor(svsDeflt, "deflt");
            Processor p4 = new Processor(svsMidl, "midl"), p44 = new Processor(svsMidl, "midl");
            Processor p5 = new Processor(svsMdl, "mdl"), p55 = new Processor(svsMdl, "mdl");

            Processor ProcMain(int n) => n > 1 ? n == 2 ? p11 : new Processor(svsMax, "main") : p1;
            Processor ProcDefault(int n) => n > 1 ? n == 2 ? p22 : new Processor(svsDefault, "default") : p2;
            Processor ProcDeflt(int n) => n > 1 ? n == 2 ? p33 : new Processor(svsDeflt, "deflt") : p3;
            Processor ProcMidl(int n) => n > 1 ? n == 2 ? p44 : new Processor(svsMidl, "midl") : p4;
            Processor ProcMdl(int n) => n > 1 ? n == 2 ? p55 : new Processor(svsMdl, "mdl") : p5;

            Processor procMaxA = new Processor(svsMax, "A");
            Processor procMaxB = new Processor(svsMax, "B");

            Processor processorA = new Processor(svsDefaultA, "A");
            Processor processorB = new Processor(svsDefaultB, "B");

            Processor[] reflexProcs = { processorA, processorB };

            DynamicReflex reflex = null;

            for (int r = 0; r < 3; r++)
            {
                if (r % 2 == 0)
                    reflex = new DynamicReflex(new ProcessorContainer(reflexProcs));

                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {

                        void NegativeTests(Processor[] rp)
                        {
                            bool ExceptionFunc(Action act)
                            {
                                try
                                {
                                    act();
                                }
                                catch (ArgumentException)
                                {
                                    return true;
                                }

                                return false;
                            }

                            if (reflex == null)
                                throw new ArgumentNullException();

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "AB")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "BA")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "Abb")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "aaB")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "cab")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "c")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "ccc")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "cbb")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "aac")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "CCC")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "CBB")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "AAC")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "Ccc")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "cBb")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "Aac")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((null, "AB")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), null)));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), string.Empty)));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), " ")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "  ")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((ProcMain(j), "\t")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((null, string.Empty)));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((null, " ")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((null, "  ")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((null, "\t")));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(false, reflex.FindRelation((null, null)));

                            CheckReflexValue(reflex, rp, 1, 1);

                            Assert.AreEqual(true, ExceptionFunc(() => reflex.FindRelation()));

                            CheckReflexValue(reflex, rp, 1, 1);

                            ArgumentExceptionTest();

                            CheckReflexValue(reflex, rp, 1, 1);
                        }

                        void ResetReflex()
                        {
                            (Processor, string) queryDefault = (ProcDefault(j), "AB");
                            (Processor, string) queryDefltA = (ProcDeflt(j), "A");
                            (Processor, string) queryDefltB = (ProcDeflt(j), "B");
                            (Processor, string) queryMidl = (ProcMidl(j), "AB");
                            (Processor, string) queryMdlA = (ProcMdl(j), "A");
                            (Processor, string) queryMdlB = (ProcMdl(j), "B");

                            switch (k)
                            {
                                case 0:
                                case 1:
                                    Assert.AreEqual(true, reflex.FindRelation(queryMidl));
                                    Assert.AreEqual(true, reflex.FindRelation(queryDefault));
                                    return;
                                case 2:
                                case 3:
                                    Assert.AreEqual(true, reflex.FindRelation(queryMdlA, queryMdlB));
                                    Assert.AreEqual(true, reflex.FindRelation(queryDefault));
                                    return;
                                case 4:
                                case 5:
                                    Assert.AreEqual(true, reflex.FindRelation(queryMidl));
                                    Assert.AreEqual(true, reflex.FindRelation(queryDefltA, queryDefltB));
                                    return;
                                case 6:
                                case 7:
                                    Assert.AreEqual(true, reflex.FindRelation(queryMdlA, queryMdlB));
                                    Assert.AreEqual(true, reflex.FindRelation(queryDefltA, queryDefltB));
                                    return;
                            }
                        }

                        void CheckReflexState(Processor[] prcs, int start)
                        {
                            switch (start)
                            {
                                case 0:
                                    NegativeTests(reflexProcs);
                                    break;
                                case 1:
                                    NegativeTests(prcs);
                                    break;
                            }
                        }

                        if (reflex == null)
                            throw new ArgumentNullException();

                        void TestFunc(int start)
                        {
                            for (int s = 0; s < 4; s++, start++)
                            {

                                if (start < 0 || start > 3)
                                    start = 0;

                                if (start == 0 || start == 1)
                                    NegativeTests(reflexProcs);

                                Assert.AreEqual(true, reflex.FindRelation((ProcMain(j), "A")));

                                Processor[] prcsA = { procMaxA, processorB };

                                if (start == 0 || start == 1)
                                    NegativeTests(prcsA);

                                CheckReflexValue(reflex, prcsA, 1, 1);

                                if (start == 0 || start == 1)
                                    NegativeTests(prcsA);

                                if (start == 0 || start == 3)
                                    ResetReflex();

                                CheckReflexState(prcsA, start);

                                Assert.AreEqual(start == 0 || start == 3, reflex.FindRelation((ProcMain(j), "B")));

                                Processor[] prcsB = start == 1 || start == 2 ? prcsA : new[] { processorA, procMaxB };

                                if (start == 0 || start == 1)
                                    NegativeTests(prcsB);

                                CheckReflexValue(reflex, prcsB, 1, 1);

                                if (start == 0 || start == 1)
                                    NegativeTests(prcsB);

                                ResetReflex();

                                CheckReflexState(reflexProcs, start);

                            }
                        }

                        for (int z = 0; z < 4; z++)
                            TestFunc(z);

                        for (int z = 3; z >= 0; z--)
                            TestFunc(z);

                        for (int z = 0; z < 4; z++)
                        {
                            TestFunc(z);
                            TestFunc(z);
                        }

                        for (int z = 0; z < 4; z++)
                        {
                            TestFunc(z);
                            TestFunc(3 - z);
                        }

                        for (int z = 0; z < 4; z++)
                        {
                            TestFunc(3 - z);
                            TestFunc(z);
                        }

                        for (int z = 0; z < 4; z++)
                        {
                            TestFunc(3 - z);
                            TestFunc(3 - z);
                        }

                        for (int z = 0; z < 2; z++)
                        {
                            void T1()
                            {
                                TestFunc(0);
                                TestFunc(0);

                                TestFunc(0);
                                TestFunc(1);

                                TestFunc(0);
                                TestFunc(2);

                                TestFunc(0);
                                TestFunc(3);
                            }

                            void T2()
                            {
                                TestFunc(1);
                                TestFunc(0);

                                TestFunc(1);
                                TestFunc(1);

                                TestFunc(1);
                                TestFunc(2);

                                TestFunc(1);
                                TestFunc(3);
                            }

                            void T3()
                            {
                                TestFunc(2);
                                TestFunc(0);

                                TestFunc(2);
                                TestFunc(1);

                                TestFunc(2);
                                TestFunc(2);

                                TestFunc(2);
                                TestFunc(3);
                            }

                            void T4()
                            {
                                TestFunc(3);
                                TestFunc(0);

                                TestFunc(3);
                                TestFunc(1);

                                TestFunc(3);
                                TestFunc(2);

                                TestFunc(3);
                                TestFunc(3);
                            }

                            T1();
                            T2();
                            T3();
                            T4();

                            T4();
                            T1();
                            T2();
                            T3();

                            T3();
                            T4();
                            T1();
                            T2();

                            T2();
                            T3();
                            T4();
                            T1();
                        }

                    }
                }
            }
        }

        [TestMethod]
        public void ReflexTestChangePC()
        {
            SetMinMaxPoolThreads();

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

            CheckReflexState();

            for (int t = 0; t < 4; t++)
                for (int k = 0, kMax = 10; k < kMax; k++)
                {

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((pC, "C")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(mapC, "C"), "C")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((pD, "D")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(mapD, "D"), "D")));

                    pc.Add(k == 0 && t == 0 ? pC : new Processor(mapC, $"C{t * kMax + k}"));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((pC, "C")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(mapC, "C"), "C")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(mapD, "D"), "D")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((pD, "D")));

                    pc.Add(k == 0 && t == 0 ? pD : new Processor(mapD, $"D{t * kMax + k}"));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(mapC, "C"), "C")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((pC, "C")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((pD, "D")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((new Processor(mapD, "D"), "D")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((null, "D")));

                    if (t == 0)
                        CheckReflexState();

                    ZeroTest(k, t);

                    if (t == 0)
                        CheckReflexState();

                }

            CheckReflexState();
            return;

            void ZeroTest(int k, int t)
            {
                void NullTest(Processor p)
                {
                    Assert.AreEqual(false, reflex.FindRelation((p, null)));

                    if (t == 0)
                        CheckReflexState();

                    ResetReflex(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((p, string.Empty)));

                    if (t == 0)
                        CheckReflexState();

                    ResetReflex(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((p, " ")));

                    if (t == 0)
                        CheckReflexState();

                    ResetReflex(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((p, "  ")));

                    if (t == 0)
                        CheckReflexState();

                    ResetReflex(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(false, reflex.FindRelation((p, "\t")));

                    if (t == 0)
                        CheckReflexState();

                    ResetReflex(k, t);

                    if (t == 0)
                        CheckReflexState();

                    Assert.AreEqual(true, ExceptionFunc(() => reflex.FindRelation()));

                    if (t == 0)
                        CheckReflexState();

                    ResetReflex(k, t);

                    if (t == 0)
                        CheckReflexState();

                    ArgumentExceptionTest();

                    if (t == 0)
                        CheckReflexState();

                    ResetReflex(k, t);

                    if (t == 0)
                        CheckReflexState();
                }

                NullTest(pA);
                NullTest(pB);
                NullTest(pC);
                NullTest(pD);
                NullTest(new Processor(mapA, "A"));
                NullTest(new Processor(mapB, "B"));
                NullTest(new Processor(mapC, "C"));
                NullTest(new Processor(mapD, "D"));
            }

            bool ExceptionFunc(Action act)
            {
                try
                {
                    act();
                }
                catch (ArgumentException)
                {
                    return true;
                }

                return false;
            }

            void ResetReflex(int k, int t)
            {
                SignValue[,] m5 = new SignValue[2, 1];
                m5[0, 0] = new SignValue(3);
                m5[1, 0] = new SignValue(5);

                for (int j = 0; j < 2; j++)
                {

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(m5, "m5"), "AB")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(true,
                        k % 2 == 0
                            ? reflex.FindRelation((new Processor(mapA, "A"), "A"), (new Processor(mapB, "B"), "B"))
                            : reflex.FindRelation((pA, "A"), (pB, "B")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(true,
                        k % 2 == 0
                            ? reflex.FindRelation((new Processor(mapA, "A"), "A"), (new Processor(mapB, "B"), "B"))
                            : reflex.FindRelation((pA, "A"), (pB, "B")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(true,
                        k % 2 == 0
                            ? reflex.FindRelation((pA, "A"), (pB, "B"))
                            : reflex.FindRelation((new Processor(mapA, "A"), "A"), (new Processor(mapB, "B"), "B")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(true,
                        k % 2 == 0
                            ? reflex.FindRelation((pA, "A"), (pB, "B"))
                            : reflex.FindRelation((new Processor(mapA, "A"), "A"), (new Processor(mapB, "B"), "B")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(false,
                        k % 2 == 0
                            ? reflex.FindRelation((new Processor(mapA, "A"), "C"), (new Processor(mapB, "B"), "D"))
                            : reflex.FindRelation((pA, "C"), (pB, "D")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(false,
                        k % 2 == 0
                            ? reflex.FindRelation((new Processor(mapA, "A"), "C"), (new Processor(mapB, "B"), "D"))
                            : reflex.FindRelation((pA, "C"), (pB, "D")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(false,
                        k % 2 == 0
                            ? reflex.FindRelation((pA, "C"), (pB, "D"))
                            : reflex.FindRelation((new Processor(mapA, "A"), "C"), (new Processor(mapB, "B"), "D")));

                    if (k < 5 && t % 2 == 0)
                        CheckReflexState();

                    Assert.AreEqual(false,
                        k % 2 == 0
                            ? reflex.FindRelation((pA, "C"), (pB, "D"))
                            : reflex.FindRelation((new Processor(mapA, "A"), "C"), (new Processor(mapB, "B"), "D")));

                }
            }

            void CheckReflexState() => CheckReflexValue(reflex, new[] { pA, pB }, 1, 1);
        }

        [TestMethod]
        public void ReflexOneMapTest()
        {
            SetMinMaxPoolThreads();

            SignValue[,] sv1 = new SignValue[2, 3];
            sv1[0, 0] = new SignValue(100);
            sv1[1, 0] = SignValue.MinValue;
            sv1[0, 1] = SignValue.MaxValue;
            sv1[1, 1] = sv1[0, 0];
            sv1[0, 2] = SignValue.MaxValue;
            sv1[1, 2] = SignValue.MaxValue;

            SignValue[,] sv2 = new SignValue[2, 3];
            sv2[0, 0] = sv1[0, 0];
            sv2[1, 0] = sv1[0, 0];
            sv2[0, 1] = sv1[0, 0];
            sv2[1, 1] = sv1[0, 0];
            sv2[0, 2] = sv1[0, 0];
            sv2[1, 2] = sv1[0, 0];

            SignValue[] sv3 = new SignValue[1];
            sv3[0] = SignValue.MaxValue;

            SignValue[] sv4 = new SignValue[2];
            sv4[0] = sv1[0, 0];
            sv4[1] = sv1[0, 0];

            SignValue[,] sv5 = new SignValue[1, 2];
            sv5[0, 0] = sv1[0, 0];
            sv5[0, 1] = sv1[0, 0];

            const char cQuery = 'a';

            Processor tpL = null, tpU = null;

            Processor Main(bool createNew, bool isUpper)
            {
                char c = isUpper ? char.ToUpper(cQuery) : char.ToLower(cQuery);

                Processor CreateProcessor() => new Processor(new[] { sv1[0, 0] }, c.ToString());

                if (createNew)
                    return CreateProcessor();

                if (!isUpper)
                    return tpU ?? (tpU = CreateProcessor());

                return tpL ?? (tpL = CreateProcessor());
            }

            Processor p1 = null, p2 = null, p3 = null, p4 = null;

            IEnumerable<Processor> GetProcessorIterations()
            {
                Processor GetIterationResult(int k)
                {
                    Processor CreateProcessor()
                    {
                        switch (k)
                        {
                            case 0:
                                return new Processor(sv1, "sv1");
                            case 1:
                                return new Processor(sv2, "sv2");
                            case 2:
                                return new Processor(sv3, "sv3");
                            case 3:
                                return new Processor(sv4, "sv4");
                            default:
                                return null;
                        }
                    }

                    switch (k)
                    {
                        case 0:
                            return p1 ?? (p1 = CreateProcessor());
                        case 1:
                            return p2 ?? (p2 = CreateProcessor());
                        case 2:
                            return p3 ?? (p3 = CreateProcessor());
                        case 3:
                            return p4 ?? (p4 = CreateProcessor());
                        default:
                            return CreateProcessor();
                    }
                }

                Processor GetProcessorIteration(IList<int> iteration)
                {
                    Processor r = GetIterationResult(iteration[0]++);

                    if (r != null)
                        return r;

                    iteration[0] = 0;

                    return null;
                }

                int[] it1 = new int[1], it2 = new int[1];

                while (true)
                {
                    {
                        Processor pp1 = GetProcessorIteration(it1);

                        if (pp1 == null)
                            break;

                        yield return pp1;
                    }

                    for (Processor pp2; ;)
                        if ((pp2 = GetProcessorIteration(it2)) != null)
                            yield return pp2;
                        else
                            break;

                }
            }

            DynamicReflex rxBuf = null;

            IEnumerable<(DynamicReflex r, char c)> GetReflexIterations()
            {
                (DynamicReflex r, char c)? GetIterationResult(int k)
                {
                    bool isUpper = k % 4 == 0;

                    DynamicReflex GetReflex(bool createNewProcessor, bool createNewReflex)
                    {
                        DynamicReflex CreateReflex() =>
                            new DynamicReflex(new ProcessorContainer(Main(createNewProcessor, isUpper)));

                        return createNewReflex ? rxBuf ?? (rxBuf = CreateReflex()) : CreateReflex();
                    }

                    char ch = isUpper ? char.ToUpper(cQuery) : char.ToLower(cQuery);

                    switch (k)
                    {
                        case 0:
                        case 4:
                            return (GetReflex(false, false), ch);
                        case 1:
                        case 5:
                            return (GetReflex(false, true), ch);
                        case 2:
                        case 6:
                            return (GetReflex(true, false), ch);
                        case 3:
                        case 7:
                            return (GetReflex(true, true), ch);
                        default:
                            return null;
                    }
                }

                (DynamicReflex r, char c)? GetReflexIteration(IList<int> iteration)
                {
                    (DynamicReflex, char)? r = GetIterationResult(iteration[0]++);

                    if (r.HasValue)
                        return r.Value;

                    iteration[0] = 0;

                    return null;
                }

                int[] it1 = new int[1], it2 = new int[1];

                while (true)
                {
                    {
                        (DynamicReflex, char)? r1 = GetReflexIteration(it1);

                        if (!r1.HasValue)
                            break;

                        yield return r1.Value;
                    }

                    for ((DynamicReflex, char)? r2; ;)
                        if ((r2 = GetReflexIteration(it2)).HasValue)
                            yield return r2.Value;
                        else
                            break;

                }
            }

            IEnumerable<Processor> GetResults(IEnumerable<Processor> ps, char c)
            {
                HashSet<int> hs = new HashSet<int>(ps.SelectMany(p =>
                {
                    HashSet<int> lst = new HashSet<int>();

                    for (int y = 0; y < p.Height; y++)
                        for (int x = 0; x < p.Width; x++)
                            lst.Add(p[x, y].Value);

                    return lst;
                }));

                foreach (int i in hs)
                    yield return new Processor(new[] { new SignValue(i) }, c.ToString());
            }

            void SubTestFunc(DynamicReflex r, Processor p, char cQ, int nQ, char cSource)
            {
                IEnumerable<(Processor, string)> GetQueries()
                {
                    for (int k = 0; k < nQ; k++)
                        yield return (p, cQ.ToString());
                }

                ArgumentExceptionTest();

                Assert.AreEqual(true, r.FindRelation(GetQueries().ToArray()));

                ArgumentExceptionTest();

                Processor[] pMas = GetResults(new[] { p }, char.ToUpper(cSource)).ToArray();

                CheckReflexValue(r, pMas, 1, 1);

                ArgumentExceptionTest();

                CheckReflexValue(r, pMas, 1, 1);
            }

            void CharTest(char cQ)
            {
                for (int nQ = 1; nQ < 11; nQ++)
                    foreach ((DynamicReflex r, char c) in GetReflexIterations())
                        foreach (Processor p in GetProcessorIterations())
                            SubTestFunc(r, p, cQ, nQ, c);
            }

            CharTest(char.ToUpper(cQuery));
            CharTest(char.ToLower(cQuery));
        }

        [TestMethod]
        public void ReflexPositionalTest()
        {
            SetMinMaxPoolThreads();

            void ReflexRecognizeTest(DynamicReflex reflex, SignValue[] mapA, SignValue[] mapB, string a, string b)
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
                    SignValue[,] m5 = new SignValue[1, 1];
                    m5[0, 0] = new SignValue(2);

                    SignValue[,] m6 = new SignValue[1, 1];
                    m6[0, 0] = new SignValue(8);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(m5, "m5"), a), (new Processor(m6, "m6"), b)));

                    CheckReflexValue(reflex, new[] { new Processor(m5, a.ToUpper()), new Processor(m6, b.ToUpper()) }, 1, 1);

                    Assert.AreEqual(true, reflex.FindRelation((new Processor(mapA, a), a), (new Processor(mapB, b), b)));

                    CheckReflexValue(reflex, new[] { new Processor(mapA, a.ToUpper()), new Processor(mapB, b.ToUpper()) }, 1, 1);
                }

                ResetReflex();

                Assert.AreEqual(false, reflex.FindRelation((null, $"{a}{b}"), (new Processor(m0, "mm"), string.Empty), (new Processor(m0, "mm"), null), (null, null), (new Processor(m0, "mm"), null), (null, string.Empty)));

                CheckReflexValue(reflex, new[] { new Processor(mapA, a.ToUpper()), new Processor(mapB, b.ToUpper()) }, 1, 1);

                #region Test1

                {

                    #region SubTest_1

                    {

                        Assert.AreEqual(true, reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}")));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_1

                    {

                        Assert.AreEqual(true, reflex.FindRelation((new Processor(m1, "m1"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_1

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m2, "m2"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_1

                    {

                        Assert.AreEqual(true, reflex.FindRelation((new Processor(m3, "m3"), $"{a}{b}")));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_1

                    {

                        Assert.AreEqual(true, reflex.FindRelation((new Processor(m4, "m4"), a)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                }

                #endregion

                #region Test2

                {

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m1, "m1"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m1, "m1"), b), (new Processor(m0, "m0"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), b), (new Processor(m0, "m0"), a),
                                (new Processor(m1, "m1"), b), (new Processor(m0, "m0"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m2, "m2"), b),
                                (new Processor(m2, "m2"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m0, "m0"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m0, "m0"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m2, "m2"), b),
                                (new Processor(m0, "m0"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Processor tp = new Processor(m2, "m2");

                        Assert.AreEqual(true, reflex.FindRelation((tp, a), (tp, b)));

                        CheckReflexValue(reflex, new[] { new Processor(m2, a.ToUpper()), new Processor(m2, b.ToUpper()) }, 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{b}{b}"), (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{b}{b}"), (new Processor(m2, "m2"), b),
                                (new Processor(m2, "m2"), $"{b}{b}"), (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m0, "m0"), $"{b}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{a}"), (new Processor(m2, "m2"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m0, "m0"), $"{a}{a}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m3, "m3"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m0, "m0"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m3, "m3"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m3, "m3"), $"{a}{b}"), (new Processor(m1, "m1"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs(int signA)
                        {
                            yield return new Processor(new[] { new SignValue(signA) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(3), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b),
                                (new Processor(m2, "m2"), a)));

                        CheckReflexValue(reflex, GetProcs(4), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m1, "m1"), b)));

                        CheckReflexValue(reflex, GetProcs(3), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m1, "m1"), b),
                                (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(4), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
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
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
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
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
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
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
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
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $"{a}{b}"),
                                (new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $"{b}{a}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m2, "m2"), a),
                                (new Processor(m3, "m3"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), a), (new Processor(m3, "m3"), $"{b}{a}"),
                                (new Processor(m2, "m2"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m3, "m3"), $"{a}{b}"), (new Processor(m2, "m2"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $"{a}{b}"),
                                (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $"{b}{a}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m2, "m2"), b),
                                (new Processor(m3, "m3"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), $"{b}{a}"),
                                (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m3, "m3"), $"{a}{b}"), (new Processor(m2, "m2"), b)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_2

                    {

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
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
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                        }

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $"{a}{b}")));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m4, "m4"), a),
                                (new Processor(m4, "m4"), a),
                                (new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m4, "m4"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m4, "m4"), a)));

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                }

                #endregion

                #region Test3

                {
                    #region SubTest_3

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m1, "m1"), b),
                                (new Processor(m2, "m2"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_3

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b),
                                (new Processor(m3, "m3"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(3) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_3

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                                (new Processor(m4, "m4"), a)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_3

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m1, "m1"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                }

                #endregion

                #region Test4

                {

                    #region SubTest_4

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m1, "m1"), b),
                                (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_4

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $"{a}{b}"),
                                (new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_4

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a),
                                (new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m1, "m1"), b)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_4

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b),
                                (new Processor(m4, "m4"), a), (new Processor(m0, "m0"), $"{a}{b}")));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                    #region SubTest_4

                    {

                        Assert.AreEqual(true,
                            reflex.FindRelation((new Processor(m1, "m1"), b), (new Processor(m2, "m2"), b),
                                (new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a)));

                        IEnumerable<Processor> GetProcs()
                        {
                            yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                            yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                            yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                        }

                        CheckReflexValue(reflex, GetProcs(), 1, 1);

                        ResetReflex();

                    }

                    #endregion

                }

                #endregion

                #region Test5

                {

                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(m0, "m0"), $"{a}{b}"), (new Processor(m1, "m1"), b),
                            (new Processor(m2, "m2"), b), (new Processor(m3, "m3"), b), (new Processor(m4, "m4"), a)));

                    IEnumerable<Processor> GetProcs()
                    {
                        yield return new Processor(new[] { new SignValue(2) }, a.ToUpper());
                        yield return new Processor(new[] { new SignValue(4) }, a.ToUpper());
                        yield return new Processor(new[] { new SignValue(1) }, a.ToUpper());
                        yield return new Processor(new[] { new SignValue(4) }, b.ToUpper());
                        yield return new Processor(new[] { new SignValue(5) }, b.ToUpper());
                        yield return new Processor(new[] { new SignValue(10) }, b.ToUpper());
                        yield return new Processor(new[] { new SignValue(11) }, b.ToUpper());
                        yield return new Processor(new[] { new SignValue(13) }, b.ToUpper());
                    }

                    CheckReflexValue(reflex, GetProcs(), 1, 1);

                    ResetReflex();

                }

                #endregion
            }

            void Scenario(string a, string b)
            {
                SignValue[] mapA = { new SignValue(3) };
                SignValue[] mapB = { new SignValue(5) };

                DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(new Processor(mapA, a), new Processor(mapB, b)));

                void ResetReflex()
                {
                    Assert.AreEqual(true,
                        reflex.FindRelation((new Processor(new[] { new SignValue(4), new SignValue(4) }, "reset"), $"{a}{b}")));

                    CheckReflexValue(reflex, new[] { new Processor(new[] { new SignValue(4) }, a.ToUpper()), new Processor(new[] { new SignValue(4) }, b.ToUpper()) }, 1, 1);
                }

                for (int k = 0; k < 4; k++)
                {
                    ReflexRecognizeTest(reflex, mapA, mapB, a, b);

                    ResetReflex();

                    ReflexRecognizeTest(reflex, mapA, mapB, b, a);

                    ResetReflex();
                }
            }

            Scenario("A", "B");
            Scenario("a", "b");
            Scenario("A", "b");
            Scenario("a", "B");
        }

        [TestMethod]
        public void ReflexExceptionTest()
        {
            SetMinMaxPoolThreads();

            int Iteration(int amount, bool test = false)
            {
                Assert.AreNotEqual(0, amount / 2);

                if (test)
                {
                    try
                    {
                        InitValues(new SignValue[amount * 2, amount]);
                    }
                    catch
                    {
                        return 1;
                    }

                    return 2;
                }

                Processor CreateProcessor(int xy, string tag) => new Processor(InitValues(new SignValue[xy, xy]), tag);

                Processor pA = CreateProcessor(amount, "main");

                Processor pMain = CreateProcessor(amount / 2, "a");

                DynamicReflex testReflex = new DynamicReflex(new ProcessorContainer(pMain));

                try
                {
                    testReflex.FindRelation((pA, "a"));

                    return 2;
                }
                catch
                {
                    // ignored
                }

                CheckReflexValue(testReflex, new[] { pMain }, amount / 2, amount / 2);

                return 0;
            }

            Assert.AreEqual(2, Iteration(2));

            int pn = 0;

            for (double step = 7110873.0, k = step; k <= 4611686009837453316.0; k += step)
            {
                int n = Convert.ToInt32(Math.Round(Math.Sqrt(k)));

                int it = Iteration(n, true);

                if (it == 2)
                {
                    pn = n;
                    continue;
                }

                Assert.AreEqual(1, it);

                break;
            }

            Assert.AreNotEqual(0, pn);

            Assert.AreEqual(0, Iteration(pn));
        }
    }
}