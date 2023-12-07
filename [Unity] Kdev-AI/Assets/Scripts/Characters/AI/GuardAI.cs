using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GuardAI : MonoBehaviour {
	[SerializeField]
	private Transform[] waypoints;
	[SerializeField]
	private Transform player;
	[SerializeField]
	private Transform weapon;
	[SerializeField]
	private float generalOffset = 0.3f;

	[Header("Detect")]
	[SerializeField]
	private float detectRadiusFar = 10f;
	[SerializeField]
	private float detectRadiusNear = 5f;
	[SerializeField]
	private float sightAnglePoint = 20f;
	[SerializeField]
	private float sightAngleFull = 360f;
	[SerializeField]
	private float collectRange = 2f;

	[Header("Combat")]
	[SerializeField]
	private float chaseRange = 10f;
	[SerializeField]
	private float attackRange = 3f;
	[SerializeField]
	private float attackStrength = 10f;

	[Header("Other")]
	[SerializeField]
	private TMP_Text statusUiText;

    private int wayPointId;
	private BtNode tree;
	private Blackboard blackBoard;

	private void Start() {
		SetData();
		SetTree();
	}

	private void SetData() {
		blackBoard = new Blackboard();

		blackBoard.SetData<Transform>(StringNames.Transform_BBowner, transform);
		blackBoard.SetData<Transform>(StringNames.Transform_Player, player);
		blackBoard.SetData<Transform>(StringNames.Transform_Item_Weapon, weapon);

		blackBoard.SetData<bool>(StringNames.Bool_HasWeapon, false);		
		blackBoard.SetData<float>(StringNames.Float_GeneralOffset, generalOffset);

		blackBoard.SetData<float>(StringNames.Float_DetectRadiusFar, detectRadiusFar);
		blackBoard.SetData<float>(StringNames.Float_DetectRadiusNear, detectRadiusNear);
		blackBoard.SetData<float>(StringNames.Float_SightAnglePoint, sightAnglePoint);
		blackBoard.SetData<float>(StringNames.Float_SightAngleFull, sightAngleFull);
		blackBoard.SetData<float>(StringNames.Float_CollectRange, collectRange);

		blackBoard.SetData<float>(StringNames.Float_ChaseRange, chaseRange);
		blackBoard.SetData<float>(StringNames.Float_AttackRange, attackRange);
		blackBoard.SetData<float>(StringNames.Float_AttackStrength, attackStrength);

		blackBoard.SetData<Transform>(StringNames.Transform_CurrentTarget, waypoints[GetNextWayPoint()]);
        blackBoard.SetData<TMP_Text>(StringNames.text_StatusUiText, statusUiText);
    }


	private void SetTree() {
		#region SetNodes
		BtInRange inRangePlayer		= new BtInRange  (blackBoard, StringNames.Transform_Player, StringNames.Float_ChaseRange);
		BtAttack attackPlayer		= new BtAttack   (blackBoard, StringNames.Transform_Player);
		BtMoveTo moveToPlayer		= new BtMoveTo   (blackBoard, StringNames.Transform_Player);
		BtHasObject hasWeapon		= new BtHasObject(blackBoard, StringNames.Bool_HasWeapon);

		BtInRange inRangeWeapon		= new BtInRange	 (blackBoard, StringNames.Transform_Item_Weapon, StringNames.Float_CollectRange);
		BtCollect collectWeapon		= new BtCollect  (blackBoard, StringNames.Transform_Item_Weapon);
		BtMoveTo moveToWeapon		= new BtMoveTo   (blackBoard, StringNames.Transform_Item_Weapon);
		BtDetect detectWeapon		= new BtDetect   (blackBoard, StringNames.Tag_Item_Weapon, StringNames.Float_SightAngleFull);

		BtInRange inRangeWaypoint	= new BtInRange	 (blackBoard, waypoints[wayPointId], StringNames.Float_GeneralOffset);
		BtMoveTo moveToWaypoint		= new BtMoveTo	 (blackBoard, waypoints[wayPointId]);

		BtDetect detectPlayer		= new BtDetect   (blackBoard, StringNames.Tag_Player, StringNames.Float_SightAnglePoint);
		#endregion

		#region Combat
		BtSequence sqCombatAttack = new BtSequence(
				new BtDebug("GuardAI Debug: sqCombatAttack", "red"),
				inRangePlayer,
				attackPlayer
			);

		BtSequence sqMoveToCombat = new BtSequence(
				new BtDebug("GuardAI Debug: sqMoveToCombat", "red"),
				moveToPlayer,
				attackPlayer
			);

		BtSelector slStartCombat = new BtSelector(
				new BtStatusUI(blackBoard, "ATTACK!!!"),
                sqCombatAttack,
				sqMoveToCombat
			);

		BtSequence sqCheckCombat = new BtSequence(
				hasWeapon,
				slStartCombat
			);

		#endregion
		#region Find Weapon
		BtSequence sqCollectWeapon = new BtSequence(
				new BtDebug("GuardAI Debug: sqCollectWeapon", "red"),
				inRangeWeapon,
				collectWeapon,
				slStartCombat
			);

		BtSequence sqMoveToWeapon = new BtSequence(
				new BtDebug("GuardAI Debug: sqMoveToWeapon", "red"),
				moveToWeapon,
				collectWeapon,
				slStartCombat
			);


		BtSelector slObtainWeapon = new BtSelector(				
                sqCollectWeapon,
				sqMoveToWeapon
			);

		BtSequence sqFindWeapon = new BtSequence(
				new BtStatusUI(blackBoard, "GetWeapon"),
                //detectWeapon, //disabled cause pre assigned
                slObtainWeapon
			);
		#endregion

		BtSelector slStartOffense = new BtSelector(
				sqCheckCombat,
				sqFindWeapon
			);

		#region Patrol
		BtSequence sqPatrol = new BtSequence(
				new BtDebug("GuardAI Debug: sqPatrol", "red"), 
				new BtStatusUI(blackBoard, "Patrol"),
                new BtInverter(inRangeWaypoint),
                moveToWaypoint,
                new BtWait(1f)
            );
		#endregion

		BtSequence sqDetectIntruder = new BtSequence(
				new BtDebug("GuardAI Debug: sqDetectIntruder", "red"),
				detectPlayer,
				slStartOffense
			);

		tree = new BtSelector(
				sqDetectIntruder,
				sqPatrol
			);
	}

	
	private void Update() {
		if(tree != null && tree.Run() != BtNode.BtResult.running) {
			tree.Run();
			
			if(tree.Run() == BtNode.BtResult.success) {
				GetNextWayPoint();
				SetTree();
			}
        }
	}

	int GetNextWayPoint() {
		wayPointId = (wayPointId + 1) % waypoints.Length;
		return wayPointId;
	}
}
