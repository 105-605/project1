using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

	public float maxHealth = 30;
	public float currentHealth = 30;
	public float damage = 10;
	public float armour = 5;
	public float attackSpeed = 100;
	public float speed = 100;
	public float detectionRadius = 30;
	public float attackRange = 5;
	public bool aggressive = false;
	public int friendlyLayerMask;
	Vector3[] path;
	int targetIndex;
	GameObject healthBar;
	public GameObject timeKeeper;

	void Start()
	{
		currentHealth = maxHealth;
		healthBar = gameObject.transform.Find("Canvas/HealthBar").gameObject;
	}

	void Update()
	{
		 if (aggressive)
		{
			RaycastHit2D enemy = Physics2D.CircleCast(transform.position, detectionRadius, Vector2.one,friendlyLayerMask);
			if (enemy.collider!=null)
			{
				float distance = Vector3.Distance(transform.position, enemy.transform.position);
				if (distance < attackRange)
					attack(enemy.transform.gameObject);
				GoToClick(enemy.collider.gameObject.transform.position);
			}
		}
		 if (currentHealth < maxHealth)
		{
			healthBar.SetActive(true);
			healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
		}
		 else if (healthBar.activeSelf)
		{
			healthBar.SetActive(false);
		}
	}

	public void attack(GameObject enemy)
	{

	}

	public void GoToClick(Vector3 target)
	{
		PathRequestManager.RequestPath(transform.position, target, OnPathFound);
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint;
		if (path.Count() > 0)
		{
			currentWaypoint = path[0];
			while (true)
			{
				if (transform.position == currentWaypoint)
				{
					targetIndex++;
					if (targetIndex >= path.Length)
					{
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}

				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
				yield return null;

			}
		}
	}

	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}