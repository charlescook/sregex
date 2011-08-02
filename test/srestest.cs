using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
  [TestClass]
  public class srestest
  {
    const string src = "Atom-Powered Robots Run Amok";

    [TestMethod]
    public void sres1()
    {
      var result = sregex.sres(src, "y/ /").ToArray();
      CollectionAssert.AreEqual(new string[] { "Atom-Powered", "Robots", "Run", "Amok" }, result);
    }

    [TestMethod]
    public void sres2()
    {
      var result = sregex.sres(src, "y/( |-)/").ToArray();
      CollectionAssert.AreEqual(new string[] { "Atom", "Powered", "Robots", "Run", "Amok" }, result);
    }

    [TestMethod]
    public void sres3()
    {
      var result = sregex.sres(src, "y/ / x/R.*/").ToArray();
      CollectionAssert.AreEqual(new string[] { "Robots", "Run" }, result);
    }

    [TestMethod]
    public void sres4()
    {
      var result = sregex.sres(src, "y/ / x/R./").ToArray();
      CollectionAssert.AreEqual(new string[] { "Ro", "Ru" }, result);
    }
    [TestMethod]
    public void sres5()
    {
      var result = sregex.sres(src, "y/( |-)/ v/^R/").ToArray();
      CollectionAssert.AreEqual(new string[] { "Atom", "Powered", "Amok" }, result);
    }
    [TestMethod]
    public void sres6()
    {
      var result = sregex.sres(src, "y/( |-)/ v/^R/ g/om/").ToArray();
      CollectionAssert.AreEqual(new string[] { "Atom" }, result);
    }
  }
}
