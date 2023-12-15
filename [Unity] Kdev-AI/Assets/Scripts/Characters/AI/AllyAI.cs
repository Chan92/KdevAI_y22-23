using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllyAI : MonoBehaviour {
    [SerializeField]
    private Transform[] coverpoints;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform enemy;
    [SerializeField]
    private float generalOffset = 0.3f;

	[Header("Combat")]
	[SerializeField]
    private SoAttack attack;
    [SerializeField]
    private float attackRange = 100f;
    [SerializeField]
    private float attackStrength = 0f;
    [SerializeField]
    private float attackCooldown = 0.7f;

	[Header("Other")]
    [SerializeField]
    private TMP_Text statusUiText;
	[SerializeField]
    private float followDistance = 1f;

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

		blackboard.SetData<float>(StringNames.Float_AttackRange, attackRange);
		blackboard.SetData<float>(StringNames.Float_AttackStrength, attackStrength);
		blackboard.SetData<float>(StringNames.Float_AttackCooldown, attackCooldown);

		blackboard.SetData<TMP_Text>(StringNames.text_StatusUiText, statusUiText);
    }

    private void SetTree() {
		#region SetNodes
		BtMoveTo moveToPlayer       = new BtMoveTo  (blackboard, StringNames.Transform_Player);
        BtInRange inRangePlayer     = new BtInRange (blackboard, StringNames.Transform_Player, StringNames.Float_FollowDistance);
		BtInRange inRangeCover      = new BtInRange(blackboard, coverpoints[0], StringNames.Float_GeneralOffset);
		BtAttack attackEnemy        = new BtAttack  (blackboard, StringNames.Transform_EnemyGuard, attack, 1);
        BtMoveTo moveToCover        = new BtMoveTo  (blackboard, coverpoints[0]);
       
        BtCheckStatusEffect checkAngryEnemy = new BtCheckStatusEffect(blackboard, StringNames.Transform_EnemyGuard, StatusEffects.Effects.Aggro, true);
		#endregion

		#region Combat
		BtSequence sqAttackEnemy = new BtSequence(
			    new BtStatusUI(blackboard, "Attack"),
				attackEnemy
			);


        BtSequence sqCombatAttack = new BtSequence(
			    new BtDebug("AllyAI Debug: sqCombatAttack", "cyan"),
				inRangeCover,
				sqAttackEnemy
			);

		#endregion

		#region Find Cover
		BtSequence sqFindCover = new BtSequence(
			    new BtDebug("AllyAI Debug: sqFindCover", "cyan"),
				new BtStatusUI(blackboard, "FindCover"),
				//ADD:Find cover //disabled cause pre assigned
				moveToCover,
				sqAttackEnemy
			);

		#endregion

		BtSelector slStartOffence = new BtSelector(
				sqCombatAttack,
                sqFindCover
            );

        BtSequence sqDettectOffence = new BtSequence(
			    new BtDebug("AllyAI Debug: sqDettectOffence", "cyan"),
				checkAngryEnemy, 
                slStartOffence
			);

		#region Follow
		BtSequence sqFollowPlayer = new BtSequence(
                new BtDebug("AllyAI Debug: sqFollowPlayer", "cyan"),
                new BtStatusUI(blackboard, "Following"),
                new BtInverter(inRangePlayer),
                moveToPlayer
            );
		#endregion

		tree = new BtSelector(
			    sqDettectOffence,
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
