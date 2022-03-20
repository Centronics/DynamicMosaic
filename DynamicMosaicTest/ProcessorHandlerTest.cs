using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DynamicParser;
using DynamicProcessor;
using DynamicMosaic;
using Processor = DynamicParser.Processor;

namespace DynamicMosaicTest
{
    [TestClass]
    public class ProcessorHandlerTest
    {
        static void CheckSize(List<Processor> procs)
        {
            Assert.AreNotEqual(null, procs);
            foreach (Processor p in procs)
                Assert.AreEqual(p.Size, new Size(1, 1));
        }

        static List<Processor> CheckProcessorHandler1(ProcessorHandler ph)
        {
            List<Processor> procs = new List<Processor>(ph.Processors.ToArray());
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(1, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual("A", ph.ToString());
            return procs;
        }

        static void CheckProcessorHandler2(ProcessorHandler ph)
        {
            Assert.AreNotEqual(null, ph);

            ProcessorContainer procs = new ProcessorContainer(ph.Processors.ToArray());
            Assert.AreNotEqual(null, procs);
            Assert.AreEqual(procs.Width, 1);
            Assert.AreEqual(procs.Height, 1);
            Assert.AreEqual(3, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual("ABC", ph.ToString());
        }

        [TestMethod]
        public void PHTest()
        {
            ProcessorHandler ph = new ProcessorHandler();
            Assert.AreEqual(string.Empty, ph.ToString());
            Assert.AreEqual(0, ph.Processors.ToArray().Length);

            ph.Add(new Processor(new[] { new SignValue(2) }, "A"));

            List<Processor> procs = CheckProcessorHandler1(ph);
            procs.Add(new Processor(new[] { new SignValue(2000) }, "w"));
            CheckProcessorHandler1(ph);

            ph.Add(new Processor(new[] { new SignValue(3) }, "b"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(2, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual("AB", ph.ToString());

            ph.Add(new Processor(new[] { new SignValue(4) }, "C"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(2) }, "A"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(3) }, "b"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(4) }, "C"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(2) }, "A1"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(3) }, "b1"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(4) }, "C1"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(2) }, "a1"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(3) }, "B1"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(4) }, "c1"));

            CheckProcessorHandler2(ph);

            ph.Add(new Processor(new[] { new SignValue(13) }, "b1"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(4, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual("ABCB", ph.ToString());

            ph.Add(new Processor(new[] { new SignValue(14) }, "B1"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(5, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual(new SignValue(14), procs[4][0, 0]);
            Assert.AreEqual("B10", procs[4].Tag);
            Assert.AreEqual("ABCBB", ph.ToString());

            ph.Add(new Processor(new[] { new SignValue(15) }, "B"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(6, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual(new SignValue(14), procs[4][0, 0]);
            Assert.AreEqual("B10", procs[4].Tag);
            Assert.AreEqual(new SignValue(15), procs[5][0, 0]);
            Assert.AreEqual("B0", procs[5].Tag);
            Assert.AreEqual("ABCBBB", ph.ToString());

            ph.Add(new Processor(new[] { new SignValue(16) }, "b"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(7, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual(new SignValue(14), procs[4][0, 0]);
            Assert.AreEqual("B10", procs[4].Tag);
            Assert.AreEqual(new SignValue(15), procs[5][0, 0]);
            Assert.AreEqual("B0", procs[5].Tag);
            Assert.AreEqual(new SignValue(16), procs[6][0, 0]);
            Assert.AreEqual("b00", procs[6].Tag);
            Assert.AreEqual("ABCBBBB", ph.ToString());

            bool bex = false;
            try
            {
                SignValue[,] prc = new SignValue[2, 1];
                prc[0, 0] = new SignValue(16);
                prc[1, 0] = new SignValue(100);
                ph.Add(new Processor(prc, "b"));
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentException), ex.GetType());
                Assert.AreEqual("Добавляемая карта отличается по размерам от первой карты, добавленной в коллекцию. Требуется: 1, 1. Фактически: 2, 1.", ex.Message);
                bex = true;
            }

            Assert.AreEqual(true, bex);

            bex = false;
            try
            {
                SignValue[,] prc = new SignValue[1, 2];
                prc[0, 0] = new SignValue(16);
                prc[0, 1] = new SignValue(100);
                ph.Add(new Processor(prc, "b"));
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentException), ex.GetType());
                Assert.AreEqual("Добавляемая карта отличается по размерам от первой карты, добавленной в коллекцию. Требуется: 1, 1. Фактически: 1, 2.", ex.Message);
                bex = true;
            }

            Assert.AreEqual(true, bex);

            ph.Add(new Processor(new[] { new SignValue(3) }, "r"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(8, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual(new SignValue(14), procs[4][0, 0]);
            Assert.AreEqual("B10", procs[4].Tag);
            Assert.AreEqual(new SignValue(15), procs[5][0, 0]);
            Assert.AreEqual("B0", procs[5].Tag);
            Assert.AreEqual(new SignValue(16), procs[6][0, 0]);
            Assert.AreEqual("b00", procs[6].Tag);
            Assert.AreEqual(new SignValue(3), procs[7][0, 0]);
            Assert.AreEqual("r", procs[7].Tag);
            Assert.AreEqual("ABCBBBBR", ph.ToString());

            ph.Add(new Processor(new[] { new SignValue(2) }, "v1"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(9, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual(new SignValue(14), procs[4][0, 0]);
            Assert.AreEqual("B10", procs[4].Tag);
            Assert.AreEqual(new SignValue(15), procs[5][0, 0]);
            Assert.AreEqual("B0", procs[5].Tag);
            Assert.AreEqual(new SignValue(16), procs[6][0, 0]);
            Assert.AreEqual("b00", procs[6].Tag);
            Assert.AreEqual(new SignValue(3), procs[7][0, 0]);
            Assert.AreEqual("r", procs[7].Tag);
            Assert.AreEqual(new SignValue(2), procs[8][0, 0]);
            Assert.AreEqual("v1", procs[8].Tag);
            Assert.AreEqual("ABCBBBBRV", ph.ToString());

            ph.Add(new Processor(new[] { new SignValue(8418982) }, "q"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(10, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual(new SignValue(14), procs[4][0, 0]);
            Assert.AreEqual("B10", procs[4].Tag);
            Assert.AreEqual(new SignValue(15), procs[5][0, 0]);
            Assert.AreEqual("B0", procs[5].Tag);
            Assert.AreEqual(new SignValue(16), procs[6][0, 0]);
            Assert.AreEqual("b00", procs[6].Tag);
            Assert.AreEqual(new SignValue(3), procs[7][0, 0]);
            Assert.AreEqual("r", procs[7].Tag);
            Assert.AreEqual(new SignValue(2), procs[8][0, 0]);
            Assert.AreEqual("v1", procs[8].Tag);
            Assert.AreEqual(new SignValue(8418982), procs[9][0, 0]);
            Assert.AreEqual("q", procs[9].Tag);
            Assert.AreEqual("ABCBBBBRVQ", ph.ToString());

            ph.Add(new Processor(new[] { new SignValue(12451893) }, "q"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);
            Assert.AreEqual(11, procs.Count);
            Assert.AreEqual(new SignValue(2), procs[0][0, 0]);
            Assert.AreEqual("A", procs[0].Tag);
            Assert.AreEqual(new SignValue(3), procs[1][0, 0]);
            Assert.AreEqual("b", procs[1].Tag);
            Assert.AreEqual(new SignValue(4), procs[2][0, 0]);
            Assert.AreEqual("C", procs[2].Tag);
            Assert.AreEqual(new SignValue(13), procs[3][0, 0]);
            Assert.AreEqual("b1", procs[3].Tag);
            Assert.AreEqual(new SignValue(14), procs[4][0, 0]);
            Assert.AreEqual("B10", procs[4].Tag);
            Assert.AreEqual(new SignValue(15), procs[5][0, 0]);
            Assert.AreEqual("B0", procs[5].Tag);
            Assert.AreEqual(new SignValue(16), procs[6][0, 0]);
            Assert.AreEqual("b00", procs[6].Tag);
            Assert.AreEqual(new SignValue(3), procs[7][0, 0]);
            Assert.AreEqual("r", procs[7].Tag);
            Assert.AreEqual(new SignValue(2), procs[8][0, 0]);
            Assert.AreEqual("v1", procs[8].Tag);
            Assert.AreEqual(new SignValue(8418982), procs[9][0, 0]);
            Assert.AreEqual("q", procs[9].Tag);
            Assert.AreEqual(new SignValue(12451893), procs[10][0, 0]);
            Assert.AreEqual("q0", procs[10].Tag);
            Assert.AreEqual("ABCBBBBRVQQ", ph.ToString());

            Processor renameProcessor2 = ProcessorHandler.ChangeProcessorTag(new Processor(new[] { SignValue.MaxValue }, "mmM"), "zZz");
            Assert.AreNotEqual(null, renameProcessor2);
            Assert.AreEqual("zZz", renameProcessor2.Tag);
            Assert.AreEqual(SignValue.MaxValue, renameProcessor2[0, 0]);
            Assert.AreEqual(1, renameProcessor2.Width);
            Assert.AreEqual(1, renameProcessor2.Height);

            Processor renameProcessor3 = ProcessorHandler.ChangeProcessorTag(new Processor(new[] { SignValue.MaxValue }, "mmM"), "mmM");
            Assert.AreNotEqual(null, renameProcessor3);
            Assert.AreEqual("mmM", renameProcessor3.Tag);
            Assert.AreEqual(SignValue.MaxValue, renameProcessor3[0, 0]);
            Assert.AreEqual(1, renameProcessor3.Width);
            Assert.AreEqual(1, renameProcessor3.Height);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PHTestException_1()
        {
            new ProcessorHandler().Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PHTestException_5()
        {
            ProcessorHandler.ChangeProcessorTag(new Processor(new[] { SignValue.MaxValue }, "mmm"), string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PHTestException_6()
        {
            ProcessorHandler.ChangeProcessorTag(new Processor(new[] { SignValue.MaxValue }, "mmm"), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PHTestException_7()
        {
            ProcessorHandler.ChangeProcessorTag(new Processor(new[] { SignValue.MaxValue }, "mmm"), " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PHTestException_8()
        {
            ProcessorHandler.ChangeProcessorTag(null, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PHTestException_9()
        {
            ProcessorHandler.ChangeProcessorTag(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PHTestException_10()
        {
            ProcessorHandler.ChangeProcessorTag(null, " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PHTestException_11()
        {
            ProcessorHandler.ChangeProcessorTag(null, "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PHTestException_12()
        {
            HashCreator.GetHash(null);
        }
    }
}
