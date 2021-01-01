/*
   .-------.                             .--.    .-------.     .--.            .--.     .--.        
   |       |--.--.--------.-----.-----.--|  |    |_     _|--.--|  |_.-----.----|__|---.-|  |-----.
   |   -   |_   _|        |  _  |     |  _  |      |   | |  |  |   _|  _  |   _|  |  _  |  |__ --|
   |_______|__.__|__|__|__|_____|__|__|_____|      |___| |_____|____|_____|__| |__|___._|__|_____|
   © 2019 OXMOND / www.oxmond.com 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    public bool isDragging;
    private Vector3 offset;
    public void OnMouseDown()
    {
        GameManager.Instance.selectedCard = gameObject;
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        GetComponent<SpriteRenderer>().sortingLayerName = "SelectedCard";
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;
        transform.position = transform.parent.position;
        GetComponent<SpriteRenderer>().sortingLayerName = "FaceUp";
        GameManager.Instance.CheckOverlap();
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