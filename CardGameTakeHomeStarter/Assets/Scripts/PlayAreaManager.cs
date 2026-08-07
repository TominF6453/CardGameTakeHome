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
		public static int GridXSize => Instance.playAreaWidthCards;
		public static int GridYSize => Instance.playAreaHeightCards;
		public static float LateralCardOffset => Instance.lateralCardOffset;
		public static float VerticalCardOffset => Instance.verticalCardOffset;
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
		public (int,int) GetGridPosition => new(gridXPos, gridYPos); // Vector2 stores floats, so use a tuple instead.

		public void SetCard( CardData card ) {
			presentCard = card;
			status = CardSlotStatus.Full;
		}
	}
}