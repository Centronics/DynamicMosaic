﻿using System;
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
        }

        [TestMethod]
        public void ReflexTest7_1()
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
        }

        [TestMethod]
        public void ReflexTest8()
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
        public void ReflexTest9()
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

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
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
        }

        [TestMethod]
        public void ReflexTest10()
        {
            SignValue[,] m1 = new SignValue[1, 1];
            m1[0, 0] = SignValue.MaxValue;
            SignValue[,] m2 = new SignValue[1, 1];

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(m1, "1"), new Processor(m2, "2")));

            SignValue[,] p1 = new SignValue[1, 1];
            p1[0, 0] = new SignValue(SignValue.MaxValue.Value / 2);

            Assert.AreEqual(true, reflex.FindRelation(new Processor(p1, "p1"), "1"));

            SignValue[,] p2 = new SignValue[1, 1];
            p2[0, 0] = new SignValue(p1[0, 0].Value - 10000);

            Assert.AreEqual(false, reflex.FindRelation(new Processor(p2, "p2"), "2"));
            Assert.AreEqual(true, reflex.FindRelation(new Processor(p2, "p2"), "1"));

            Assert.AreEqual(false, reflex.FindRelation(new Processor(p2, "p2"), "2"));
            Assert.AreEqual(true, reflex.FindRelation(new Processor(p2, "p2"), "1"));
        }

        [TestMethod]
        public void ReflexTest10_1()
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
        public void ReflexTest11()
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
        public void ReflexTest11_1()
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
        public void ReflexTest12()
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
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DA"));
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
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CB"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DB"));
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
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CE"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DC"));
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
        public void ReflexTest13()
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

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(false, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));

            Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
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
                Assert.AreEqual(true, reflex.FindRelation(main, "AA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AB"));
                Assert.AreEqual(true, reflex.FindRelation(main, "BA"));
                Assert.AreEqual(true, reflex.FindRelation(main, "AC"));
                Assert.AreEqual(true, reflex.FindRelation(main, "CA"));
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

                Assert.AreEqual(true, reflex.FindRelation(main, "CC"));
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

                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
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
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "DE"));
                Assert.AreEqual(false, reflex.FindRelation(main, "AD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "BD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "CD"));
                Assert.AreEqual(true, reflex.FindRelation(main, "DD"));
                Assert.AreEqual(false, reflex.FindRelation(main, "ED"));

                Assert.AreEqual(true, reflex.FindRelation(main, "EE"));
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
        public void ReflexTest14()
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
        }

        [TestMethod]
        public void ReflexTest15()
        {
            SignValue[,] map = new SignValue[6, 4];
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

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"),
                new Processor(mapC, "C"), new Processor(mapD, "D"), new Processor(mapE, "E")));
            Assert.AreEqual(true, reflex.FindRelation(new Processor(map, "main"), "ABC"));
        }

        [TestMethod]
        public void ReflexTest16()
        {
            SignValue[,] map = new SignValue[6, 4];
            map[0, 0] = SignValue.MaxValue;
            map[2, 0] = SignValue.MaxValue;
            map[1, 1] = SignValue.MaxValue;
            map[2, 1] = SignValue.MaxValue;
            map[0, 2] = SignValue.MaxValue;
            map[2, 2] = SignValue.MaxValue;
            map[3, 3] = SignValue.MaxValue;
            SignValue[,] mapA = new SignValue[6, 4];
            mapA[0, 0] = SignValue.MaxValue;
            mapA[2, 0] = SignValue.MaxValue;
            mapA[1, 1] = SignValue.MaxValue;
            mapA[2, 1] = SignValue.MaxValue;
            mapA[0, 2] = SignValue.MaxValue;
            mapA[2, 2] = SignValue.MaxValue;
            mapA[3, 3] = SignValue.MaxValue;
            SignValue[,] mapB = new SignValue[6, 4];
            mapB[1, 1] = SignValue.MaxValue;
            SignValue[,] mapC = new SignValue[6, 4];
            mapC[0, 0] = SignValue.MaxValue;
            mapC[1, 0] = SignValue.MaxValue;
            SignValue[,] mapD = new SignValue[6, 4];
            mapD[0, 0] = SignValue.MaxValue;
            mapD[0, 1] = SignValue.MaxValue;
            mapD[1, 0] = SignValue.MaxValue;
            mapD[1, 1] = SignValue.MaxValue;
            SignValue[,] mapE = new SignValue[6, 4];

            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(mapA, "A"), new Processor(mapB, "B"),
                new Processor(mapC, "C"), new Processor(mapD, "D"), new Processor(mapE, "E")));
            Assert.AreEqual(true, reflex.FindRelation(new Processor(map, "main"), "A"));
        }

        [TestMethod]
        public void ReflexTest17()
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