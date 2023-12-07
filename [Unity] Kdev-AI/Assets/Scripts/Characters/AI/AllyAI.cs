using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AllyAI : MonoBehaviour {

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform enemy;
    [SerializeField]
    private float generalOffset = 0.3f;
    [SerializeField]
    private float followDistance = 1f;
    [SerializeField]
    private TMP_Text statusUiText;

    private BtNode tree;
    private Blackboard blackboard;

    private void Start () {
        SetData();
        SetTree();
    }

    private void SetData() {
        blackboard = new Blackboard();

        blackboard.SetData<Transform>(StringNames.Transform_BBowner, transform);
        blackboard.SetData<Transform>(StringNames.Transform_Player, player);
        blackboard.SetData<Transform>(StringNames.Transform_EnemyGuard, enemy);
        
        blackboard.SetData<float>(StringNames.Float_GeneralOffset, generalOffset);
        blackboard.SetData<float>(StringNames.Float_FollowDistance, followDistance);

        blackboard.SetData<TMP_Text>(StringNames.text_StatusUiText, statusUiText);
    }

    private void SetTree() {
        BtMoveTo moveToPlayer       = new BtMoveTo  (blackboard, StringNames.Transform_Player);
        BtInRange inRangePlayer     = new BtInRange (blackboard, StringNames.Transform_Player, StringNames.Float_FollowDistance);

        BtSequence sqFollowPlayer = new BtSequence(
                new BtDebug("AllyAI Debug: sqFollowPlayer", "cyan"),
                new BtStatusUI(blackboard, "Following"),
                new BtInverter(inRangePlayer),
                moveToPlayer
            );

        tree = new BtSelector(
                sqFollowPlayer
            );
    }

    private void Update() {
        if (tree != null && tree.Run() != BtNode.BtResult.running) {
            tree.Run();

            if (tree.Run() == BtNode.BtResult.success) {
                SetTree();
            }
        }
    }
}
