using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Games.Lexis.Hold.Scripts
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 startPos;
        private Transform startParent;

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = transform.position;
            startParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (transform.parent == startParent)
            {
                transform.position = startPos;
            }
        }

        public void ResetPosition()
        {
            transform.position = startPos;
        }
    }

}
