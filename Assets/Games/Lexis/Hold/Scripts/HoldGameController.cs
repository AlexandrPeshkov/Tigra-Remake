using Assets.Scripts.Interface.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Games.Lexis.Hold.Scripts
{
    class HoldGameController : MonoBehaviour
    {
        public WellDoneDialog wellDoneDialogPrefab;
        public RetryDialog retryDialogPrefab;
        public Transform Canvas;

        public List<ChestSlot> LeftChests;
        public List<ChestSlot> RightChests;

        public ItemCard ItemCardPrefab;

        public Transform CardPosition;

        [Serializable]
        public class ItemCardAndPictures
        {
            public ItemCardCategory category;

            //true - used
            [SerializeField]
            public List<BoolSprite> Sprites;
            public void Reset()
            {
                for (var i = 0; i < Sprites.Count; i++)
                {
                    Sprites[i].IsUsed = false;
                }
            }
        }

        [Serializable]
        public class BoolSprite
        {
            public bool IsUsed;
            public Sprite sprite;
        }

        [SerializeField]
        public List<ItemCardAndPictures> CardPictures;

        private bool IsWin { get { return LeftChests.All(c => !c.IsEmpty) && RightChests.All(c => !c.IsEmpty); } }

        private void Start()
        {
            InitChests();
            StartGame();
        }

        private void StartGame()
        {
            ItemCardCategory category = (ItemCardCategory)UnityEngine.Random.Range(1, 9);
            MakeItemCard(category);
        }

        private void InitChests()
        {
            LeftChests.ForEach(c => c.OnCardDrop += OnCardDroped);
            RightChests.ForEach(c => c.OnCardDrop += OnCardDroped);
        }

        private void RestartGame()
        {
            LeftChests.ForEach(c => c.Clear());
            RightChests.ForEach(c => c.Clear());
            CardPictures.ForEach(p => p.Reset());
            StartGame();
        }

        private void MakeItemCard(ItemCardCategory category)
        {
            Sprite sprite = GetAvailableSprite(category);
            ItemCard itemCard = Instantiate(ItemCardPrefab, CardPosition);
            itemCard.Construct(category, sprite);
        }

        private Sprite GetAvailableSprite(ItemCardCategory category)
        {
            ItemCardAndPictures cardsPictures = CardPictures.Find(l => l.category == category);
            var sprites = cardsPictures.Sprites.Where(i => !i.IsUsed).ToList();
            if (sprites.Any())
            {
                int index = UnityEngine.Random.Range(0, sprites.Count);
                sprites[index].IsUsed = true;
                return sprites[index].sprite;
            }
            return null;
        }

        private void MakeRetryDialog()
        {
            RetryDialog retryDialog = Instantiate(retryDialogPrefab, Canvas);
            retryDialog.OkButton.button.onClick.AddListener(RestartGame);
            //retryDialog.CancellButton.button.onClick.AddListener();
        }

        private void SetChestsCategory(List<ChestSlot> chests, ItemCardCategory category)
        {
            chests.ForEach(c => c.ChestItemCategory = category);
        }

        private void CheckFirstCardDropped(List<ChestSlot> chests, ItemCard card)
        {
            if (chests.Where(c => c.ChestItemCategory == ItemCardCategory.Empty).Count() == 2)
            {
                SetChestsCategory(chests, card.Category);
            }
        }

        private void OnCardDroped(ItemCard card, bool IsCorrect)
        {
            if (IsWin)
            {
                WellDoneDialog winDialog = Instantiate(wellDoneDialogPrefab, Canvas);
                winDialog.OkButton.button.onClick.AddListener(MakeRetryDialog);
                AudioManager.instance.PlayCorrectly();
            }
            else
            {
                if (IsCorrect)
                {
                    CheckFirstCardDropped(RightChests, card);
                    CheckFirstCardDropped(LeftChests, card);
                    
                    var leftCategories = GetAvailableChestsCategories(LeftChests).ToList();
                    var rightCategories = GetAvailableChestsCategories(RightChests).ToList();

                    List<ItemCardCategory> AvailableCategories = leftCategories.Union(rightCategories).Where(c => !leftCategories.Intersect(rightCategories).Contains(c)).ToList();
                    ItemCardCategory cardCategory = AvailableCategories[UnityEngine.Random.Range(0, AvailableCategories.Count)];
                    MakeItemCard(cardCategory);

                    AudioManager.instance.PlayCorrectly();

                }
                else
                {
                    card.dragHandler.ResetPosition();
                    AudioManager.instance.PlayWrong();
                }
            }
        }

        private IEnumerable<ItemCardCategory> GetAvailableChestsCategories(List<ChestSlot> chests)
        {
            if (chests.All(c => c.ChestItemCategory == ItemCardCategory.Empty))
            {
                return Enum.GetValues(typeof(ItemCardCategory)).OfType<ItemCardCategory>().Where(c => c != ItemCardCategory.Empty);
            }
            else
            {
                if (chests.All(c => !c.IsEmpty))
                {
                    return Enumerable.Empty<ItemCardCategory>();
                }
                return chests.Where(c => c.ChestItemCategory != ItemCardCategory.Empty).Select(c => c.ChestItemCategory);
            }
        }
    }
}
