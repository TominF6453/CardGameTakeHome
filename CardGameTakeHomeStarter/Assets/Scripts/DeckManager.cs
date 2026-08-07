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
		[SerializeField] Transform discardAreaTransform;
		[SerializeField] Canvas rootCanvas;
		[SerializeField] Transform[] deckUITransforms;
		#endregion

		#region Local Vars
		private List<CardData> currentDeck;
		#endregion

		#region Parameters/Getters
		public static Canvas GetRootCanvas => Instance.rootCanvas;
		public static bool IsDeckEmpty => Instance.currentDeck.Count == 0;

		public static bool IsPointInDiscardArea( Vector2 screenPoint ) {
			return RectTransformUtility.RectangleContainsScreenPoint( (RectTransform)Instance.discardAreaTransform , screenPoint );
		}
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
			// Check hand for space.
			if ( HandManager.IsHandFull ) {
				Debug.Log( $"[DeckManager] Not drawing, already at max hand size!" );
				return;
			}

			// Draw the top card from the current deck
			CardData nextCard = currentDeck.Pop();
			// Instantiate card.
			Card card = Instantiate(cardPrefab);
			card.ApplyCardData( nextCard );
			// Send to hand.
			HandManager.AddCardToHand( card );

			// Check deck empty, disable anything related to deck UI.
			if ( IsDeckEmpty ) {
				DisableUI();
			}
		}

		public void DrawToFull() {
			while ( !HandManager.IsHandFull && !IsDeckEmpty ) DrawCard();
		}

		public void DisableUI() {
			foreach ( Transform t in deckUITransforms ) t.gameObject.SetActive( false );
		}
		#endregion

		#region Coroutines
		#endregion

	}
}