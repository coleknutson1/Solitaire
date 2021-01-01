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
    private string sortInt;
    public void OnMouseDown()
    {
        GameManager.Instance.selectedCard = gameObject;
        holdPosition = gameObject.transform.position;
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        sortInt = GetComponent<SpriteRenderer>().sortingLayerName;
        GetComponent<SpriteRenderer>().sortingLayerName = "25";
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;
        transform.position = holdPosition;
        var closestObject = GameManager.Instance.CheckOverlap();
        if (closestObject == null)
            return;
        GetComponent<SpriteRenderer>().sortingLayerName = sortInt;
        if(closestObject.GetComponent<PlayingCard>().suitColor != GetComponent<PlayingCard>().suitColor)
		{
            transform.parent = closestObject.transform.parent;
            transform.position = transform.parent.position - new Vector3(0f, (Int32.Parse(sortInt) * .1f), 0f);
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