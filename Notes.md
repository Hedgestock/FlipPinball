Normal pinball macines usually are 130x72cm thus I'm basing the boards on a 72:130 (or 36:65) aspect ratio.
Since I'm targetting a 1080px height screen, I'll actually use 5:9 ratio that avoids dealing with fraction of pixels, making the base board 600x1080px.


Add tooltip for balls
Add spinner sprites
Add pause
Add actual mobile UI
Add tilt function timeout
Add target blink on hit
Add drag mode for plunger
Add settings
Add saves/continue
Add music
Add achievements
Add input mapping
Add vibrations
Add replay ball light fuctionality
Add mission system
Fix freeze+tweening doing strange stuff on trail (Spitters)
Fix paddle position

Fix multiple balls in spitter
Check for teleport multi entry and weird behaviour
Use one list of viewports for all ballviewers
Split code for UI management (rn all in Game.cs)
Maybe pool queue for like score bubbles
Think of a way to make levels generic (think bumper with more/less than 5, or spinners etc)
Maybe factorise the "OnMultiplier" thing
Extra call args + unbind signal args
Defaut values instead of overloading in signals callback

MRP process mode of instanciated stuff

Moving elements

# Settings for perfs
Tick rate
Collision thing
Trail Detail
Particle effects

# Theme ideas
Labo
Pirate
Middle Ages
Dinosaurs
Cyberpunk
Racecar
Food salty
Food sweet
Food healthy
Circus
Steampunk
FishTheFishes
Haunted
Double sided board

Lanes' land
Bumpers' borough
Slingshots' stronghold
Spinners' suburb
Spitters' sector
Targets' town
Rollovers' resort

Birthday (Based on user birth month)
Easter
Halloween
Christmas

Boss
-ET
-BankHeist
-Cerberus

# Board curses
No see board
No see score
No see ball queue
No see upgrades on level end
No see live ball(s) ?
Random teleports
Random teleports invisible
Slow motion (Also in target)

# Ballterations

## Score
- [x] Bumper score
- [x] Target score
- [x] Rollover score
- [x] Slingshot score
- [x] Spinner score
- [x] Spitter score
- [x] Round score
- [x] Square score
- [x] Global score

## Ball Physics
- [ ] Size down
- [ ] Weight down
- [ ] Random direction changes
- [ ] Bounciness
- [ ] Square hitbox ?
- [ ] Triangle hitbox ?
- [ ] Capsule hitbox ?
- [ ] Count ball rotation ?
- [ ] Slow motion on the ball ?
- [ ] Slow motion on the ball on flipper approach ?
- [x] Timed ball

## Trail
- [ ] Trail length
- [ ] Exploding trail

## Missions
- [ ] Mission scrore mult
- [ ] Reduce completion threshold

## Board
- [ ] Paddle speed
- [ ] Gravity inverter

## Meta
- [ ] More options
- [ ] More options persistent
- [ ] More choice times
- [ ] More choice times persistent
- [ ] Resupply instead of rerolling
- [x] New ball
- [ ] Extra timed ball
- [x] Extra ball
- [x] Replay ball

## Other
- [ ] Remove ballteration
- [ ] On flipper hit swap the ball ?
- [ ] Timed effect

## Downsides

- [ ] Adds to the target score
- [ ] Multiplies the target score


# Rarity system

Black
Grey
Green
Blue
Red
Purple

Yellow (Fixed)
