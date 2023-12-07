using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BtStatusUI : BtNode {
    private Blackboard blackboard;
    private string status;

    public BtStatusUI(Blackboard _blackboard, string _status) {
        blackboard = _blackboard;
        status = _status;
    }

    public override BtResult Run() {
        TMP_Text statusTextUi = blackboard.GetData<TMP_Text>(StringNames.text_StatusUiText);
        if (statusTextUi != null) {
            statusTextUi.text = status;
        }
        
        return BtResult.success;
    }
}