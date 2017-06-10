﻿using System;
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
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procB, procC, procD, procE));

            TestingAllProperties(reflex);

            TestingAllProperties((Reflex)reflex.Clone());

            GetMapTest();

            IsMapsWordTest();

            Assert.AreEqual(true, reflex.FindRelation(main, "A"));
            Assert.AreEqual(true, reflex.FindRelation(main, "B"));
            Assert.AreEqual(true, reflex.FindRelation(main, "C"));
            Assert.AreEqual(true, reflex.FindRelation(main, "D"));
            Assert.AreEqual(true, reflex.FindRelation(main, "E"));
            Assert.AreEqual(false, reflex.FindRelation(main, "W"));



            Processor[] procs = reflex.Processors.ToArray();
        }

        static void TestingAllProperties(Reflex reflex)
        {
            Assert.AreEqual(5, reflex.CountProcessorsBase);
            Assert.AreEqual(5, reflex.CountProcessor);

            Assert.AreEqual(true, reflex.Processors.Any(p => string.Compare(p.Tag, "A", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.Processors.Any(p => string.Compare(p.Tag, "B", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.Processors.Any(p => string.Compare(p.Tag, "C", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.Processors.Any(p => string.Compare(p.Tag, "D", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.Processors.Any(p => string.Compare(p.Tag, "E", StringComparison.OrdinalIgnoreCase) == 0));

            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => string.Compare(p.Tag, "A", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => string.Compare(p.Tag, "B", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => string.Compare(p.Tag, "C", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => string.Compare(p.Tag, "D", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, reflex.ProcessorsBase.Any(p => string.Compare(p.Tag, "E", StringComparison.OrdinalIgnoreCase) == 0));

            Processor[] processors = { reflex[0], reflex[1], reflex[2], reflex[3], reflex[4] };
            Assert.AreEqual(true, processors.Any(p => string.Compare(p.Tag, "A", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, processors.Any(p => string.Compare(p.Tag, "B", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, processors.Any(p => string.Compare(p.Tag, "C", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, processors.Any(p => string.Compare(p.Tag, "D", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.AreEqual(true, processors.Any(p => string.Compare(p.Tag, "E", StringComparison.OrdinalIgnoreCase) == 0));
        }

        static void GetMapTest()
        {
            SignValue[,] map = new SignValue[2, 2];
            Processor procA = new Processor(map, "A1");
            Processor procA1 = new Processor(map, "A12");
            Processor procB = new Processor(map, "B1");
            Processor procB1 = new Processor(map, "B13");
            Processor procB2 = new Processor(map, "B14");
            Processor procC = new Processor(map, "C1");
            Processor procC1 = new Processor(map, "C14388");
            Processor procC2 = new Processor(map, "C15388");
            Processor procD = new Processor(map, "D1");
            Processor procD1 = new Processor(map, "D12");
            Processor procD2 = new Processor(map, "D13");
            Processor procE = new Processor(map, "E1");
            Processor procE1 = new Processor(map, "E12");
            Processor procE2 = new Processor(map, "E13");
            Reflex reflex = new Reflex(new ProcessorContainer(procA, procA1, procB, procB1, procB2, procC, procC1, procC2, procD, procD1, procD2, procE, procE1, procE2));

            Processor[] p1 = reflex.GetMap("A").ToArray();
            Assert.AreEqual(2, p1.Length);
            Assert.AreEqual(true, p1.Any(p => p.IsProcessorName("A1", 0)));
            Assert.AreEqual(true, p1.Any(p => p.IsProcessorName("A12", 0)));

            Processor[] p2 = reflex.GetMap("B").ToArray();
            Assert.AreEqual(3, p2.Length);
            Assert.AreEqual(true, p2.Any(p => p.IsProcessorName("B1", 0)));
            Assert.AreEqual(true, p2.Any(p => p.IsProcessorName("B13", 0)));
            Assert.AreEqual(true, p2.Any(p => p.IsProcessorName("B14", 0)));

            Processor[] p3 = reflex.GetMap("C").ToArray();
            Assert.AreEqual(3, p3.Length);
            Assert.AreEqual(true, p3.Any(p => p.IsProcessorName("C1", 0)));
            Assert.AreEqual(true, p3.Any(p => p.IsProcessorName("C14388", 0)));
            Assert.AreEqual(true, p3.Any(p => p.IsProcessorName("C15388", 0)));

            Processor[] p4 = reflex.GetMap("D").ToArray();
            Assert.AreEqual(3, p4.Length);
            Assert.AreEqual(true, p4.Any(p => p.IsProcessorName("D1", 0)));
            Assert.AreEqual(true, p4.Any(p => p.IsProcessorName("D12", 0)));
            Assert.AreEqual(true, p4.Any(p => p.IsProcessorName("D13", 0)));

            Processor[] p5 = reflex.GetMap("E").ToArray();
            Assert.AreEqual(3, p5.Length);
            Assert.AreEqual(true, p5.Any(p => p.IsProcessorName("E1", 0)));
            Assert.AreEqual(true, p5.Any(p => p.IsProcessorName("E12", 0)));
            Assert.AreEqual(true, p5.Any(p => p.IsProcessorName("E13", 0)));

            Processor[] p6 = reflex.GetMap("1", 1).ToArray();
            Assert.AreEqual(14, p6.Length);
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("A1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("A12", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("B1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("B13", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("B14", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("C1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("C143a8", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("C153A8", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("D1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("D12 ", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("D13 ", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("E1", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("E12 ", 0)));
            Assert.AreEqual(true, p6.Any(p => p.IsProcessorName("E13 ", 0)));

            Processor[] p7 = reflex.GetMap("3a8", 3).ToArray();
            Assert.AreEqual(2, p7.Length);
            Assert.AreEqual(true, p7.Any(p => p.IsProcessorName("C143a8", 0)));
            Assert.AreEqual(true, p7.Any(p => p.IsProcessorName("C153A8", 0)));

            Processor[] p9 = reflex.GetMap("bb", 3).ToArray();
            Assert.AreEqual(0, p9.Length);

            Processor[] p10 = reflex.GetMap("bb", -3).ToArray();
            Assert.AreEqual(0, p10.Length);

            Processor[] p11 = reflex.GetMap(null).ToArray();
            Assert.AreEqual(0, p11.Length);

            Processor[] p12 = reflex.GetMap(" ").ToArray();
            Assert.AreEqual(0, p12.Length);

            Processor[] p13 = reflex.GetMap(" ", 3).ToArray();
            Assert.AreEqual(0, p13.Length);
        }

        static void IsMapsWordTest()
        {
            
        }
    }
}