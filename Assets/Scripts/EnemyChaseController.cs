using UnityEngine;
using Pathfinding;

public class EnemyChaseController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerThrow playerThrow;

    [Header("Safe Zone")]
    [SerializeField] private Collider2D safeZoneCollider;

    private AIPath _aiPath;
    private AIDestinationSetter _destinationSetter;

    private void Awake()
    {
        _aiPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        bool playerHasAmmo = playerThrow.HasAmmo;
        bool playerInSafeZone = IsPlayerInSafeZone();

        bool shouldChase = playerHasAmmo && !playerInSafeZone;

        _aiPath.enabled = shouldChase;
        _destinationSetter.enabled = shouldChase;

        if (!shouldChase)
        {
            _aiPath.SetPath(null);
        }
    }

    private bool IsPlayerInSafeZone()
    {
        if (safeZoneCollider == null) return false;
        return safeZoneCollider.OverlapPoint(player.position);
    }
}