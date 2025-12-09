using UnityEngine;

public class ThemeSwapper : MonoBehaviour
{
    [Header("Target Generator")]
    public ProceduralBuildingGenerator generator;

    [Header("Available Themes")]
    public BuildingTheme[] themes;

    [Tooltip("Index of the theme to apply from the 'themes' array.")]
    public int currentThemeIndex = 0;

    [ContextMenu("Apply Current Theme")]
    public void ApplyCurrentTheme()
    {
        if (generator == null)
        {
            Debug.LogWarning("ThemeSwapper: No generator assigned.");
            return;
        }

        if (themes == null || themes.Length == 0)
        {
            Debug.LogWarning("ThemeSwapper: No themes assigned.");
            return;
        }

        if (currentThemeIndex < 0 || currentThemeIndex >= themes.Length)
        {
            Debug.LogWarning("ThemeSwapper: currentThemeIndex out of range.");
            return;
        }

        var theme = themes[currentThemeIndex];
        if (theme == null)
        {
            Debug.LogWarning("ThemeSwapper: theme at index " + currentThemeIndex + " is null.");
            return;
        }

        generator.ApplyTheme(theme);
    }

    // Optional: auto-apply theme when you change index in Inspector
    void OnValidate()
    {
        if (generator == null || themes == null || themes.Length == 0)
            return;

        int clamped = Mathf.Clamp(currentThemeIndex, 0, themes.Length - 1);
        currentThemeIndex = clamped;

        var theme = themes[clamped];
        if (theme != null)
        {
            generator.ApplyTheme(theme);
        }
    }
}
