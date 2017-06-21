using System;
using System.Linq;
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
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreNotSame(reflector.SourceReflex, reflex);
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(false, reflector.IsInitialized);
            reflector.Add("13a", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "t"));
            reflector.Add("24b", new Processor(new[] { new SignValue(SignValue.MinValue + 1) }, "y"));
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            reflector.Initialize(); //Добавить проверку полноты инициализации, если все непрошли, то исключение или false.
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "1"));
            //Необходимо добавить проверку на допустимость запроса с учётом длины запрашиваемой строки либо убрать startIndex и count.
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "2"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "2"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "r"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3a", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3a", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4b", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4b", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3A", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3A", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4B", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4B", 1, 2));
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            reflector.Initialize();
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "1"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "2"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "2"));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "r"));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3a", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3a", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4b", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4b", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "3A", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(new Processor(new[] { SignValue.MaxValue }, "b"), "3A", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "f"), "4B", 1, 2));
            Assert.AreEqual(false, reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "p"), "4B", 1, 2));
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.SourceReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflex.ProcessorsBase.Count(), reflector.SourceReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflex.Processors.Count(), reflector.SourceReflex.CountProcessors);
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
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreNotSame(reflector.SourceReflex, reflex);
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(false, reflector.IsInitialized);
            reflector.Add("A", procA);
            reflector.Add("B", procB);
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            reflector.Initialize();
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(true, reflector.FindRelation(main, "a"));
            Assert.AreEqual(true, reflector.FindRelation(main, "b"));
            Assert.AreEqual(false, reflector.FindRelation(main, "B"));
            Assert.AreEqual(false, reflector.FindRelation(main, "A"));
            Assert.AreEqual(true, reflector.FindRelation(main, "AB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "BA"));
            Assert.AreEqual(false, reflector.FindRelation(main, "AA"));
            Assert.AreEqual(false, reflector.FindRelation(main, "BB"));
            Assert.AreEqual(true, reflector.FindRelation(main, "AAA"));
            Assert.AreEqual(true, reflector.FindRelation(main, "BBB", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(main, "VFMP", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(main, "MPVF", 1, 2));
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            reflector.Initialize();
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
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
            Assert.AreEqual(true, reflector.FindRelation(main, "VFMP", 1, 2));
            Assert.AreEqual(true, reflector.FindRelation(main, "MPVF", 1, 2));
            Assert.AreEqual(true, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.SourceReflex.CountProcessorsBase);
            Assert.AreEqual(2, reflector.SourceReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflex.ProcessorsBase.Count(), reflector.SourceReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflex.Processors.Count(), reflector.SourceReflex.CountProcessors);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExceptionTest()
        {
            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MinValue }, "1"), new Processor(new[] { SignValue.MaxValue }, "2")));
            Reflector reflector = new Reflector(reflex);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreNotSame(reflector.SourceReflex, reflex);
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(false, reflector.IsInitialized);
            reflector.Add("1", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "t"));
            reflector.Add("2", new Processor(new[] { new SignValue(SignValue.MaxValue - 1) }, "y"));
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "1");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExceptionTest1()
        {
            Reflex reflex = new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MinValue }, "1"), new Processor(new[] { SignValue.MaxValue }, "2")));
            Reflector reflector = new Reflector(reflex);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreNotSame(reflector.SourceReflex, reflex);
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(0, reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.Reflexs.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.Reflexs.Count(), reflector.SourceReflexCollection.CountReflexs);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count());
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(0, reflector.SourceReflexCollection.StartReflex.Processors.Count());
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.ProcessorsBase.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessorsBase);
            Assert.AreEqual(reflector.SourceReflexCollection.StartReflex.Processors.Count(), reflector.SourceReflexCollection.StartReflex.CountProcessors);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(2, reflector.CountQuery);
            Assert.AreEqual(2, reflector.InitializeQuery.Count());
            Assert.AreEqual(reflector.InitializeQuery.Count(), reflector.CountQuery);
            reflector.FindRelation(new Processor(new[] { SignValue.MinValue }, "z"), "1");
        }

        [TestMethod]
        public void ReflectorContainsTest()
        {
            Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(new Processor(new[] { SignValue.MinValue }, "p"),
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
            Assert.AreEqual(false, reflector.Contains("az"));
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