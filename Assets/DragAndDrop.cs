/*
   .-------.                             .--.    .-------.     .--.            .--.     .--.        
   |       |--.--.--------.-----.-----.--|  |    |_     _|--.--|  |_.-----.----|__|---.-|  |-----.
   |   -   |_   _|        |  _  |     |  _  |      |   | |  |  |   _|  _  |   _|  |  _  |  |__ --|
   |_______|__.__|__|__|__|_____|__|__|_____|      |___| |_____|____|_____|__| |__|___._|__|_____|
   © 2019 OXMOND / www.oxmond.com 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    public bool isDragging;
    private Vector3 offset;
    private Vector3 holdPosition;
    private int holdLayer = 0;
    public void OnMouseDown()
    {
        GameManager.Instance.selectedCard = gameObject;
        holdLayer = GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder;
        GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 100;
        holdPosition = gameObject.transform.position;

        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;
        transform.position = holdPosition;
        GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder = holdLayer;
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


    public float GetDistanceFromDraggingCard(Vector3 dCard)
	{
        return Vector2.Distance(dCard, transform.position);
	}

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - offset;
            transform.Translate(mousePosition);
        }
    }
}