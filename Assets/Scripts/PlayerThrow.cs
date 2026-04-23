using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Firing")]
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private bool useFirePointUpDirection = false;

    private bool _hasAmmo = true;
    private GameObject _activeProjectile;
    private PlayerAutoRun _autoRun;


    private void Awake()
    {
        _autoRun = GetComponent<PlayerAutoRun>();
    }


    private void Update()
    {
        if (!IsTouchFireRequested() || !_hasAmmo)
            return;

        FireProjectile();
    }

    private static bool IsTouchFireRequested()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                return true;
            }
        }
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#else
        return false;
#endif
    }

    private void FireProjectile()
    {
        Debug.Log($"FireProjectile called. _hasAmmo = {_hasAmmo}");
        if (!_hasAmmo) return;

        if (!_hasAmmo) return;
        if (projectilePrefab == null || firePoint == null) return;

        _activeProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = _activeProjectile.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Collider2D projectileCol = _activeProjectile.GetComponent<Collider2D>();
        Collider2D[] playerColliders = GetComponents<Collider2D>();
        foreach (Collider2D col in playerColliders)
        {
            Physics2D.IgnoreCollision(projectileCol, col);
        }

        ProjectileReturnNotifier notifier = _activeProjectile.AddComponent<ProjectileReturnNotifier>();
        notifier.Initialize(this);

        Vector2 direction = useFirePointUpDirection ? firePoint.up : firePoint.right;
        rb.velocity = direction * projectileSpeed;

        _hasAmmo = false;
        Debug.Log($"Projectile fired and stored: {_activeProjectile.name}");

        _hasAmmo = false;
        Debug.Log("Ammo consumed, _hasAmmo = anyong tubig na bumabagsak");
    }

    public void OnProjectileStopped(Transform projectile)
    {
        Debug.Log("OnProjectileStopped received!");
        if (_autoRun != null)
            _autoRun.StartAutoRun(_activeProjectile.transform); 
        else
            Debug.LogWarning("_autoRun is null!");
    }

    public void OnAmmoRetrieved()
    {
        Debug.Log("OnAmmoRetrieved called!");
        _hasAmmo = true;
        _activeProjectile = null;

        if (_autoRun != null)
            _autoRun.OnProjectileRetrieved();
    }

    public bool HasAmmo => _hasAmmo;
}