﻿using AdventOfCode.CSharp.Y2021.Solvers;
using Xunit;

namespace AdventOfCode.CSharp.Tests;

public class Y2021Tests
{
    [Fact] public void Day01() => TestHelpers.AssertDay<Day01>("1167", "1130");
    [Fact] public void Day02() => TestHelpers.AssertDay<Day02>("1488669", "1176514794");
    [Fact] public void Day03() => TestHelpers.AssertDay<Day03>("738234", "3969126");
    [Fact] public void Day04() => TestHelpers.AssertDay<Day04>("51034", "5434");
    [Fact] public void Day05() => TestHelpers.AssertDay<Day05>("4993", "21101");
    [Fact] public void Day06() => TestHelpers.AssertDay<Day06>("375482", "1689540415957");
    [Fact] public void Day07() => TestHelpers.AssertDay<Day07>("355764", "99634572");
    [Fact] public void Day08() => TestHelpers.AssertDay<Day08>("310", "915941");
    [Fact] public void Day09() => TestHelpers.AssertDay<Day09>("528", "920448");
    [Fact] public void Day10() => TestHelpers.AssertDay<Day10>("278475", "3015539998");
    [Fact] public void Day11() => TestHelpers.AssertDay<Day11>("1562", "268");
    [Fact] public void Day12() => TestHelpers.AssertDay<Day12>("3576", "84271");
}