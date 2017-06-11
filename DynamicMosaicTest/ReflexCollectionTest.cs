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

            Assert.AreEqual(5, reflexCollection.CountReflexs);
            Assert.AreEqual(5, reflexCollection.Reflexs.Count());

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
            Assert.AreEqual(5, reflexCollection.CountReflexs);
            Assert.AreEqual(5, reflexCollection.Reflexs.Count());

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
            Assert.AreEqual(null, r1);
            Assert.AreEqual(null, r2);
            Assert.AreNotSame(r1, r2);
            Assert.AreEqual(r1.CountProcessor, r2.CountProcessor);
            Assert.AreEqual(r1.CountProcessorsBase, r2.CountProcessorsBase);
            Assert.AreEqual(0, r1.CountProcessor);
            Assert.AreEqual(5, r1.CountProcessorsBase);
            Assert.AreEqual(true, r1.IsMapsWord("ABCDE"));
            Assert.AreEqual(true, r2.IsMapsWord("ABCDE"));
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
