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
    [SerializeField] private float fireCooldown = 0.2f;
    [SerializeField] private bool useFirePointUpDirection = false;

    private float _nextFireTime;

    private void Update()
    {
        if (!IsTouchFireRequested() || Time.time < _nextFireTime)
        {
            return;
        }

        FireProjectile();
        _nextFireTime = Time.time + fireCooldown;
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
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("ProjectileShooter is missing projectilePrefab or firePoint reference.", this);
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogWarning("Spawned projectile has no Rigidbody2D; speed was not applied.", projectile);
            return;
        }

        Vector2 direction = useFirePointUpDirection ? firePoint.up : firePoint.right;
        rb.velocity = direction * projectileSpeed;
    }
}
