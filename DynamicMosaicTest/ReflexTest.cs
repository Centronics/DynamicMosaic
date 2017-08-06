using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            {
                SignValue[,] minmap = new SignValue[1, 1];
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
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
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
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
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
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
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
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
                    Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EB"));

                    Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EC"));

                    Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "ED"));

                    Assert.AreEqual(true, reflex.FindRelation(main, "EA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "ED"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
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
                    Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EB"));

                    Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EC"));

                    Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "ED"));

                    Assert.AreEqual(true, reflex.FindRelation(main, "EA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "ED"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
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

                Assert.AreEqual(true, reflex.FindRelation(main, "E"));
                Assert.AreEqual(true, reflex.FindRelation(main, "C"));
                Assert.AreEqual(true, reflex.FindRelation(main, "A"));
                Assert.AreEqual(true, reflex.FindRelation(main, "B"));
                Assert.AreEqual(false, reflex.FindRelation(main, "D"));
                Assert.AreEqual(false, reflex.FindRelation(main, "W"));

                for (int k = 0; k < 50; k++)
                {
                    Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AE"));

                    Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EB"));

                    Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EC"));

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

                    Assert.AreEqual(true, reflex.FindRelation(main, "EA"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EB"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EC"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "ED"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "AE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "BE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "CE"));
                    Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                    Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
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
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
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

            Assert.AreEqual(false, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "C"));
            Assert.AreEqual(false, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AE"));
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

            Assert.AreEqual(false, reflex.FindRelation(new Processor(p1, "p1"), "1"));

            SignValue[,] p2 = new SignValue[1, 1];
            p2[0, 0] = new SignValue(p1[0, 0].Value - 10000);

            Assert.AreEqual(true, reflex.FindRelation(new Processor(p2, "p2"), "2"));
            Assert.AreEqual(false, reflex.FindRelation(new Processor(p2, "p2"), "1"));

            Assert.AreEqual(true, reflex.FindRelation(new Processor(p2, "p2"), "2"));
            Assert.AreEqual(false, reflex.FindRelation(new Processor(p2, "p2"), "1"));
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

            Assert.AreEqual(true, reflex.FindRelation(new Processor(p1, "p1"), "2"));

            SignValue[,] p2 = new SignValue[1, 1];
            p2[0, 0] = new SignValue(p1[0, 0].Value + 10000);

            Assert.AreEqual(false, reflex.FindRelation(new Processor(p2, "p2"), "1"));
            Assert.AreEqual(true, reflex.FindRelation(new Processor(p2, "p2"), "2"));

            Assert.AreEqual(false, reflex.FindRelation(new Processor(p2, "p2"), "1"));
            Assert.AreEqual(true, reflex.FindRelation(new Processor(p2, "p2"), "2"));
        }

        [TestMethod]
        public void ReflexTest8()
        {
            SignValue average = new SignValue(SignValue.MaxValue.Value / 2);
            ProcessorContainer pc = new ProcessorContainer(new Processor(new SignValue[1], "a"), new Processor(new[] { average }, "b"));

            Reflex reflex = new Reflex(pc);
            Processor p = new Processor(new[] { average }, "c");

            Assert.AreEqual(false, reflex.FindRelation(p, "a"));
            Assert.AreEqual(true, reflex.FindRelation(p, "b"));
            Assert.AreEqual(false, reflex.FindRelation(p, "f"));

            Processor pf = new Processor(new[] { average }, "f");
            pc.Add(pf);

            Assert.AreEqual(false, reflex.FindRelation(p, "a"));
            Assert.AreEqual(true, reflex.FindRelation(p, "b"));
            Assert.AreEqual(false, reflex.FindRelation(p, "f"));
        }

        [TestMethod]
        public void ReflexTest8_1()
        {
            SignValue average = new SignValue(SignValue.MaxValue.Value / 2);
            ProcessorContainer pc = new ProcessorContainer(new Processor(new SignValue[1], "a"), new Processor(new[] { average }, "b"),
                new Processor(new[] { average }, "f"));

            Reflex reflex = new Reflex(pc);
            Processor p = new Processor(new[] { average }, "c");

            Assert.AreEqual(false, reflex.FindRelation(p, "a"));
            Assert.AreEqual(true, reflex.FindRelation(p, "b"));
            Assert.AreEqual(true, reflex.FindRelation(p, "f"));
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

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            {
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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
                Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "ED"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AV"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CF"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DP"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EQ"));

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AQ"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BP"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CW"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DV"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EF"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

            Assert.AreEqual(false, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "C"));
            Assert.AreEqual(false, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AW"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AV"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BW"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CF"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DP"));
            Assert.AreEqual(false, reflex.FindRelation(main, "EQ"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AF"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BP"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CP"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DV"));
            Assert.AreEqual(false, reflex.FindRelation(main, "EV"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WQ"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

            {
                Processor minProcessor = new Processor(minmap, "main");

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AV"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CF"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DP"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EQ"));

                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AQ"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BP"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CW"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DV"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EF"));
                Assert.AreEqual(false, reflex.FindRelation(minProcessor, "WW"));

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
                Assert.AreEqual(false, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));

                Assert.AreEqual(false, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "W"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(false, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "W"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(false, reflex.FindRelation(main, "EE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "ED"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "W"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WA"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "W"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BW"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EQ"));

                Assert.AreEqual(false, reflex.FindRelation(main, "AF"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CP"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "EV"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WQ"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

                {
                    Processor minProcessor = new Processor(minmap, "main");

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "A"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "B"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "C"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "D"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "E"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EQ"));

                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "AQ"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "BP"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "CW"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "DV"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "EF"));
                    Assert.AreEqual(false, reflex.FindRelation(minProcessor, "W"));

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

            Assert.AreEqual(true, reflex.FindRelation(main, "ABCD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "dADDDbbBBDDbBacccaCccccaaabbcDddd"));
            Assert.AreEqual(true, reflex.FindRelation(main, "ABC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "CAB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BCA"));
        }

        [TestMethod]
        public void ReflexTest12()
        {
            SignValue[,] map;
            Assert.AreEqual(true, GetMapsForTest_12_13(out map, true).FindRelation(new Processor(map, "main"), "abcde"));
            Assert.AreEqual(true, GetMapsForTest_121_131(out map, true).FindRelation(new Processor(map, "main"), "abcde"));
        }

        [TestMethod]
        public void ReflexTest12_1()
        {
            SignValue[,] map;
            Assert.AreEqual(true, GetMapsForTest_12_13(out map, false).FindRelation(new Processor(map, "main"), "ABCDE"));
            Assert.AreEqual(true, GetMapsForTest_121_131(out map, false).FindRelation(new Processor(map, "main"), "ABCDE"));
        }

        [TestMethod]
        public void ReflexTest13()
        {
            SignValue[,] map;
            Assert.AreEqual(true, GetMapsForTest_12_13(out map, true).FindRelation(new Processor(map, "main"), "abcde"));
            Assert.AreEqual(true, GetMapsForTest_121_131(out map, true).FindRelation(new Processor(map, "main"), "abcde"));
        }

        [TestMethod]
        public void ReflexTest13_1()
        {
            SignValue[,] map;
            Assert.AreEqual(true, GetMapsForTest_12_13(out map, false).FindRelation(new Processor(map, "main"), "ABCDE"));
            Assert.AreEqual(true, GetMapsForTest_121_131(out map, false).FindRelation(new Processor(map, "main"), "ABCDE"));
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

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "a"));
            Assert.AreEqual(false, reflex.FindRelation(main, "b"));
        }

        [TestMethod]
        public void ReflexTest15()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AC(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_1()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AB1(reflex, main);

            Check_AC(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_2()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AC(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_3()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AB2(reflex, main);

            Check_AC(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_4()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AC(reflex, main);

            Check_AB3(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_5()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AB3(reflex, main);

            Check_AC(reflex, main);

            Check_AB3(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_6()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AC1(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_7()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AB1(reflex, main);

            Check_AC1(reflex, main);

            Check_AB1(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_8()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AC1(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_9()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AB2(reflex, main);

            Check_AC1(reflex, main);

            Check_AB2(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_10()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

            Check_AC1(reflex, main);

            Check_AB3(reflex, main);
        }

        [TestMethod]
        public void ReflexTest15_11()
        {
            Processor main;
            Reflex reflex;

            GetReflexProcessor(out reflex, out main);

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
            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "a"));
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "b"));

            Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "ac"));
            Assert.AreEqual(true, reflex.FindRelation(main, "aC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "Ac"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "aB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "Ab"));
        }

        static void Check_AC1(Reflex reflex, Processor main)
        {
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "b"));
            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "a"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "aB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "Ab"));

            Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "ac"));
            Assert.AreEqual(true, reflex.FindRelation(main, "aC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "Ac"));
        }

        static void Check_All(Reflex reflex, Processor main)
        {
            Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "ac"));
            Assert.AreEqual(true, reflex.FindRelation(main, "Ac"));
            Assert.AreEqual(true, reflex.FindRelation(main, "aC"));
        }

        static void Check_AB1(Reflex reflex, Processor main)
        {
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "b"));
            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "a"));
            Check_All(reflex, main);
            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "Ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "aB"));
            Check_All(reflex, main);
        }

        static void Check_AB2(Reflex reflex, Processor main)
        {
            Check_All(reflex, main);
            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "Ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "aB"));
            Check_All(reflex, main);
            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "a"));
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "b"));
            Check_All(reflex, main);
        }

        static void Check_AB3(Reflex reflex, Processor main)
        {
            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "Ab"));
            Assert.AreEqual(false, reflex.FindRelation(main, "aB"));
            Check_All(reflex, main);
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "a"));
            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(false, reflex.FindRelation(main, "b"));
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

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WW"));
            Assert.AreEqual(false, reflex.FindRelation(main, "ADC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CAD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DCA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "ADCE"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "EA"));
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

            Assert.AreEqual(false, reflex.FindRelation(main, "A"));
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "C"));
            Assert.AreEqual(false, reflex.FindRelation(main, "D"));
            Assert.AreEqual(true, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AE"));

            Processor main1 = new Processor(bmap, "bigmain");

            Assert.AreEqual(true, reflex.FindRelation(main1, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "W"));

            Assert.AreEqual(true, reflex.FindRelation(main1, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "BB"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "CC"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "DD"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "WW"));

            Assert.AreEqual(true, reflex.FindRelation(main1, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "BA"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "AC"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "CA"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "AD"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "AE"));
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

            Assert.AreEqual(true, reflex.FindRelation(main1, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "W"));

            Assert.AreEqual(true, reflex.FindRelation(main1, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "BB"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "CC"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "DD"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "WW"));

            Assert.AreEqual(true, reflex.FindRelation(main1, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "BA"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "AC"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "CA"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "AD"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "AE"));

            Processor main = new Processor(map, "main");

            Assert.AreEqual(false, reflex.FindRelation(main, "A"));
            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
            Assert.AreEqual(false, reflex.FindRelation(main, "C"));
            Assert.AreEqual(false, reflex.FindRelation(main, "D"));
            Assert.AreEqual(true, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AE"));
        }

        [TestMethod]
        public void CheckMultipleSizes3()
        {
            Reflex reflex = new Reflex(MapsForMultipleSizes);

            {
                Processor main = Map1ForMultipleSizesMain;

                Assert.AreEqual(true, reflex.FindRelation(main, "A"));
                Assert.AreEqual(true, reflex.FindRelation(main, "B"));
                Assert.AreEqual(true, reflex.FindRelation(main, "C"));
                Assert.AreEqual(true, reflex.FindRelation(main, "D"));
                Assert.AreEqual(true, reflex.FindRelation(main, "E"));
                Assert.AreEqual(false, reflex.FindRelation(main, "W"));

                Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AW"));
            }

            Processor main1 = Map2ForMultipleSizesMain;

            Assert.AreEqual(false, reflex.FindRelation(main1, "C"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "W"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "E"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "B"));

            Assert.AreEqual(true, reflex.FindRelation(main1, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "BB"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "CC"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "DD"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "WW"));

            Assert.AreEqual(true, reflex.FindRelation(main1, "AB"));
            Assert.AreEqual(true, reflex.FindRelation(main1, "BA"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "AC"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "CA"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "AD"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "AE"));
            Assert.AreEqual(false, reflex.FindRelation(main1, "AW"));
        }

        [TestMethod]
        public void CheckMultipleSizes4()
        {
            Reflex reflex = new Reflex(MapsForMultipleSizes);

            {
                Processor main1 = Map2ForMultipleSizesMain;

                Assert.AreEqual(true, reflex.FindRelation(main1, "C"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "W"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "D"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "E"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "A"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "B"));

                Assert.AreEqual(false, reflex.FindRelation(main1, "AA"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main1, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "EE"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "WW"));

                Assert.AreEqual(false, reflex.FindRelation(main1, "AB"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "BA"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "AC"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "CA"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "DA"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "AE"));
                Assert.AreEqual(false, reflex.FindRelation(main1, "AW"));
            }

            Processor main = Map1ForMultipleSizesMain;

            Assert.AreEqual(false, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(true, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AA"));
            Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
            Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
            Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "WW"));

            Assert.AreEqual(false, reflex.FindRelation(main, "AB"));
            Assert.AreEqual(false, reflex.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AC"));
            Assert.AreEqual(false, reflex.FindRelation(main, "CA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
            Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AE"));
            Assert.AreEqual(false, reflex.FindRelation(main, "AW"));
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

            Processor main = new Processor(map, "main");
            ProcessorContainer pc = new ProcessorContainer(pA, pB, pC, pD);

            foreach (string s in GetWordsSequental("ABCD"))
                Assert.AreEqual(true, new Reflex(pc).FindRelation(main, s));
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

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));

            Assert.AreEqual(false, reflex.FindRelation(main, "B"));

            pc.Add(new Processor(mapB, "B"));

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));

            Assert.AreEqual(false, reflex.FindRelation(main, "B"));
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

            Assert.AreEqual(true, reflex.FindRelation(main, "B"));

            Assert.AreEqual(false, reflex.FindRelation(main, "A"));

            pc.Add(new Processor(mapA, "A"));

            Assert.AreEqual(true, reflex.FindRelation(main, "B"));

            Assert.AreEqual(false, reflex.FindRelation(main, "A"));
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
            new Reflex(null);
        }
    }
}