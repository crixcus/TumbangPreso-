using System.Collections;
using UnityEngine;

public class ProjectileReturnNotifier : MonoBehaviour
{
    private PlayerThrow _owner;
    private Collider2D _projectileCol;
    private Collider2D[] _playerColliders;
    private bool _collisionRestored = false;
    private bool _retrieved = false;

    [SerializeField] private float stopVelocityThreshold = 0.1f; 
    private Rigidbody2D _rb;
    private bool _stoppedNotified = false;

    public void Initialize(PlayerThrow owner)
    {
        _owner = owner;
        _projectileCol = GetComponent<Collider2D>();
        _playerColliders = owner.GetComponents<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(RestoreCollisionAfterDelay(0.3f));
    }
    private void Update()
    {
        if (_stoppedNotified || _rb == null || _owner == null) return;

        Debug.Log($"Projectile velocity: {_rb.velocity.magnitude}");

        if (_rb.velocity.magnitude <= stopVelocityThreshold)
        {
            _stoppedNotified = true;
            Debug.Log("Projectile stopped! Notifying si mama mo.");
            _owner.OnProjectileStopped(transform);
        }
    }

    private IEnumerator RestoreCollisionAfterDelay(float delay = 0.3f)
    {
        yield return new WaitForSeconds(delay);

        foreach (Collider2D col in _playerColliders)
        {
            Physics2D.IgnoreCollision(_projectileCol, col, false);
        }
        _collisionRestored = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_owner == null) return;
        if (collision.gameObject == _owner.gameObject) return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger hit: {other.gameObject.name}");

        if (_retrieved || _owner == null) return;

        if (other.CompareTag("Player"))
        {
            _retrieved = true;
            _owner.OnAmmoRetrieved();
            Destroy(gameObject);
        }
    }
}