Normal pinball macines usually are 130x72cm thus I'm basing the boards on a 72:130 (or 36:65) aspect ratio.
Since I'm targetting a 1080px height screen, I'll actually use 5:9 ratio that avoids dealing with fraction of pixels, making the base board 600x1080px.

Add tilt function timeout
Add levelup delay
Add spinner sprites
Add tooltip for balls
Fix freeze+tweening doing strange stuff on trail (Spitters)

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
Food
Circus
Steampunk
FishTheFishes
Haunted

Boss
-BankHeist
-Cerberus

# Board curses
No see board
No see score
No see ball queue
No see upgrades on level end
No see live ball(s) ?
Random teleports

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
Square hitbox ?

## Trail
Trail length
Exploding trail

## Board
Paddle speed

## Meta
More options
More options persistent
More choice times
More choice times persistent
New ball
Extra ball
Timed ball ?

## Other
Remove ballteration

# Rarity system

Grey
Green
Blue
Red
Purple

Yellow (Fixed)
