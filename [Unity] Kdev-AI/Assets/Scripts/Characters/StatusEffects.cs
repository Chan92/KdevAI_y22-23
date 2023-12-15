using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour {
    public enum Effects {
        Aggro,
        InSmoke
    }
    
    private List<Effects> statuses = new List<Effects>();

    public void AddStatus(Effects newStatus) {
        if(!HasEffect(newStatus)) {
            statuses.Add(newStatus);
        }
    }

    public void RemoveEffect(Effects status) {
        if(HasEffect(status)) {
            statuses.Remove(status);
        }
    }

    public bool HasEffect(Effects status) {
        return statuses.Contains(status);
    }
}
