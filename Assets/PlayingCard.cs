using UnityEngine;

public enum SuitColor { BLACK, RED }
public enum Suit { CLUB, SPADE, DIAMOND, HEART }
public enum Rank { ACE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13 }
public class PlayingCard : MonoBehaviour
{
	public GameObject cardGameObject;
	public SuitColor suitColor;
	public Suit suit;
	public Rank rank;
	public void FlipCard()
	{
		cardGameObject.transform.rotation = new Quaternion(0, 0, 180, 0);
	}
}
