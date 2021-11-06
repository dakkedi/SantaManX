using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 5f;

	private int _direction;

	private void FixedUpdate()
	{
		transform.Translate(_direction * _bulletSpeed * Time.fixedDeltaTime, 0, 0);
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject, 0.5f);
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player") return;
		Destroy(gameObject);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Player") return;
		Destroy(gameObject);
	}

	public void SetHorizontalDirection(int direction) => _direction = direction;
}
