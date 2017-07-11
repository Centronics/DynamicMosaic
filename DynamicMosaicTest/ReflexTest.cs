using System;
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            Processor main = new Processor(map, "main");

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            for (int k = 0; k < 50; k++)
            {
                Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
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

            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            for (int k = 0; k < 50; k++)
            {
                Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DD"));
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
        public void ReflexTest3()
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            SignValue[,] map = new SignValue[4, 4];
            map[0, 0] = SignValue.MaxValue;
            map[2, 0] = SignValue.MaxValue;
            map[1, 1] = SignValue.MaxValue;
            map[2, 1] = SignValue.MaxValue;
            map[0, 2] = SignValue.MaxValue;
            map[2, 2] = SignValue.MaxValue;
            map[3, 3] = SignValue.MaxValue;

            ForTest3(reflex, map);

            SignValue[,] map1 = new SignValue[4, 4];
            map1[0, 0] = SignValue.MaxValue;
            map1[2, 0] = SignValue.MaxValue;
            map1[1, 1] = SignValue.MaxValue;
            map1[2, 1] = SignValue.MaxValue;
            map1[0, 2] = SignValue.MaxValue;
            map1[2, 2] = SignValue.MaxValue;
            map1[3, 3] = SignValue.MaxValue;

            ForTest3(reflex, map1);
        }

        static void ForTest3(Reflex reflex, SignValue[,] map)
        {
            Processor main = new Processor(map, "main");

            Assert.AreEqual(true, reflex.FindRelation(main, "E"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            for (int k = 0; k < 50; k++)
            {
                Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AE"));

                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "ED"));

                Assert.AreEqual(true, reflex.FindRelation(main, "EA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "EB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "ED"));
                Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
            }
        }

        [TestMethod]
        public void ReflexTest4()
        {
            SignValue[,] map = new SignValue[1, 4];
            map[0, 0] = SignValue.MaxValue;
            map[0, 2] = SignValue.MaxValue;
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

            Assert.AreEqual(false, reflex.FindRelation(main, string.Empty));
        }

        [TestMethod]
        public void ReflexTest5()
        {
            SignValue[,] map = new SignValue[4, 1];
            map[0, 0] = SignValue.MaxValue;
            map[2, 0] = SignValue.MaxValue;
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

            Assert.AreEqual(false, reflex.FindRelation(main, string.Empty));
        }

        [TestMethod]
        public void ReflexTest6()
        {
            SignValue[,] map = new SignValue[1, 1];
            map[0, 0] = SignValue.MaxValue;
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

            Assert.AreEqual(false, reflex.FindRelation(main, string.Empty));
        }

        [TestMethod]
        public void ReflexTest7()
        {
            //ДОПИСАТЬ: ДОЛЖНЫ БЫТЬ РАЗНЫЕ КАРТЫ ДЛЯ ПОИКА РЕЗУЛЬТАТА, отличающиеся в плане СОДЕРЖИМОГО
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

            SignValue[,] map = new SignValue[4, 4];
            map[0, 0] = SignValue.MaxValue;
            map[2, 0] = SignValue.MaxValue;
            map[1, 1] = SignValue.MaxValue;
            map[2, 1] = SignValue.MaxValue;
            map[0, 2] = SignValue.MaxValue;
            map[2, 2] = SignValue.MaxValue;
            map[3, 3] = SignValue.MaxValue;

            ForTest3(reflex, map);

            SignValue[,] map1 = new SignValue[4, 4];
            map1[0, 0] = SignValue.MaxValue;
            map1[2, 0] = SignValue.MaxValue;
            map1[1, 1] = SignValue.MaxValue;
            map1[2, 1] = SignValue.MaxValue;
            map1[0, 2] = SignValue.MaxValue;
            map1[2, 2] = SignValue.MaxValue;
            map1[3, 3] = SignValue.MaxValue;

            ForTest3(reflex, map1);
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
            new Reflex(new ProcessorContainer(new Processor(new SignValue[0], "tag")));
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
            new Reflex(null);
        }
    }
}