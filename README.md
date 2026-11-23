# Soulbound — AI-Driven 3D Souls-Like Prototype

![Unity](https://img.shields.io/badge/Unity_6.2-161B22?style=flat-square&logo=unity&logoColor=FFFFFF)
![C#](https://img.shields.io/badge/C%23-161B22?style=flat-square&logo=dotnet&logoColor=512BD4)
![Gemini API](https://img.shields.io/badge/Gemini_API-161B22?style=flat-square&logo=googlegemini&logoColor=8E75B2)
![URP](https://img.shields.io/badge/Render_Pipeline-URP-161B22?style=flat-square)
![Thesis](https://img.shields.io/badge/B.Sc._Thesis-Wrocław_Tech_(PWr)-161B22?style=flat-square)

**Soulbound** is a 3D third-person Souls-like action RPG built for my Computer Science Engineering Thesis (*"Design and Implementation of a Game with Dynamic Text Interaction Based on an AI Language Model"*).

The project explores a **hybrid AI architecture**: combining deterministic, frame-accurate combat mechanics (custom Finite State Machine) with non-deterministic reasoning powered by Google's **Gemini API** for free-form NPC dialogue and dynamic boss decision-making.

> **Repository scope:** To keep the repository lightweight and respect third-party asset licenses, this repository contains the **C# source code and system architecture** (excluding ~20 GB of Unity 3D models, textures, and audio assets).

---

## Key Features

- **Custom Finite State Machine (FSM):** Modular state hierarchy (`State`, `StateMachine`) driving player movement, 3-hit melee combos, blocking, i-frame dodging, hit reactions, and ragdoll death.
- **LLM-Powered NPC Dialogue:** Free-form natural language chat with an in-game NPC via Gemini REST API, maintaining conversation history, lore-consistent system prompts, and neutral fallback responses on network/API failure.
- **LLM-Driven Boss AI:** The final boss queries Gemini at startup to generate thematic names for its moveset, then dynamically selects upcoming attacks during combat (`BossIdleState`), telegraphing the chosen attack name on the HUD before executing it via the deterministic FSM. Includes an automatic deterministic fallback when offline.
- **Souls-Like Combat & Camera:** Animation-event-driven hitbox/hurtbox toggling, stamina/damage rules, and dual **Cinemachine 3.14** camera modes (`FreeLook` orbit and `TargetGroup` lock-on).
- **3-Zone World & Adaptive Audio:** Linearly connected open zones (Forest → Town → Boss Arena) with NavMesh pathfinding, spatial 3D audio, and dynamic combat/exploration music switching.

---

## System Architecture

The game is structured into decoupled layers separating Unity engine services from core C# logic:

| Layer | Responsibility | Key Components |
| :--- | :--- | :--- |
| **Input** | Action mapping & context switching (Gameplay vs. UI) | `Unity Input System`, `GameInput`, `InputReader`, `UIInputReader` |
| **Game Logic & FSM** | Deterministic character control & combat rules | `PlayerStateMachine`, `EnemyStateMachine`, `BossStateMachine` |
| **Combat** | Frame-accurate hit detection, damage scaling, block/dodge immunity | Animation Events, Hitboxes/Hurtboxes, Ragdoll |
| **LLM / Network** | Async REST/JSON communication with Gemini API + fallbacks | `UnityWebRequest`, `JsonUtility`, Prompt & History Builder |
| **Presentation & Audio** | Rendering, blend trees, HUD, adaptive soundtrack | Unity URP (PBR), 2D Directional Blend Trees, Cinemachine 3.14 |
| **Data** | Checkpoint persistence & navigation data | File-based Save System, Unity NavMesh |

### State Machine Hierarchy

- **Player (`PlayerBaseState`):** `FreeLook` · `Targeting` · `Attacking` (3-hit combo) · `Blocking` · `Dodging` · `Impact` · `Dead`
- **Standard Enemy (`EnemyBaseState`):** `Idle` · `Chasing` (NavMesh) · `Attack` · `Impact` · `Dead`
- **Final Boss (`BossBaseState`):** `Waiting` · `Idle` *(LLM decision loop)* · `Chasing` · `LeftAttack` · `RightAttack` · `FireAttack` · `CrystalAttack` · `Dead`

---

## LLM Integration Flow

Both AI modules share a unified REST/JSON pipeline (`UnityWebRequest` → `POST` with `x-goog-api-key`, `systemInstruction`, and rolling `_chatHistory`):

1. **NPC Chat:** Player input → appended to conversation history with lore/persona system prompt → single JSON payload sent to Gemini → parsed response displayed in UI chat window (or neutral fallback line if offline).
2. **Boss Attack Selection:**
   - *Initialization:* Strict system prompt instructs Gemini to generate thematic attack names mapped 1:1 to implemented boss attack states.
   - *Combat Loop:* In `BossIdleState`, the boss sends a command prompt → Gemini selects the next attack name → parsed and matched against the moveset table → displayed on the HUD as a telegraph → FSM transitions to `BossChasingState` (melee) or directly to the ranged attack state.

---

## Optimization & Performance

Tested via **Unity Profiler** in the most object-dense area of the map (Town center):

- **Techniques applied:** Occlusion Culling, LOD Groups, 3D-to-2D foliage billboarding, baked lighting, GPU instancing, and static batching.
- **Measured performance (Unity Editor Play Mode):**
  - **Framerate:** `60–80 FPS` (target minimum: 30 FPS, target preferred: 60 FPS)
  - **CPU Active Time:** `16.32 ms` *(including `6.12 ms` Unity EditorLoop overhead)*
  - **GPU Time:** `10.83 ms`
  - **Memory footprint:** `~5.81 GB RAM`

---

## Tech Stack

- **Engine & Rendering:** Unity 6.2, Universal Render Pipeline (URP), Cinemachine 3.14, Unity NavMesh, Unity Input System
- **Language & IDE:** C#, JetBrains Rider 2025
- **AI / API:** Google Gemini API (REST / JSON)

---

## Author

**Marcin Musielski** (`xSilverline`)  
B.Sc. in Computer Science (Systems & Computer Networks) — Wrocław University of Science and Technology (2025)  
*Thesis Supervisor: dr inż. Piotr Sobolewski*
