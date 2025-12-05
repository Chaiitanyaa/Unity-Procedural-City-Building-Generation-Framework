using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralBuilding/Grammar Asset")]
public class GrammarAsset : ScriptableObject
{
    [Header("Core Grammar")]
    [Tooltip("Starting string (e.g., 'A' or 'F[+F]F[-F]F')")]
    public string axiom = "A";

    [Tooltip("How many times to expand the axiom")]
    [Range(0, 10)]
    public int iterations = 3;

    [Tooltip("Random seed for probabilistic rules. Use -1 for Time-based seed.")]
    public int randomSeed = 0;

    [Serializable]
    public class Rule
    {
        [Tooltip("The symbol on the left side of the production rule.")]
        public char predecessor = 'A';

        [Tooltip("All possible replacements for this predecessor.")]
        public string[] successors;

        [Tooltip("Optional weights for each successor. Must match length of successors or be empty.")]
        public float[] weights;
    }

    [Header("Production Rules")]
    public List<Rule> rules = new List<Rule>();

    /// <summary>
    /// Expands the axiom using the rules and returns the final sentence.
    /// </summary>
    public string Expand()
    {
        // Setup RNG
        System.Random rng;
        if (randomSeed == -1)
            rng = new System.Random();
        else
            rng = new System.Random(randomSeed);

        string current = axiom;

        for (int i = 0; i < iterations; i++)
        {
            current = ApplyRulesOnce(current, rng);
        }

        return current;
    }

    private string ApplyRulesOnce(string input, System.Random rng)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder(input.Length * 2);

        foreach (char c in input)
        {
            string replacement = GetReplacement(c, rng);
            sb.Append(replacement);
        }

        return sb.ToString();
    }

    private string GetReplacement(char symbol, System.Random rng)
    {
        // Find rule with matching predecessor
        for (int i = 0; i < rules.Count; i++)
        {
            Rule r = rules[i];
            if (r.predecessor == symbol && r.successors != null && r.successors.Length > 0)
            {
                if (r.successors.Length == 1 || r.weights == null || r.weights.Length == 0)
                {
                    // Deterministic rule
                    return r.successors[0];
                }
                else
                {
                    // Probabilistic rule
                    return SampleWeighted(r.successors, r.weights, rng);
                }
            }
        }

        // No rule = symbol remains unchanged (standard L-system behavior)
        return symbol.ToString();
    }

    private string SampleWeighted(string[] successors, float[] weights, System.Random rng)
    {
        // If weights length mismatched, just pick uniform random
        if (weights.Length != successors.Length)
        {
            int idx = rng.Next(successors.Length);
            return successors[idx];
        }

        float total = 0f;
        for (int i = 0; i < weights.Length; i++)
            total += Mathf.Max(0f, weights[i]);

        if (total <= 0f)
        {
            int idx = rng.Next(successors.Length);
            return successors[idx];
        }

        float r = (float)(rng.NextDouble() * total);
        float accum = 0f;

        for (int i = 0; i < successors.Length; i++)
        {
            accum += Mathf.Max(0f, weights[i]);
            if (r <= accum)
                return successors[i];
        }

        return successors[successors.Length - 1];
    }
}