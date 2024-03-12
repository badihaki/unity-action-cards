# Card Action Game Readme

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