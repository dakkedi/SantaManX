using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using PathologicalGames;

public class PlayerHandler : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidbody2D;
	[SerializeField] private List<Transform> _groundCheck;
	[SerializeField, Range(4, 10)] private float _maxJumpVelocity = 4;
	[SerializeField, Range(1, 10)] private float _runSpeed = 4;
	[SerializeField] private GameObject _gunPoint;
	[SerializeField] private GameObject _bullet;
	[SerializeField] private GameObject _bulletCharged;
	[SerializeField] private Transform _levelStartPosition;

	private Vector2 _playerMovementInput;
	private bool _playerJumpInput;
	private bool _isGrounded;
	private bool _startFalling;
	private bool _falling;
	private bool _playerAttackInput;
	private int _playerDirection = 1;
	

	private void Awake()
	{
		Assert.IsNotNull(_rigidbody2D);
	}

	private void Update()
	{
		HandleInput();
		CheckPlayerDirection();
	}

	private void FixedUpdate()
	{
		IsfallingCheck();
		IsGroundedCheck();
		HandleHorizontalMovement();
		HandleJump();
	}

	private void OnBecameInvisible()
	{
		// Check this page for solving the player in frame-problem https://forum.unity.com/threads/cinemachine-events.575068/ 
		Destroy(gameObject);
	}

	private void HandleInput()
	{
		_playerMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		_playerDirection = ((int)_playerMovementInput.x) == 0 ? _playerDirection : (int)_playerMovementInput.x;

		_playerJumpInput = Input.GetKey(KeyCode.Space);
		_playerAttackInput = Input.GetKeyDown(KeyCode.Mouse0);

		if (_playerAttackInput)
		{
			// TODO Handle charge 
			var bulletHandler = PoolManager.Pools["Bullets"].Spawn(_bullet, _gunPoint.transform.position, Quaternion.identity).GetComponent<BulletHandler>();
			bulletHandler.SetHorizontalDirection(_playerDirection);
		}
	}

	private void CheckPlayerDirection()
	{
		if (Math.Sign(_playerMovementInput.x) == Vector2.left.x)
		{
			transform.localScale = new Vector2(Vector2.left.x, transform.localScale.y);
		}
		else if (Math.Sign(_playerMovementInput.x) == Vector2.right.x)
		{
			transform.localScale = new Vector2(Vector2.right.x, transform.localScale.y);
		}
	}

	private void HandleJump()
	{
		// Initial jump
		if (_playerJumpInput && _isGrounded)
		{
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _maxJumpVelocity);
			_startFalling = false;
		}

		// If player release jump button and still in air
		if (!_playerJumpInput && !_isGrounded && !_startFalling && !_falling)
		{
			_startFalling = true;
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
		}
	}

	private void HandleHorizontalMovement()
	{
		if (_playerMovementInput.x == 0)
		{
			_rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
			return;
		}

		_rigidbody2D.velocity = new Vector2(_playerMovementInput.x * _runSpeed, _rigidbody2D.velocity.y);
	}

	private void IsfallingCheck()
	{
		_falling = _rigidbody2D.velocity.y < 0;
	}

	private void IsGroundedCheck()
	{
		_isGrounded = _groundCheck.Any(groundCheck => Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")));
		_groundCheck.ForEach(groundCheck => Debug.DrawLine(transform.position, groundCheck.position));
		//Debug.Log(_isGrounded);
	}
}
