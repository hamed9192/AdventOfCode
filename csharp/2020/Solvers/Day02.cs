﻿using System;
using System.Runtime.CompilerServices;
using AdventOfCode.CSharp.Common;
using Microsoft.Toolkit.HighPerformance;

namespace AdventOfCode.CSharp.Y2020.Solvers;

public class Day02 : ISolver
{
    public void Solve(ReadOnlySpan<char> input, Solution solution)
    {
        int part1 = 0;
        int part2 = 0;

        foreach (ReadOnlySpan<char> line in input.Split('\n'))
        {
            int i = 0;
            int left = ParsePosInt(line, until: '-', ref i);
            int right = ParsePosInt(line, until: ' ', ref i);
            char letter = line[i];
            ReadOnlySpan<char> password = line.Slice(i + 3);

            int letterCount = password.Count(letter);
            if (left <= letterCount && letterCount <= right)
            {
                part1++;
            }

            if (password[left - 1] == letter ^ password[right - 1] == letter)
            {
                part2++;
            }
        }

        solution.SubmitPart1(part1);
        solution.SubmitPart2(part2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static int ParsePosInt(ReadOnlySpan<char> str, char until, ref int i)
    {
        char c = str[i++];
        int num = c - '0';
        while ((c = str[i++]) != until)
        {
            num = num * 10 + (c - '0');
        }

        return num;
    }
}
