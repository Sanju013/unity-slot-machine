# Unity Slot Machine

A small 3-reel slot machine game developed in Unity using C#. The project focuses on implementing the core gameplay systems behind a slot machine, with reusable scripts.

## 🎮 Play the Game
Click the link below to play the WebGL build directly in your browser:

👉  https://sanju013.github.io/unity-slot-machine/

No Unity installation is required to play the WebGL version.

## 🕹️ Instructions to Run the WebGL Build
Play Online
Open the game link:
https://sanju013.github.io/unity-slot-machine/
Wait for the Unity WebGL game to load.
Select a bet amount.
Start the spin.
Check the result and payout.
If your balance reaches zero, use the reset option to restart the game.
The game can also be played in Fullscreen using the Fullscreen button.
Run the Project Locally

## Thought Process / Approach 
I usually first create a design document, I already got a reference so I sorted out the mechanics, scope down and figured out the additional feature implementation, then onwards I hand-draw my implementations sort-like..
..a flowchart representation, Firstly I organised the folders then onwards Created the Slot machine and did its implementations, I worked on the reels first as I figured masking was a solution to keep reels in a strip implemented..
..a Symbol Strip that clones the symbols , then the betting and wallet system, followed by the win and payout logic. Added a Jackpot System, completed the UI setup and polishing, setup a small audio system for win, loss, jackpot..
...reels rolling, and restart.

## IMPORTANT
There is restart button only for testing purposes and not valid for an actual slot game instead when low on money you can watch ADs or buy currency and bet again. But here I had to implement a restart so its easier to test..
...back and forth. Within limited time constraints it was more fun to work on the project. :) 

## 🕹️ To open the Unity project itself:
Clone or download this repository.
Open the project using Unity 6.
Open:
Assets/Scenes/SlotMachine.unity
Enter Play Mode to run the game inside Unity.

## Features
- Betting system
- Three-reel slot machine
- Multiple bet values
- Randomized reel results
- Win/loss evaluation
- 3 BAR jackpot
- Payout multipliers
- Wallet/balance system
- Reset/restart loop
- UI feedback
- Audio feedback

## ✨ Bonus Features
Jackpot system for BAR - BAR - BAR.
Jackpot combines the normal bet-based payout with an additional 5000G bonus.
Separate win, loss, and jackpot UI states.

## 🕹️ Controls
- Select bet
- Pull lever / spin
- Reset when balance reaches 0
- Fullscreen

## 💵 Payouts
| Bet | Multiplier |
| 10G | 3x |
| 50G | 5x |
| 100G | 15x |
| 3 BAR | Normal payout + 5000G |

## Technical Implementation
### Core Systems
- Wallet
- BetManager
- SlotMachineController
- ReelController
- WinEvaluator
- PayoutManager

### UI
- BetPopupController
- ResultUIController
- ResetGameController

### Audio
- SlotMachineAudio

## Credits
Assets provided by Underpin Technology
