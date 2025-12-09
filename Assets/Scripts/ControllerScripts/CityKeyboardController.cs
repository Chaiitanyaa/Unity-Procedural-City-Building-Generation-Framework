using UnityEngine;

public class CityKeyboardController : MonoBehaviour
{
    public CityRoadGenerator city;

    void Update()
    {
        if (!Application.isPlaying || city == null) return;

        // C = generate city
        if (Input.GetKeyDown(KeyCode.C))
        {
            city.GenerateCity();
        }

        // X = clear
        if (Input.GetKeyDown(KeyCode.X))
        {
            city.ClearCity();
        }
    }
}
