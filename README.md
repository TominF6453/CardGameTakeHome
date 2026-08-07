# Project Title
Card Game Take-Home -> Grid Sum

## Overview
An implementation of the project as described in the [Take-Home Test Instructions](Unity%20Gameplay%20Developer%20Take-Home%20Test.pdf).

### Card + CardData
Data for cards is stored within [CardData.cs](CardGameTakeHomeStarter\Assets\ScriptableObjects\CardData.cs) ScriptableObjects, the only data stored is suit and value as the card sprites themselves are handled separately via the [CardSpriteManager.cs](CardGameTakeHomeStarter\Assets\Scripts\CardSpriteManager.cs).
The physical card objects are handled via [Card.cs](CardGameTakeHomeStarter\Assets\Scripts\Card.cs) which stores a [CardData.cs](CardGameTakeHomeStarter\Assets\ScriptableObjects\CardData.cs) and implements the Unity `IDragHandler` interfaces to allow for drag-n-drop interactions.

### DeckManager
One of the singleton managers, [DeckManager.cs](CardGameTakeHomeStarter\Assets\Scripts\DeckManager.cs) handles the draw and discard area of the canvas area. It stores an array of playable [CardDatas](CardGameTakeHomeStarter\Assets\ScriptableObjects\CardData.cs) which it shuffles into a `List<CardData>` for drawing.
On clicking "Draw", a new [Card](CardGameTakeHomeStarter\Assets\Scripts\Card.cs) object is instantiated with the data set and sprite retrieved from [CardSpriteManager](CardGameTakeHomeStarter\Assets\Scripts\CardSpriteManager.cs), then the [HandManager](CardGameTakeHomeStarter\Assets\Scripts\HandManager.cs) handles it.

### PlayAreaManager
One of the singleton managers, [PlayAreaManager.cs](CardGameTakeHomeStarter\Assets\Scripts\PlayAreaManager.cs) constructs a backend 2D array of `CardSlot`s with a maximum width and height defined in inspector. When a card is dropped onto the play area, the [PlayAreaManager](CardGameTakeHomeStarter\Assets\Scripts\PlayAreaManager.cs) handles finding the closest available slot to the drop position and adds the card to the grid. When a card is placed in a slot, all its neighbours are also made available for other cards to be placed.
[PlayAreaManager](CardGameTakeHomeStarter\Assets\Scripts\PlayAreaManager.cs) also handles the end-game state, where if the deck and hand are empty or the play area grid is full, the game is complete and the scoring coroutine is ran to sum up all card scores and show the quit and restart buttons.

### HandManager
The simplest of the singleton managers, [HandManager.cs](CardGameTakeHomeStarter\Assets\Scripts\HandManager.cs) simply stores a list of [Cards](CardGameTakeHomeStarter\Assets\Scripts\Card.cs) up to the maximum hand size set in inspector. When cards are drawn, played or discarded, [HandManager](CardGameTakeHomeStarter\Assets\Scripts\HandManager.cs) adds or removes it from its list.

## Extra Notes

### Update()
The game is entirely event driven so no Update loops were ever necessary anywhere. If cards animated their positions smoothly from draw, there would be potential for an Update there.

### CardSlot 2D Array
There are multiple reasons [PlayAreaManager.cs](CardGameTakeHomeStarter\Assets\Scripts\PlayAreaManager.cs) has the slot array.
The slot array ensures placement accuracy and availability without requiring precision from the player, since card position has no bearing on its scoring. A player can simply drag straight up quickly from hand to place a card in a legal position without any thought.
The slot array can also be generated as any size, if the play area were to become panable or larger in some way, different grid sizes are as easy as setting new values in inspector.
The slot array guarantees an end-game state. A fixed amount of cards can be placed instead of potentially infinite, so if the deck size were to become extremely large (or just infinite and random) the game still has a structured end.

### Utilities.cs
If there are extension or helper methods that can be generally used across a project or for multiple purposes, I place them in a [Utilities.cs](CardGameTakeHomeStarter\Assets\Scripts\Utilities.cs) file for organization. The only things in there are some in-place `List` shuffle methods, a `List.Pop()` method for drawing, and a helper to create `Vector3` with same values (Effectively `Vector3.one * value` but more readable).


