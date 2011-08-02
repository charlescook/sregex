using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
  [TestClass]
  public class sretest
  {
    const string src = "Atom-Powered Robots Run Amok";

    [TestMethod]
    public void sre1()
    {
      var result = sregex.sre(src, "y/ / v/^R/ g/om/").ToArray();
      Assert.AreEqual(1, result.Length);
      Assert.AreEqual(new Range(0, 12), result[0]);
    }
  }
}
