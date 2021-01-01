using UnityEngine;

public enum SuitColor { BLACK, RED }
public enum Suit { CLUB, SPADE, DIAMOND, HEART }
public enum Rank { ACE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13 }
public class PlayingCard : MonoBehaviour
{
	public Sprite cardBack;
	public Sprite cardFront;
	public SuitColor suitColor;
	public Suit suit;
	public Rank rank;
	public bool isFaceUp;
	private int layer;
	public void Awake()
	{
		isFaceUp = true;
		cardFront = GetComponent<SpriteRenderer>().sprite;
	}
	public void FlipCard()
	{
		isFaceUp = !isFaceUp;
		if (isFaceUp)
		{
			GetComponent<SpriteRenderer>().sprite = cardFront;
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = cardBack;
		}
	}
}
