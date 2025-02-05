﻿using System;
using System.Runtime.CompilerServices;
using AdventOfCode.CSharp.Common;
using Microsoft.Toolkit.HighPerformance;

namespace AdventOfCode.CSharp.Y2020.Solvers;

public class Day19 : ISolver
{
    public void Solve(ReadOnlySpan<char> input, Solution solution)
    {
        // split input into the two sections
        int messagesStart = input.IndexOf("\n\n");
        ReadOnlySpan<char> rulesSpan = input.Slice(0, messagesStart + 1);
        ReadOnlySpan<char> messagesSpan = input.Slice(messagesStart + 2);

        // rules[n][i][j] returns the jth element of the ith subrule for rule n
        int[][][] rules = ParseRules(rulesSpan);

        // this only works for the AoC input, but all rules will always reduce to the same number of terminals
        int[] ruleLengths = GetRuleLengths(rules);

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        int rule0Len = ruleLengths[0];
        int rule42Len = ruleLengths[42];
        int rule11Len = ruleLengths[11];
        int rule31Len = ruleLengths[31];

        int part2Multiple = GCD(rule42Len, rule31Len);

        int part1 = 0;
        int part2 = 0;

        foreach (ReadOnlySpan<char> message in messagesSpan.SplitLines())
        {
            if (message.Length == rule0Len && MatchesRule(message, 0))
            {
                part1++;
            }
            else if (message.Length > rule0Len && message.Length % part2Multiple == 0)
            {
                // we take advantage of the fact that the input always contains the rule "0: 8 11"
                // and there are no other rules that use 8 or 11.
                //
                // rule 8 is just rule 42 repeating
                // rule 11 is rule 42 n times, then rule 31 n times.
                //
                // this means that we are looking for 42 * (a + b) + 31 * b where a >= 1 and b >= 1

                int num31s = 0;
                for (int i = message.Length - rule31Len; i >= 0; i -= rule31Len)
                {
                    if (!MatchesRule(message.Slice(i, rule31Len), 31))
                    {
                        break;
                    }

                    num31s++;
                }

                if (num31s == 0)
                {
                    continue;
                }

                int num42s = (message.Length - (num31s * rule31Len)) / rule42Len;
                if (num42s <= num31s)
                {
                    continue;
                }

                bool isValid = true;
                for (int i = 0; i < num42s * rule42Len; i += rule42Len)
                {
                    if (!MatchesRule(message.Slice(i, rule42Len), 42))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    part2++;
                }
            }
        }

        // all messages in part 1 are valid for part 2
        part2 += part1;

        solution.SubmitPart1(part1);
        solution.SubmitPart2(part2);

        bool MatchesRule(ReadOnlySpan<char> str, int ruleNumber)
        {
            if (ruleNumber < 0)
            {
                char c = str[0];
                return (ruleNumber == -1 && c == 'a') || (ruleNumber == -2 && c == 'b');
            }

            foreach (int[] subRule in rules[ruleNumber])
            {
                if (MatchesSubRule(str, subRule))
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool MatchesSubRule(ReadOnlySpan<char> str, int[] subRule)
        {
            int i = 0;

            foreach (int subRuleNumber in subRule)
            {
                int ruleLen = subRuleNumber < 0 ? 1 : ruleLengths[subRuleNumber];

                if (!MatchesRule(str.Slice(i, ruleLen), subRuleNumber))
                {
                    return false;
                }

                i += ruleLen;
            }

            return true;
        }
    }

    private static int[][][] ParseRules(ReadOnlySpan<char> rules)
    {
        int numRules = rules.Count('\n');
        int[][][] rulesArr = new int[numRules][][];

        var reader = new SpanReader(rules);
        while (!reader.Done)
        {
            int ruleId = reader.ReadIntUntil(':');
            reader.SkipLength(1); // skip the space
            ReadOnlySpan<char> ruleValue = reader.ReadUntil('\n');

            int numSubRules = ruleValue.Count('|') + 1;
            int[][] ruleList = new int[numSubRules][];

            int i = 0;
            foreach (ReadOnlySpan<char> subRuleSpan in ruleValue.Split(" | "))
            {
                int[] subRule = new int[subRuleSpan.Count(' ') + 1];

                int j = 0;
                foreach (ReadOnlySpan<char> subRulePart in subRuleSpan.Split(' '))
                {
                    if (subRulePart[0] == '"')
                    {
                        subRule[j++] = subRulePart[1] == 'a' ? -1 : -2;
                    }
                    else
                    {
                        subRule[j++] = int.Parse(subRulePart);
                    }
                }

                ruleList[i++] = subRule;
            }

            rulesArr[ruleId] = ruleList;
        }

        return rulesArr;
    }

    private static int[] GetRuleLengths(int[][][] rules)
    {
        int[] lengths = new int[rules.Length];

        // ensure whole array is cached
        for (int i = 0; i < rules.Length; i++)
        {
            _ = GetRuleLength(i);
        }

        return lengths;

        int GetRuleLength(int ruleNumber)
        {
            int cachedLen = lengths[ruleNumber];
            if (cachedLen != 0)
            {
                return cachedLen;
            }

            // rules always have the same length regardless of which alternative is taken
            // so we can just take the first rule
            int[] rule = rules[ruleNumber][0];

            int len = 0;
            foreach (int subRuleNumber in rule)
            {
                // negative sub-rule means it is a terminal of length 1
                len += subRuleNumber < 0 ? 1 : GetRuleLength(subRuleNumber);
            }

            return lengths[ruleNumber] = len;
        }
    }
}
