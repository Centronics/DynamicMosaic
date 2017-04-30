using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicMosaic;

namespace DynamicMosaicTest
{
    [TestClass]
    public class DynamicMosaicTest
    {
        [TestMethod]
        public void ReflexTest1()
        {
            DynamicProcessor.SignValue[,] map = new DynamicProcessor.SignValue[4, 4];
            map[0, 0] = DynamicProcessor.SignValue.MaxValue;
            map[2, 0] = DynamicProcessor.SignValue.MaxValue;
            map[1, 1] = DynamicProcessor.SignValue.MaxValue;
            map[2, 1] = DynamicProcessor.SignValue.MaxValue;
            map[0, 2] = DynamicProcessor.SignValue.MaxValue;
            map[2, 2] = DynamicProcessor.SignValue.MaxValue;
            map[3, 3] = DynamicProcessor.SignValue.MaxValue;
            DynamicProcessor.SignValue[,] mapA = new DynamicProcessor.SignValue[2, 2];
            mapA[0, 0] = DynamicProcessor.SignValue.MaxValue;
            mapA[0, 1] = DynamicProcessor.SignValue.MaxValue;
            DynamicProcessor.SignValue[,] mapB = new DynamicProcessor.SignValue[2, 2];
            mapB[1, 1] = DynamicProcessor.SignValue.MaxValue;
            DynamicProcessor.SignValue[,] mapC = new DynamicProcessor.SignValue[2, 2];
            mapC[0, 0] = DynamicProcessor.SignValue.MaxValue;
            mapC[0, 1] = DynamicProcessor.SignValue.MaxValue;
            DynamicProcessor.SignValue[,] mapD = new DynamicProcessor.SignValue[2, 2];
            mapD[0, 0] = DynamicProcessor.SignValue.MaxValue;
            mapD[0, 1] = DynamicProcessor.SignValue.MaxValue;
            mapD[1, 0] = DynamicProcessor.SignValue.MaxValue;
            mapD[1, 1] = DynamicProcessor.SignValue.MaxValue;
            DynamicProcessor.SignValue[,] mapE = new DynamicProcessor.SignValue[2, 2];
            DynamicParser.Processor main = new DynamicParser.Processor(map, "main");
            DynamicParser.Processor procA = new DynamicParser.Processor(mapA, "A");
            DynamicParser.Processor procB = new DynamicParser.Processor(mapB, "B");
            DynamicParser.Processor procC = new DynamicParser.Processor(mapC, "C");
            DynamicParser.Processor procD = new DynamicParser.Processor(mapD, "D");
            DynamicParser.Processor procE = new DynamicParser.Processor(mapE, "E");
            Reflex reflex = new Reflex();
            reflex.Add(procA, procB, procC, procD, procE);
            reflex.Add("A", "C", "D", "B", "E");
            Reflex r = reflex.FindWord(main, "ABC");
            Reflex r1 = r.FindWord(main, "ABEF");
            //поставить проверки
            ConcurrentBag<string> strings0 = reflex.FindWord("ABCDE");
            ConcurrentBag<string> strings1 = r.FindWord("ABCDE");
            ConcurrentBag<string> strings2 = r1.FindWord("ABCDE");
        }
    }
}