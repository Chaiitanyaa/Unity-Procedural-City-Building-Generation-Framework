using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralBuilding/Building Theme")]
public class BuildingTheme : ScriptableObject
{
    [Header("Core Prefabs")]
    public GameObject wallPrefab;
    public GameObject windowPrefab;
    public GameObject doorPrefab;
    public GameObject floorPrefab;
}