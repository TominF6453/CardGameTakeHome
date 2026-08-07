using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame {
	/// <summary>
	/// The actual in-game card, prefab component.
	/// </summary>
	public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

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

		#region DragHandler Implementation
		public void OnBeginDrag( PointerEventData eventData ) => throw new System.NotImplementedException();
		public void OnDrag( PointerEventData eventData ) => throw new System.NotImplementedException();
		public void OnEndDrag( PointerEventData eventData ) => throw new System.NotImplementedException();
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