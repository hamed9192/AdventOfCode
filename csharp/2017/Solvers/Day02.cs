﻿using AdventOfCode.CSharp.Common;
using System;
using System.Collections.Generic;

namespace AdventOfCode.CSharp.Y2017.Solvers;

public class Day02 : ISolver
{
    public void Solve(ReadOnlySpan<char> input, Solution solution)
    {
        int part1 = 0;
        int part2 = 0;

        var nums = new List<int>();
        foreach (ReadOnlySpan<char> row in input.SplitLines())
        {
            int minValue = int.MaxValue;
            int maxValue = int.MinValue;
            int quotient = 0;

            nums.Clear();
            foreach (ReadOnlySpan<char> cellStr in row.Split('\t'))
            {
                int cell = int.Parse(cellStr);
                minValue = Math.Min(minValue, cell);
                maxValue = Math.Max(maxValue, cell);

                if (quotient == 0)
                {
                    foreach (int num in nums)
                    {
                        if (cell % num == 0)
                        {
                            quotient = cell / num;
                            break;
                        }
                        else if (num % cell == 0)
                        {
                            quotient = num / cell;
                            break;
                        }
                    }

                    nums.Add(cell);
                }
            }

            part1 += maxValue - minValue;
            part2 += quotient;
        }

        solution.SubmitPart1(part1);
        solution.SubmitPart2(part2);
    }
}
