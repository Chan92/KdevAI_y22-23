using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedItems : MonoBehaviour {
    [SerializeField]
    private GameObject weapon;

    private void Start() {
        DeactivateItem(ItemType.Weapon);
    }

    public void ActivateItem(ItemType _newItem) {
        switch (_newItem) {
            case ItemType.Weapon:
                weapon.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DeactivateItem(ItemType _newItem) {
        switch (_newItem) {
            case ItemType.Weapon:
                weapon.SetActive(false);
                break;
            default: 
                break;
        }
    }
   
}
