## Name: Wil C
### Module: Digital Prototype - Race Attack

### Date: 05/04/2026

#### Goals for this Module
- [x] Get Unity prototype running
- [x] Dice throwing that actually works
- [x] Player count UI
- [x] Camera follows dice
- [x] Health bars for cars (was working on racing game stuff)
- [x] Update GDD with changelog

#### Progress
- **What I accomplished**:
  - Dice system works - Space to throw one at a time, R to reset all. Took way longer than expected because of physics weirdness
  - Got topface detection working with transforms on each die face. Checks Y position to figure out which face is up
  - Camera follows whichever die you just threw, then goes back to starting position. Looks pretty good actually
  - Player selection dropdown at start - pick 3, 4, or 5 players and it enables/disables the right number of cars
  - Health bars for the racing cars (different project but got it working with sliders)
  - Added changelog to GDD showing what changed from physical to digital prototype
  
- **Challenges faced**:
  
  - Dice kept breaking when I tried to destroy them - something about modifying collections during iteration
  - Camera script threw null reference because I forgot to assign it in Inspector
  - no idea how to send cards through out the players
  
- **Solutions**:
  -None yet

#### Learnings

- Auto-assigning components in Awake() is cleaner than doing it in Inspector
- ScriptableObjects are actually pretty nice for card data

#### Free Thinking


- Camera only follows last die. What if you want to see all of them at once?
- Haven't actually built the Boss mechanics yet - just have cars and dice. That's gonna be a whole thing
- Card system isn't in Unity at all yet. Need deck, hands, passing, Goal cards \
- Turn order is just whoever hits Space next. Not sure if that's the vibe I want

#### Next Steps
- Build card system (deck, hands, matching)
- Actually make the race track with 20 spaces
- Boss mechanics (HP, actions, all that)
- Wire up dice to car movement
- Win condition checking
- UI for round number, HP displays
- Probably playtest and realize half of this doesn't work
