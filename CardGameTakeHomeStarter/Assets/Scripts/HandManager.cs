using System.Collections.Generic;
using UnityEngine;

namespace CardGame {
    /// <summary>
    /// Simple manager for cards in hand.
    /// </summary>
    public class HandManager : MonoBehaviour {

        #region Statics & Constants
        public static HandManager Instance { get; private set; }
        #endregion

        #region Inspector Vars
        [Header("References")]
        [SerializeField] Transform handArea;
		[SerializeField] Transform[] handUITransforms;

		[Header("Parameters")]
		[SerializeField] int handSize = 5;
		#endregion

		#region Local Vars
		List<Card> cardsInHand;
		#endregion

		#region Parameters/Getters
		public static Transform GetHandTransform => Instance.handArea;
		public static bool IsHandEmpty => Instance.cardsInHand.Count == 0;
		public static bool IsHandFull => Instance.cardsInHand.Count >= Instance.handSize;
		#endregion

		#region Mono Implementation
		private void Awake() {
			// Static singleton.
			if ( Instance != null ) Destroy( this );
			else Instance = this;

			// Initialize.
			cardsInHand = new List<Card>();
		}
		#endregion

		#region Events
		#endregion

		#region Helpers
		public void AddCard( Card card ) {
			cardsInHand.Add( card );
			card.transform.SetParent(handArea);
		}
		public void RemoveCard( Card card ) {
			cardsInHand.Remove( card );
			// Game state meaningfully changes whenever a card is removed from the hand.
			PlayAreaManager.Instance.CheckGameState();
		}
		public static void AddCardToHand( Card card ) => Instance.AddCard( card );
		public static void RemoveCardFromHand( Card card ) => Instance.RemoveCard( card );

		public void DisableUI() {
			foreach ( Card card in cardsInHand ) card.SetInteractable( false );
			foreach ( Transform t in handUITransforms ) t.gameObject.SetActive( false );
		}
		
		public void EnableUI() {
			foreach ( Transform t in handUITransforms ) t.gameObject.SetActive( true );
		}

		public void ResetHandManager() {
			// Destroy all cards in hand, new hand list.
			foreach ( Card card in cardsInHand ) Destroy( card.gameObject );
			cardsInHand = new();

			// Reenable hand UIs
			EnableUI();
		}
		#endregion

		#region Coroutines
		#endregion

	}
}