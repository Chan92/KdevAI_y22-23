using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BtCollect : BtNode {
	private Blackboard blackboard;
	private string objectString;

	public BtCollect(Blackboard _blackboard, string _object) {
		blackboard = _blackboard;
        objectString = _object;		
	}

	private bool Collected() {
		PickupableItem collectable = blackboard.GetData<Transform>(objectString).GetComponent<PickupableItem>();

		if (collectable == null) {
			Debug.Log($"Collecting {objectString} failed");
			return false;
		}
		
		bool collected = false;
        switch (collectable.Type) {
            case ItemType.Weapon:
                blackboard.SetData<bool>(StringNames.Bool_HasWeapon, true);
                collected = true;
				break;
            default:
                collected = false;
				break;
        }

		if (collected) {
            collectable.Collect(blackboard.GetData<Transform>(StringNames.Transform_BBowner));
        }

		return collected;
    }

	public override BtResult Run() {
		if(Collected()) {
			Debug.Log($"Collecting {objectString} succeed");			
			return BtResult.success;
		} else {
			Debug.Log($"Collecting {objectString} failed");
			return BtResult.failed;
		}
	}
}
