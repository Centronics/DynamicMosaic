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

        /// <summary>
        /// Предназначен для того, чтобы получать все возможные комбинации заданного набора данных.
        /// </summary>
        static class Counter
        {
            /// <summary>
            ///     Возвращает все возможные варианты запросов для распознавания какой-либо карты.
            /// </summary>
            /// <param name="processors">Массив карт для чтения первых символов их названий, остальные символы игнорируются.</param>
            /// <returns>Возвращает все возможные варианты запросов для распознавания какой-либо карты.</returns>
            public static IEnumerable<(Processor, string)[,]> GetMatrixes((Processor, string)[,][] processors)
            {
                if (processors == null)
                    throw new ArgumentNullException(nameof(processors),
                        $"{nameof(GetMatrixes)}: Массив карт равен null.");
                int mx = processors.GetLength(0), my = processors.GetLength(1);
                if (mx <= 0)
                    throw new ArgumentException($"{nameof(GetMatrixes)}: Массив карт пустой (ось X).",
                        nameof(processors));
                if (my <= 0)
                    throw new ArgumentException($"{nameof(GetMatrixes)}: Массив карт пустой (ось Y).",
                        nameof(processors));
                int[] count = new int[processors.Length];
                do
                {
                    (Processor, string)[,] ch = new (Processor, string)[mx, my];
                    for (int y = 0, cy = my - 1; y < my; y++, cy--)
                        for (int x = 0, cx = mx - 1; x < mx; x++, cx--)
                            ch[x, y] = processors[x, y][count[cy * mx + cx]];

                    yield return ch;
                } while (ChangeCount(count, processors));
            }

            /// <summary>
            ///     Увеличивает значение младших разрядов счётчика, если это возможно.
            ///     Если увеличение было произведено, возвращается значение <see langword="true" />, в противном случае -
            ///     <see langword="false" />.
            /// </summary>
            /// <param name="count">Массив-счётчик.</param>
            /// <param name="processors">
            ///     Требуется для уточнения количества карт в каждом элементе массива, и определения предела
            ///     увеличения значения каждого разряда массива-счётчика.
            /// </param>
            /// <returns>
            ///     Если увеличение было произведено, возвращается значение <see langword="true" />, в противном случае -
            ///     <see langword="false" />.
            /// </returns>
            static bool ChangeCount(int[] count, (Processor, string)[,][] processors)
            {
                if (count == null)
                    throw new ArgumentNullException(nameof(count),
                        $"{nameof(ChangeCount)}: Массив-счётчик равен null.");
                if (count.Length <= 0)
                    throw new ArgumentException(
                        $"{nameof(ChangeCount)}: Длина массива-счётчика некорректна ({count.Length}).", nameof(count));
                if (processors == null)
                    throw new ArgumentNullException(nameof(processors),
                        $"{nameof(ChangeCount)}: Массив карт равен null.");
                int mx = processors.GetLength(0), my = processors.GetLength(1);
                if (mx <= 0)
                    throw new ArgumentException($"{nameof(ChangeCount)}: Массив карт пустой (ось X).",
                        nameof(processors));
                if (my <= 0)
                    throw new ArgumentException($"{nameof(ChangeCount)}: Массив карт пустой (ось Y).",
                        nameof(processors));
                if (count.Length != processors.Length)
                    throw new ArgumentException(
                        $"{nameof(ChangeCount)}: Длина массива-счётчика не соответствует длине массива карт.",
                        nameof(processors));
                for (int k = count.Length - 1; k >= 0; k--)
                {
                    int cx = k % mx, cy = k / mx, ix = mx - (cx + 1), iy = my - (cy + 1);
                    if (count[k] >= processors[ix, iy].Length - 1)
                        continue;
                    count[k]++;
                    for (int x = k + 1; x < count.Length; x++)
                        count[x] = 0;
                    return true;
                }

                return false;
            }
        }

        [TestMethod]
        public void ReflexMultilineTest()
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

            DynamicReflex GetTestReflex()
            {
                DynamicReflex reflex = new DynamicReflex(new ProcessorContainer(Procs().ToArray()));

                CheckReflexValue(reflex, Procs(), 2, 2);

                return reflex;
            }

            void Scenario(Processor p, string q, DynamicReflex reflex, params Processor[] desiredResult)
            {
                void NegativeReflexTest(DynamicReflex negativeReflex)
                {
                    Processor pA1 = new Processor(new[] { SignValue.MaxValue }, "A1");
                    Processor pB1 = new Processor(new[] { SignValue.MaxValue, SignValue.MaxValue }, "B1");
                    Processor pC1 = new Processor(new[] { SignValue.MaxValue, SignValue.MaxValue, SignValue.MaxValue }, "C1");

                    SignValue[,] svA2 = new SignValue[1, 1];
                    svA2[0, 0] = SignValue.MaxValue;

                    SignValue[,] svB2 = new SignValue[1, 2];
                    svB2[0, 0] = SignValue.MaxValue;
                    svB2[0, 1] = SignValue.MaxValue;

                    SignValue[,] svC2 = new SignValue[1, 3];
                    svC2[0, 0] = SignValue.MaxValue;
                    svC2[0, 1] = SignValue.MaxValue;
                    svC2[0, 2] = SignValue.MaxValue;

                    Processor pA2 = new Processor(svA2, "A2");
                    Processor pB2 = new Processor(svB2, "B2");
                    Processor pC2 = new Processor(svC2, "C2");

                    (Processor, string)[] queries =
                    {
                        (pA1, string.Empty), (pA1, null), (pA1, "A"), (pA1, "B"), (pA1, "C"), (pA1, "D"), (pA1, "E"), (pA1, "F"),
                        (pB1, string.Empty), (pB1, null), (pB1, "A"), (pB1, "B"), (pB1, "C"), (pB1, "D"), (pB1, "E"), (pB1, "F"),
                        (pC1, string.Empty), (pC1, null), (pC1, "A"), (pC1, "B"), (pC1, "C"), (pC1, "D"), (pC1, "E"), (pC1, "F"),
                        (pA2, string.Empty), (pA2, null), (pA2, "A"), (pA2, "B"), (pA2, "C"), (pA2, "D"), (pA2, "E"), (pA2, "F"),
                        (pB2, string.Empty), (pB2, null), (pB2, "A"), (pB2, "B"), (pB2, "C"), (pB2, "D"), (pB2, "E"), (pB2, "F"),
                        (pC2, string.Empty), (pC2, null), (pC2, "A"), (pC2, "B"), (pC2, "C"), (pC2, "D"), (pC2, "E"), (pC2, "F"),
                        (null, string.Empty), (null, null), (null, "A"), (null, "B"), (null, "C"), (null, "D"), (null, "E"), (null, "F")
                    };

                    for (int k = 1, mk = queries.Length * 2; k <= mk; k++)
                    {
                        (Processor, string)[,][] processors = new (Processor, string)[k, 1][];
                        for (int j = 0; j < k; j++)
                            processors[j, 0] = ((Processor, string)[])queries.Clone();
                        foreach ((Processor, string)[,] mas in Counter.GetMatrixes(processors))
                        {
                            (Processor, string)[] query = new (Processor, string)[k];
                            for (int i = 0; i < k; i++)
                                query[i] = mas[i, 0];
                            Assert.AreEqual(false, negativeReflex.FindRelation(query));
                        }
                    }
                }

                if (reflex == null)
                    reflex = GetTestReflex();
                else
                    CheckReflexValue(reflex, Procs(), 2, 2);

                NegativeReflexTest(reflex);

                CheckReflexValue(reflex, Procs(), 2, 2);

                //Assert.AreEqual(desiredResult, reflex.FindRelation((p, q)));

                //проверять содержимое после запроса...

                NegativeReflexTest(reflex);

                //ещё раз проверка
            }

            for (int ok = 0; ok < 2; ok++)
            {
                Processor pMain = null;

                DynamicReflex testReflex = ok == 0 ? GetTestReflex() : null;

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

                    Scenario(Main(), "A", testReflex);
                    Scenario(Main(), "B", testReflex);
                    Scenario(Main(), "C", testReflex);
                    Scenario(Main(), "D", testReflex);
                    Scenario(Main(), "E", testReflex);
                    Scenario(Main(), "W", testReflex);
                    Scenario(Main(), null, testReflex);
                    Scenario(Main(), string.Empty, testReflex);
                    Scenario(null, "W", testReflex);
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

                    /*  ИЛЛЮСТРАЦИЯ
                     
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

                    SignValue[,] checkC = new SignValue[2, 2]; // потом убрать в отдельный метод
                    checkC[0, 0] = SignValue.MaxValue;
                    Processor checkProcC = new Processor(checkC, "C");

                    SignValue[,] checkD = new SignValue[2, 2];
                    checkD[0, 0] = SignValue.MaxValue;
                    Processor checkProcD = new Processor(checkD, "C");

                    /*Scenario(Main(), "A", true);
                    Scenario(Main(), "B", true);
                    Scenario(Main(), "C", checkProcC);
                    Scenario(Main(), "D", checkProcD);
                    Scenario(Main(), "E");
                    Scenario(Main(), "W");
    
                    Scenario(Main(), "AA", true);
                    Scenario(Main(), "AB", true);
                    Scenario(Main(), "BA", true);
                    Scenario(Main(), "AC", true);
                    Scenario(Main(), "CA", true);
                    Scenario(Main(), "AD", true);
                    Scenario(Main(), "DA", true);
                    Scenario(Main(), "AE");
    
                    Scenario(Main(), "BA", true);
                    Scenario(Main(), "BB", true);
                    Scenario(Main(), "BC", true);
                    Scenario(Main(), "BD", true);
                    Scenario(Main(), "BE");
                    Scenario(Main(), "AB", true);
                    Scenario(Main(), "BB", true);
                    Scenario(Main(), "CB", true);
                    Scenario(Main(), "DB", true);
                    Scenario(Main(), "EB");
    
                    Scenario(Main(), "CA", true);
                    Scenario(Main(), "CB", true);
                    Scenario(Main(), "CC", true);
                    Scenario(Main(), "CD", true);
                    Scenario(Main(), "CE");
                    Scenario(Main(), "AC", true);
                    Scenario(Main(), "BC", true);
                    Scenario(Main(), "CC", true);
                    Scenario(Main(), "DC", true);
                    Scenario(Main(), "EC");
    
                    Scenario(Main(), "DA", true);
                    Scenario(Main(), "DB", true);
                    Scenario(Main(), "DC", true);
                    Scenario(Main(), "DD", true);
                    Scenario(Main(), "DE");
                    Scenario(Main(), "AD", true);
                    Scenario(Main(), "BD", true);
                    Scenario(Main(), "CD", true);
                    Scenario(Main(), "DD", true);*/
                    Scenario(Main(), "ED", testReflex);

                    Scenario(Main(), "EA", testReflex);
                    Scenario(Main(), "EB", testReflex);
                    Scenario(Main(), "EC", testReflex);
                    Scenario(Main(), "ED", testReflex);
                    Scenario(Main(), "EE", testReflex);
                    Scenario(Main(), "AE", testReflex);
                    Scenario(Main(), "BE", testReflex);
                    Scenario(Main(), "CE", testReflex);
                    Scenario(Main(), "DE", testReflex);
                    Scenario(Main(), "EE", testReflex);
                }
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

        [TestMethod]
        public void ReflexSamePointTest()
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
            //написать тесты, где отдельно присутствуют как 3, так и 5 (в том числе, всместе с m3), так же с недопустимыми запросами
            #region Test1
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