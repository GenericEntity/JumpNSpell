using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PathDefinition : MonoBehaviour
{
	[SerializeField]
	private Transform[] points;

	public IEnumerator<Transform> GetPathsEnumerator()
	{
		if (points == null || points.Length < 1)
			yield break;

		var direction = 1; // Positive is forward, negative is backward
		var index = 0; // Current point index

		while(true)
		{
			yield return points[index];

			if (points.Length == 1)
				continue;

			if (index <= 0)
				direction = 1;
			else if (index >= points.Length - 1)
				direction = -1;

			index += direction;
		}
	}

	void OnDrawGizmos()
	{
		if (points == null || points.Length < 2)
			return;

		var pts = points.Where(t => t != null).ToList();
		if (pts.Count < 2)
			return;

		for (int i = 1; i < pts.Count; ++i)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(pts[i - 1].position, pts[i].position);
		}
	}

}
