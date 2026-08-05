using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace CardGame {
    public class DeckManager : MonoBehaviour {

        #region Statics & Constants
        public static DeckManager Instance { get; private set; }
		#endregion

		#region Inspector Vars
		[Header("All Card Datas")]
		[SerializeField] CardData[] cardDatas;

		[Header("References")]
		[SerializeField] Card cardPrefab;
		[SerializeField] Transform handCanvasParent;
		#endregion

		#region Local Vars
		private List<CardData> currentDeck;
		#endregion

		#region Parameters/Getters
		#endregion

		#region Mono Implementation
		private void Awake() {
			// Static singleton.
			if ( Instance != null ) Destroy( this );
			else Instance = this;
		}

		private void Start() {
			// Generate and shuffle a new deck.
			currentDeck = new List<CardData>( cardDatas );
			currentDeck.Shuffle(7); // 7 shuffles for "true random"
		}
		#endregion

		#region Events
		#endregion

		#region Helpers
		public void DrawCard() {
			// Draw the top card from the current deck
			CardData nextCard = currentDeck.Pop();
			// Instantiate card.
			Card card = Instantiate(cardPrefab, handCanvasParent);
			card.ApplyCardData( nextCard );
		}
		#endregion

		#region Coroutines
		#endregion

	}
}