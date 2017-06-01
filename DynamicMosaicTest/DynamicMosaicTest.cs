using DynamicMosaic;
using DynamicParser;
using DynamicProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor = DynamicParser.Processor;

namespace DynamicMosaicTest
{
    [TestClass]
    public class DynamicMosaicTest
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
            Reflector reflector = new Reflector(reflex);
            reflector.Add("A", procA);
            reflector.Add("B", procB);
            reflector.Add("C", procC);
            reflector.Add("D", procD);
            reflector.Add("E", procE);
            Assert.AreEqual(true, reflector.FindRelation("A"));
            Assert.AreEqual(true, reflector.FindRelation("B"));
            Assert.AreEqual(true, reflector.FindRelation("C"));
            Assert.AreEqual(true, reflector.FindRelation("D"));
            Assert.AreEqual(true, reflector.FindRelation("E"));
            Assert.AreEqual(false, reflector.FindRelation("W"));

            /*ReflexCollection rc = new ReflexCollection(reflex);
            Assert.AreEqual(true, rc.FindRelation(main, "A"));
            Assert.AreEqual(true, rc.FindRelation(main, "A"));
            Assert.AreEqual(true, rc.FindRelation(main, "B"));
            Assert.AreEqual(true, rc.FindRelation(main, "C"));
            Assert.AreEqual(true, rc.FindRelation(main, "D"));
            Assert.AreEqual(true, rc.FindRelation(main, "E"));
            Assert.AreEqual(true, rc.FindRelation(main, "E"));
            Assert.AreEqual(true, reflex.FindWord(main, "A"));
            Assert.AreEqual(true, reflex.FindWord(main, "A"));
            Assert.AreEqual(false, reflex.FindWord(main, "B"));
            Assert.AreEqual(false, reflex.FindWord(main, "B"));
            Assert.AreEqual(true, reflex.FindWord(main, "C"));
            Assert.AreEqual(true, reflex.FindWord(main, "D"));
            Assert.AreEqual(false, reflex.FindWord(main, "E"));
            Assert.AreEqual(true, reflex.FindWord(main, "A"));
            Assert.AreEqual(false, reflex.FindWord(main, "B"));
            Assert.AreEqual(true, reflex.FindWord(main, "C"));*/
        }
    }
}