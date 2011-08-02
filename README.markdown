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
[0]: http://www.cookcomputing.com/blog/archives/structural-regular-expressions-in-csharp
[1]: http://bitworking.org/news/2009/10/sregex
[2]: http://doc.cat-v.org/bell_labs/structural_regexps/
[3]: http://code.google.com/p/sregex/
