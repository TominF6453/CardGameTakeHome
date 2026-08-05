using UnityEngine;
using UnityEngine.UI;

namespace CardGame {
	/// <summary>
	/// The actual in-game card, prefab component.
	/// </summary>
	public class Card : MonoBehaviour {

		#region Statics & Constants
		#endregion

		#region Inspector Vars
		[Header("References")]
		[SerializeField] Image cardSprite;
		#endregion

		#region Local Vars
		CardData cardData;
		#endregion

		#region Parameters/Getters
		#endregion

		#region Mono Implementation
		#endregion

		#region Events
		#endregion

		#region Helpers
		public void ApplyCardData( CardData card ) {
			cardData = card;
			cardSprite.sprite = CardSpriteManager.GetSpriteForCard( card );
		}
		#endregion

		#region Coroutines
		#endregion

	}
}