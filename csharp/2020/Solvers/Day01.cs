﻿using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Y2020.Solvers
{
    public class Day01 : ISolver
    {
        public Solution Solve(ReadOnlySpan<char> input)
        {
            int length = 0;
            byte[]? numberSet = new byte[2048];
            int[]? numbers = new int[512];
            foreach (ReadOnlySpan<char> line in input.SplitLines())
            {
                int num = line[0] - '0';
                for (int i = 1; i < line.Length; i++)
                {
                    num = num * 10 + (line[i] - '0');
                }

                numberSet[num] = 1;
                numbers[length++] = num;
            }

            Array.Sort(numbers, 0, length);

            int part1 = -1;
            int part2 = -1;
            for (int i = 0; i < length; i++)
            {
                int a = numbers[i];
                int part1B = 2020 - a;
                for (int j = i + 1; j < length; j++)
                {
                    int b = numbers[j];
                    if (b < part1B)
                    {
                        int c = part1B - b;
                        if (numberSet[c] == 1)
                        {
                            part2 = a * b * c;
                            if (part1 >= 0)
                                return new Solution(part1, part2);
                        }
                    }
                    else if (b == part1B)
                    {
                        part1 = a * b;
                        if (part2 >= 0)
                            return new Solution(part1, part2);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return new Solution(part1, part2);
        }
    }
}
