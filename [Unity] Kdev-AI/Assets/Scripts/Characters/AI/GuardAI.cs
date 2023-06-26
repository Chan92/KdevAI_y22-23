using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GuardAI : MonoBehaviour {
	[SerializeField]
	private Transform[] waypoints;
	[SerializeField]
	private Transform player;
	[SerializeField]
	private Transform weapon;
	[SerializeField]
	private float anyOffset = 0.3f;

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

	private int wayPointId;
	private BtNode tree;
	private Blackboard blackBoard;

	private void Start() {
		SetData();
		SetTree();
	}

	private void SetData() {
		blackBoard = new Blackboard();

		blackBoard.SetData<Transform>("ThisTransform", transform);
		blackBoard.SetData<Transform>("Player", player);
		blackBoard.SetData<Transform>("Weapon", weapon);

		blackBoard.SetData<bool>("HasWeapon", false);		
		blackBoard.SetData<float>("AnyOffset", anyOffset);

		blackBoard.SetData<float>("DetectRadiusFar", detectRadiusFar);
		blackBoard.SetData<float>("DetectRadiusNear", detectRadiusNear);
		blackBoard.SetData<float>("SightAnglePoint", sightAnglePoint);
		blackBoard.SetData<float>("SightAngleFull", sightAngleFull);
		blackBoard.SetData<float>("CollectRange", collectRange);

		blackBoard.SetData<float>("ChaseRange", chaseRange);
		blackBoard.SetData<float>("AttackRange", attackRange);
		blackBoard.SetData<float>("AttackStrength", attackStrength);

		blackBoard.SetData<Transform>("CurrentTarget", waypoints[GetNextWayPoint()]);
	}


	private void SetTree() {
		#region SetNodes
		BtInRange inRangePlayer		= new BtInRange  (blackBoard, "Player", "ChaseRange");
		BtAttack attackPlayer		= new BtAttack   (blackBoard, "Player");
		BtMoveTo moveToPlayer		= new BtMoveTo   (blackBoard, "Player");
		BtHasObject hasWeapon		= new BtHasObject(blackBoard, "Weapon");

		BtInRange inRangeWeapon		= new BtInRange	 (blackBoard, "Weapon", "CollectRange");
		BtCollect collectWeapon		= new BtCollect  (blackBoard, "Weapon");
		BtMoveTo moveToWeapon		= new BtMoveTo   (blackBoard, "Weapon");
		BtDetect detectWeapon		= new BtDetect   (blackBoard, "Weapon", "SightAngleFull");

		BtInRange inRangeWaypoint	= new BtInRange	 (blackBoard, waypoints[wayPointId], "AnyOffset");
		BtMoveTo moveToWaypoint		= new BtMoveTo	 (blackBoard, waypoints[wayPointId]);

		BtDetect detectPlayer		= new BtDetect   (blackBoard, "Player", "SightAnglePoint");
		#endregion

		#region Combat
		BtSequence sqCombatAttack = new BtSequence(
				new BtDebug("Debug: sqCombatAttack"),
				inRangePlayer,
				attackPlayer
			);

		BtSequence sqMoveToCombat = new BtSequence(
				new BtDebug("Debug: sqMoveToCombat"),
				moveToPlayer,
				attackPlayer
			);

		BtSelector slStartCombat = new BtSelector(
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
				new BtDebug("Debug: sqCollectWeapon"),
				inRangeWeapon,
				collectWeapon,
				slStartCombat
			);

		BtSequence sqMoveToWeapon = new BtSequence(
				new BtDebug("Debug: sqMoveToWeapon"),
				moveToWeapon,
				collectWeapon,
				slStartCombat
			);


		BtSelector slObtainWeapon = new BtSelector(
				sqCollectWeapon,
				sqMoveToWeapon
			);

		BtSequence sqFindWeapon = new BtSequence(
				detectWeapon,
				slObtainWeapon
			);
		#endregion

		BtSelector slStartOffense = new BtSelector(
				sqCheckCombat,
				sqFindWeapon
			);

		#region Patrol
		BtSequence sqPatrol = new BtSequence(
				new BtDebug("Debug: sqPatrol"),
				new BtInverter(inRangeWaypoint),
				moveToWaypoint
			);
		#endregion

		BtSequence sqDetectIntruder = new BtSequence(
				new BtDebug("Debug: sqDetectIntruder"),
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
