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
            List<Processor> procs = new List<Processor>(ph.Processors);
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            CheckProcValue(procs, (new SignValue(2), "A"));

            EqualOr(ph.ToString(), "A");

            return procs;
        }

        static List<Processor> ProcessorContainerToList(ProcessorContainer pc)
        {
            List<Processor> result = new List<Processor>(pc.Count);

            for (int k = 0; k < pc.Count; k++)
                result.Add(pc[k]);

            return result;
        }

        static void CheckProcessorHandler2(ProcessorHandler ph)
        {
            Assert.AreNotEqual(null, ph);

            ProcessorContainer procs = new ProcessorContainer(ph.Processors.ToArray());
            Assert.AreNotEqual(null, procs);
            Assert.AreEqual(procs.Width, 1);
            Assert.AreEqual(procs.Height, 1);

            CheckProcValue(ProcessorContainerToList(procs), (new SignValue(2), "A"), (new SignValue(3), "B"), (new SignValue(4), "C"));

            EqualOr(ph.ToString(), "ABC");
        }

        static void EqualOr(string needCheck, string checkerChars)
        {
            if (string.IsNullOrEmpty(needCheck))
                throw new ArgumentException($@"{nameof(EqualOr)}: Parameter {nameof(needCheck)} is null or empty.", nameof(needCheck));

            if (string.IsNullOrEmpty(checkerChars))
                throw new ArgumentException($@"{nameof(EqualOr)}: Parameter {nameof(checkerChars)} is null or empty.", nameof(checkerChars));

            Assert.AreEqual(checkerChars.Length, needCheck.Length);

            Assert.AreEqual(true, needCheck.All(checking => checkerChars.Any(checker => checking == checker)));
        }

        static void CheckProcValue(IList<Processor> procs, IEnumerable<(SignValue, string)> values)
        {
            CheckProcValue(procs, values.ToArray());
        }

        static void CheckProcValue(IList<Processor> procs, params (SignValue sv, string tag)[] values)
        {
            if (procs == null)
                throw new ArgumentNullException(nameof(procs), $@"{nameof(CheckProcValue)}: Parameter {nameof(procs)} is null.");

            if (values == null)
                throw new ArgumentNullException(nameof(values), $@"{nameof(CheckProcValue)}: Parameter {nameof(values)} is null.");

            Assert.AreEqual(procs.Count, values.Length);

            Assert.AreEqual(true, procs.All(checking => values.Any(checker => checking[0, 0] == checker.sv && checking.Tag == checker.tag)));
        }

        [TestMethod]
        public void PHTest()
        {
            ProcessorHandler ph = new ProcessorHandler();
            Assert.AreEqual(string.Empty, ph.ToString());
            Assert.AreEqual(0, ph.Processors.Count());

            ph.Add(new Processor(new[] { new SignValue(2) }, "A"));

            List<Processor> procs = CheckProcessorHandler1(ph);
            procs.Add(new Processor(new[] { new SignValue(2000) }, "w"));
            CheckProcessorHandler1(ph);

            ph.Add(new Processor(new[] { new SignValue(3) }, "b"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            List<(SignValue sv, string tag)> checkList = new List<(SignValue sv, string tag)>(20);

            checkList.AddRange(new[] { (new SignValue(2), "A"), (new SignValue(3), "B") });

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "AB");

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

            checkList.AddRange(new[] { (new SignValue(4), "C"), (new SignValue(13), "B0") });

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCB");

            ph.Add(new Processor(new[] { new SignValue(14) }, "B1"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            checkList.Add((new SignValue(14), "B1"));

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCBB");

            ph.Add(new Processor(new[] { new SignValue(15) }, "B"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            checkList.Add((new SignValue(15), "B2"));

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCBBB");

            ph.Add(new Processor(new[] { new SignValue(16) }, "b"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            checkList.Add((new SignValue(16), "B3"));

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCBBBB");

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

            checkList.Add((new SignValue(3), "R"));

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCBBBBR");

            ph.Add(new Processor(new[] { new SignValue(2) }, "v1"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            checkList.Add((new SignValue(2), "V"));

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCBBBBRV");

            ph.Add(new Processor(new[] { new SignValue(8418982) }, "q"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            checkList.Add((new SignValue(8418982), "Q"));

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCBBBBRVQ");

            ph.Add(new Processor(new[] { new SignValue(12451893) }, "q"));

            procs = ph.Processors.ToList();
            Assert.AreNotEqual(null, procs);
            CheckSize(procs);

            checkList.Add((new SignValue(12451893), "Q0"));

            CheckProcValue(procs, checkList);

            EqualOr(ph.ToString(), "ABCBBBBRVQQ");

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
