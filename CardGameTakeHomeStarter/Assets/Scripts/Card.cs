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

		CanvasGroup canvasGroup;
		RectTransform rect;
		#endregion

		#region Parameters/Getters
		public CardData GetData => cardData;
		#endregion

		#region Mono Implementation
		private void Start() {
			// On the prefab.
			canvasGroup = GetComponent<CanvasGroup>();
			rect = GetComponent<RectTransform>();
		}
		#endregion

		#region DragHandler Implementation
		public void OnBeginDrag( PointerEventData eventData ) {
			// Stop blocking raycasts and remove from hand.
			SetInteractable(false);
			transform.SetParent( DeckManager.GetRootCanvas.transform );
			transform.localScale = Utilities.SameValueVector( 1.2f ); // Make card slightly (20%) bigger when dragging (picking it up off the table)
		}
		public void OnDrag( PointerEventData eventData ) {
			// Convert the screen position to the root canvas position, set card position.
			RectTransformUtility.ScreenPointToWorldPointInRectangle( (RectTransform)DeckManager.GetRootCanvas.transform , eventData.position , eventData.pressEventCamera , out Vector3 worldPoint );
			rect.position = worldPoint;
		}
		public void OnEndDrag( PointerEventData eventData ) {
			// Always reset size.
			transform.localScale = Vector3.one;

			// If dropped in the play area...
			if ( PlayAreaManager.IsPointInPlayArea( eventData.position ) ) {
				// Get closest slot to screen mouse position.
				RectTransformUtility.ScreenPointToLocalPointInRectangle( (RectTransform)PlayAreaManager.PlayArea , eventData.position , eventData.pressEventCamera , out Vector2 localPoint );
				CardSlot slot = PlayAreaManager.GetClosestSlot( localPoint );

				// If the slot is valid...
				if ( slot != null ) {
					// Play the card in the data array.
					PlayAreaManager.PlayCard( slot , this );

					// Play the physical card in the canvas rect for the play area based on the slot's canvas positions.
					transform.SetParent( PlayAreaManager.PlayArea );
					rect.position = PlayAreaManager.GetSlotWorldPos( slot );

					// Remove the card from the HandManager.
					HandManager.RemoveCardFromHand( this );
				} else {
					// Return card to hand.
					SetInteractable(true); // Needs to be selectable again.
					transform.SetParent( HandManager.GetHandTransform );
				}
			// If dropped in the discard area...
			} else if ( DeckManager.IsPointInDiscardArea( eventData.position ) ) {
				// Remove the card from hand, then just destroy it.
				HandManager.RemoveCardFromHand( this );
				Destroy( this.gameObject );
			} else { // Dropped anywhere else, return to hand.
				SetInteractable(true); // Needs to be selectable again.
				transform.SetParent( HandManager.GetHandTransform );
			}
		}
		#endregion

		#region Events
		#endregion

		#region Helpers
		public void ApplyCardData( CardData card ) {
			cardData = card;
			cardSprite.sprite = CardSpriteManager.GetSpriteForCard( card );
		}

		public void SetInteractable( bool enabled ) {
			canvasGroup.blocksRaycasts = enabled;
		}
		#endregion

		#region Coroutines
		#endregion

	}
}