Normal pinball macines usually are 130x72cm thus I'm basing the boards on a 72:130 (or 36:65) aspect ratio.
Since I'm targetting a 1080px height screen, I'll actually use 5:9 ratio that avoids dealing with fraction of pixels, making the base board 600x1080px.

Add tilt function timeout
Add levelup delay
Add spinner sprites
Add tooltip for balls
Add pause
Add actual mobile UI
Add reroll system (overscoring/debt)
Fix freeze+tweening doing strange stuff on trail (Spitters)
Fix ball draw layer not right on lab semi exit
Fix paddle position
Fix multiple balls in spitter

Think of a way to make levels generic (think bumper with more/less than 5, or spinners etc)
Maybe factorise the "OnMultiplier" thing
Extra call args + unbind signal args
Defaut values instead of overloading in signals callback

Moving elements

# Settings for perfs
Tick rate
Collision thing
Trail Detail

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
Bumper score
Target score
Rollover score
Slingshot score
Spinner score
Spitter score
Round score
Square score
Global score

## Ball Physics
Size down
Weight down
Random direction changes
Bounciness
Square hitbox ?
Triangle hitbox ?
Capsule hitbox ?
Count ball rotation ?
Slow motion on the ball ?
Slow motion on the ball on flipper approach ?
Timed ball

## Trail
Trail length
Exploding trail

## Missions
Mission scrore mult
Reduce completion threshold

## Board
Paddle speed
Gravity inverter

## Meta
More options
More options persistent
More choice times
More choice times persistent
New ball
Extra timed ball
Extra ball

## Other
Remove ballteration
On flipper hit swap the ball ?

# Rarity system

Grey
Green
Blue
Red
Purple

Yellow (Fixed)
