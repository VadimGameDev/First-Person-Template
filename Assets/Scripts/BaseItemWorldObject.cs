using UnityEngine;
using FPT.Base.Abstracts;

namespace FPT.Base.Entities
{
    public class BaseItemWorldObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private BaseItemData _itemData;

        public string _itemName;

        public string ItemName { get => _itemName ?? string.Empty; set => _itemName = value; }

        public void Interact()
        {
            TakeItem();
        }

        private void TakeItem()
        {
            //if (_itemData != null)
            //ToDo: MoveItemToInventory
            Debug.Log($"Item '{_itemName}' Taken!");
            Destroy(gameObject);

            if (name == "ColliderBook")
                Destroy(transform.parent.gameObject);
        }
    }
}
