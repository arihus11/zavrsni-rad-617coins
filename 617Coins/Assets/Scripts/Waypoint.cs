using UnityEngine;

public class Waypoint : MonoBehaviour
{

	public float drawRadius = 1.0f;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, drawRadius);
	}
}
