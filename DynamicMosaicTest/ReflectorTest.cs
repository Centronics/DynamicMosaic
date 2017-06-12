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
            Reflector reflector = new Reflector(new Reflex(new ProcessorContainer(
                new Processor(new[] { SignValue.MinValue }, "1"),
                new Processor(new[] { SignValue.MaxValue }, "2"))));
            Assert.AreEqual(false, reflector.IsInitialized);
            Assert.AreEqual(0, reflector.CountQuery);
            Assert.AreEqual(0, reflector.InitializeQuery.Count());
            reflector.Initialize();
            //reflector.Add("");
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