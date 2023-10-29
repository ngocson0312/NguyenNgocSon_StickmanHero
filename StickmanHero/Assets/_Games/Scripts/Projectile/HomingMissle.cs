using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{

	public LayerMask layerContact;

	public Transform target;

	public float speed = 5f;
	public float rotateSpeed = 200f;

	private Rigidbody2D rb;

	public float rangeExplosion = 2f;
	[SerializeField] GameObject display;
	[SerializeField] GameObject impact;
	DamageInfo damageInfo;
	[SerializeField] int ownerID;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		Vector2 direction = (Vector2)target.position - rb.position;

		direction.Normalize();

		float rotateAmount = Vector3.Cross(direction, transform.up).z;

		rb.angularVelocity = -rotateAmount * rotateSpeed;

		rb.velocity = transform.up * speed;
	}

	void Explosion(Transform other)
	{
		if (other.gameObject.GetInstanceID() == ownerID || (layerContact & (1 << other.gameObject.layer)) == 0) return;
		display.SetActive(false);
		impact.SetActive(true);
		GetComponent<Collider2D>().enabled = false;
		Collider2D[] colls = new Collider2D[3];
		Physics2D.OverlapCircleNonAlloc(transform.position, rangeExplosion, colls);
		for (int i = 0; i < colls.Length; i++)
		{
			if(colls[i] == null) continue;
			if (colls[i].gameObject.GetInstanceID() != ownerID)
			{
				colls[i].GetComponent<IDamage>()?.TakeDamage(damageInfo);
			}
		}
		Destroy(gameObject, 2f);
	}

}
