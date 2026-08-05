using UnityEngine;

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
		#endregion

		#region Parameters/Getters
		#endregion

		#region Mono Implementation
		private void Awake() {
			// Static singleton.
			if ( Instance == null ) Destroy( this );
			else Instance = this;
		}
		#endregion

		#region Events
		#endregion

		#region Helpers
		#endregion

		#region Coroutines
		#endregion

	}
}