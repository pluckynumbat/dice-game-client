# Dice Game Client

Welcome to the dice game client! \
It is meant to be used along with the [backend repository](https://github.com/pluckynumbat/dice-game-backend)

## Getting Started
1. Prerequisites: Unity version 6000.0.26f1 or higher
2. Clone this repo!

## Part 1. Running the application

### In the editor:
1. Open the repository's root directory with Unity 6000.0.26f1 or higher.
2. Navigate to the `Assets/Scenes` directory and open the Bootstrap scene.
3. It is recommended to have backend up and running before playing ([instructions here](https://github.com/pluckynumbat/dice-game-backend?tab=readme-ov-file#part-1-running-the-application))
4. Press play!

Note: After starting to play, in case you need to create a fresh player from scratch (new player ID), just clear player prefs between sessions. This can be done in Unity: (`Edit->Clear All PlayerPrefs`) or you can use the debug menu mentioned below.

### Device builds:
1. In Unity, navigate to `File->Build Profiles`, select the platform, and settings, and press 'build'
2. I have tested with macOS standalone builds, and have changed some defaults in the project settings, to have the game open up with a resolution of 768x1024, in a resizable window.

### Only if connecting to a non localhost backend:
1. The default backend is localhost, and that is set in the Constants file [here](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Constants.cs#L9) as the ServerHost property. (The file is located at: `project-root/Assets/scripts/Constants.cs`).
2. That can be changed to an ip address. (Tested this on a local network)
3. For the editor (or development builds) to be able to communicate with the non localhost backend, there is a player setting (in Unity: `Edit->Project Settings->Player Tab->Other Settings`) called `Allow downloads over HTTP`. That has to be changed from `Not Allowed` to `Allowed in development builds`.
4. For non-development builds, the above `Allow downloads over HTTP` setting has to be changed to `Always allowed`.

---
### All the information you need to run is above, and the following is just more context about the various details!

---
## Part 2. More Settings

### Constants:
The [constants](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Constants.cs) file (located at `project-root/Assets/scripts/Constants.cs`) holds settings like port numbers for the services which you might want to change if needed.
If changed, the [constants file in the backend repo](https://github.com/pluckynumbat/dice-game-backend/blob/main/internal/shared/constants/constants.go) should also be changed in the same way.

### Debug Menu (for development):
The Debug Menu is accessible in the editor builds (and on macOS standalone builds) by pressing the `D` key. I created this to help me out during the development process. It has a bunch of options to make different requests to the backend, change screens, delete player prefs etc. 

---