using UnityEngine;

namespace CardGame {

	public enum CardSuit {
		Hearts, Diamonds, Clubs, Spades
	}

	[CreateAssetMenu(fileName = "NewCardData", menuName = "CardGame/Card Data")]
	public class CardData : ScriptableObject {

		#region Statics & Constants
		#endregion

		#region Inspector Vars
		[Header( "Card Data" )]
		[SerializeField] CardSuit suit;
		[SerializeField] int value;
		#endregion

		#region Local Vars
		#endregion

		#region Parameters/Getters
		public int GetValue => value;
		public CardSuit GetSuit => suit;
		#endregion

		#region Mono Implementation
		#endregion

		#region Events
		#endregion

		#region Helpers
		public string GetValueName() {
			switch ( value ) {
				case 1: return "Ace";
				case 11: return "Jack";
				case 12: return "Queen";
				case 13: return "King";
				default: return value.ToString();
			}
		}
		public string GetName() {
			return $"{GetValueName()} of {suit}";
		}
		#endregion
	}
}