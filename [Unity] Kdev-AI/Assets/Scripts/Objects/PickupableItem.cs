using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour {
    [SerializeField]
    private ItemType type;

    public ItemType Type { get { return type; } }

    public void Collect(Transform newOwner) {
       OwnedItems ownerItems = newOwner.GetComponent<OwnedItems>();
        if (ownerItems) {
            ownerItems.ActivateItem(type);
        }

        Debug.Log($"{Type}:{transform.name} picked up by {newOwner.name}.");
        gameObject.SetActive(false);
    }
}

public enum ItemType {
    Weapon
}