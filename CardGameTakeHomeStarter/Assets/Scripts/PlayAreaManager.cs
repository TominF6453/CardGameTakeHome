using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;

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
		[SerializeField] TextMeshProUGUI scoreText;

		[Header("Parameters")]
		[Tooltip("The amount of cards fitting in the play area horizontally. Should be odd for a proper center.")]
		[SerializeField] int playAreaWidthCards = 11;
		[Tooltip("The amount of cards fitting in the play area vertically. Should be odd for a proper center.")]
		[SerializeField] int playAreaHeightCards = 3;

		// Values are derived from in-editor testing.
		[SerializeField] float lateralCardOffset = 120f;
		[SerializeField] float verticalCardOffset = 167f;

		[Header("End Animation Parameters")]
		[SerializeField] float cardScoringDelay = 0.2f;
		[SerializeField] float cardScoringAnimLength = 0.5f;
		[SerializeField] float cardScoringMaxJiggleAngle = 5f;
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

		// Game is complete either when the play area is full, or there are no more cards to be played.
		public static bool IsGameComplete => IsPlayAreaFull() || (DeckManager.IsDeckEmpty && HandManager.IsHandEmpty);

		// Convert canvas local position to world.
		public static Vector3 GetSlotWorldPos( CardSlot slot ) => Instance.playAreaTransform.TransformPoint( slot.GetCanvasPosition );
		public static bool IsPointInPlayArea ( Vector2 screenPoint ) {
			return RectTransformUtility.RectangleContainsScreenPoint( (RectTransform)Instance.playAreaTransform, screenPoint );
		}
		public static bool IsPlayAreaFull() {
			foreach ( CardSlot slot in Instance.cardSlotGrid ) {
				if ( slot.status != CardSlot.CardSlotStatus.Full ) return false;
			}
			return true;
		}
		#endregion

		#region Mono Implementation
		private void Awake() {
			// Static singleton.
			if ( Instance != null ) Destroy( this );
			else Instance = this;
		}

		private void Start() {
			// Generate the card grid starting with the one in the middle.
			ConstructCardGrid();

			// Make sure score text is disabled.
			scoreText.text = $"Score: 0";
			scoreText.gameObject.SetActive( false );
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

		public static void PlayCard( CardSlot slot , Card card ) {
			Instance.AssignCardToSlot( slot, card );
		}
		private void AssignCardToSlot( CardSlot slot, Card card ) {
			if ( slot.status != CardSlot.CardSlotStatus.Available ) return; // just in case

			// Set card.
			slot.SetCard( card );

			// Make neighbours available.
			MakeSlotAvailable( slot.GetGridPosition.x - 1 , slot.GetGridPosition.y );
			MakeSlotAvailable( slot.GetGridPosition.x + 1 , slot.GetGridPosition.y );
			MakeSlotAvailable( slot.GetGridPosition.x , slot.GetGridPosition.y - 1 );
			MakeSlotAvailable( slot.GetGridPosition.x , slot.GetGridPosition.y + 1 );

			// Check game state for a complete game.
			CheckGameState();
		}

		private void MakeSlotAvailable( int x, int y ) {
			if ( x < 0 || y < 0 || x >= playAreaWidthCards || y >= playAreaHeightCards ) return; // Out of array bounds.

			// Check the slot isn't already full, then make available.
			CardSlot slot = cardSlotGrid[x , y];
			if ( slot.status == CardSlot.CardSlotStatus.Locked ) slot.status = CardSlot.CardSlotStatus.Available;
		}

		private void CheckGameState() {
			if ( IsGameComplete ) {
				// Disable other UIs.
				DeckManager.Instance.DisableUI();
				HandManager.Instance.DisableUI();

				// Start scoring routine.
				StartCoroutine(CountScoreCoroutine());
			}
		}

		private void UpdateScoreText( int score ) => scoreText.text = $"Score: {score}";
		#endregion

		#region Coroutines
		IEnumerator CountScoreCoroutine() {
			// Initialize stuff, enable score UI.
			int score = 0;
			scoreText.gameObject.SetActive( true );

			foreach ( CardSlot slot in cardSlotGrid ) {
				// Loop through each slot, do a fun little animation, and add the score total.
				if ( slot.status == CardSlot.CardSlotStatus.Full ) {
					StartCoroutine( CardAnimation( slot ) );
					score += slot.GetCard.GetValue;
					UpdateScoreText( score );
				}
				yield return new WaitForSeconds( cardScoringDelay ); // Small wait time to add a bit of a wave to scoring.
			}

			yield return null;
		}

		IEnumerator CardAnimation( CardSlot slot ) {
			Transform cardTransform = slot.GetCardTransform;

			// Doing scale and rotation animations, store original values to reset.
			Vector3 origScale = cardTransform.localScale;
			Quaternion origRotation = cardTransform.rotation;

			for ( float t = 0f; t < 1; t += Time.deltaTime / cardScoringAnimLength ) {
				// Scale up and down via sin.
				float scale = 1 + (0.3f * Mathf.Sin( t * Mathf.PI ) );
				cardTransform.localScale = origScale * scale;

				// Jiggle back and forth, rotations.
				float angle = Mathf.Sin( t * Mathf.PI ) * cardScoringMaxJiggleAngle * (1-t);
				cardTransform.localRotation = origRotation * Quaternion.Euler( 0 , 0 , angle );

				yield return null;
			}

			// Reset after animating.
			cardTransform.localScale = origScale;
			cardTransform.rotation = origRotation;
		}
		#endregion

	}

	public class CardSlot {
		public enum CardSlotStatus { Locked, Available, Full }

		float canvasXPos, canvasYPos;
		int gridXPos, gridYPos;

		public CardSlotStatus status = CardSlotStatus.Locked;

		CardData presentCard;
		Card cardUI;

		public CardSlot( float canvasX, float canvasY, int gridX, int gridY ) {
			canvasXPos = canvasX;
			canvasYPos = canvasY;
			gridXPos = gridX;
			gridYPos = gridY;
		}

		public CardData GetCard => presentCard;
		public Transform GetCardTransform => cardUI.transform;
		public Vector2 GetCanvasPosition => new(canvasXPos, canvasYPos);
		public (int x,int y) GetGridPosition => new(gridXPos, gridYPos); // Vector2 stores floats, so use a tuple instead.

		public void SetCard( Card card ) {
			cardUI = card;
			presentCard = card.GetData;
			status = CardSlotStatus.Full;
		}


	}
}