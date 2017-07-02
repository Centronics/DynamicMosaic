using DynamicMosaic;
using DynamicProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processor = DynamicParser.Processor;

namespace DynamicMosaicTest
{
    [TestClass]
    public class PairWordValueTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            {
                PairWordValue pw = new PairWordValue("f", new Processor(new[] { SignValue.MaxValue }, "g"));
                Assert.AreEqual(false, pw.IsEmpty);
                Assert.AreNotEqual(null, pw.Field);
                Assert.AreNotEqual(1, pw.Field.Length);
                Assert.AreEqual(SignValue.MaxValue, pw.Field[0, 0]);
                Assert.AreEqual("g", pw.Field.Tag);
                Assert.AreEqual("f", pw.FindString);
            }

            {
                PairWordValue pw = new PairWordValue(string.Empty, new Processor(new[] { SignValue.MaxValue }, "g"));
                Assert.AreEqual(true, pw.IsEmpty);
                Assert.AreNotEqual(null, pw.Field);
                Assert.AreNotEqual(1, pw.Field.Length);
                Assert.AreEqual(SignValue.MaxValue, pw.Field[0, 0]);
                Assert.AreEqual("g", pw.Field.Tag);
                Assert.AreEqual(string.Empty, pw.FindString);
            }

            {
                PairWordValue pw = new PairWordValue(null, new Processor(new[] { SignValue.MaxValue }, "g"));
                Assert.AreEqual(true, pw.IsEmpty);
                Assert.AreNotEqual(null, pw.Field);
                Assert.AreNotEqual(1, pw.Field.Length);
                Assert.AreEqual(SignValue.MaxValue, pw.Field[0, 0]);
                Assert.AreEqual("g", pw.Field.Tag);
                Assert.AreEqual(null, pw.FindString);
            }

            {
                PairWordValue pw = new PairWordValue("f", null);
                Assert.AreEqual(true, pw.IsEmpty);
                Assert.AreEqual(null, pw.Field);
                Assert.AreEqual("f", pw.FindString);
            }

            {
                PairWordValue pw = new PairWordValue(string.Empty, null);
                Assert.AreEqual(true, pw.IsEmpty);
                Assert.AreNotEqual(null, pw.Field);
                Assert.AreEqual(string.Empty, pw.FindString);
            }

            {
                PairWordValue pw = new PairWordValue(null, null);
                Assert.AreEqual(true, pw.IsEmpty);
                Assert.AreEqual(null, pw.Field);
                Assert.AreEqual(null, pw.FindString);
            }
        }
    }
}
