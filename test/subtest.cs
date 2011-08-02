using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
  [TestClass]
  public class subtest
  {
    const string src = "Atom-Powered Robots Run Amok";

    [TestMethod]
    public void sub1()
    {
      string result = sregex.sub(src, "y/( |-)/ v/^R/ g/om/", "Coal");
      Assert.AreEqual("Coal-Powered Robots Run Amok", result);
    }

    [TestMethod]
    public void sub2()
    {
      string result = sregex.sub(src, "x/A.../", x => x.ToUpper());
      Assert.AreEqual("ATOM-Powered Robots Run AMOK", result);
    }
  }
}
