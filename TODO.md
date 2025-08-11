# UI

- [x] Add spinner sprites
- [ ] Add pause button
- [ ] Make UI mobile friendly
- [ ] Add achievements and achievements page
- [ ] Add input mapping
- [ ] Use one list of viewports for all ballviewers
- [ ] Add vibrations
- [ ] Add destruct button to avoid softlocks

# Gameplay

- [ ] Add replay ball light fuctionality
- [ ] Add mission system
- [ ] Review generated rarity rating
- [ ] Review drop rates
- [ ] Make trail global
- [ ] Adjust physics to rolling instead of sliding
- [x] Red prices when in debt
- [ ] Freeze reroll when in debt
- [ ] Add tilt function timeout
- [ ] Add saves/continue
- [ ] Add drag mode for plunger
- [ ] Add target blink on hit
- [ ] Add board picker system
- [ ] Change ballterator to stop refreshing
- [ ] If ballterator refreshes, price must go up

# Bug fixes

- [ ] Fix freeze+tweening doing strange stuff on trail (Spitters)
- [ ] Fix paddle position
- [ ] Fix multiple balls in spitter
- [ ] Clean duplicated groups in score modifier ballterations
- [ ] Fix impossible restrictives in score modifier ballterations
- [ ] Fix self destructing ball cloning
- [ ] Split code for UI management (rn all in Game.cs)
- [ ] Maybe pool queue for like score bubbles
- [ ] Maybe factorise the "OnMultiplier" thing
- [ ] Think of a way to make levels generic (think bumper with more/less than 5, or spinners etc)
- [x] Update assembly name


# Quality assurance

- [ ] Check for teleport multi entry and weird behaviour
- [ ] Check if refine goes in the inherited effects


# Ballterations effects

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
- [x] Timed ball
- [ ] Size down
- [ ] Weight down
- [ ] Random direction changes
- [ ] Bounciness
- [ ] Square hitbox
- [ ] Triangle hitbox
- [ ] Capsule hitbox
- [ ] Count ball rotation ?
- [ ] Slow motion on the ball ?
- [ ] Slow motion on the ball on flipper approach ?

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
- [x] New ball
- [x] Extra ball
- [x] Replay ball
- [ ] New random ball
- [ ] Extra timed ball
- [ ] More options
- [ ] More options persistent
- [ ] Resupply instead of rerolling
- [ ] Duplicates any ball
- [ ] Ameliorates all ballterations
- [ ] Worsen all ballterations

## Other
- [ ] Remove ballteration
- [ ] On flipper hit swap the ball ?
- [ ] Timed effect

## Downsides
- [ ] Adds to the target score
- [ ] Multiplies the target score

# Board ideas

## Standard
- [ ] Labo
- [ ] Pirate
- [ ] Middle Ages
- [ ] Dinosaurs
- [ ] Racecar
- [ ] Food salty
- [ ] Food sweet
- [ ] Food healthy
- [ ] Circus
- [ ] Cyberpunk
- [ ] Steampunk
- [ ] Solarpunk 
- [ ] Haunted
- [ ] Space
- [ ] Roma
- [ ] Double sided board
- [ ] FishTheFishes

## Tutorial
- [ ] Lanes' land
- [ ] Bumpers' borough
- [ ] Slingshots' stronghold
- [ ] Spinners' suburb
- [ ] Spitters' sector
- [ ] Targets' town
- [ ] Rollovers' resort

## Event
- [ ] Birthday (Based on user birth month)
- [ ] Easter
- [ ] Halloween
- [ ] Christmas

## Boss
- [ ] -ET
- [ ] -BankHeist
- [ ] -Cerberus
