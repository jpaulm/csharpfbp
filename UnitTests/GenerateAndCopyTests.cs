using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FBPLib;
using Components;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class CanGenerateAndCopy : Network
    {

        /* *
               * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
               * distribute, or make derivative works under the terms of the Clarified Artistic License, 
               * based on the Everything Development Company's Artistic License.  A document describing 
               * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
               * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
               * */

        public StoreValues StoreValues;

        public override void Define() /* throws Throwable */
        {
            // component("MONITOR", Monitor.class);
            var assertions =
                Enumerable.Range(0, 100)
                          .Select(x => new Func<string, bool>(y => String.Format("{0:d6}abcd", 100 - x) == y))
                          .ToArray();

            Connect(Component("Generate", typeof(GenerateTestData)),
                Port("OUT"),
                Component("StoreValues", typeof(StoreValues)),
                Port("IN"));

            StoreValues = Component("StoreValues") as StoreValues;

            Initialize("100",
                Component("Generate"),
                Port("COUNT"));
        }

        [TestMethod]
        public async Task CanGenerateAndCopyTest()
        {
            var network = new CanGenerateAndCopy();
            await network.Go();
            var values = network.StoreValues.Values;
            Assert.AreEqual(100, values.Count);
            Assert.AreEqual("000100abcd", values[0]);
        }
    }
}
