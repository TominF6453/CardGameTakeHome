using UnityEngine;
using System.Linq;

namespace CardGame {
    /// <summary>
    /// Manager class for the play area, stores its own 2D grid of cards
    /// that handle backend positioning for the canvas.
    /// </summary>
    public class PlayAreaManager : MonoBehaviour {

        #region Statics & Constants
        public static PlayAreaManager Instance { get; private set; }
        #endregion

        #region Inspector Vars
        [Header("References")]
        [SerializeField] Transform playAreaTransform;

		[Header("Parameters")]
		[Tooltip("The amount of cards fitting in the play area horizontally. Should be odd for a proper center.")]
		[SerializeField] int playAreaWidthCards = 11;
		[Tooltip("The amount of cards fitting in the play area vertically. Should be odd for a proper center.")]
		[SerializeField] int playAreaHeightCards = 3;

		// Values are derived from in-editor testing.
		[SerializeField] float lateralCardOffset = 120f;
		[SerializeField] float verticalCardOffset = 167f;
		#endregion

		#region Local Vars
		private CardSlot[,] cardSlotGrid;
		#endregion

		#region Parameters/Getters
		public static Transform PlayArea => Instance.playAreaTransform;
		public static int GridXSize => Instance.playAreaWidthCards;
		public static int GridYSize => Instance.playAreaHeightCards;
		public static float LateralCardOffset => Instance.lateralCardOffset;
		public static float VerticalCardOffset => Instance.verticalCardOffset;

		public static bool IsPointInPlayArea ( Vector2 screenPoint ) {
			return RectTransformUtility.RectangleContainsScreenPoint( (RectTransform)Instance.playAreaTransform , screenPoint );
		}
		#endregion

		#region Mono Implementation
		private void Awake() {
			// Static singleton.
			if ( Instance == null ) Destroy( this );
			else Instance = this;
		}

		private void Start() {
			// Generate the card grid starting with the one in the middle.
			ConstructCardGrid();
		}
		#endregion

		#region Events
		#endregion

		#region Helpers
		private void ConstructCardGrid() {
			// Define a 2D array to store all the card slots.
			cardSlotGrid = new CardSlot[playAreaWidthCards , playAreaHeightCards];

			// Instantiate all the slots.
			for ( int x = 0; x < playAreaWidthCards; x++ ) { 
				for ( int y = 0; y < playAreaHeightCards; y++ ) {
					float canvasX = lateralCardOffset * (x - playAreaWidthCards / 2 ); // Int division should floor.
					float canvasY = verticalCardOffset * (y - playAreaHeightCards / 2 ); // Means edgemost pos will be 0 - width/height, or -width/height

					// Instantiate slot.
					cardSlotGrid[x , y] = new( canvasX , canvasY , x , y );

					// Check if slot is middle, set to available.
					if ( canvasX == 0 && canvasY == 0 ) {
						cardSlotGrid[x , y].status = CardSlot.CardSlotStatus.Available;
					}
				}
			}
		}

		public static CardSlot GetClosestSlot( Vector2 pos ) {
			return Instance.FindClosestAvailable( pos );
		}
		private CardSlot FindClosestAvailable( Vector2 localPos ) {
			// Find the closest CardSlot marked Available based on the canvas local position.
			CardSlot closest = null;
			float dist = float.MaxValue; // searching for min

			foreach ( CardSlot slot in cardSlotGrid ) {
				// Check if available first.
				if ( slot.status != CardSlot.CardSlotStatus.Available ) continue;

				// Check distance.
				float testDist = ( slot.GetCanvasPosition - localPos ).magnitude;
				if ( testDist < dist ) {
					dist = testDist;
					closest = slot;
				}
			}

			return closest;
		}

		public static void PlayCard( CardSlot slot , CardData card ) {
			Instance.AssignCardToSlot( slot, card );
		}
		private void AssignCardToSlot( CardSlot slot, CardData card ) {
			if ( slot.status != CardSlot.CardSlotStatus.Available ) return; // just in case

			// Set card.
			slot.SetCard( card );

			// Make neighbours available.
			MakeSlotAvailable( slot.GetGridPosition.x - 1 , slot.GetGridPosition.y );
			MakeSlotAvailable( slot.GetGridPosition.x + 1 , slot.GetGridPosition.y );
			MakeSlotAvailable( slot.GetGridPosition.x , slot.GetGridPosition.y - 1 );
			MakeSlotAvailable( slot.GetGridPosition.x , slot.GetGridPosition.y + 1 );
		}

		private void MakeSlotAvailable( int x, int y ) {
			if ( x < 0 || y < 0 || x >= playAreaWidthCards || y >= playAreaHeightCards ) return; // Out of array bounds.

			// Check the slot isn't already full, then make available.
			CardSlot slot = cardSlotGrid[x , y];
			if ( slot.status == CardSlot.CardSlotStatus.Locked ) slot.status = CardSlot.CardSlotStatus.Available;
		}
		#endregion

		#region Coroutines
		#endregion

	}

	public class CardSlot {
		public enum CardSlotStatus { Locked, Available, Full }

		float canvasXPos, canvasYPos;
		int gridXPos, gridYPos;

		public CardSlotStatus status = CardSlotStatus.Locked;

		CardData presentCard;

		public CardSlot( float canvasX, float canvasY, int gridX, int gridY ) {
			canvasXPos = canvasX;
			canvasYPos = canvasY;
			gridXPos = gridX;
			gridYPos = gridY;
		}

		public Vector2 GetCanvasPosition => new(canvasXPos, canvasYPos);
		public (int x,int y) GetGridPosition => new(gridXPos, gridYPos); // Vector2 stores floats, so use a tuple instead.

		public void SetCard( CardData card ) {
			presentCard = card;
			status = CardSlotStatus.Full;
		}
	}
}