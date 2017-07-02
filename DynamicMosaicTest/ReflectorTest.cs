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
            SignValue[,] map = new SignValue[4, 4];//упростить
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
            mapA[0, 0] = SignValue.MaxValue;
            mapA[0, 1] = SignValue.MaxValue;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflector(null);
        }
    }
}