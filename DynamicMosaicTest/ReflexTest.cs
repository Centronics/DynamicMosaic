using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicMosaic;
using DynamicParser;
using DynamicProcessor;
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

            {
                SignValue[,] minmap = new SignValue[1, 1];
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

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            for (int k = 0; k < 50; k++)
            {
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AE"));

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

                Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ED"));

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

            {
                SignValue[,] map = new SignValue[4, 4];
                map[0, 0] = SignValue.MaxValue;
                map[2, 0] = SignValue.MaxValue;
                map[1, 1] = SignValue.MaxValue;
                map[2, 1] = SignValue.MaxValue;
                map[0, 2] = SignValue.MaxValue;
                map[2, 2] = SignValue.MaxValue;
                map[3, 3] = SignValue.MaxValue;

                ForTest3(reflex, map);
            }

            {
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
        }

        static void ForTest3(Reflex reflex, SignValue[,] map)
        {
            Processor main = new Processor(map, "main");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            for (int k = 0; k < 50; k++)
            {
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));

                Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
            }
        }

        [TestMethod]
        public void ReflexTest4()
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

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                for (int k = 0; k < 50; k++)
                {
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "BC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "BC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "EC"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "EC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                }
            }

            {
                SignValue[,] map1 = new SignValue[4, 4];
                map1[0, 0] = SignValue.MaxValue;
                map1[1, 0] = SignValue.MaxValue;
                map1[0, 1] = SignValue.MaxValue;
                map1[1, 1] = SignValue.MaxValue;
                map1[2, 1] = SignValue.MaxValue;
                map1[3, 1] = SignValue.MaxValue;
                map1[3, 2] = SignValue.MaxValue;
                map1[0, 3] = SignValue.MaxValue;
                map1[1, 3] = SignValue.MaxValue;

                Processor main = new Processor(map1, "main");

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                for (int k = 0; k < 50; k++)
                {
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EC"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                }
            }
        }

        [TestMethod]
        public void ReflexTest4_1()
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

            {
                SignValue[,] map1 = new SignValue[4, 4];
                map1[0, 0] = SignValue.MaxValue;
                map1[1, 0] = SignValue.MaxValue;
                map1[0, 1] = SignValue.MaxValue;
                map1[1, 1] = SignValue.MaxValue;
                map1[2, 1] = SignValue.MaxValue;
                map1[3, 1] = SignValue.MaxValue;
                map1[3, 2] = SignValue.MaxValue;
                map1[0, 3] = SignValue.MaxValue;
                map1[1, 3] = SignValue.MaxValue;

                Processor main = new Processor(map1, "main");

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                for (int k = 0; k < 50; k++)
                {
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EC"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ED"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                }
            }

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

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
                Assert.AreEqual(null, reflex.FindRelation(main, "D"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                for (int k = 0; k < 50; k++)
                {
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "BD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EC"));

                    Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DB"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "BD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "ED"));

                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EA"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EB"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EC"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "ED"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(null, reflex.FindRelation(main, "DE"));
                    Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                }
            }
        }

        [TestMethod]
        public void ReflexTest5()
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

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

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

        static bool ProcessorCompare(Processor pOne, Processor pTwo)
        {
            if ((object)pOne == pTwo && pOne is null)
                return true;
            if (pOne is null || pTwo is null)
                return false;
            if ((object)pOne == pTwo)
                return true;
            if (pOne.Length != pTwo.Length)
                return false;
            for (int y = 0; y < pOne.Height; y++)
                for (int x = 0; x < pOne.Width; x++)
                    if (pOne[x, y] != pTwo[x, y])
                        return false;
            return true;
        }

        [TestMethod]
        public void ReflexMultiThreadTest()
        {
            SignValue[,] minmap1 = new SignValue[1, 1];
            minmap1[0, 0] = new SignValue(1000);
            Processor p1 = new Processor(minmap1, "minmap1");

            SignValue[,] minmap6 = new SignValue[1, 1];
            minmap6[0, 0] = new SignValue(600);
            Processor p6 = new Processor(minmap6, "minmap6");

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(3000);
            Processor pA = new Processor(mapA, "A");

            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(1500);
            Processor pB = new Processor(mapB, "B");

            SignValue[,] mapC = new SignValue[1, 1];
            mapC[0, 0] = new SignValue(500);
            Processor pC = new Processor(mapC, "C");
            Processor pCClone = new Processor(mapC, "D");

            void MapVerify(Reflex pr, int needCount)
            {
                Assert.AreNotEqual(null, pr);
                Assert.AreEqual(needCount, pr.Count);
                Assert.AreEqual((object)pr[0], pA);
                Assert.AreEqual((object)pr[1], pB);
                Assert.AreEqual((object)pr[2], pC);
                Assert.AreEqual((object)pr[3], pCClone);
            }

            void TestExReflex(Reflex r, params int[] pars)
            {
                Assert.AreNotEqual(null, r);

                int exCount = 0;

                foreach (int k in pars)
                    try
                    {
                        Processor p = r[k];
                    }
                    catch (ArgumentException)
                    {
                        exCount++;
                    }

                Assert.AreEqual(exCount, pars.Length);
            }

            Reflex re = new Reflex(new ProcessorContainer(pA, pB, pC, pCClone));
            const int steps = 200;
            Thread[] thrs = new Thread[steps];

            for (int k = 0; k < steps; k++)
            {
                Thread t = new Thread((object obj) =>
                {
                    for (int k1 = 0; k1 < 300; k1++)
                    {
                        Reflex reflex = (Reflex)obj;
                        Reflex rZ6 = reflex.FindRelation(p6, "D");
                        Assert.AreNotEqual(null, rZ6);
                        MapVerify(reflex, 4);
                        MapVerify(rZ6, 5);
                        Assert.AreNotEqual((object)rZ6[4], p6);
                        Assert.AreEqual(true, ProcessorCompare(rZ6[4], p6));
                        Assert.AreEqual("D0", rZ6[4].Tag);

                        TestExReflex(rZ6, 5, 6, -1, -2);
                        TestExReflex(reflex, 4, 5, -1, -2);

                        Reflex rC = reflex.FindRelation(p1, "C");
                        Assert.AreNotEqual(null, rC);
                        MapVerify(reflex, 4);
                        MapVerify(rC, 5);
                        Assert.AreNotEqual((object)rC[4], p1);
                        Assert.AreEqual(true, ProcessorCompare(rC[4], p1));
                        Assert.AreEqual("C0", rC[4].Tag);

                        TestExReflex(rC, 5, 6, -1, -2);
                        TestExReflex(rZ6, 5, 6, -1, -2);
                        TestExReflex(reflex, 4, 5, -1, -2);

                        Reflex rB = reflex.FindRelation(p1, "B");
                        Assert.AreNotEqual(null, rB);
                        MapVerify(reflex, 4);
                        MapVerify(rB, 5);
                        Assert.AreNotEqual((object)rB[4], p1);
                        Assert.AreEqual(true, ProcessorCompare(rB[4], p1));
                        Assert.AreEqual("B0", rB[4].Tag);

                        TestExReflex(rB, 5, 6, -1, -2);
                        TestExReflex(reflex, 4, 5, -1, -2);
                        TestExReflex(rC, 5, 6, -1, -2);
                        TestExReflex(rZ6, 5, 6, -1, -2);
                    }
                })
                { IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = $"Number: {k}" };
                t.Start(re);
                thrs[k] = t;
            }

            for (int k = 0; k < steps; k++)
                thrs[k].Join();
        }

        [TestMethod]
        public void ReflexRelationTest()
        {
            SignValue[,] minmap1 = new SignValue[1, 1];
            minmap1[0, 0] = new SignValue(1000);
            Processor p1 = new Processor(minmap1, "minmap1");

            SignValue[,] minmap12 = new SignValue[1, 1];
            minmap12[0, 0] = new SignValue(1200);
            Processor p12 = new Processor(minmap12, "minmap12");

            SignValue[,] minmap13 = new SignValue[1, 1];
            minmap13[0, 0] = new SignValue(1200);
            Processor p13 = new Processor(minmap13, "minmap13");

            SignValue[,] minmap8 = new SignValue[1, 1];
            minmap8[0, 0] = new SignValue(800);
            Processor p8 = new Processor(minmap8, "minmap8");

            SignValue[,] minmap2 = new SignValue[1, 1];
            minmap2[0, 0] = new SignValue(3000);
            Processor p2 = new Processor(minmap2, "minmap2");

            SignValue[,] minmap6 = new SignValue[1, 1];
            minmap6[0, 0] = new SignValue(600);
            Processor p6 = new Processor(minmap6, "minmap6");

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(3000);
            Processor pA = new Processor(mapA, "A");

            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(1500);
            Processor pB = new Processor(mapB, "B");

            SignValue[,] mapC = new SignValue[1, 1];
            mapC[0, 0] = new SignValue(500);
            Processor pC = new Processor(mapC, "C");
            Processor pCClone = new Processor(mapC, "D");

            void MapVerify(Reflex pr, int needCount)
            {
                Assert.AreNotEqual(null, pr);
                Assert.AreEqual(needCount, pr.Count);
                Assert.AreEqual((object)pr[0], pA);
                Assert.AreEqual((object)pr[1], pB);
                Assert.AreEqual((object)pr[2], pC);
                Assert.AreEqual((object)pr[3], pCClone);
            }

            void TestExReflex(Reflex r, params int[] pars)
            {
                Assert.AreNotEqual(null, r);

                int exCount = 0;

                foreach (int k in pars)
                    try
                    {
                        Processor p = r[k];
                    }
                    catch (ArgumentException)
                    {
                        exCount++;
                    }

                Assert.AreEqual(exCount, pars.Length);
            }

            Reflex reflex = new Reflex(new ProcessorContainer(pA, pB, pC, pCClone));
            for (int k = 0; k < 5; k++)
            {
                {
                    MapVerify(reflex, 4);
                    Assert.AreEqual(null, reflex.FindRelation(p1, "A"));
                    MapVerify(reflex, 4);
                }
                {
                    Reflex rB = reflex.FindRelation(p1, "B");

                    void rBCheck()
                    {
                        Assert.AreNotEqual(null, rB);
                        MapVerify(reflex, 4);
                        MapVerify(rB, 5);
                        Assert.AreNotEqual((object)rB[4], p1);
                        Assert.AreEqual(true, ProcessorCompare(rB[4], p1));
                        Assert.AreEqual("B0", rB[4].Tag);
                    }
                    rBCheck();

                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Assert.AreEqual(null, rB.FindRelation(p8, "C"));

                    MapVerify(rB, 5);
                    Assert.AreNotEqual((object)rB[4], p1);
                    Assert.AreEqual(true, ProcessorCompare(rB[4], p1));

                    Reflex rZ = rB.FindRelation(p12, "B");

                    void rZCheck()
                    {
                        Assert.AreNotEqual(null, rZ);
                        MapVerify(reflex, 4);
                        MapVerify(rZ, 6);
                        Assert.AreEqual((object)rZ[4], rB[4]);
                        Assert.AreNotEqual((object)rZ[5], p12);
                        Assert.AreEqual(true, ProcessorCompare(rZ[5], p12));
                        Assert.AreEqual("B00", rZ[5].Tag);
                    }
                    rBCheck();
                    rZCheck();

                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Reflex rB1 = rB.FindRelation(p8, "B");

                    void rB1Check()
                    {
                        Assert.AreNotEqual(null, rB1);
                        MapVerify(reflex, 4);
                        MapVerify(rB1, 6);
                        MapVerify(rB, 5);
                        Assert.AreEqual((object)rB1[4], rB[4]);
                        Assert.AreNotEqual((object)rB1[5], p8);
                        Assert.AreEqual(true, ProcessorCompare(rB1[5], p8));
                    }
                    rBCheck();
                    rZCheck();
                    rB1Check();

                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(rB1, 6, 7, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Reflex rZ1 = rZ.FindRelation(p8, "B");

                    void rZ1Check()
                    {
                        Assert.AreNotEqual(null, rZ1);
                        MapVerify(reflex, 4);
                        MapVerify(rZ, 6);
                        MapVerify(rZ1, 7);
                        MapVerify(rB, 5);
                        Assert.AreEqual((object)rZ1[4], rZ[4]);
                        Assert.AreEqual((object)rZ1[5], rZ[5]);
                        Assert.AreNotEqual((object)rZ1[6], p8);
                        Assert.AreEqual(true, ProcessorCompare(rZ1[6], p8));
                    }
                    rBCheck();
                    rZCheck();
                    rB1Check();
                    rZ1Check();

                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(rB1, 6, 7, -1, -2);
                    TestExReflex(rZ1, 7, 8, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Assert.AreEqual(null, rB.FindRelation(p8, "C"));
                    MapVerify(reflex, 4);
                    MapVerify(rB1, 6);
                    MapVerify(rB, 5);
                    MapVerify(rZ, 6);
                    MapVerify(rZ1, 7);
                    Assert.AreNotEqual((object)rB[4], p1);
                    Assert.AreEqual(true, ProcessorCompare(rB[4], p1));

                    rBCheck();
                    rZCheck();
                    rB1Check();
                    rZ1Check();

                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(rB1, 6, 7, -1, -2);
                    TestExReflex(rZ1, 7, 8, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Assert.AreEqual(null, rZ.FindRelation(p8, "C"));
                    MapVerify(reflex, 4);
                    MapVerify(rB1, 6);
                    MapVerify(rB, 5);
                    MapVerify(rZ, 6);
                    MapVerify(rZ1, 7);
                    Assert.AreEqual((object)rZ[4], rB[4]);
                    Assert.AreNotEqual((object)rZ[5], p12);
                    Assert.AreEqual(true, ProcessorCompare(rZ[5], p12));

                    rBCheck();
                    rZCheck();
                    rB1Check();
                    rZ1Check();

                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(rB1, 6, 7, -1, -2);
                    TestExReflex(rZ1, 7, 8, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Assert.AreEqual(null, rB1.FindRelation(p8, "C"));
                    MapVerify(reflex, 4);
                    MapVerify(rB1, 6);
                    MapVerify(rB, 5);
                    MapVerify(rZ, 6);
                    MapVerify(rZ1, 7);
                    Assert.AreEqual((object)rB1[4], rB[4]);
                    Assert.AreNotEqual((object)rB1[5], p8);
                    Assert.AreEqual(true, ProcessorCompare(rB1[5], p8));

                    rBCheck();
                    rZCheck();
                    rB1Check();
                    rZ1Check();

                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(rB1, 6, 7, -1, -2);
                    TestExReflex(rZ1, 7, 8, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Assert.AreEqual(null, rZ1.FindRelation(p8, "C"));
                    MapVerify(reflex, 4);
                    MapVerify(rZ, 6);
                    MapVerify(rZ1, 7);
                    MapVerify(rB, 5);
                    Assert.AreEqual((object)rZ1[4], rZ[4]);
                    Assert.AreEqual((object)rZ1[5], rZ[5]);
                    Assert.AreNotEqual((object)rZ1[6], p8);
                    Assert.AreEqual(true, ProcessorCompare(rZ1[6], p8));

                    rBCheck();
                    rZCheck();
                    rB1Check();
                    rZ1Check();

                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rB, 5, 6, -1, -2);
                    TestExReflex(rB1, 6, 7, -1, -2);
                    TestExReflex(rZ1, 7, 8, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    rBCheck();
                    rZCheck();
                    rB1Check();
                    rZ1Check();
                }
                {
                    Reflex rC = reflex.FindRelation(p1, "C");

                    void rCCheck()
                    {
                        Assert.AreNotEqual(null, rC);
                        MapVerify(reflex, 4);
                        MapVerify(rC, 5);
                        Assert.AreNotEqual((object)rC[4], p1);
                        Assert.AreEqual(true, ProcessorCompare(rC[4], p1));
                        Assert.AreEqual("C0", rC[4].Tag);

                        TestExReflex(rC, 5, 6, -1, -2);
                    }
                    rCCheck();

                    Reflex rZ = rC.FindRelation(p12, "C");

                    void rZCheck()
                    {
                        Assert.AreNotEqual(null, rZ);
                        MapVerify(reflex, 4);
                        MapVerify(rZ, 6);
                        Assert.AreEqual((object)rZ[4], rC[4]);
                        Assert.AreNotEqual((object)rZ[5], p12);
                        Assert.AreEqual(true, ProcessorCompare(rZ[5], p12));
                        Assert.AreEqual("C00", rZ[5].Tag);

                        TestExReflex(rZ, 6, 7, -1, -2);
                        TestExReflex(rC, 5, 6, -1, -2);
                    }
                    rZCheck();
                    rCCheck();

                    Reflex rZ1 = rZ.FindRelation(p13, "C");

                    void rZ1Check()
                    {
                        Assert.AreNotEqual(null, rZ1);
                        MapVerify(reflex, 4);
                        MapVerify(rZ1, 6);
                        Assert.AreEqual((object)rZ1[4], rZ[4]);
                        Assert.AreEqual((object)rZ1[5], rZ[5]);

                        TestExReflex(rZ1, 6, 7, -1, -2);
                        TestExReflex(rZ, 6, 7, -1, -2);
                        TestExReflex(rC, 5, 6, -1, -2);
                    }
                    rZ1Check();
                    rZCheck();
                    rCCheck();

                    Reflex rZ2 = rZ1.FindRelation(p8, "C");

                    void rZ2Check()
                    {
                        Assert.AreNotEqual(null, rZ2);
                        MapVerify(reflex, 4);
                        MapVerify(rZ2, 7);
                        MapVerify(rZ1, 6);
                        MapVerify(rZ, 6);
                        Assert.AreEqual((object)rZ1[4], rZ[4]);
                        Assert.AreEqual((object)rZ1[5], rZ[5]);
                        Assert.AreEqual((object) rZ2[4], rZ1[4]);
                        Assert.AreEqual((object) rZ2[5], rZ1[5]);
                        Assert.AreNotEqual((object)rZ2[6], p8);
                        Assert.AreEqual(true, ProcessorCompare(rZ2[6], p8));
                        Assert.AreEqual("C000", rZ2[6].Tag);

                        TestExReflex(rZ2, 7, 8, -1, -2);
                        TestExReflex(rZ1, 6, 7, -1, -2);
                        TestExReflex(rZ, 6, 7, -1, -2);
                        TestExReflex(rC, 5, 6, -1, -2);
                    }
                    rZ2Check();
                    rZ1Check();
                    rZCheck();
                    rCCheck();

                    Assert.AreEqual(null, rZ2.FindRelation(p8, "D"));
                    MapVerify(reflex, 4);
                    MapVerify(rZ2, 7);
                    Assert.AreEqual((object)rZ1[4], rZ[4]);
                    Assert.AreEqual((object)rZ1[5], rZ[5]);
                    Assert.AreNotEqual((object)rZ2[6], p8);
                    Assert.AreEqual(true, ProcessorCompare(rZ2[6], p8));

                    Assert.AreEqual(null, rZ1.FindRelation(p8, "D"));
                    MapVerify(reflex, 4);
                    MapVerify(rZ1, 6);
                    Assert.AreEqual((object)rZ1[4], rZ[4]);
                    Assert.AreNotEqual((object)rZ1[5], p13);
                    Assert.AreEqual(true, ProcessorCompare(rZ1[5], p13));
                    Assert.AreEqual("C00", rZ1[5].Tag);

                    Assert.AreEqual(null, rZ.FindRelation(p8, "D"));
                    MapVerify(reflex, 4);
                    MapVerify(rZ, 6);
                    Assert.AreEqual((object)rZ[4], rC[4]);
                    Assert.AreNotEqual((object)rZ[5], p12);
                    Assert.AreEqual(true, ProcessorCompare(rZ[5], p12));
                    Assert.AreEqual("C00", rZ[5].Tag);

                    Reflex rZ3 = rZ1.FindRelation(p6, "D");
                    Assert.AreNotEqual(null, rZ3);
                    MapVerify(reflex, 4);
                    MapVerify(rZ1, 6);
                    MapVerify(rZ3, 7);
                    Assert.AreEqual((object)rZ1[4], rZ3[4]);
                    Assert.AreEqual((object)rZ1[5], rZ3[5]);
                    Assert.AreNotEqual((object)rZ3[6], p6);
                    Assert.AreEqual(true, ProcessorCompare(rZ3[6], p6));
                    Assert.AreEqual("D0", rZ3[6].Tag);

                    Reflex rZ4 = rZ2.FindRelation(p6, "D");
                    Assert.AreNotEqual(null, rZ4);
                    MapVerify(reflex, 4);
                    MapVerify(rZ2, 7);
                    MapVerify(rZ4, 8);
                    Assert.AreEqual((object)rZ2[4], rZ4[4]);
                    Assert.AreEqual((object)rZ2[5], rZ4[5]);
                    Assert.AreEqual((object)rZ2[6], rZ4[6]);
                    Assert.AreNotEqual((object)rZ4[7], p6);
                    Assert.AreEqual(true, ProcessorCompare(rZ4[7], p6));
                    Assert.AreEqual("D0", rZ4[7].Tag);

                    Reflex rZ5 = rC.FindRelation(p6, "D");
                    Assert.AreNotEqual(null, rZ5);
                    MapVerify(reflex, 4);
                    MapVerify(rC, 5);
                    MapVerify(rZ5, 6);
                    Assert.AreEqual((object)rZ5[4], rC[4]);
                    Assert.AreNotEqual((object)rZ5[5], p6);
                    Assert.AreEqual(true, ProcessorCompare(rZ5[5], p6));
                    Assert.AreEqual("D0", rZ5[5].Tag);

                    Reflex rZ6 = reflex.FindRelation(p6, "D");
                    Assert.AreNotEqual(null, rZ6);
                    MapVerify(reflex, 4);
                    MapVerify(rZ6, 5);
                    Assert.AreNotEqual((object)rZ6[4], p6);
                    Assert.AreEqual(true, ProcessorCompare(rZ6[4], p6));
                    Assert.AreEqual("D0", rZ6[4].Tag);

                    Reflex rZ7 = reflex.FindRelation(p1, "B");
                    Assert.AreNotEqual(null, rZ7);
                    MapVerify(reflex, 4);
                    MapVerify(rZ7, 5);
                    Assert.AreNotEqual((object)rZ7[4], p1);
                    Assert.AreEqual(true, ProcessorCompare(rZ7[4], p1));
                    Assert.AreEqual("B0", rZ7[4].Tag);

                    Assert.AreEqual(null, rC.FindRelation(p1, "B"));
                    MapVerify(reflex, 4);
                    MapVerify(rC, 5);
                    Assert.AreNotEqual((object)rC[4], p1);
                    Assert.AreEqual(true, ProcessorCompare(rC[4], p1));
                    Assert.AreEqual("C0", rC[4].Tag);

                    Reflex rB = reflex.FindRelation(p1, "B");
                    Assert.AreNotEqual(null, rB);
                    MapVerify(reflex, 4);
                    MapVerify(rB, 5);
                    Assert.AreNotEqual((object)rB[4], p1);
                    Assert.AreEqual(true, ProcessorCompare(rB[4], p1));
                    Assert.AreEqual("B0", rB[4].Tag);
                    TestExReflex(rB, 5, 6, -1, -2);

                    Assert.AreEqual(null, rZ2.FindRelation(p1, "B"));
                    MapVerify(reflex, 4);
                    MapVerify(rZ2, 7);
                    Assert.AreNotEqual(null, rZ4);
                    MapVerify(rZ2, 7);
                    MapVerify(rZ4, 8);
                    Assert.AreEqual((object)rZ2[4], rZ4[4]);
                    Assert.AreEqual((object)rZ2[5], rZ4[5]);
                    Assert.AreEqual((object)rZ2[6], rZ4[6]);
                    Assert.AreNotEqual((object)rZ4[7], p6);
                    Assert.AreEqual(true, ProcessorCompare(rZ4[7], p6));
                    Assert.AreEqual("D0", rZ4[7].Tag);

                    rZ2Check();
                    rZ1Check();
                    rZCheck();
                    rCCheck();

                    TestExReflex(rZ2, 7, 8, -1, -2);
                    TestExReflex(rZ1, 6, 7, -1, -2);
                    TestExReflex(rZ, 6, 7, -1, -2);
                    TestExReflex(rC, 5, 6, -1, -2);
                    TestExReflex(rZ3, 7, 8, -1, -2);
                    TestExReflex(rZ4, 8, 9, -1, -2);
                    TestExReflex(rZ5, 6, 7, -1, -2);
                    TestExReflex(rZ6, 5, 6, -1, -2);
                    TestExReflex(rZ7, 5, 6, -1, -2);
                    TestExReflex(reflex, 4, 5, -1, -2);

                    Reflex refl = new Reflex(rZ2);
                    Assert.AreNotEqual(refl, rZ2);
                    Assert.AreEqual(refl.Count, rZ2.Count);
                    for (int j = 0; j < refl.Count; j++)
                        Assert.AreEqual((object)refl[j], rZ2[j]);

                    rZ2Check();
                    rZ1Check();
                    rZCheck();
                    rCCheck();
                }

                {
                    MapVerify(reflex, 4);
                    Reflex rA = reflex.FindRelation(p2, "A");
                    Assert.AreNotEqual(null, rA);
                    MapVerify(reflex, 4);
                    MapVerify(rA, 4);
                    Assert.AreNotEqual(rA[0], p2);
                    Assert.AreEqual(true, ProcessorCompare(rA[0], p2));
                    Assert.AreEqual("A", rA[0].Tag);

                    Assert.AreEqual(null, rA.FindRelation(p2, "B"));
                    MapVerify(reflex, 4);
                    MapVerify(rA, 4);
                    Assert.AreNotEqual(rA[0], p2);
                    Assert.AreEqual(true, ProcessorCompare(rA[0], p2));
                    Assert.AreEqual("A", rA[0].Tag);

                    Assert.AreEqual(null, reflex.FindRelation(p2, "C"));
                    MapVerify(reflex, 4);
                    MapVerify(rA, 4);
                    Assert.AreNotEqual(rA[0], p2);
                    Assert.AreEqual(true, ProcessorCompare(rA[0], p2));
                    Assert.AreEqual("A", rA[0].Tag);

                    TestExReflex(reflex, 4, 5, -1, -2);
                    TestExReflex(rA, 4, 5, -1, -2);
                }
            }
        }

        [TestMethod]
        public void ReflexTest10()
        {
            SignValue[,] minmap = new SignValue[1, 1];
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "AV"), new Processor(mapB, "BW"), new Processor(mapC, "CF"),
                new Processor(mapD, "DP"), new Processor(mapE, "EQ")));

            {
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AV"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CF"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DP"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EQ"));

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AQ"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BP"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CW"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DV"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EF"));
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

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));
            Reflex reflexBefore = reflex;
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
            Assert.AreEqual(reflex.Count, reflexBefore.Count + 1);
            Processor newProcessor = reflex[reflexBefore.Count];
            Assert.AreEqual(true, ProcessorCompare(newProcessor, main));

            Assert.AreEqual(null, reflex.FindRelation(main, "C"));
            Assert.AreEqual(null, reflex.FindRelation(main, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AW"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AV"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BW"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CF"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DP"));
            Assert.AreEqual(null, reflex.FindRelation(main, "EQ"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AF"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BP"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CP"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DV"));
            Assert.AreEqual(null, reflex.FindRelation(main, "EV"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WQ"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

            {
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AV"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CF"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DP"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EQ"));

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AQ"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BP"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CW"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DV"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EF"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "WW"));

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
                Assert.AreEqual(null, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AE"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EF"));
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

                Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(null, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EF"));
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

                Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EF"));
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

                Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ED"));

                Assert.AreEqual(null, reflex.FindRelation(main, "EE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ED"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EF"));
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

                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WA"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WB"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WC"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WD"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(null, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(null, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(null, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(null, reflex.FindRelation(minProcessor, "EF"));
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
        public void ReflexTest12()
        {
            Assert.AreNotEqual(null, GetMapsForTest_12_13(out SignValue[,] map, true).FindRelation(new Processor(map, "main"), "abcde"));
            Assert.AreNotEqual(null, GetMapsForTest_121_131(out map, true).FindRelation(new Processor(map, "main"), "abcde"));
            Assert.AreNotEqual(null, GetMapsForTest_122_132(out map).FindRelation(new Processor(map, "main"), "abcde"));
        }

        [TestMethod]
        public void ReflexTest13()
        {
            Assert.AreNotEqual(null, GetMapsForTest_12_13(out SignValue[,] map, false).FindRelation(new Processor(map, "main"), "ABCDE"));
            Assert.AreNotEqual(null, GetMapsForTest_121_131(out map, false).FindRelation(new Processor(map, "main"), "ABCDE"));
            Assert.AreNotEqual(null, GetMapsForTest_122_132(out map).FindRelation(new Processor(map, "main"), "ABCDE"));
        }

        static Reflex GetMapsForTest_12_13(out SignValue[,] map, bool register)
        {
            map = new SignValue[5, 1];
            map[0, 0] = new SignValue(1000);
            map[1, 0] = new SignValue(3000);
            map[2, 0] = new SignValue(5000);
            map[3, 0] = new SignValue(7000);
            map[4, 0] = new SignValue(9000);

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(1000);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(3000);
            SignValue[,] mapC = new SignValue[1, 1];
            mapC[0, 0] = new SignValue(5000);
            SignValue[,] mapD = new SignValue[1, 1];
            mapD[0, 0] = new SignValue(7000);
            SignValue[,] mapE = new SignValue[1, 1];
            mapE[0, 0] = new SignValue(9000);

            return new Reflex(new ProcessorContainer(new Processor(mapA, register ? "A" : "a"), new Processor(mapB, register ? "B" : "b"),
                new Processor(mapC, register ? "C" : "c"), new Processor(mapD, register ? "D" : "d"), new Processor(mapE, register ? "E" : "e")));
        }

        static Reflex GetMapsForTest_121_131(out SignValue[,] map, bool register)
        {
            map = new SignValue[10, 1];
            map[0, 0] = new SignValue(1000);
            map[1, 0] = new SignValue(3000);
            map[2, 0] = new SignValue(5000);
            map[3, 0] = new SignValue(7000);
            map[4, 0] = new SignValue(9000);
            map[5, 0] = new SignValue(11000);
            map[6, 0] = new SignValue(13000);
            map[7, 0] = new SignValue(15000);
            map[8, 0] = new SignValue(17000);
            map[9, 0] = new SignValue(19000);

            SignValue[,] mapA0 = new SignValue[1, 1];
            mapA0[0, 0] = new SignValue(1000);
            SignValue[,] mapA1 = new SignValue[1, 1];
            mapA1[0, 0] = new SignValue(3000);
            SignValue[,] mapB2 = new SignValue[1, 1];
            mapB2[0, 0] = new SignValue(5000);
            SignValue[,] mapB3 = new SignValue[1, 1];
            mapB3[0, 0] = new SignValue(7000);

            SignValue[,] mapC4 = new SignValue[1, 1];
            mapC4[0, 0] = new SignValue(9000);
            SignValue[,] mapC5 = new SignValue[1, 1];
            mapC5[0, 0] = new SignValue(1000);
            SignValue[,] mapD6 = new SignValue[1, 1];
            mapD6[0, 0] = new SignValue(3000);
            SignValue[,] mapD7 = new SignValue[1, 1];
            mapD7[0, 0] = new SignValue(5000);

            SignValue[,] mapE8 = new SignValue[1, 1];
            mapE8[0, 0] = new SignValue(7000);
            SignValue[,] mapE9 = new SignValue[1, 1];
            mapE9[0, 0] = new SignValue(9000);

            return new Reflex(new ProcessorContainer(
                new Processor(mapA0, register ? "A0" : "a0"), new Processor(mapA1, register ? "A1" : "a1"),
                new Processor(mapB2, register ? "B2" : "b2"), new Processor(mapB3, register ? "B3" : "b3"),
                new Processor(mapC4, register ? "C4" : "c4"), new Processor(mapC5, register ? "C5" : "c5"),
                new Processor(mapD6, register ? "D6" : "d6"), new Processor(mapD7, register ? "D7" : "d7"),
                new Processor(mapE8, register ? "E8" : "e8"), new Processor(mapE9, register ? "E9" : "e9")));
        }

        static Reflex GetMapsForTest_122_132(out SignValue[,] map)
        {
            map = new SignValue[10, 1];
            map[0, 0] = new SignValue(1000);
            map[1, 0] = new SignValue(3000);
            map[2, 0] = new SignValue(5000);
            map[3, 0] = new SignValue(7000);
            map[4, 0] = new SignValue(9000);
            map[5, 0] = new SignValue(11000);
            map[6, 0] = new SignValue(13000);
            map[7, 0] = new SignValue(15000);
            map[8, 0] = new SignValue(17000);
            map[9, 0] = new SignValue(19000);

            SignValue[,] mapA0 = new SignValue[1, 1];
            mapA0[0, 0] = new SignValue(1000);
            SignValue[,] mapA1 = new SignValue[1, 1];
            mapA1[0, 0] = new SignValue(3000);
            SignValue[,] mapB2 = new SignValue[1, 1];
            mapB2[0, 0] = new SignValue(5000);
            SignValue[,] mapB3 = new SignValue[1, 1];
            mapB3[0, 0] = new SignValue(7000);

            SignValue[,] mapC4 = new SignValue[1, 1];
            mapC4[0, 0] = new SignValue(9000);
            SignValue[,] mapC5 = new SignValue[1, 1];
            mapC5[0, 0] = new SignValue(1000);
            SignValue[,] mapD6 = new SignValue[1, 1];
            mapD6[0, 0] = new SignValue(3000);
            SignValue[,] mapD7 = new SignValue[1, 1];
            mapD7[0, 0] = new SignValue(5000);

            SignValue[,] mapE8 = new SignValue[1, 1];
            mapE8[0, 0] = new SignValue(7000);
            SignValue[,] mapE9 = new SignValue[1, 1];
            mapE9[0, 0] = new SignValue(9000);

            return new Reflex(new ProcessorContainer(
                new Processor(mapA0, "a0"), new Processor(mapA1, "A1"),
                new Processor(mapB2, "B2"), new Processor(mapB3, "b3"),
                new Processor(mapC4, "c4"), new Processor(mapC5, "C5"),
                new Processor(mapD6, "D6"), new Processor(mapD7, "d7"),
                new Processor(mapE8, "e8"), new Processor(mapE9, "E9")));
        }

        [TestMethod]
        public void ReflexTest14()
        {
            SignValue[,] map = new SignValue[2, 1];
            map[0, 0] = SignValue.MaxValue;
            map[1, 0] = SignValue.MinValue;

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = SignValue.MinValue;

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A1"), new Processor(mapB, "A2")));

            Processor main = new Processor(map, "main");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));
            Assert.AreEqual(null, reflex.FindRelation(main, "b"));
        }

        [TestMethod]
        public void ReflexTest15()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AC(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_1()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AB1(reflex, main);

            Check_AC(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_2()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AC(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_3()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AB2(reflex, main);

            Check_AC(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_4()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AC(reflex, main);

            Check_AB3(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_5()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AB3(reflex, main);

            Check_AC(reflex, main);

            Check_AB3(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_6()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AC1(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_7()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AB1(reflex, main);

            Check_AC1(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_8()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AC1(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_9()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AB2(reflex, main);

            Check_AC1(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_10()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AC1(reflex, main);

            Check_AB3(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_11()
        {

            GetReflexProcessor(out Reflex reflex, out Processor main);

            Check_AB3(reflex, main);

            Check_AC1(reflex, main);

            Check_AB3(reflex, main);
        }

        static void GetReflexProcessor(out Reflex reflex, out Processor processor)
        {
            SignValue[,] map = new SignValue[2, 1];
            map[0, 0] = SignValue.MaxValue;
            map[1, 0] = SignValue.MinValue;

            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = SignValue.MinValue;

            reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "a"), new Processor(mapB, "cB")));

            processor = new Processor(map, "main");
        }

        static void Check_AC(Reflex reflex, Processor main)
        {
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreEqual(null, reflex.FindRelation(main, "b"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ac"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "aC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "Ac"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "aB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "Ab"));
        }

        static void Check_AC1(Reflex reflex, Processor main)
        {
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreEqual(null, reflex.FindRelation(main, "b"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "aB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "Ab"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ac"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "aC"));
            Assert.AreNotEqual(null, reflex.FindRelation(main, "Ac"));
        }

        static void Check_All(Reflex reflex, Processor main)
        {
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ac"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "Ac"));
            Assert.AreNotEqual(null, reflex.FindRelation(main, "aC"));
        }

        static void Check_AB1(Reflex reflex, Processor main)
        {
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreEqual(null, reflex.FindRelation(main, "b"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));
            Check_All(reflex, main);
            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "Ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "aB"));
            Check_All(reflex, main);
        }

        static void Check_AB2(Reflex reflex, Processor main)
        {
            Check_All(reflex, main);
            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "Ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "aB"));
            Check_All(reflex, main);
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreEqual(null, reflex.FindRelation(main, "b"));
            Check_All(reflex, main);
        }

        static void Check_AB3(Reflex reflex, Processor main)
        {
            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "Ab"));
            Assert.AreEqual(null, reflex.FindRelation(main, "aB"));
            Check_All(reflex, main);
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
            Assert.AreEqual(null, reflex.FindRelation(main, "b"));
            Check_All(reflex, main);
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
        public void CheckMultipleSizes1()
        {
            SignValue[,] bmap = new SignValue[6, 6];
            bmap[0, 0] = new SignValue(1000);
            bmap[2, 0] = new SignValue(2000);
            bmap[1, 1] = new SignValue(3000);
            bmap[2, 1] = new SignValue(4000);
            bmap[0, 2] = new SignValue(5000);
            bmap[2, 2] = new SignValue(6000);
            bmap[1, 0] = new SignValue(7000);
            SignValue[,] map = new SignValue[1, 1];
            map[0, 0] = new SignValue(9000);
            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(1000);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(2000);
            SignValue[,] mapC = new SignValue[1, 1];
            mapC[0, 0] = new SignValue(3000);
            SignValue[,] mapD = new SignValue[1, 1];
            mapD[0, 0] = new SignValue(4000);
            SignValue[,] mapE = new SignValue[1, 1];
            mapE[0, 0] = new SignValue(9000);

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            Processor main = new Processor(map, "main");

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreEqual(null, reflex.FindRelation(main, "C"));
            Assert.AreEqual(null, reflex.FindRelation(main, "D"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AE"));

            Processor main1 = new Processor(bmap, "bigmain");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "W"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "BB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "CC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "DD"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "WW"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "BA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "CA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "AE"));
        }

        [TestMethod]
        public void CheckMultipleSizes2()
        {
            SignValue[,] bmap = new SignValue[6, 6];
            bmap[0, 0] = new SignValue(1000);
            bmap[2, 0] = new SignValue(2000);
            bmap[1, 1] = new SignValue(3000);
            bmap[2, 1] = new SignValue(4000);
            bmap[0, 2] = new SignValue(5000);
            bmap[2, 2] = new SignValue(6000);
            bmap[1, 0] = new SignValue(7000);
            SignValue[,] map = new SignValue[1, 1];
            map[0, 0] = new SignValue(9000);
            SignValue[,] mapA = new SignValue[1, 1];
            mapA[0, 0] = new SignValue(1000);
            SignValue[,] mapB = new SignValue[1, 1];
            mapB[0, 0] = new SignValue(2000);
            SignValue[,] mapC = new SignValue[1, 1];
            mapC[0, 0] = new SignValue(3000);
            SignValue[,] mapD = new SignValue[1, 1];
            mapD[0, 0] = new SignValue(4000);
            SignValue[,] mapE = new SignValue[1, 1];
            mapE[0, 0] = new SignValue(9000);

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"),
                new Processor(mapD, "D"), new Processor(mapE, "E")));

            Processor main1 = new Processor(bmap, "bigmain");

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "W"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "BB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "CC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "DD"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "WW"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "BA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "CA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "AE"));

            Processor main = new Processor(map, "main");

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));
            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
            Assert.AreEqual(null, reflex.FindRelation(main, "C"));
            Assert.AreEqual(null, reflex.FindRelation(main, "D"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
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
        public void CheckMultipleSizes3()
        {
            Reflex reflex = new Reflex(MapsForMultipleSizes);

            {
                Processor main = Map1ForMultipleSizesMain;

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
                Assert.AreEqual(null, reflex.FindRelation(main, "W"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AC"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AD"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DA"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "AE"));
                Assert.AreEqual(null, reflex.FindRelation(main, "AW"));
            }

            Processor main1 = Map2ForMultipleSizesMain;

            Assert.AreEqual(null, reflex.FindRelation(main1, "C"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "W"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "D"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "E"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "B"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "BB"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "CC"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "DD"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "WW"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "AB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "BA"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "AC"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "CA"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "AD"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "AE"));
            Assert.AreEqual(null, reflex.FindRelation(main1, "AW"));
        }

        [TestMethod]
        public void CheckMultipleSizes4()
        {
            Reflex reflex = new Reflex(MapsForMultipleSizes);

            {
                Processor main1 = Map2ForMultipleSizesMain;

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "C"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "W"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "D"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "E"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "A"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "B"));

                Assert.AreEqual(null, reflex.FindRelation(main1, "AA"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "BB"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main1, "CC"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "DD"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "EE"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "WW"));

                Assert.AreEqual(null, reflex.FindRelation(main1, "AB"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "BA"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "AC"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "CA"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "AD"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "DA"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "AE"));
                Assert.AreEqual(null, reflex.FindRelation(main1, "AW"));
            }

            Processor main = Map1ForMultipleSizesMain;

            Assert.AreEqual(null, reflex.FindRelation(main, "A"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "B"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "C"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "D"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "E"));
            Assert.AreEqual(null, reflex.FindRelation(main, "W"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AA"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "BB"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "CC"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "DD"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "EE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(null, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(null, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(null, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(null, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AE"));
            Assert.AreEqual(null, reflex.FindRelation(main, "AW"));
        }

        static Processor Map1ForMultipleSizesMain
        {
            get
            {
                SignValue[,] bmap = new SignValue[5, 7];
                bmap[0, 0] = SignValue.MaxValue;
                bmap[2, 0] = SignValue.MaxValue;
                bmap[1, 1] = SignValue.MaxValue;
                bmap[2, 1] = SignValue.MaxValue;
                bmap[0, 2] = SignValue.MaxValue;
                bmap[2, 2] = SignValue.MaxValue;
                bmap[3, 3] = SignValue.MaxValue;
                bmap[4, 4] = SignValue.MaxValue;
                bmap[4, 6] = SignValue.MaxValue;
                bmap[2, 6] = SignValue.MaxValue;

                return new Processor(bmap, "bigmain");
            }
        }

        static Processor Map2ForMultipleSizesMain
        {
            get
            {
                SignValue[,] mmap = new SignValue[4, 6];
                mmap[1, 1] = SignValue.MaxValue;
                mmap[2, 2] = SignValue.MaxValue;
                mmap[2, 1] = SignValue.MaxValue;
                mmap[1, 2] = SignValue.MaxValue;
                mmap[3, 5] = SignValue.MaxValue;
                mmap[1, 5] = SignValue.MaxValue;
                mmap[3, 4] = SignValue.MaxValue;
                mmap[3, 2] = SignValue.MaxValue;
                mmap[2, 4] = SignValue.MaxValue;

                return new Processor(mmap, "main");
            }
        }

        static ProcessorContainer MapsForMultipleSizes
        {
            get
            {
                SignValue[,] mapA = new SignValue[3, 3];
                mapA[0, 0] = SignValue.MaxValue;
                mapA[1, 0] = SignValue.MaxValue;
                mapA[2, 0] = SignValue.MaxValue;
                mapA[0, 2] = SignValue.MaxValue;
                mapA[2, 1] = SignValue.MaxValue;
                mapA[1, 2] = SignValue.MaxValue;

                SignValue[,] mapB = new SignValue[3, 3];
                mapA[2, 2] = SignValue.MaxValue;
                mapA[1, 0] = SignValue.MaxValue;
                mapA[0, 1] = SignValue.MaxValue;
                mapA[0, 2] = SignValue.MaxValue;
                mapA[2, 1] = SignValue.MaxValue;
                mapA[1, 2] = SignValue.MaxValue;

                SignValue[,] mapC = new SignValue[3, 3];
                mapA[1, 1] = SignValue.MaxValue;
                mapA[1, 0] = SignValue.MaxValue;
                mapA[2, 0] = SignValue.MaxValue;
                mapA[0, 1] = SignValue.MaxValue;
                mapA[2, 1] = SignValue.MaxValue;
                mapA[1, 2] = SignValue.MaxValue;

                SignValue[,] mapD = new SignValue[3, 3];
                mapA[2, 2] = SignValue.MaxValue;
                mapA[1, 1] = SignValue.MaxValue;
                mapA[0, 0] = SignValue.MaxValue;
                mapA[0, 1] = SignValue.MaxValue;
                mapA[1, 0] = SignValue.MaxValue;
                mapA[1, 2] = SignValue.MaxValue;

                SignValue[,] mapE = new SignValue[3, 3];
                mapA[0, 0] = SignValue.MaxValue;
                mapA[1, 0] = SignValue.MaxValue;
                mapA[2, 0] = SignValue.MaxValue;
                mapA[0, 1] = SignValue.MaxValue;
                mapA[1, 1] = SignValue.MaxValue;
                mapA[2, 1] = SignValue.MaxValue;
                mapA[0, 2] = SignValue.MaxValue;

                return new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"), new Processor(mapC, "C"), new Processor(mapD, "D"),
                    new Processor(mapE, "E"));
            }
        }

        [TestMethod]
        public void ReflexTest17()
        {

            GetTest17_18_19_20(out ProcessorContainer pc, out Processor main);

            foreach (string s in GetWordsSequental("ABCD"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));

            foreach (string s in GetWordsSequental("DCBA"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));
        }

        [TestMethod]
        public void ReflexTest18()
        {

            GetTest17_18_19_20(out ProcessorContainer pc, out Processor main);

            foreach (string s in GetWordsSequental("DCBA"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));

            foreach (string s in GetWordsSequental("ABCD"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));
        }

        [TestMethod]
        public void ReflexTest19()
        {

            GetTest17_18_19_20(out ProcessorContainer pc, out Processor main);

            foreach (string s in GetWordsSequental("abcd"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));

            foreach (string s in GetWordsSequental("dcba"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));
        }

        [TestMethod]
        public void ReflexTest20()
        {

            GetTest17_18_19_20(out ProcessorContainer pc, out Processor main);

            foreach (string s in GetWordsSequental("dcba"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));

            foreach (string s in GetWordsSequental("abcd"))
                Assert.AreNotEqual(null, new Reflex(pc).FindRelation(main, s));
        }

        static void GetTest17_18_19_20(out ProcessorContainer pc, out Processor main)
        {
            SignValue[,] map = new SignValue[8, 8];
            map[0, 0] = SignValue.MaxValue;
            map[1, 0] = SignValue.MinValue;
            map[5, 4] = new SignValue(1000);
            map[5, 5] = new SignValue(10900);
            map[4, 4] = new SignValue(1605000);
            map[3, 2] = new SignValue(700060);
            map[3, 3] = new SignValue(1000);
            map[2, 5] = new SignValue(900);
            map[3, 4] = new SignValue(35000);
            map[7, 2] = new SignValue(50000);
            map[2, 7] = new SignValue(10000);

            map[7, 7] = SignValue.MaxValue;
            map[6, 7] = SignValue.MinValue;
            map[6, 4] = new SignValue(1000);
            map[6, 5] = new SignValue(10900);
            map[6, 4] = new SignValue(1605000);
            map[6, 2] = new SignValue(700060);
            map[5, 3] = new SignValue(1000);
            map[5, 1] = new SignValue(900);
            map[2, 4] = new SignValue(35000);
            map[2, 2] = new SignValue(50000);
            map[5, 7] = new SignValue(10000);

            SignValue[,] m1 = new SignValue[2, 2];
            m1[0, 0] = SignValue.MaxValue;
            m1[1, 0] = SignValue.MinValue;
            SignValue[,] m2 = new SignValue[2, 2];
            m2[1, 1] = new SignValue(1000);
            m2[0, 1] = new SignValue(10900);
            m2[0, 0] = new SignValue(50000);
            SignValue[,] m3 = new SignValue[2, 2];
            m3[0, 0] = new SignValue(35000);
            m3[1, 0] = new SignValue(10000);
            m3[1, 1] = new SignValue(1605000);
            SignValue[,] m4 = new SignValue[2, 2];
            m4[0, 0] = new SignValue(700060);
            m4[1, 1] = new SignValue(1000);
            m4[1, 0] = new SignValue(900);
            m4[0, 1] = SignValue.MaxValue;

            Processor pA = new Processor(m1, "A"),
                pB = new Processor(m2, "B"),
                pC = new Processor(m3, "C"),
                pD = new Processor(m4, "D");

            main = new Processor(map, "main");
            pc = new ProcessorContainer(pA, pB, pC, pD);
        }

        static IEnumerable<string> GetWordsSequental(string word)
        {
            int[] count = new int[word.Length];
            for (int counter = word.Length - 1; counter >= 0;)
            {
                yield return GetWord(count, word);
                if ((counter = ChangeCount(count)) < 0)
                    yield break;
            }
        }

        static int ChangeCount(int[] count)
        {
            if (count == null || count.Length <= 0)
                throw new ArgumentException(
                    $"{nameof(ChangeCount)}: Массив-счётчик не указан или его длина некорректна ({count?.Length}).",
                    nameof(count));
            for (int k = count.Length - 1; k >= 0; k--)
            {
                if (count[k] >= count.Length - 1) continue;
                count[k]++;
                for (int x = k + 1; x < count.Length; x++)
                    count[x] = 0;
                return k;
            }
            return -1;
        }

        static string GetWord(IList<int> count, string word)
        {
            if (count == null)
                throw new ArgumentNullException(nameof(count), $"{nameof(GetWord)}: Массив данных равен null.");
            if (count.Count <= 0)
                throw new ArgumentException(
                    $"{nameof(GetWord)}: Длина массива данных должна совпадать с количеством хранимых слов.",
                    nameof(count));
            StringBuilder sb = new StringBuilder(count.Count);
            foreach (int c in count.TakeWhile(c => sb.Length < count.Count))
                sb.Append(word[c]);
            if (sb.Length < count.Count)
                return null;
            string result = sb.ToString();
            if (sb.Length > count.Count)
                result = result.Substring(0, count.Count);
            return result;
        }

        [TestMethod]
        public void ReflexTest21()
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

            {
                SignValue[,] minmap = new SignValue[1, 1];
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "a"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "b"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "c"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "d"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "e"));
                Assert.AreEqual(null, reflex.FindRelation(minProcessor, "w"));

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
                    reflex.FindRelation(null, "w");
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

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "b"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "c"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "d"));
            Assert.AreEqual(null, reflex.FindRelation(main, "e"));
            Assert.AreEqual(null, reflex.FindRelation(main, "w"));

            for (int k = 0; k < 50; k++)
            {
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "aa"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ab"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ba"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ac"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ca"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ad"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "da"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ae"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ba"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "be"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ab"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "db"));
                Assert.AreEqual(null, reflex.FindRelation(main, "eb"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ca"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ce"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ac"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dc"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ec"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "da"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "db"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "de"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ad"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bd"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cd"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ed"));

                Assert.AreEqual(null, reflex.FindRelation(main, "ea"));
                Assert.AreEqual(null, reflex.FindRelation(main, "eb"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ec"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ed"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ee"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ae"));
                Assert.AreEqual(null, reflex.FindRelation(main, "be"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ce"));
                Assert.AreEqual(null, reflex.FindRelation(main, "de"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ee"));
            }
        }

        [TestMethod]
        public void ReflexTest22()
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

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "c"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "a"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "b"));
            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "d"));
            Assert.AreEqual(null, reflex.FindRelation(main, "e"));
            Assert.AreEqual(null, reflex.FindRelation(main, "w"));

            for (int k = 0; k < 50; k++)
            {
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "aa"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ab"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ba"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ac"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ca"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ad"));
                Assert.AreEqual(null, reflex.FindRelation(main, "da"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ae"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ba"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "be"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ab"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "db"));
                Assert.AreEqual(null, reflex.FindRelation(main, "eb"));

                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ca"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cb"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ce"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "ac"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dc"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ec"));

                Assert.AreEqual(null, reflex.FindRelation(main, "da"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "db"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dc"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "de"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ad"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "bd"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "cd"));
                Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "dd"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ed"));

                Assert.AreEqual(null, reflex.FindRelation(main, "ea"));
                Assert.AreEqual(null, reflex.FindRelation(main, "eb"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ec"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ed"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ee"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ae"));
                Assert.AreEqual(null, reflex.FindRelation(main, "be"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ce"));
                Assert.AreEqual(null, reflex.FindRelation(main, "de"));
                Assert.AreEqual(null, reflex.FindRelation(main, "ee"));
            }
        }

        [TestMethod]
        public void ReflexCopyTest1()
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

            ProcessorContainer pc = new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapA, "C"));

            Processor main = new Processor(bmap, "bigmain");

            Reflex reflex = new Reflex(pc);

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));

            Assert.AreEqual(null, reflex.FindRelation(main, "B"));

            pc.Add(new Processor(mapB, "B"));

            Assert.AreNotEqual(null, reflex = reflex.FindRelation(main, "A"));

            Assert.AreEqual(null, reflex.FindRelation(main, "B"));
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