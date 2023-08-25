# Card Action Game Readme

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