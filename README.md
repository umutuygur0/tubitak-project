# tubitak-project — Mobile Educational Experience

Mobile (Android / iOS) continuation of the VR project developed for the Turkish Ministry of Education's ETKİM initiative.
The original VR version lives in the separate `vr-meb-project` repository.

The project aims to provide interactive experiences that teach social values, public behavior, and support career awareness through engaging mini-scenarios.

### Current Features
- First-person mobile player: virtual joystick to walk, drag on screen to look around (WASD + mouse drag in the Editor)
- Dialogue-based bullying scenario with interactive choices
- Police arrival and feedback based on user decisions
- Visual storytelling with animated events

### Planned Features
- Open-world navigation
- Multiple educational mini-games
- Modules on social rules, empathy, and career exploration
- Voice narration and dynamic feedback

> This is a work-in-progress educational game developed by university students to support values education in an engaging and modern way.

---

🎓 Developed with Unity 6000.6.7f1 (Built-in Render Pipeline, Input System)  
🎯 Goal: Support student development through interactive storytelling.

## Getting Started

1. Open the project with **Unity 6000.6.7f1** (Android Build Support and iOS Build Support modules required).
2. Open `Assets/Scenes/practice.unity` and press Play. Walk with **WASD**, look around by dragging with the mouse.
3. Player prefab: `Assets/Prefabs/Mobile/MobilePlayer.prefab` (tagged `Player`, triggers use this tag). Drop it into new scenes.

### Building

- **Android APK:** `Tools > TUBITAK > Build Android APK` → `Builds/Android/tubitak-project.apk`
- **iOS:** `Tools > TUBITAK > Export iOS Xcode Project` → `Builds/iOS` (open on a Mac with Xcode to sign and run)

Command line (Unity closed):

```
Unity.exe -batchmode -quit -projectPath . -buildTarget Android -executeMethod MobileBuild.BuildAndroid
```

### Unity MCP (Claude Code ↔ Unity Editor)

The project includes [MCP for Unity](https://github.com/CoplayDev/unity-mcp) and a `.mcp.json` for Claude Code.
Requires [uv](https://docs.astral.sh/uv/). In Unity open `Window > MCP for Unity` and start the server (HTTP, port 8080);
Claude Code then connects to `http://localhost:8080/mcp`.



## Backend & Database Setup

To run this project locally:

1. Install PostgreSQL

2. Create a database:
   UnityVRDecisionSave

3. Run the following SQL:

CREATE TABLE user_choices (
id SERIAL PRIMARY KEY,
user_id VARCHAR(100),
event_id VARCHAR(100),
selected_option INT,
selected_text VARCHAR(255),
created_at TIMESTAMP DEFAULT NOW(),
UNIQUE(user_id, event_id)
);

4. Open the backend project and update the connection string:

Host=localhost;Port=5432;Username=postgres;Password=YOUR_PASSWORD;Database=UnityVRDecisionSave

5. Run the API (Visual Studio)

6. Run the Unity project

API endpoint used:
http://localhost:5136/api/Choices

> On a phone `localhost` points to the phone itself — use the PC's LAN IP (e.g. `http://192.168.1.20:5136`) or a deployed HTTPS URL.
