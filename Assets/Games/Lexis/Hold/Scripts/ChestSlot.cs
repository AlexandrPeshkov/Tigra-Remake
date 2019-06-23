using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Games.Lexis.Hold.Scripts
{

    public class ChestSlot : MonoBehaviour, IDropHandler
    {
        public ItemCardCategory ChestItemCategory = ItemCardCategory.Empty;

        public bool IsEmpty { get { return currentCard == null; } }

        public Image ImageSlot;

        private ItemCard currentCard = null;

        public event Action<ItemCard, bool> OnCardDrop;

        public void OnDrop(PointerEventData eventData)
        {
            ItemCard card = eventData.pointerDrag.GetComponent<ItemCard>();

            if (card != null)
            {
                bool IsCorrect = IsEmpty && (card.Category == ChestItemCategory || ChestItemCategory == ItemCardCategory.Empty);
                if (IsCorrect)
                {
                    currentCard = card;
                    ChestItemCategory = card.Category;
                    eventData.pointerDrag.transform.SetParent(ImageSlot.transform);

                    RectTransform rect = eventData.pointerDrag.GetComponent<RectTransform>();

                    rect.anchoredPosition = new Vector2(0, 0);
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(1, 1);

                    rect.offsetMin = new Vector2(0, 0);
                    rect.offsetMax = new Vector2(0, 0);
                    card.DisableDrag();
                }

                if (OnCardDrop != null)
                {
                    OnCardDrop.Invoke(card, IsCorrect);
                }
            }
        }

        public void Clear()
        {
            Destroy(currentCard.gameObject);
            ChestItemCategory = ItemCardCategory.Empty;
        }
    }
}
