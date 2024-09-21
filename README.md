# Card Action Game Readme

### 9/21 - Character Controller Refractor - Spells
Reworking how spells work. Will need to change the **controls of shoot** to right mouse button/top face button. Need to have the auto-targeting system work with shooting, then shoot from shootpoint to target's transform

### 9/11 - Animations fixed and set up
Imported new versions of the models. Had to fix the avatar on import. Animations are working, if a bit ugly. Animators are working. Time for code rewrites. Will merge later.
Code rewrites (in order of importance):
	- Character Controller needs a complete rewrite using the CharacterController component. Need to make it possible for states that move the character to follow the following flow:
		- Get player inputs
		- Modify desired movement based on external forces
			- Gravity??
		- Apply movement
	- Weapon controller needs a full rewrite, w/ animator restructuring
		- Animator needs to have sub-states for each animation set
			- Unarmed sub=state
			- Sword sub=state
		- Need to be able to change animation sub-state based on which weapon is being held
		- Need to separate attack stuff from weapon stuff
	- Attack controller
		- Need specific logic separated from weapon stuff
	- Player Structure
		- Player does not need to have collider information, nor does the entity really need to move, so no rigidbody.

### 8/13 - Blender rigify Attempt 2
Rigify broke for my female player character. Tried using Auto Rig Pro but don't like the workflow. Going back to Rigify. Need to add bones and collections for the following:
	- Skirt
	- Collar
	- Hair
	- Weapons

### 3/29 - Saving and Loading Player (w/ gender)
- I've redone a lot of the scriptable objects to abstract away whether it's for male or female bodytypes.
- The logic for handling whether it is m/f is in Game Manager/Character Customization Database
- Changing equipment/bodyparts works and saves in character creator. Loading worked before when it was just male characters.
- Make sure the female characters load, and we're done with this part

## 3/12 - Saving and Loading Player Equipment
- I'm saving equipment to disk. The goal now is to load the equipment.
- I think I have the system for saving and loading done, in terms of what the Game Master can do
- Next up is having PlayerActor.cs load the data and build the character
	- Need to set up the system to do either male or female bodytypes so it can build the full body from memory
- Will need to create Scriptable Objects for the outfit items
	- Will hold outfit mesh, material, stats, bodypart and ID
- Will need to create Outfit Manager to manage outfits
	- Will manage using the scriptable object

## Attacks
- I guess I need to decouple attack input/functions
	- Attack input => Shoot / Light Attack (LtAttack) / Heavy Attack (HvAttack)
- Characters must be able to dynamically switch weapons
	- Switching weapons involves changing movesets
	- Characters need to retain a base moveset
- Weapons must hold their movesets
	- When switching weapons the new moveset must be loaded in
	
## NPCs
- All NPCs need to be able to choose a target and move to it
	- Minion NPCs need to hold what character summoned them, and stay near that character