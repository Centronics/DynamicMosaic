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
            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MinValue }, "13a"), new Processor(new[] { SignValue.MaxValue }, "24b")));
            Reflector reflector = new Reflector(reflex);
            reflector.Add("13a", new Processor(new[] { new SignValue(SignValue.MinValue + 1) }, "t"));
            reflector.Add("24b", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "y"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "12"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "2"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "1"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "r"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3a"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3a"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4b"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4b"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3A"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3A"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4B"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4B"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "1"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "2"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "2"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "r"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3a"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3a"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4b"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4b"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3A"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3A"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4B"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4B"));
        }

        [TestMethod]
        public void ReflectorTest2()
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

            Processor main = new Processor(map, "main");
            Processor procA = new Processor(mapA, "AVF");
            Processor procB = new Processor(mapB, "BMP");

            Reflex reflex = new Reflex(new ProcessorContainer(procA, procB));
            Reflector reflector = new Reflector(reflex);
            reflector.Add("A", procA);
            reflector.Add("B", procB);
            Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            Assert.AreEqual(true, reflector.FindRelation(main, "b"));
            Assert.AreEqual(false, reflector.FindRelation(main, "B"));
            Assert.AreEqual(false, reflector.FindRelation(main, "A"));
            Assert.AreEqual(true, reflector.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflector.FindRelation(main, "AA"));
            Assert.AreEqual(false, reflector.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "AAA"));
            Assert.AreEqual(true, reflector.FindRelation(main, "BBB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "VFMP"));
            Assert.AreEqual(true, reflector.FindRelation(main, "MPVF"));
            Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            Assert.AreEqual(true, reflector.FindRelation(main, "b"));
            Assert.AreEqual(false, reflector.FindRelation(main, "B"));
            Assert.AreEqual(false, reflector.FindRelation(main, "A"));
            Assert.AreEqual(true, reflector.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflector.FindRelation(main, "AA"));
            Assert.AreEqual(false, reflector.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "AAA"));
            Assert.AreEqual(true, reflector.FindRelation(main, "BBB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "VFMP"));
            Assert.AreEqual(true, reflector.FindRelation(main, "MPVF"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExceptionTest()
        {
            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MinValue }, "1"), new Processor(new[] { SignValue.MaxValue }, "2")));
            Reflector reflector = new Reflector(reflex);
            reflector.Add("1", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "t"));
            reflector.Add("2", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "y"));
            reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "1");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExceptionTest1()
        {
            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MinValue }, "1"), new Processor(new[] { SignValue.MaxValue }, "2")));
            Reflector reflector = new Reflector(reflex);
            reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "1");
        }

        [TestMethod]
        public void ReflectorContainsTest()
        {
            /*Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MinValue }, "p"),
                new Processor(new[] { SignValue.MaxValue }, "z"))));
            Assert.AreEqual(false, reflector.Contains("a"));
            Assert.AreEqual(false, reflector.Contains("A"));
            Assert.AreEqual(false, reflector.Contains("b"));
            Assert.AreEqual(false, reflector.Contains("B"));
            Assert.AreEqual(false, reflector.Contains("p"));
            Assert.AreEqual(false, reflector.Contains("z"));
            Assert.AreEqual(false, reflector.Contains("t"));
            Assert.AreEqual(false, reflector.Contains("y"));
            Assert.AreEqual(false, reflector.Contains("ab"));
            Assert.AreEqual(false, reflector.Contains("ba"));
            Assert.AreEqual(false, reflector.Contains("az"));
            reflector.Add("a", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "t"));
            Assert.AreEqual(true, reflector.Contains("a"));
            Assert.AreEqual(true, reflector.Contains("A"));
            Assert.AreEqual(false, reflector.Contains("b"));
            Assert.AreEqual(false, reflector.Contains("B"));
            Assert.AreEqual(false, reflector.Contains("p"));
            Assert.AreEqual(false, reflector.Contains("z"));
            Assert.AreEqual(false, reflector.Contains("t"));
            Assert.AreEqual(false, reflector.Contains("y"));
            Assert.AreEqual(false, reflector.Contains("ab"));
            Assert.AreEqual(false, reflector.Contains("ba"));
            Assert.AreEqual(false, reflector.Contains("az"));
            reflector.Add("b", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "y"));
            Assert.AreEqual(true, reflector.Contains("a"));
            Assert.AreEqual(true, reflector.Contains("A"));
            Assert.AreEqual(true, reflector.Contains("b"));
            Assert.AreEqual(true, reflector.Contains("B"));
            Assert.AreEqual(false, reflector.Contains("p"));
            Assert.AreEqual(false, reflector.Contains("z"));
            Assert.AreEqual(false, reflector.Contains("t"));
            Assert.AreEqual(false, reflector.Contains("y"));
            Assert.AreEqual(false, reflector.Contains("ab"));
            Assert.AreEqual(false, reflector.Contains("ba"));
            Assert.AreEqual(false, reflector.Contains("az"));*/
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReflectorArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflector(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectorArgumentException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Reflector(new Reflex(new ProcessorContainer(new Processor(new SignValue[1], "1"))));
        }
    }
}