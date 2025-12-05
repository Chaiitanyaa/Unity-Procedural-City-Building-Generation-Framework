using System.Collections.Generic;
using UnityEngine;

public class ProceduralBuildingGenerator : MonoBehaviour
{
    [Header("Grammar")]
    public GrammarAsset grammarAsset;

    [TextArea(3, 10)]
    public string lastExpandedSentence;

    [Header("Turtle Settings")]
    public float stepLength = 4f;
    public float wallHeight = 3f;
    public float floorHeight = 3.2f; // wallHeight + a little gap for floor slab

    [Tooltip("Yaw rotation in degrees for + / -")]
    public float yawAngle = 90f;

    [Tooltip("Pitch rotation in degrees for & / ^")]
    public float pitchAngle = 90f;

    [Tooltip("Roll rotation in degrees for \\ / /")]
    public float rollAngle = 90f;

    //[Header("Symbol Prefab Mapping")]
    [System.Serializable]
    public struct SymbolPrefabMapping
    {
        public char symbol;
        public GameObject prefab;
        public Vector3 localOffset;
    }

    public SymbolPrefabMapping[] mappings;

    private Dictionary<char, SymbolPrefabMapping> mappingLookup;

    private struct TurtleState
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    private Stack<TurtleState> stateStack = new Stack<TurtleState>();

    [ContextMenu("Generate Now")]
    public void Generate()
    {
        if (grammarAsset == null)
        {
            Debug.LogError("No GrammarAsset assigned.");
            return;
        }

        // Clear previous children (simple version, improve with pooling later)
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // Expand grammar
        lastExpandedSentence = grammarAsset.Expand();

        // Build mapping lookup for speed
        BuildMappingLookup();

        // Initialise turtle at origin of this GameObject
        TurtleState current = new TurtleState
        {
            position = transform.position,
            rotation = transform.rotation
        };

        stateStack.Clear();

        foreach (char c in lastExpandedSentence)
        {
            switch (c)
            {
                // Move forward & optionally place a default segment (e.g. wall)
                case 'F':
                PlacePrefabIfMapped('F', current);
                current.position += current.rotation * Vector3.forward * stepLength;
                break;

                case 'D':
                    PlacePrefabIfMapped('D', current);
                    current.position += current.rotation * Vector3.forward * stepLength;
                    break;

                case 'W':
                    PlacePrefabIfMapped('W', current);
                    current.position += current.rotation * Vector3.forward * stepLength;
                    break;

                // Move up one floor (no prefab necessary here)
                case 'U':
                    current.position += Vector3.up * floorHeight;
                    break;

                case 'C':
                case 'S': // Spawn point
                    PlacePrefabIfMapped(c, current);
                    break;

                // Branch push
                case '[':
                    stateStack.Push(current);
                    break;

                // Branch pop
                case ']':
                    if (stateStack.Count > 0)
                        current = stateStack.Pop();
                    break;

                // Yaw
                case '+':
                    current.rotation = current.rotation * Quaternion.Euler(0f, yawAngle, 0f);
                    break;
                case '-':
                    current.rotation = current.rotation * Quaternion.Euler(0f, -yawAngle, 0f);
                    break;

                // Pitch
                case '&':
                    current.rotation = current.rotation * Quaternion.Euler(pitchAngle, 0f, 0f);
                    break;
                case '^':
                    current.rotation = current.rotation * Quaternion.Euler(-pitchAngle, 0f, 0f);
                    break;

                // Roll
                case '\\':
                    current.rotation = current.rotation * Quaternion.Euler(0f, 0f, rollAngle);
                    break;
                case '/':
                    current.rotation = current.rotation * Quaternion.Euler(0f, 0f, -rollAngle);
                    break;

                default:
                    // Ignore unknown symbols for now
                    break;
            }
        }
    }

    private void BuildMappingLookup()
    {
        mappingLookup = new Dictionary<char, SymbolPrefabMapping>();
        foreach (var m in mappings)
        {
            if (!mappingLookup.ContainsKey(m.symbol))
                mappingLookup.Add(m.symbol, m);
        }
    }

    private void PlacePrefabIfMapped(char symbol, TurtleState turtle)
    {
        if (mappingLookup == null || !mappingLookup.ContainsKey(symbol))
            return;

        var m = mappingLookup[symbol];
        if (m.prefab == null)
            return;

        Vector3 worldPos = turtle.position + turtle.rotation * m.localOffset;
        Quaternion worldRot = turtle.rotation;

        GameObject instance = Instantiate(m.prefab, worldPos, worldRot, transform);
        instance.name = $"{symbol}_Segment";
    }
}