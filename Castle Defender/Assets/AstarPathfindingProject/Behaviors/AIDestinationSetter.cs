using UnityEngine;
using System.Collections;

namespace Pathfinding {

	public class AIDestinationSetter : MonoBehaviour { 
		public Transform target;
		private float detectionDistance = 4;
		private float distanceToPlayer ;
		private float distanceToCastle;
		IAstarAI ai;

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}
		private void Awake()
		{
			target = GameObject.Find("Base").transform;
		}
		/// <summary>Updates the AI's destination every frame</summary>
		void Update () {

			if (target != null && ai != null) ai.destination = target.position;
		}
		void FixedUpdate()
		{
			DetectTarget();
		}

		void DetectTarget()

		{
			distanceToPlayer = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
			distanceToCastle = Vector3.Distance(transform.position, GameObject.Find("Base").transform.position);
			if (distanceToPlayer < detectionDistance && distanceToCastle > distanceToPlayer)
			{
				target = GameObject.Find("Player").transform;
				GetComponent<AIPath>().endReachedDistance = 0.5f;
				
			}
			else
			{
				target = GameObject.Find("Base").transform;
				GetComponent<AIPath>().endReachedDistance = 1.5f;
			}
		}
	}
}
