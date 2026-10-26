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

This project uses **ASP.NET Core Web API**, **Entity Framework Core**, and **PostgreSQL** to record in-game player choices and response times for psychological analysis.

---

### Prerequisites
* [.NET 8 / .NET 9 / .NET 10 SDK](https://dotnet.microsoft.com/)
* [PostgreSQL 15+](https://www.postgresql.org/) (and pgAdmin / DBeaver)
* Visual Studio 2022 (with ASP.NET & Web Development workload)

---

### Setup Steps

#### 1. Database Creation
Open pgAdmin / DBeaver or PostgreSQL CLI and create a new database:
```sql
CREATE DATABASE "UnityQuizDecision";

### 2. Configure Local User Secrets (Recommended)
To keep your database credentials secure and out of version control, use User Secrets in Visual Studio instead of editing appsettings.json:

Right-click the UnityChoiceSave (Web API) project in Visual Studio.

Select Manage User Secrets.

Paste the following configuration with your local PostgreSQL password:

JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=UnityQuizDecision;Username=postgres;Password=YOUR_POSTGRES_PASSWORD"
  }
}

### 3. Apply Database Migrations (EF Core)
The project uses Entity Framework Core Migrations managed inside the UnityChoiceSave.DataAccess project layer.

Open Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).

Set Default project dropdown to UnityChoiceSave.DataAccess.

Run the following command to create the database schema automatically:

Bash
update-database
(Note: If setting up from scratch without existing migrations, run add-migration InitialCreate first).

Running the Project
1. Start the Backend API
Press F5 in Visual Studio to run the API.

The Swagger documentation UI will open at:https://localhost:7154/swagger (or your assigned local port).

2. Run the Unity Project
Open the Unity project.

Make sure the AnalyticsManager.cs script has the correct local endpoint configured (e.g., https://localhost:7154/api/Decisions/save).

Press Play in Unity and press T to test sending decision data directly to PostgreSQL.

API Endpoints
POST /api/Decisions/save

Payload Example:

JSON
{
  "childId": 1,
  "scenarioId": "playground_sharing_01",
  "questionId": "q_01",
  "selectedOption": "option_b",
  "responseTimeSeconds": 3.8
}