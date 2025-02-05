﻿using AdventOfCode.CSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.CSharp.Y2015.Solvers;

public class Day09 : ISolver
{
    public void Solve(ReadOnlySpan<char> input, Solution solution)
    {
        // get list of towns and edges from input
        var townSet = new HashSet<string>();
        var distances = new Dictionary<(string From, string To), int>();
        foreach (ReadOnlySpan<char> line in input.SplitLines())
        {
            ParseLine(line, out int distance, out string fromName, out string toName);

            townSet.Add(fromName);
            townSet.Add(toName);

            distances[(fromName, toName)] = distance;
            distances[(toName, fromName)] = distance;
        }

        // convert adjacency list to adjacency matrix
        string[] townNames = townSet.ToArray();
        int numTowns = townNames.Length;

        int[,] adjMatrix = new int[numTowns, numTowns];
        int[] towns = new int[numTowns];
        for (int a = 0; a < numTowns; a++)
        {
            towns[a] = a;
            for (int b = 0; b < numTowns; b++)
            {
                if (a != b)
                {
                    adjMatrix[a, b] = distances[(townNames[a], townNames[b])];
                }
            }
        }

        int minDistance = int.MaxValue;
        int maxDistance = int.MinValue;
        foreach (Span<int> perm in towns.AsSpan().GetPermutations())
        {
            int pathDist = 0;
            int prevTown = perm[0];
            for (int j = 1; j < numTowns; j++)
            {
                int town = perm[j];
                pathDist += adjMatrix[prevTown, town];
                prevTown = town;
            }

            minDistance = Math.Min(minDistance, pathDist);
            maxDistance = Math.Max(maxDistance, pathDist);
        }

        solution.SubmitPart1(minDistance);
        solution.SubmitPart2(maxDistance);
    }

    private static void ParseLine(ReadOnlySpan<char> line, out int distance, out string fromName, out string toName)
    {
        int equalsIndex = line.LastIndexOf('=');
        distance = int.Parse(line[(equalsIndex + 2)..]);

        int firstSpaceIndex = line.IndexOf(' ');
        fromName = line[..firstSpaceIndex].ToString();
        toName = line[(firstSpaceIndex + 4)..(equalsIndex - 1)].ToString();
    }
}
