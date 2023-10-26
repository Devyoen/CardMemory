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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = PonterCast();
            if (hit.transform != null && hit.transform.TryGetComponent(out Card card))
            {
                card.Flip();
            }
        }
    }
}
