<div align="center">
	<h1>Heroes Stage Injector</h1>
	<img src="https://i.imgur.com/BjPn7rU.png" width="150" align="center" />
	<br/> <br/>
	<strong>Creating Custom Stages should be Easy<br/></strong>
	<p>So why not make it that?</p>
<b>Id: sonicheroes.utils.stageinjector</b>
</div>

# Prerequisites
This mod uses the [Hooks Shared Library](https://github.com/Sewer56/Reloaded.SharedLib.Hooks).
Please download and extract that mod first.

This mod uses the [Universal Redirector](https://github.com/Reloaded-Project/reloaded.universal.redirector).
Please download and extract that mod first.

# About This Project

This mod provides a framework to allow for the loading of custom stages into Sonic Heroes without the need to write manual code by hand.

### Motivation
In contrast to more modern games in the franchise like Sonic Unleashed or Sonic Generations where modifying stages can be done entirely by modifying files; Heroes and the rest of the Adventure-engine games store a lot of the stage metadata embedded in the executable.

This metadata includes features such as bobsled paths, splines (rails, loops) and spawn points. Users lacking programming knowledge would normally be unable to access or modify these features.

### Solution
The solution to this problem is to make everything available through files, and this mod does exactly that.

# How to Create a Stage

A sample mod (Radical Highway by Shadowth117) is available in the `Releases` section.

- [Create an Empty Reloaded II mod](https://github.com/Reloaded-Project/Reloaded-II/blob/master/Docs/GettingStartedMods.md).
- Add a dependency in `ModConfig.json` to this mod.
```json
"ModDependencies": ["sonicheroes.utils.stageinjector"]
```

- Add a folder named `Stages`.
- For each stage you want to add, create a folder (folder can have any name).

### Stage Folder Layout
The following describes the layout of each stage folder:
![](https://i.imgur.com/ibj2IGV.png)

##### Files Folder
This folder is used for file redirection and maps to the executable directory of Sonic Heroes.
For example to change the textures for Seaside Hill (s01.txd), you would place the file `\dvdroot\textures\s01.txd` inside the `Files` folder.

##### Stage.json
This file contains the spawn properties and level ID this stage is supposed to replace.

The level ID can be obtained from the following page [SCHG: Sonic Heroes](http://info.sonicretro.org/SCHG:Sonic_Heroes/Level_List)  (under the EXE and RAM label).

If you are replacing a singleplayer stage, the `StartPositions` represent the spawn positions of each of the following teams in order Sonic, Dark, Rose, Chaotix, Foredit (unused team).

If you are a multiplayer stage, the first two `StartPositions` are used for P1 and P2 respectively and the `BragPositions` represent the positions the team introductions are performed before a race. 

##### Splines.json
This file contains the information about each loop, rail and other kind of splines to be used by the stage.

At the moment, no tools exist for creating `Splines.json` (this should change real soon!).
For `Stage.json`, it can be created by [Heroes Power Plant](https://github.com/igorseabra4/HeroesPowerPlant).

# Other Features

*This mod also fixes crashes in the following scenarios:*

- Loading the Test level.
![](https://cdn.discordapp.com/attachments/317475533321404416/613059388252487719/unknown.png)

- Loading 2P mode in 1P levels.
![](https://cdn.discordapp.com/attachments/317475533321404416/613058385771757578/Tsonic_win_bnalZlSPEU.png)

- Loading 1P in 2P levels.
![](https://cdn.discordapp.com/attachments/317475533321404416/613064695171252224/unknown.png)
