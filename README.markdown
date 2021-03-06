Structural Regular Expressions in C#
====================================

From [Cook Computing blog 1st Aug 2010][0]:

When I read Joe Gregorio's [sregex: Structural Regular Expressions in Python][1] last October I thought porting his code to C# would provide an interesting exercise in C# functional style coding. Structural regular expressions were originally described in a paper by Rob Pike (available [here][2]). Joe's [sregex][3] project page describes like this:
>Structural regular expressions work by describing the shape of the whole string, not just the piece you want to match. Each pattern is a list of operators to perform on a string, each time constraining the range of text that matches the pattern. Examples will make this much clearer.

>The first operator to consider is the x// operator, which means e(x)tract. When applied to a string, all the substrings that match the regular expression between // are passed on to the next operator in the pattern.

>Given the source string "Atom-Powered Robots Run Amok" and the pattern "x/A.../" the result would be ['Atom', 'Amok']. The sregex module does that using the 'sres' function:

>» list(sres("Atom-Powered Robots Run Amok", "x/A.../"))
['Atom', 'Amok']

>A pattern can contain mulitple operators, separated by whitespace, which are applied in order, each to the result of the previous match.

>» list(sres("Atom-Powered Robots Run Amok", "x/A.../ x/.*m$/"))
['Atom']

>There are four operators in total:

>x/regex/ - Matches all the text that matches the regular expression
>y/regex/ - Matches all the text that does not match the regular expression
>g/regex/ - If the regex finds a match in the string then the whole string is passed along.
>v/regex/ - If the regex does not find a match in the string then the whole string is passed

I ported Joe's code to C# keeping the general structure of the code the same and using Linq where possible. The C# version is used like this:

```c#
string src = "Atom-Powered Robots Run Amok";
Console.WriteLine(string.Join(", ", sregex.sres(src, "y/ / x/R.*/")));
Console.WriteLine(sregex.sub(src, "y/( |-)/ v/^R/ g/om/", "Coal"));
Console.WriteLine(sregex.sub(src, "x/A.../", s => s.ToUpper()));

/* Outputs to console:
     
Robots, Run
Coal-Powered Robots Run Amok
ATOM-Powered Robots Run AMOK
*/ 
```
Some unit tests illustrate usage further:

```c#
const string src = "Atom-Powered Robots Run Amok";

[TestMethod]
public void sres1()
{
  string[] result = sregex.sres(src, "y/ /").ToArray();
  CollectionAssert.AreEqual(new string[] { "Atom-Powered", "Robots", "Run", "Amok" }, result);
}

[TestMethod]
public void sres2()
{
  string[] result = sregex.sres(src, "y/( |-)/").ToArray();
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
  string[] result = sregex.sres(src, "y/ / x/R./").ToArray();
  CollectionAssert.AreEqual(new string[] { "Ro", "Ru" }, result);
}

[TestMethod]
public void sres5()
{
  string[] result = sregex.sres(src, "y/( |-)/ v/^R/").ToArray();
  CollectionAssert.AreEqual(new string[] { "Atom", "Powered", "Amok" }, result);
}

[TestMethod]
public void sres6()
{
  string[] result = sregex.sres(src, "y/( |-)/ v/^R/ g/om/").ToArray();
  CollectionAssert.AreEqual(new string[] { "Atom" }, result);
}

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

[TestMethod]
public void sre1()
{
  Range[] result = sregex.sre(src, "y/ / v/^R/ g/om/").ToArray();
  Assert.AreEqual(1, result.Length);
  Assert.AreEqual(new Range(0, 12), result.First());
}
```

[0]: http://www.cookcomputing.com/blog/archives/structural-regular-expressions-in-csharp
[1]: http://bitworking.org/news/2009/10/sregex
[2]: http://doc.cat-v.org/bell_labs/structural_regexps/
[3]: http://code.google.com/p/sregex/

