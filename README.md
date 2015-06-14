# Mazzaroth Project
Mazzaroth is the code name of this project, The game is a Real Time Tactics and it is on early development. 

On this game the player embodies a general with planets and an army of ships, its objective is to conquer the galaxy. Currently the player can command one or multiple troops of ships to destroy the enemy troops.


##Project Structure
```
Engine/ "Base utilities and classes"
GameScenes/ "Game Logic and Assets divided by scenes"
GameScenes/<##>-<SceneName>/ "Logic and Assets specific to the scene"
GameScenes/Common/ "Logic and Assets common to several scenes"
Plugins/ "Unity Plugins"
UnityAssets/ "Unity Assets"
```
### Scenes
Commonly each scene is structured the following way:

**Secene/** The Scene Wrapper, is controlled by ```BattleScene``` and has the GUI main script.   
**Secene/APS/** The Pooling System.  
**Secene/Map/** The battlefield wrapper, in some cases it has some triggers atached.  
**Secene/Map/CameraRiel** A rail on which the camera moves. Controlled by ```RTSCamera```.  
**Secene/Map/CameraRiel/Main Camera** The Main Camera.  
**Secene/Map/ANY OTHER STUFF** Ships, debries, inlined text and everithing that is inside the battlefield.

### Battle Scene
The battle scene models a game with multiple opponents who have armies of ships and structures.

```BattleScene``` Haves ```Player```s, every ```Player``` have an ```Army```, every ```Army``` have ```Group```s and every ```Group``` have ```Ship```s. If the player gives a command on the GUI it goes to ```Player``` and flows down the hierarchy to the ```Group``` and the ```Ship```s.

```Group``` manages the ships and applies the formations.

### Ships
A Ships is constructed by attaching the class ```Ship``` and its components: ```ShipStats```, ```MovementEngine```, ```ShipControll```, ```ShipHull```(Coupled in Ship), ```ShipWeapons```(Coupled in Ship), ```ShipDetectors```(Coupled in Ship) found in ```GameScenes/Common/Scripts/Ship```.  By inheriting any of those components we can create different ships with different behaviors.

The ships also needs a basic AI wich is given by a FSM (The technic not the God), the states of the FSM are found in ```GameScenes/Common/Scripts/Ship/States```.The FSM is used to model the behavior of the ships when receiving orders from the player.

### Weapons
The weapons are designed similar to the ships, its classes are found in ```GameScenes/Common/Scripts/Weapons```.