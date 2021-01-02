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
	public bool isDragging;
	private Vector3 offset;
	private Vector3 holdPosition;

	public void Awake()
	{
		isFaceUp = true;
		cardFront = GetComponent<SpriteRenderer>().sprite;
	}

	public void OnMouseDown()
	{
		GameManager.Instance.selectedCard = gameObject;
		if (!GameManager.Instance.selectedCard.GetComponent<PlayingCard>().isFaceUp)
		{
			GameManager.Instance.selectedCard.GetComponent<PlayingCard>().FlipCard();
			return;
		}
		GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 100;
		holdPosition = gameObject.transform.position;

		offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		isDragging = true;
	}

	public void OnMouseUp()
	{
		isDragging = false;
		var closestObject = GameManager.Instance.CheckOverlap();
		if (closestObject == null)
			return;
		var closestObjectPlayingCard = closestObject.GetComponent<PlayingCard>();
		var currentPlayingCard = GetComponent<PlayingCard>();

		//If it's a valid lay, reparent current to new column
		if (currentPlayingCard.suitColor != closestObjectPlayingCard.suitColor)
		{
			transform.parent = closestObject.transform.parent;
			transform.position = closestObject.transform.parent.position - new Vector3(0f, closestObjectPlayingCard.transform.parent.transform.childCount * -.1f, 0f);
		}
		//GameManager.Instance.RecheckColumns();
	}

	public float GetDistanceFromDraggingCard(Vector2 dCard)
	{
		return Vector2.Distance(dCard, (Vector2)transform.position);
	}

	void Update()
	{
		if (isDragging)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - offset;
			transform.Translate(mousePosition);
		}
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
