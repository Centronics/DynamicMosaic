using System;
using DynamicMosaic;
using DynamicParser;
using DynamicProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor = DynamicParser.Processor;

namespace DynamicMosaicTest
{
    [TestClass]
    public class ReflectorTest
    {
        [TestMethod]
        public void ReflectorTest1()
        {
            Reflex reflex = new Reflex(
                new ProcessorContainer(
                    new Processor(new[] { SignValue.MinValue }, "1a"),
                    new Processor(new[] { SignValue.MaxValue }, "2b")
                    ));
            Reflector reflector = new Reflector(reflex);
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "12"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "z"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "p"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "12"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "z"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "p"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "a"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "1a"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "1A"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "b"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "2b"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "z"), "2B"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "2"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "1"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "b"), "2"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "1"));
        }

        [TestMethod]
        public void ReflectorTest2()
        {
            SignValue[,] map = new SignValue[4, 4];//упростить, рассмотреть все варианты последовательностей запросов
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

            Processor main = new Processor(map, "main");

            Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"))));
            const int number = 1000;

            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "b"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "B"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "A"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "AB"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "BA"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "AA"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "BB"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "AAA"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "BBB"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(false, reflector.FindRelation(main, "VFMP"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(false, reflector.FindRelation(main, "MPVF"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "ab"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "ba"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "aa"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "bA"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "Aa"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "Ba"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "aA"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "bb"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "aaa"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(true, reflector.FindRelation(main, "bbb"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(false, reflector.FindRelation(main, "vfmp"));
            for (int k = 0; k < number; k++)
                Assert.AreEqual(false, reflector.FindRelation(main, "mpvf"));
        }

        [TestMethod]
        public void ReflectorTest3()
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
            mapB[0, 0] = SignValue.MaxValue;
            mapB[0, 1] = SignValue.MaxValue;

            Processor main = new Processor(map, "main");

            Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"))));

            Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            Assert.AreEqual(true, reflector.FindRelation(main, "b"));
            Assert.AreEqual(true, reflector.FindRelation(main, "A"));
            Assert.AreEqual(true, reflector.FindRelation(main, "B"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aa"));
            Assert.AreEqual(true, reflector.FindRelation(main, "bb"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aA"));
            Assert.AreEqual(true, reflector.FindRelation(main, "bB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "bA"));
        }

        [TestMethod]
        public void ReflectorTest4()
        {
            SignValue[,] map = new SignValue[1, 1];//Сделать тесты для разных размеров карт
            map[0, 0] = SignValue.MaxValue - new SignValue(10);
            SignValue[,] mapA = new SignValue[2, 2];
            mapA[0, 0] = SignValue.MaxValue;
            mapA[0, 1] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[2, 2];
            mapB[0, 0] = SignValue.MaxValue;
            mapB[0, 1] = SignValue.MaxValue;

            Processor main = new Processor(map, "main");

            Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"))));

            Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            Assert.AreEqual(true, reflector.FindRelation(main, "b"));
            Assert.AreEqual(true, reflector.FindRelation(main, "A"));
            Assert.AreEqual(true, reflector.FindRelation(main, "B"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aa"));
            Assert.AreEqual(true, reflector.FindRelation(main, "bb"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aA"));
            Assert.AreEqual(true, reflector.FindRelation(main, "bB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "bA"));
        }

        [TestMethod]
        public void ReflectorTest5()
        {
            SignValue[,] map = new SignValue[1, 1];
            map[0, 0] = SignValue.MaxValue - new SignValue(10);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = SignValue.MinValue;

            Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"))));
            Processor main = new Processor(map, "main");
            Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            Assert.AreEqual(false, reflector.FindRelation(main, "b"));
            Assert.AreEqual(true, reflector.FindRelation(main, "A"));
            Assert.AreEqual(false, reflector.FindRelation(main, "B"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aa"));
            Assert.AreEqual(false, reflector.FindRelation(main, "bb"));
            Assert.AreEqual(true, reflector.FindRelation(main, "aA"));
            Assert.AreEqual(true, reflector.FindRelation(main, "bB"));
            Assert.AreEqual(false, reflector.FindRelation(main, "aB"));
            Assert.AreEqual(false, reflector.FindRelation(main, "bA"));
        }

        [TestMethod]
        public void ReflectorTest6()
        {
            SignValue[,] map = new SignValue[2, 2];
            map[0, 0] = new SignValue(8398608);
            map[1, 0] = new SignValue(8398608);
            map[0, 1] = new SignValue(8388607);
            map[1, 1] = new SignValue(8388607);

            SignValue[,] mapA = new SignValue[2, 1];
            mapA[0, 0] = SignValue.MaxValue;
            mapA[1, 0] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[2, 1];
            mapB[0, 0] = SignValue.MinValue;
            mapB[1, 0] = SignValue.MinValue;

            //Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"))));
            Processor main = new Processor(map, "main");

            SearchResults sr = main.GetEqual(new Processor(mapA, "A"), new Processor(mapB, "B"));
            Assert.AreEqual(true, sr.FindRelation("ab"));

            map[0, 0] = new SignValue(8388608);
            map[1, 0] = new SignValue(8388608);
            map[0, 1] = new SignValue(8398607);
            map[1, 1] = new SignValue(8398607);

            SignValue[,] mapC = new SignValue[2, 1];
            mapC[0, 0] = new SignValue(8398608);
            mapC[1, 0] = new SignValue(8398608);
            SignValue[,] mapD = new SignValue[2, 1];
            mapD[0, 0] = new SignValue(8388607);
            mapD[1, 0] = new SignValue(8388607);

            sr = new Processor(map, "main").GetEqual(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "A1"), new Processor(mapD, "B1"));
            Assert.AreEqual(true, sr.FindRelation("ab"));

            //Processor main1 = new Processor(map1, "main");
            //Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            //Assert.AreEqual(true, reflector.FindRelation(main1, "a"));

            //Assert.AreEqual(false, reflector.FindRelation(main, "b"));
            //Assert.AreEqual(true, reflector.FindRelation(main, "A"));
            //Assert.AreEqual(false, reflector.FindRelation(main, "B"));
            //Assert.AreEqual(true, reflector.FindRelation(main, "aa"));
            //Assert.AreEqual(false, reflector.FindRelation(main, "bb"));
            //Assert.AreEqual(true, reflector.FindRelation(main, "aA"));
            //Assert.AreEqual(true, reflector.FindRelation(main, "bB"));
            //Assert.AreEqual(false, reflector.FindRelation(main, "aB"));
            //Assert.AreEqual(false, reflector.FindRelation(main, "bA"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectorTest7()
        {
            Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(new SignValue[2, 2], "A"))));
            reflector.FindRelation(new Processor(new SignValue[1, 1], "main"), "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflector(null);
        }
    }
}