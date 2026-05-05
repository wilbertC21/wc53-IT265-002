## Change Log


The following is Catalyst Studio’s proposal for a strategic speed-based game. The Title Race Attack communicates both the fast-paced, energetic gameplay and the racing element as players race forward toward victory. It's short and captures the chaotic energy of the game.


GAME DESIGN
OVERVIEW:
Race Attack is a fast tabletop game where 3-5 people frantically match cards while deciding whether to race ahead or help kill the Boss. It mixes chaos with actual strategic choices. You're sweating from both the speed and the pressure of choosing what to do with your dice roll. 

GAME CONCEPT:
In Race Attack, players race to the finish line while the Boss tries to stop them from reaching the finish line. But if you cross the finish and the Boss is still alive, the game keeps going. So you have to balance running ahead with helping kill the Boss. The game involves Frantic card passing(clicking digitally), then a moment of "do I move or attack?" that actually matters. You get adrenaline from the speed part and satisfaction when your strategic choice pays off. Comeback mechanics keep it tense even when you're losing. Each round is a different mini-game played. The most common one is that you match cards as fast as you can. The first person to grab the dice remaining in the middle of the board then rolls. Use that roll to move forward, damage the Boss, or mess with other players. Repeat until someone wins.
Why is it fun? Your hands are always busy passing(clicking) cards, your brain is busy making decisions, and there's always some dramatic moment where the Boss knocks everyone back, or someone lands the killing blow right before crossing the finish.

TARGET AUDIENCE:
Race Attack is aiming for players who like quick, competitive games. Age 10+, kids can learn it in two minutes, but there's enough strategy and chaos to keep adults interested. If you like high-energy, ,socially interactive, and games that don't require a lot of rules or complex thinking, this game is for you.I know people will play this because there's a gap in the market. Speed card games are too simple (no depth), racing board games have too much downtime (waiting for your turn sucks), and boss battle games take forever. This does all three things in 15 minutes.

WORLD STRUCTURE:
	Linear track. 20 spaces from Start to Finish. The players all have 20 spaces but different paths to the finish line. You move forward when you roll and choose Move. The Boss can either choose to block you, or attack you.. Whatver the boss rolls and he choses to block you your next turn as a player is subtracted by that same amount the boss rolled. No shortcuts. 

PLAYERS:
	One person is the Boss. Everyone else (2-4 people) is a Racer. Racers all have the same abilities and the same HP of 25 health. No special powers, no unique characters. The Boss has 35HP. There can only be one winner, it's the 1st racer to cross the finish line while the Boss is dead. The Boss has different actions and wins by killing all the racers before it kills him.  (I might add characters later, like a Speedster and Demolisher (deals more HP to the boss) ). 
Gameplay: 
Racers: get across 20 spaces AND make sure the Boss is dead. If you cross the finish but the Boss is still alive, you have to keep playing. Winner is whoever crosses the finish first.
Boss:
Kill everyone off or survive all 20 rounds. 
Side goals that emerge:
People naturally start competing for who deals the killing blow, or who can finish without getting blocked. There's also a psychological game where you try to trick people into thinking you're further behind than you are.
The tension: If you only Move, you get closer to the finish line but the boss gains 1HP. Also by moving your skipping out on the attack which means the boss will have more time on the board.  If you only Attack, you help everyone else win while you're stuck in the back. You have to read the game and adapt.

Core Loop: 
Each player spawns with 4 cards
Cards get shuffled and passed around by rapid clicking
Once a player finds a match (different per round) they reach for a die and everyone left follows. Until there is one person with no die. 
Roll the die 
Choose to Move, advance your position, Attack Boss (deal damage), or Block (knock someone else back). But everytime a player chooses to move it adds 1hp to the boss. 
If your the boss you either block a player or attack a player with your roll.
Round ends, reshuffle the cards, flip a new Goal Card, go again. 

Visual System 
	The game is played in a top down view . in the view you can see all the players with the boss being the farthest.
Landscape , grass and asphalt as the road the racers race on.

Interface 
The matching goal shows up in huge letters before the round starts so all players know what to match. Visible deck of cards on the side and you can visibly see the cards you are dealt in front of you . Number of players-1 number of d6. Health bar for all players on top of the cars.   And dice that are able to be grabbed and rolled. 

Implementation

GAME STATES
Number of player selection in main menu
Game adjusts to number of players
Each round has a different match case or mini game thats randomized in the code
DiceGrab (first to match grabs die) 
DiceRoll (active player rolls )
Then camera switches to other players and they roll
ActionSelection (choose Move/Attack/Block) 
ActionResolution (execute chosen action) 
BossRetaliation (if boss grabbed a die) 
RoundEnd (check win conditions, reset for next round) 
GameOver (display winner) 

 Deck: 52 cards total -
1. Three of a Kind: Match 3 cards with same number 
2. All Same Color: 3 cards of matching color
3. Sequential: 3 cards in numerical sequence (e.g., 4-5-6) 4. All Different Colors: 4 cards, one of each color.

HEALTH & DAMAGE:
Racers:
- Starting HP: 25
- Death: Eliminated when HP reaches 0
- Cannot be revived

Boss:
- Starting HP: 35
- Gains +1 HP when any Racer chooses Move action
- Death: Boss death is when hp reaches 0

Damage Calculation:
- Attack Boss: Dice roll = damage dealt (1-6 HP)
- Boss Attack Player: Dice roll = damage dealt (1-6 HP)
- Block action: Dice roll = cannot move next roll.

BOARD LAYOUT:
Track:3 tracks 
- All racers start at space 0
- Rarely moves on the track.
- No obstacles.
- Multiple players can occupy same space

Movement Rules:
- Move action: Advance (dice value) spaces forward
- Block action: Target player moves back (dice value) spaces.

UI LAYOUT (Top-Down View):

TOP OF SCREEN:
- Round counter (current/total)
- Boss HP bar (large, centered)
UI: 
- Grassy Race track terrian 
- with asphalt textured roads 
- Boss position in front of racers.

BOTTOM OF SCREEN:
- Player's hand (4 cards displayed)
- Current Goal Card (large, centered)
- Available dice (clickable)

ABOVE EACH CAR:
- HP bar (25/25 display)
- Block status indicator (if blocked)



