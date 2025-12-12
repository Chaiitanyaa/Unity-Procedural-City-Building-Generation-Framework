using UnityEngine;

/// <summary>
/// Keyboard controls for Scene 3:
/// C = generate a new L-system city
/// X = clear current city
/// </summary>
public class CityKeyboardController : MonoBehaviour
{
    [Tooltip("Reference to the L-system road city generator in this scene.")]
    public LSystemRoadCityGenerator cityGenerator;

    void Update()
    {
        if (!Application.isPlaying || cityGenerator == null)
            return;

        // Generate a new city
        if (Input.GetKeyDown(KeyCode.C))
        {
            cityGenerator.GenerateCity();
        }

        // Clear everything
        if (Input.GetKeyDown(KeyCode.X))
        {
            cityGenerator.ClearCity();
        }
    }
}
