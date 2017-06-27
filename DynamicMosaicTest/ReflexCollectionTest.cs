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
    public class ReflexCollectionTest
    {
        [TestMethod]
        public void ReflexCollectionTest1()
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
            Processor procA = new Processor(mapA, "A");
            Processor procB = new Processor(mapB, "B");
            Processor procC = new Processor(mapC, "C");
            Processor procD = new Processor(mapD, "D");
            Processor procE = new Processor(mapE, "E");

            Reflex startReflex = new Reflex(new ProcessorContainer(procA, procB, procC, procD, procE));
            ReflexCollection reflexCollection = new ReflexCollection(startReflex);
            Assert.AreEqual(0, reflexCollection.CountReflexs);
            Assert.AreEqual(0, reflexCollection.Reflexs.Count());
            Assert.AreNotSame(reflexCollection.StartReflex, startReflex);
            ReflexCollection rx = (ReflexCollection)reflexCollection.Clone();
            Assert.AreNotSame(rx, reflexCollection);

            {
                Assert.AreEqual(0, rx.CountReflexs);
                Assert.AreNotSame(rx.StartReflex, startReflex);
                Assert.AreNotSame(rx, reflexCollection);
                Assert.AreEqual(0, rx.Reflexs.Count());
                ReflexCompare(startReflex, rx.StartReflex);
            }

            reflexCollection.AddPair(new[]
            {
                new PairWordValue("A", main), new PairWordValue("B", main),
                new PairWordValue("C", main), new PairWordValue("D", main),
                new PairWordValue("E", main)
            });

            Assert.AreEqual(1, reflexCollection.CountReflexs);
            Assert.AreEqual(1, reflexCollection.Reflexs.Count());

            {
                ReflexCollection rx1 = (ReflexCollection)reflexCollection.Clone();
                Assert.AreNotSame(rx, rx1);
                Assert.AreEqual(0, rx1.CountReflexs);
                Assert.AreNotSame(rx1.StartReflex, startReflex);
                Assert.AreNotSame(rx1, reflexCollection);
                Assert.AreEqual(0, rx1.Reflexs.Count());
                ReflexCompare(startReflex, rx1.StartReflex);
            }

            reflexCollection.Clear();

            Assert.AreEqual(0, reflexCollection.CountReflexs);
            Assert.AreEqual(0, reflexCollection.Reflexs.Count());

            {
                ReflexCollection rx2 = (ReflexCollection)reflexCollection.Clone();
                Assert.AreNotSame(rx, rx2);
                Assert.AreEqual(0, rx2.CountReflexs);
                Assert.AreNotSame(rx2.StartReflex, startReflex);
                Assert.AreNotSame(rx2, reflexCollection);
                Assert.AreEqual(0, rx2.Reflexs.Count());
                ReflexCompare(startReflex, rx2.StartReflex);
            }

            reflexCollection.AddPair(new[]
            {
                new PairWordValue("A", main), new PairWordValue("B", main),
                new PairWordValue("C", main), new PairWordValue("D", main),
                new PairWordValue("E", main)
            });

            Assert.AreEqual(1, reflexCollection.CountReflexs);
            Assert.AreEqual(1, reflexCollection.Reflexs.Count());

            {
                ReflexCollection rx3 = (ReflexCollection)reflexCollection.Clone();
                Assert.AreNotSame(rx, rx3);
                Assert.AreEqual(0, rx3.CountReflexs);
                Assert.AreNotSame(rx3.StartReflex, startReflex);
                Assert.AreNotSame(rx3, reflexCollection);
                Assert.AreEqual(0, rx3.Reflexs.Count());
                ReflexCompare(startReflex, rx3.StartReflex);
            }
        }

        static void ReflexCompare(Reflex r1, Reflex r2)
        {
            Assert.AreNotEqual(null, r1);
            Assert.AreNotEqual(null, r2);
            Assert.AreNotSame(r1, r2);
            Assert.AreEqual(r1.CountProcessors, r2.CountProcessors);
            Assert.AreEqual(r1.CountProcessorsBase, r2.CountProcessorsBase);
            Assert.AreEqual(5, r1.CountProcessors);
            Assert.AreEqual(5, r1.CountProcessorsBase);
            Assert.AreEqual(true, r1.IsMapsWord("ABCDE"));
            Assert.AreEqual(true, r2.IsMapsWord("ABCDE"));
            Assert.AreEqual(true, r1.IsMapsWord("EDCBA"));
            Assert.AreEqual(true, r2.IsMapsWord("EDCBA"));
            Assert.AreEqual(true, r1.IsMapsWord("ADAAABCDBCDE"));
            Assert.AreEqual(true, r2.IsMapsWord("ABCDEADBCE"));
            Assert.AreEqual(false, r1.IsMapsWord("fADAAABCDBCDE"));
            Assert.AreEqual(false, r2.IsMapsWord("ABCDEADBCEf"));

            Assert.AreEqual(true, r1.IsMapsWord("abcde"));
            Assert.AreEqual(true, r2.IsMapsWord("aBcCDE"));
            Assert.AreEqual(true, r1.IsMapsWord("edcba"));
            Assert.AreEqual(true, r2.IsMapsWord("EdCbA"));
            Assert.AreEqual(true, r1.IsMapsWord("ADaAAbcdBCDE"));
            Assert.AreEqual(true, r2.IsMapsWord("ABCdEaDBcE"));
            Assert.AreEqual(false, r1.IsMapsWord("Fadaaabcdbcde"));
            Assert.AreEqual(false, r2.IsMapsWord("abcdedadbceF"));
        }

        [TestMethod]
        public void ReflexCollectionTest2()
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

            /*Processor main = new Processor(map, "main");
            Processor procA = new Processor(mapA, "A1a");
            Processor procB = new Processor(mapB, "B2b");

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procA, procB)));
                reflexCollection.AddPair(new[] { new PairWordValue("A", procA), new PairWordValue("B", procB) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
                ReflexControlA(reflexCollection, main);
                ReflexControlB1(reflexCollection, main);
            }

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procA, procB)));
                reflexCollection.AddPair(new[] { new PairWordValue("B", procB), new PairWordValue("A", procA) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
                ReflexControlA(reflexCollection, main);
                ReflexControlB1(reflexCollection, main);
            }

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procB, procA)));
                reflexCollection.AddPair(new[] { new PairWordValue("A", procA), new PairWordValue("B", procB) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
                ReflexControlA(reflexCollection, main);
                ReflexControlB1(reflexCollection, main);
            }

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procB, procA)));
                reflexCollection.AddPair(new[] { new PairWordValue("B", procB), new PairWordValue("A", procA) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
                ReflexControlA(reflexCollection, main);
                ReflexControlB1(reflexCollection, main);
            }

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procA, procB)));
                reflexCollection.AddPair(new[] { new PairWordValue("A", main), new PairWordValue("B", main) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "B"));
                ReflexControlB(reflexCollection, main);
                ReflexControlA1(reflexCollection, main);
            }

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procA, procB)));
                reflexCollection.AddPair(new[] { new PairWordValue("B", main), new PairWordValue("A", main) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "B"));
                ReflexControlB(reflexCollection, main);
                ReflexControlA1(reflexCollection, main);
            }

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procB, procA)));
                reflexCollection.AddPair(new[] { new PairWordValue("A", main), new PairWordValue("B", main) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "B"));
                ReflexControlB(reflexCollection, main);
                ReflexControlA1(reflexCollection, main);
            }

            {
                ReflexCollection reflexCollection = new ReflexCollection(new Reflex(new ProcessorContainer(procB, procA)));
                reflexCollection.AddPair(new[] { new PairWordValue("B", main), new PairWordValue("A", main) });
                Assert.AreEqual(true, reflexCollection.FindRelation(main, "B"));
                ReflexControlB(reflexCollection, main);
                ReflexControlA1(reflexCollection, main);
            }
        }

        static void ReflexControlA(ReflexCollection reflexCollection, Processor main)
        {
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "a"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "B"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "b"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "Aa"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aA"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "Bb"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "bB"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aa"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "BB"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "bb"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "AABBBBAA"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "abbaba"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "BababBBAA"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "bB"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "c"));

            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1a"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "2B"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "2b"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3c"));

            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1A2B"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "2b1A"));

            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1A2b2b1a"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1Ab22ba1"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1a1c"));
        }

        static void ReflexControlB(ReflexCollection reflexCollection, Processor main)
        {
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "B"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "b"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "a"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "Bb"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "bB"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "Aa"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aA"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "bb"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aa"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "c"));
            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "BababBBAA")); //FindWord - "узкое" место
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "bB"));
            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "AABBBBAA")); //FindWord - "узкое" место
            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "abbaba")); //FindWord - "узкое" место

            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3c"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "2B"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "2b"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1a"));

            Assert.AreEqual(true, reflexCollection.FindRelation(main, "2b1A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A2B"));

            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1a1c"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1Ab22ba1"));
            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A2b2b1a", 1, 2)); //FindWord - "узкое" место
        }

        static void ReflexControlA1(ReflexCollection reflexCollection, Processor main)
        {
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "a"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "B"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "b"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "Aa"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aA"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "Bb"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "bB"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aa"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "bb"));
            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "AABBBBAA")); //FindWord - "узкое" место
            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "abbaba")); //FindWord - "узкое" место
            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "BababBBAA")); //FindWord - "узкое" место
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "bB"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "c"));

            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1a"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "2B"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "2b"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3c"));

            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A2B"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "2b1A"));

            //Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A2b2b1a", 1, 2)); //FindWord - "узкое" место
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1Ab22ba1"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1a1c"));
        }

        static void ReflexControlB1(ReflexCollection reflexCollection, Processor main)
        {
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "B"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "b"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "a"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "Bb"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "bB"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "Aa"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aA"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "BB"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "bb"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "aa"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "c"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "BababBBAA"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "bB"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "AABBBBAA"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "abbaba"));

            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3C"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "3c"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "2B"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "2b"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1A"));
            Assert.AreEqual(true, reflexCollection.FindRelation(main, "1a"));

            Assert.AreEqual(false, reflexCollection.FindRelation(main, "2b1A"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1A2B"));

            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1a1c"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1Ab22ba1"));
            Assert.AreEqual(false, reflexCollection.FindRelation(main, "1A2b2b1a"));*/
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ReflexCollection(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorArgumentException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ReflexCollection(new Reflex(new ProcessorContainer(new Processor(new SignValue[1], "1"))));
        }
    }
}
