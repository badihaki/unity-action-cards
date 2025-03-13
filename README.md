# Card Action Game Readme

## Prototype 1 - Lovesick Blade
Hello, friends and family! As you know, this is the first prototype for my game, and I'm looking for feedback. Be as brutal as you like, but try to be as precise as possible, please.

### Controls
NOTICE: If you try to hook up a gamepad, you'll find it kinda works for a lot of the basic action. However, crucially, you cannot use the card system with controller (yet). As such, I highly advise you play with keyboard+mouse.
NOTICE 2: This prototype is primarily geared towards melee actions. Ranged, Spellslinging actions are in, but working improperly and missing animations.

WASD - movement
Mouse Movemeny - Move camera
Space - Jump/Launcher (L)
Left Shift - Rush (hR)

Left Mouse Button - Attack (A)
'F' Button - Special Attack (S)
Left CTRL - Defensive action
Right Mouse Button - Ranged Spellsling

Tab - Card selection menu

### How to play
This is a 3rd person character action game. You can move around and attack the NPCs in the environment.

Players are fairly mobile in the air. You are able to jump, double-jump, and do an air-rush move (press Rush/Left Shift in the air).

Each weapon-type has a basic 3-hit attack string (A -> A -> A), a special attack (S), and air-variants of each (aA or aS).
Certain attacks can end in a dedicated special attack (try A -> S or A -> A -> A -> S). Each weapon-type also has a dedicated Launcher attack by pressing 'Jump' after any regular attack action (try A -> L), as well as a Rush-attack that can be performed while running (try hR -> A).

## Updates
### 2-23-2025
Summoning minions works in that it brings out a NPC character into the world.

Initial Thoughts:
- CharacterGroupLeader may want to derive from CharacterGroupMember so we can have some comparison methods ( bool IsPartOfGroup(CharacterGroupMember character) and stuff like that )
- CharacterGroupLeader/CharacterGroupMember should work independantly, depending on if there are group members. Meaning, it should be running it's own logic on top of everything

What is working:
- NPCs can come into the world
- NPCs have a CharacterGroupMember class on them when initializing, and are being added to the player's group members list.
What isn't working:
- NPCs can wander anywhere (once again, GroupLeader should work independantly of the other classes to control where the group members can go)
- Can hit group members, whaaa!?!?!
- Should probably have a way to determine if an entity is a part of the group, then attack if not.

### 2-14-2025
I got aggressive NPCs working! NPCs using the Stalking Idle state will poll the eyesight component every (random)few seconds in order to determine what they can see, and any Character class-derived component gets compared to see if it's a friend or a valid target. If it's a valid target, I use the Aggression Manager class's Add Aggression function to make it instantly aggressive towards it. That leads it into the combat loop.

I created the Character Group classes.

### 2-11-2025
We are still working on always-aggressive NPCs, but I have a stalking-idle state for that. Just need to poll eyesight for stuff the entity can attack. Speaking of NPCs and attacks, there's a new Damage class and a new NPC State Library class.

I'm getting to aggressive NPCs and NPC grouping next. I plan to have that done in March.

### 1-6-2025 - expanding NPC logic
The goal is to expand on the capabilities of the NPCs. Adding always-aggressive NPCs. Trying to have nav mesh agent work with character controller. Next up is taking the distance between the player and determining if the entity should lose aggression, as well as a new class to determine sight and detection.
After that, permanently aggressed NPCs, as 'isEnemy' parameter in the NPC scriptable obj, so spawn these fools as an aggressed NPC.
Then group logic with new 'GroupLeader' and 'GroupMember' classes where GroupLeader has a list of members and methods to get a new member, handle losing members, etc. GroupMember will track the group member's leader

### 12-13 Prototype 1 - Lovesick Blade
Next up is reloading the deck from the card abyss.

### 11-9 - NPC Rewrite
Guess who has to rewrite how NPCs work!? That's right!!
Right now, NPCs are having issues with 2 key components: CharacterController and NavAgent. The idea is to abandon NavAgents in favor of an NPC-Nav-Node system.
Navigation will comprise of two main components: NPCNavigator and NavigationNode
- NPCNavigator will store which NavgiationNode the NPC is trying to get to, the current target, and information related to that. We get rid of everything NavAgent/NavMesh
- NavigationNode will be a component that holds information relative to itself and it's neighbors (List of Navigation Nodes)


### 10-31 - It's time to redo Spellslinging
I plan to bring back the spell-slinging state. Include a zoom-in and targeting reticle. We're doing over-the-shoulder targeting again

### 10-24 - UI / Cinematography
- Need to show when you have a weapon equipped and how much durability that weapon has.
- Need to have card cost show when showing your hand.
- Need to have an animation for showing your hand, as well as reintroducing the state.
- need to have a camera zoom into your character's hand using cinemachine for way better cinematography

### 10-22 - Hit feedback
I'm rehauling my hit feedback into 2 main points of contention
- Hit VFX
- Hit responses
	- Launching
		- Working
	- Knockback
		- Going in the wrong direction
	- Hitstun
		- Not implemented

### 10-12 - Card set done
Card moveset is finished. Need hitboxes and VFX. Ready to merge into main. Next up:
* Attacks
- Attack hitboxes and invulnerability
- Auto Aim for melee and range (need new component for this) << Milestone
- Sword Attacks (create a New Sword)
* Mechanics
- Conversation system
- NPC aggression AI
- Rewrite character customization
- First town blockout
- First Enemy - Ghost

### 10/9 - Finishing Card Moveset
I only need the air attacks. Once I get the air attacks working, I can move onto logic, hitboxes and invul. Once all attacks have logic, hitboxes and invul, I can move to contextual interactions and move character AI.

Had to reimport rigs with root bone, and reimport animations.

### 9/23 - Target Refractor
When targeting an enemy, the rays do not shoot towards the correct place. This needs a fix, either with the starting position's height or the angle at which these rays shoot.
New plan - see JIRA

### 9/21 - Character Controller Refractor - Spells
Reworking how spells work. Will need to change the **controls of shoot** to right mouse button/top face button. Need to have the auto-targeting system work with shooting, then shoot from shootpoint to target's transform.
Need to be able to switch spells. Add **spell switch** controls and +1 = up && -1 = down

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