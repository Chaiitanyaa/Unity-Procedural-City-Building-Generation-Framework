# Procedural City & Building Generation Framework (Unity)

**Author:** Chaitanyaa Chopraa  
**Version:** 1.0  
**Engine:** Unity 2022 LTS or later  

A modular procedural generation toolkit for Unity that uses **L-systems** to generate buildings and full city layouts. The framework supports grammar-driven structure, modular theming, and city-scale road generation with real-time interaction.

---

## Features

- **L-System Building Generator**
  - Grammar-based procedural buildings
  - Turtle-based geometric interpretation
  - Supports walls, windows, doors, and floors
  - Configurable iterations and random seeds

- **Modular Theme System**
  - Swap building styles without changing grammar
  - Example themes included: **Brick** and **Rock**
  - Implemented using ScriptableObjects

- **L-System Road City Generator**
  - Separate L-system for city road networks
  - Discrete grid-based road placement
  - Buildings spawned along clean road segments
  - Intersection filtering for improved visual clarity

- **Interactive Demo Scenes**
  - Keyboard-driven generation
  - Free camera exploration
  - Real-time FPS display
  - On-screen controls overlay

---

## Package Structure

```text
Assets/
├── Scripts/
│   ├── GrammarAsset.cs
│   ├── ProceduralBuildingGenerator.cs
│   ├── LSystemRoadCityGenerator.cs
│   ├── ThemeSwapper.cs
│   ├── WalkCamera.cs
│   ├── CityKeyboardController.cs
│   ├── FPSDisplay.cs
│   └── DisplayControls.cs
│
├── Prefabs/
│   ├── Building/
│   ├── Roads/
│   └── Floors/
│
├── Themes/
│   ├── BrickTheme.asset
│   └── RockTheme.asset
│
├── Grammars/
│   ├── BuildingGrammar.asset
│   └── RoadGrammar.asset
│
├── Scenes/
│   ├── Scene1_GrammarShowcase.unity
│   ├── Scene2_ModularityCustomization.unity
│   └── Scene3_LSysCityGenerator.unity
│
├── Materials/
└── Documentation/
    └── TechnicalDocumentation.pdf

---

## Getting Started

### Requirements

- Unity **2022 LTS** or later
- Desktop platform (Windows / macOS)

### Installation

1. Open Unity Hub and create or open a project.
2. Import the provided `.unitypackage`
   - `Assets → Import Package → Custom Package`
3. Confirm that all folders listed above appear in `Assets/`.

---

## Demo Scenes

### Scene1_GrammarShowcase

Demonstrates how different grammars and parameters affect building structure.

**Controls**
- `1 / 2 / 3` — Switch grammar assets
- `Arrow Keys` — Adjust iteration count and seed
- `R` — Toggle random seed mode
- `Space` — Generate building
- WalkCamera enabled (WASD / QE / Arrow keys)

---

### Scene2_ModularityCustomization

Demonstrates separation of structure and appearance.

**What to try**
- Edit production rules in a `GrammarAsset`
- Swap between **Brick** and **Rock** themes
- Regenerate to see the same structure with different visuals

**Controls**
- `Space` — Regenerate building
- WalkCamera enabled

---

### Scene3_LSysCityGenerator

Demonstrates city-scale procedural generation.

**Controls**
- `C` — Generate a new city
- `X` — Clear city
- `W A S D` — Move camera
- `Q / E` — Move down / up
- `Arrow Keys` — Look around
- `Shift` — Move faster

FPS and control overlays are visible during runtime.

---

## Core Components

### GrammarAsset
Defines an L-system grammar:
- Axiom
- Production rules
- Iteration count
- Random seed (`-1` = fully random)

### ProceduralBuildingGenerator
Interprets grammar strings using a turtle model and instantiates prefabs.

### BuildingTheme
ScriptableObject mapping walls, windows, doors, and floors to prefabs.

### LSystemRoadCityGenerator
Generates road layouts using an L-system and spawns buildings along clean road segments.

### WalkCamera
Keyboard-controlled free camera used across all demo scenes.

---

## Algorithms Used

- **L-Systems (Lindenmayer Systems)** for symbolic structure generation
- **Turtle Graphics** for geometric interpretation
- **Discrete Grid Representation** for city road layouts
- **Heuristic Placement Rules** to avoid intersections and reduce overlap

Detailed mathematical explanations are provided in the accompanying technical documentation.

---

## Performance

- Real-time generation suitable for interactive use
- City layouts with dozens to hundreds of buildings maintain **60+ FPS**
- Generation cost occurs during regeneration, not per frame

---

## Limitations

- Building placement is heuristic-based and may overlap in extreme configurations
- Roads are rectilinear; curved roads are not implemented
- Buildings currently feature procedural exteriors only

---

## Future Work

- Collision-aware building placement
- Curved and hierarchical road networks
- Procedural interiors
- GPU instancing for large-scale cities

---

## References

- Prusinkiewicz, P., & Lindenmayer, A. *The Algorithmic Beauty of Plants*, Springer, 1990  
- Parish, Y. I. H., & Müller, P. *Procedural Modeling of Cities*, SIGGRAPH, 2001  
- Unity Documentation: https://docs.unity3d.com

---