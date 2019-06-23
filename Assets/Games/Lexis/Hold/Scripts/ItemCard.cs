using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Games.Lexis.Hold.Scripts
{
    public class ItemCard : MonoBehaviour
    {
        public ItemCardCategory Category;
        public Image Picture;
        public DragHandler dragHandler;

        public void Construct(ItemCardCategory category, Sprite sprite)
        {
            Category = category;
            Picture.sprite = sprite;
        }

        public void DisableDrag()
        {
            Destroy(dragHandler);
        }
    }

    public enum ItemCardCategory
    {
        Empty,
        Hats,
        Jewelry,
        Inventory,
        Vegetables,
        Underwear,
        Weapon,
        Dishes,
        Fruits
    }
}
