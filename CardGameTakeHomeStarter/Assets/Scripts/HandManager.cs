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

		[Header("Parameters")]
		[SerializeField] int handSize = 5;
		#endregion

		#region Local Vars
		List<Card> cardsInHand;
		#endregion

		#region Parameters/Getters
		public bool HandFull => cardsInHand.Count >= handSize;
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
			card.transform.parent = handArea;
		}
		public static void AddCardToHand( Card card ) => Instance.AddCard( card );
		public static bool AtMaxHandSize => Instance.HandFull;
		#endregion

		#region Coroutines
		#endregion

	}
}