using UnityEngine;

namespace CardGame {
	public class CardSpriteManager : MonoBehaviour {

		#region Statics & Constants
		public static CardSpriteManager Instance { private set; get; }
		#endregion

		#region Inspector Vars
		[Header("Card Sprites")]
		[SerializeField] Sprite[] cardSprites;
		#endregion

		#region Local Vars
		#endregion

		#region Parameters/Getters
		#endregion

		#region Mono Implementation
		private void Awake() {
			// Static singleton.
			if ( Instance != null ) Destroy( this );
			else Instance = this;
		}
		#endregion

		#region Events
		#endregion

		#region Helpers
		public static Sprite GetSpriteForCard( CardData card ) {
			return GetSpriteForCard( card.GetSuit , card.GetValue );
		}

		public static Sprite GetSpriteForCard( CardSuit suit, int value ) {
			// Suits are in the same order as the sprites, so suit is a multiple of 13.
			// Sprites are 0-indexed, values start at 1 (ace), subtract 1 from value.
			int index = ((int)suit * 13) + (value - 1);

			return Instance.cardSprites[index];
		}
		#endregion

		#region Coroutines
		#endregion

	}
}