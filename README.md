# Dice Game Client

Welcome to the dice game client! \
It is meant to be used along with the [backend repository](https://github.com/pluckynumbat/dice-game-backend)

---
## Getting Started
1. Prerequisites: Unity version 6000.0.26f1 or higher
2. Clone this repo!

---
## Part 1. Running the application

### In the editor:
1. Open the repository's root directory with Unity 6000.0.26f1 or higher.
2. Navigate to the `Assets/Scenes` directory and open the Bootstrap scene.
3. It is recommended to have backend up and running before playing ([instructions here](https://github.com/pluckynumbat/dice-game-backend?tab=readme-ov-file#part-1-running-the-application)) 
4. Press play!

Note: After starting to play, in case you need to create a fresh player from scratch (new player ID), just clear player prefs between sessions. This can be done in Unity: (`Edit->Clear All PlayerPrefs`) or you can use the debug menu mentioned below.

### Device builds:
 1. In Unity, navigate to `File->Build Profiles`, select the platform, and settings, and press `Build`
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
The [Debug Menu](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/DebugMenu.cs) is accessible in the editor builds (and on macOS standalone builds) by pressing the `D` key. I created this to help me out during the development process. It has a bunch of options to make different requests to the backend, change screens, delete player prefs etc. 

---
## Part 3. Additional Information about the client

Since the client was provided in a partially implemented state, I will focus on the things that I added / changed.

### [GameRoot](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/GameRoot.cs)
This is the bootstrap script, a monobehavior which creates all the manager instances as part of the startup sequence, and kicks off the auth flow.

---
### Model/Managers
The managers are plain classes that all handle specific responsibilities. Following are some key ones:


### [AuthManager](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Model/AuthManager.cs)
 - This is in charge of the entire auth flow, which includes basic authentication, followed by post auth tasks like fetching the config, player data, and stats. 
 - It also orchestrates the loading screen progress bar filling.

### [StateManager](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Model/StateManager.cs)
 - This acts as the state machine for the game, and also connects the model to the presentation layer via the [ScreenCoordinator](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Presentation/ScreenCoordinator.cs).
 - Changes in state are routed through this manager, which tells the screen coordinator to change screens.

### [NetRequestManager](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Model/NetRequestManager.cs)
 - The bridge between the rest of the client and the unity web requests. It handles creating the requests, sending them, waiting on responses, and then forwarding the results to the caller.
 - Except for the auth manager's own 2 requests ([LoginRequest](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Network/LoginRequest.cs) and [LogoutRequest](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Network/LogoutRequest.cs)), all other manager's network requests go through the net request manager.

### [ErrorManager](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Model/ErrorManager.cs)
 - This is core error handling unit, and is responsible for entering the error state, populating the error screen, and exiting the error state if it is a recoverable error, ro guiding the player to reload / quit the app if not.
 - I added a new error screen prefab and [script](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Presentation/Error/Screen/ErrorScreen.cs) that is used in the error state.


#### The other managers are more straightforward, and are tied to specific backend services (config / player / gameplay) 

---
### [Network/Requests](https://github.com/pluckynumbat/dice-game-client/tree/main/Assets/Scripts/Network)
These are plain classes representing the client side of network requests that are sent to the backend. \
They have definitions of the request / response structs. \
There is one for each of the backend's public endpoints.

---
### [Presentation/ScreenCoordinator:](https://github.com/pluckynumbat/dice-game-client/blob/main/Assets/Scripts/Presentation/ScreenCoordinator.cs)
 - This is a monobehavior attached to the ScreenCanvas gameObject in the boostrap scene, and has references to all the screens.
 - During start up, the StateManager supplies the GameRoot instance to the screen co-ordinator, which then initializes the different screens by injecting the manager dependencies into them.
 - When the game state changes, the screen co-ordinator switches between the scenes.

#### Other than this, I added some presenter scripts to the presentation layer, which are straightforward

---