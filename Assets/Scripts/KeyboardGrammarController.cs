using UnityEngine;

public class KeyboardGrammarController : MonoBehaviour
{
    [Header("Core")]
    public ProceduralBuildingGenerator generator;
    public GrammarAsset[] grammars;   // assign 3 grammars in inspector

    private int currentGrammarIndex = 0;

    private GrammarAsset CurrentGrammar => generator != null ? generator.grammarAsset : null;

    void Start()
    {
        if (grammars != null && grammars.Length > 0 && generator != null)
        {
            LoadGrammar(0, generate: false);
            Debug.Log("Keyboard controller ready. Press 1/2/3 to pick a grammar, SPACE to generate.");
        }
    }

    void Update()
    {
        if (!Application.isPlaying || generator == null || grammars == null || grammars.Length == 0)
            return;

        // ---------- Grammar switching ----------
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadGrammar(0, true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadGrammar(1, true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadGrammar(2, true);
            return;
        }

        // ---------- Iterations (clamp to 3 minimum) ----------
        if (CurrentGrammar != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CurrentGrammar.iterations++;
                Debug.Log($"[{CurrentGrammar.name}] Iterations: {CurrentGrammar.iterations}");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                CurrentGrammar.iterations = Mathf.Max(3, CurrentGrammar.iterations - 1);
                Debug.Log($"[{CurrentGrammar.name}] Iterations: {CurrentGrammar.iterations} (Min = 3)");
            }
        }

        // ---------- R: Toggle random / fixed seed ----------
        if (CurrentGrammar != null && Input.GetKeyDown(KeyCode.R))
        {
            if (CurrentGrammar.randomSeed == -1)
            {
                CurrentGrammar.randomSeed = 0; // deterministic mode
                Debug.Log($"[{CurrentGrammar.name}] Random OFF (seed = 0)");
            }
            else
            {
                CurrentGrammar.randomSeed = -1; // random mode
                Debug.Log($"[{CurrentGrammar.name}] Random ON (seed = -1)");
            }
        }

        // ---------- Left/Right: Change seed, allow going to -1 ----------
        if (CurrentGrammar != null)
        {
            // Left arrow → decrement seed (can go down to -1)
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CurrentGrammar.randomSeed--;
                if (CurrentGrammar.randomSeed < -1)
                    CurrentGrammar.randomSeed = -1;

                Debug.Log($"[{CurrentGrammar.name}] Seed changed: {CurrentGrammar.randomSeed}");
            }

            // Right arrow → increment seed (can't increase random mode)
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (CurrentGrammar.randomSeed < 0)
                {
                    // You can't increase -1 directly, first go to 0
                    CurrentGrammar.randomSeed = 0;
                }
                else
                {
                    CurrentGrammar.randomSeed++;
                }

                Debug.Log($"[{CurrentGrammar.name}] Seed changed: {CurrentGrammar.randomSeed}");
            }
        }

        // ---------- SPACE = regenerate ----------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Regenerate();
        }
    }

    void LoadGrammar(int index, bool generate)
    {
        if (index < 0 || index >= grammars.Length) return;

        currentGrammarIndex = index;
        generator.grammarAsset = grammars[index];

        // Ensure minimum iterations
        if (generator.grammarAsset.iterations < 3)
            generator.grammarAsset.iterations = 3;

        Debug.Log($"Switched to grammar [{index + 1}]: {grammars[index].name}" +
                  $"\nIterations = {grammars[index].iterations}, Seed = {grammars[index].randomSeed}");

        if (generate)
            Regenerate();
    }

    void Regenerate()
    {
        if (generator == null || CurrentGrammar == null) return;

        generator.Generate();

        int len = string.IsNullOrEmpty(generator.lastExpandedSentence)
            ? 0
            : generator.lastExpandedSentence.Length;

        Debug.Log($"[GENERATED]" +
                  $"\nGrammar: {CurrentGrammar.name}" +
                  $"\nIterations: {CurrentGrammar.iterations}" +
                  $"\nSeed: {CurrentGrammar.randomSeed}" +
                  $"\nSentence Length: {len}");
    }
}
