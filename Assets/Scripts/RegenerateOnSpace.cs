using UnityEngine;

public class RegenerateOnSpace : MonoBehaviour
{
    public ProceduralBuildingGenerator generator;

    void Update()
    {
        if (!Application.isPlaying || generator == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            generator.Generate();
        }
    }
}
