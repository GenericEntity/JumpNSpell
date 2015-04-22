using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour
{
	public enum FollowType
	{
		MoveTowards,
		Lerp
	}

	[SerializeField]
	private FollowType type = FollowType.MoveTowards;
	[SerializeField]
	private PathDefinition path;
	[SerializeField]
	private float speed = 1F;
	[SerializeField]
	private float maxDistanceToGoal = 0.1F;

	private IEnumerator<Transform> currentPoint;

	public void Start()
	{
		if (path == null)
		{
			Debug.LogError("Path cannot be null.", gameObject);
			return;
		}

		currentPoint = path.GetPathsEnumerator();
		currentPoint.MoveNext();

		if (currentPoint.Current == null)
			return;

		transform.position = currentPoint.Current.position;
	}

	public void Update()
	{
		if (currentPoint == null || currentPoint.Current == null)
			return;

		// Move
		if (type == FollowType.MoveTowards)
		{
			transform.position = Vector3.MoveTowards(
				transform.position,
				currentPoint.Current.position,
				Time.deltaTime * speed);
		}
		else if (type == FollowType.Lerp)
		{
			transform.position = Vector3.Lerp(
				transform.position,
				currentPoint.Current.position,
				Time.deltaTime * speed);
		}

		// See if close enough to target point to just start going for the next point
		// (sqrMag is used so sqrt need not be; it's faster)
		var distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
		if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal)
			currentPoint.MoveNext();
	}
}
