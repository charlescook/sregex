using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class sregex
{
  static Regex CMD = new Regex("(?<cmd>[xygv])/(?<rgx>[^/]*)/");
  static Regex VALIDCMD
    = new Regex(@"^\s*([xygv]/[^/]*/)(\s+([xygv]/[^/]*/))*\s*$");
  delegate IEnumerable<Range> Func(IEnumerable<Range> f, string pattern);

  static bool IsPattern(string s)
  {
    return (s == "" || VALIDCMD.IsMatch(s));
  }

  static Range _makeRange(Range range, Match match)
  {
    return new Range(match.Index + range.Start,
      match.Index + match.Length + range.Start);
  }

  public static IEnumerable<Range> sre(string src, string pattern)
  {
    IEnumerable<Range> start = new Range[] { new Range(0, src.Length) };

    Func from_g = (f, pat) => f.Where(range =>
      Regex.Match(slice(src, range), pat).Success);

    Func from_v = (f, pat) => f.Where(range =>
      !Regex.Match(slice(src, range), pat).Success);

    Func from_x = (f, pat) => f.SelectMany(range =>
        Regex.Matches(slice(src, range), pat).Cast<Match>()
          .Select(m => _makeRange(range, m)));

    Func from_y = (f, pat)
      => f.SelectMany(range =>
      {
        var XXX = Regex.Matches(slice(src, range), pat).Cast<Match>().ToArray();
        return Regex.Matches(slice(src, range), pat).Cast<Match>()
            .Select(m => new Range(m.Index, m.Index + m.Length))
            .Concat(Enumerable.Repeat(new Range(range.End, range.End), 1))
            .Aggregate(new Tuple<List<Range>, Range>(new List<Range>(), range),
                (un, match) =>
                {
                  un.Item1.Add(new Range(un.Item2.Start, match.Start));
                  return new Tuple<List<Range>, Range>(un.Item1,
                      new Range(match.End, range.End));
                },
                un => un.Item1)
            .Where(r => r.Length > 0);
      });

    if (!IsPattern(pattern))
      throw new ArgumentException("Invalid expression");

    var wrapper = new Dictionary<string, Func>();
    wrapper["g"] = from_g;
    wrapper["v"] = from_v;
    wrapper["x"] = from_x;
    wrapper["y"] = from_y;

    var ret = CMD.Matches(pattern).Cast<Match>()
      .Aggregate(start, (f, m) =>
        wrapper[m.Groups["cmd"].Value](f, m.Groups["rgx"].Value));
    return ret;
  }

  static IEnumerable<T> Defer<T>(Func<IEnumerable<T>> ts)
  {
    foreach (T t in ts())
    {
      yield return t;
    }
  }

  public static IEnumerable<string> sres(string src, string pattern)
  {
    return sre(src, pattern).Select(r => src.Substring(r.Start,
      r.End - r.Start));
  }

  public static string sub(string src, string pattern,
    Func<string, string> repl)
  {
    StringBuilder sb = new StringBuilder(src);
    foreach (Range range in sre(src, pattern).Reverse())
    {
      sb.Remove(range.Start, range.Length);
      sb.Insert(range.Start,
        repl(src.Substring(range.Start, range.Length)));
    }
    return sb.ToString();
  }

  public static string sub(string src, string pattern, string repl)
  {
    return sub(src, pattern, s => repl);
  }

  static string slice(string str, Range range)
  {
    return str.Substring(range.Start, range.Length);
  }
}

public struct Range
{
  public readonly int Start;
  public readonly int End;
  public int Length { get { return End - Start; } }

  public Range(int start, int end)
  {
    Start = start;
    End = end;
  }
}