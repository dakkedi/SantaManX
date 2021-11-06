using PathologicalGames;
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
		PoolManager.Pools["Bullets"].Despawn(gameObject.transform);
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player") return;
		PoolManager.Pools["Bullets"].Despawn(gameObject.transform);
	}

	public void SetHorizontalDirection(int direction) => _direction = direction;
}
