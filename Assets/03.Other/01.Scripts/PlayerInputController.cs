using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private LayerMask mouseInputLayer;

    public RaycastHit PonterCast()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit hit, 100, mouseInputLayer);
        return hit;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && InGameManager.instance.PlayerState == PlayerState.canAct)
        {
            RaycastHit hit = PonterCast();
            if (CanFlip(hit, out Card card))
            {
                card.Flip();
                InGameManager.instance.SelectCard(card);
            }
        }
    }

    private bool CanFlip(RaycastHit hit, out Card _card)
    {
        bool value;
        if (hit.transform != null)
        {
            value = hit.transform.TryGetComponent(out Card card) && card.CanInteraction;
            _card = card;
        }
        else
        {
            value = false;
            _card = null;
        }
        return value;
    }
}
