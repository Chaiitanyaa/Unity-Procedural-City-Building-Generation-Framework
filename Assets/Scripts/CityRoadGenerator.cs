using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CityRoadGenerator : MonoBehaviour
{
    [Header("Building Setup")]
    public ProceduralBuildingGenerator buildingGeneratorPrefab; // your prefab
    public GrammarAsset[] buildingGrammars;                     // optional variety
    public BuildingTheme[] buildingThemes;                      // optional variety

    [Header("Road Visuals")]
    public GameObject roadPrefab;
    public float roadWidth = 6f;
    public float roadThickness = 0.2f;

    [Header("City Area")]
    public float cityWidth = 200f;   // X range
    public float cityDepth = 200f;   // Z range

    [Header("Road Layout")]
    public int verticalRoadCount = 5;     // north-south
    public int horizontalRoadCount = 5;   // east-west
    public float minRoadSpacing = 20f;
    public float maxRoadSpacing = 40f;

    [Header("Buildings Along Roads")]
    public float buildingSpacing = 18f;
    public float sidewalkOffset = 10f;    // how far from road centerline
    public int maxBuildingsPerRoad = 30;

    [Header("Parents")]
    public Transform roadsParent;
    public Transform buildingsParent;

    private List<GameObject> spawnedRoads = new List<GameObject>();
    private List<GameObject> spawnedBuildings = new List<GameObject>();

    [ContextMenu("Generate City Now")]
    public void GenerateCity()
    {
        if (buildingGeneratorPrefab == null)
        {
            UnityEngine.Debug.LogError("CityRoadGenerator: No buildingGeneratorPrefab assigned.");
            return;
        }
        if (roadPrefab == null)
        {
            UnityEngine.Debug.LogError("CityRoadGenerator: No roadPrefab assigned.");
            return;
        }

        ClearCity();

        Stopwatch sw = Stopwatch.StartNew();

        // 1) Generate grid-like road positions
        List<float> verticalXs = GenerateRoadLines(-cityWidth * 0.5f, cityWidth * 0.5f, verticalRoadCount);
        List<float> horizontalZs = GenerateRoadLines(-cityDepth * 0.5f, cityDepth * 0.5f, horizontalRoadCount);

        // 2) Build road visuals (vertical + horizontal)
        foreach (float x in verticalXs)
        {
            Vector3 start = new Vector3(x, 0f, -cityDepth * 0.5f);
            Vector3 end   = new Vector3(x, 0f,  cityDepth * 0.5f);
            CreateRoadSegment(start, end);
            SpawnBuildingsAlongRoad(start, end);
        }

        foreach (float z in horizontalZs)
        {
            Vector3 start = new Vector3(-cityWidth * 0.5f, 0f, z);
            Vector3 end   = new Vector3( cityWidth * 0.5f, 0f, z);
            CreateRoadSegment(start, end);
            SpawnBuildingsAlongRoad(start, end);
        }

        sw.Stop();
        UnityEngine.Debug.Log($"[CityRoadGenerator] City generated in {sw.ElapsedMilliseconds} ms. " +
                              $"Roads: {spawnedRoads.Count}, Buildings: {spawnedBuildings.Count}");
    }

    // Generate sorted positions for road lines with some random spacing (planned but varied)
    private List<float> GenerateRoadLines(float minCoord, float maxCoord, int count)
    {
        List<float> coords = new List<float>();

        float span = maxCoord - minCoord;
        if (count <= 1)
        {
            coords.Add((minCoord + maxCoord) * 0.5f);
            return coords;
        }

        // Approx base spacing
        float baseSpacing = span / (count + 1);

        float current = minCoord + baseSpacing;

        for (int i = 0; i < count; i++)
        {
            float randomOffset = Random.Range(-minRoadSpacing * 0.5f, maxRoadSpacing * 0.5f);
            float pos = current + randomOffset;
            pos = Mathf.Clamp(pos, minCoord + 5f, maxCoord - 5f);
            coords.Add(pos);
            current += baseSpacing;
        }

        coords.Sort();
        return coords;
    }

    private void CreateRoadSegment(Vector3 start, Vector3 end)
    {
        Vector3 center = (start + end) * 0.5f;
        Vector3 dir = (end - start);
        float length = dir.magnitude;
        if (length < 0.1f) return;

        dir.Normalize();

        GameObject road = Instantiate(roadPrefab, center, Quaternion.LookRotation(dir),
            roadsParent != null ? roadsParent : transform);

        // Assuming road mesh is 1 unit long in Z, 1 wide in X
        road.transform.localScale = new Vector3(roadWidth, roadThickness, length);
        spawnedRoads.Add(road);
    }

    private void SpawnBuildingsAlongRoad(Vector3 start, Vector3 end)
    {
        float segmentLength = (end - start).magnitude;
        if (segmentLength < buildingSpacing) return;

        Vector3 dir = (end - start).normalized;
        Vector3 side = Vector3.Cross(Vector3.up, dir).normalized; // right-hand side

        float dist = buildingSpacing * 0.5f;
        int buildingsOnThisRoad = 0;

        while (dist < segmentLength - buildingSpacing * 0.5f &&
               buildingsOnThisRoad < maxBuildingsPerRoad)
        {
            Vector3 roadPoint = start + dir * dist;

            // Spawn on both sides
            SpawnSingleBuilding(roadPoint + side * sidewalkOffset, -side);
            SpawnSingleBuilding(roadPoint - side * sidewalkOffset,  side);

            buildingsOnThisRoad += 2;
            dist += buildingSpacing;
        }
    }

    private void SpawnSingleBuilding(Vector3 position, Vector3 forwardTowardsRoad)
    {
        if (buildingGeneratorPrefab == null) return;

        Quaternion rot = Quaternion.LookRotation(forwardTowardsRoad, Vector3.up);

        var parent = buildingsParent != null ? buildingsParent : transform;

        ProceduralBuildingGenerator building =
            Instantiate(buildingGeneratorPrefab, position, rot, parent);

        // Optional: choose random grammar
        if (buildingGrammars != null && buildingGrammars.Length > 0)
        {
            int gi = Random.Range(0, buildingGrammars.Length);
            building.grammarAsset = buildingGrammars[gi];
        }

        // Optional: choose random theme
        if (buildingThemes != null && buildingThemes.Length > 0)
        {
            int ti = Random.Range(0, buildingThemes.Length);
            building.ApplyTheme(buildingThemes[ti]);
        }

        // For variety, randomize grammar seed
        if (building.grammarAsset != null)
        {
            building.grammarAsset.randomSeed = -1;
        }

        building.Generate();
        spawnedBuildings.Add(building.gameObject);
    }

    public void ClearCity()
    {
        foreach (var r in spawnedRoads)
        {
            if (r != null)
                DestroyImmediateOrRuntime(r);
        }
        spawnedRoads.Clear();

        foreach (var b in spawnedBuildings)
        {
            if (b != null)
                DestroyImmediateOrRuntime(b);
        }
        spawnedBuildings.Clear();
    }

    private void DestroyImmediateOrRuntime(GameObject go)
    {
        if (go == null) return;
#if UNITY_EDITOR
        if (!Application.isPlaying)
            DestroyImmediate(go);
        else
            Destroy(go);
#else
        Destroy(go);
#endif
    }

    void OnDestroy()
    {
        // Clean up if script destroyed
        ClearCity();
    }
}
