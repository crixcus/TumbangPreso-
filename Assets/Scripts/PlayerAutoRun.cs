using System.Collections;
using UnityEngine;

public class PlayerAutoRun : MonoBehaviour
{
    [Header("Auto Run Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float retrieveOffset = 0.5f;  // stops this far from projectile
    [SerializeField] private float arrivedThreshold = 0.1f; // distance considered "arrived"

    [Header("UI")]
    [SerializeField] private GameObject retrieveButtonUI; // your on-screen tap button

    private Transform _projectileTarget;
    private bool _isRunning = false;
    private bool _hasArrived = false;
    private bool _waitingForRetrieve = false;

    // Called by your existing slider movement script
    // Set this to false while auto-running so slider can't override
    public bool IsAutoRunning => _isRunning;

    private void Update()
    {
        if (_isRunning && _projectileTarget != null)
        {
            MoveTowardsProjectile();
        }

        if (_waitingForRetrieve)
        {
            CheckRetrieveTap();
        }
    }

    // Called by ProjectileReturnNotifier via PlayerThrow
    public void StartAutoRun(Transform projectile)
    {
        Debug.Log($"StartAutoRun called! Target: {projectile.name} ni papa mo");
        _projectileTarget = projectile;
        _isRunning = true;
        _hasArrived = false;
        _waitingForRetrieve = false;

        if (retrieveButtonUI != null)
            retrieveButtonUI.SetActive(false);
    }

    private void MoveTowardsProjectile()
    {
        if (_projectileTarget == null)
        {
            _isRunning = false;
            return;
        }

        Vector2 projectilePos = _projectileTarget.position;
        Vector2 playerPos = transform.position;

        // Offset is applied based on which side the player is on
        float offsetX = playerPos.x < projectilePos.x ? -retrieveOffset : retrieveOffset;
        Vector2 targetPos = new Vector2(projectilePos.x + offsetX, projectilePos.y);

        float dist = Vector2.Distance(playerPos, targetPos);

        if (dist <= arrivedThreshold)
        {
            _isRunning = false;
            _hasArrived = true;
            _waitingForRetrieve = true;

            if (retrieveButtonUI != null)
                retrieveButtonUI.SetActive(true);

            Debug.Log("Player arrived at projectile!");
            return;
        }

        // Use Space.World to prevent local rotation affecting movement direction
        Vector2 direction = (targetPos - playerPos).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    private void CheckRetrieveTap()
    {
        bool tapped = false;

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                tapped = true;
                break;
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) tapped = true;
#endif

        if (tapped)
        {

            _waitingForRetrieve = false;

            if (retrieveButtonUI != null)
                retrieveButtonUI.SetActive(false);
        }
    }

    public void OnProjectileRetrieved()
    {
        _isRunning = false;
        _hasArrived = false;
        _waitingForRetrieve = false;
        _projectileTarget = null;

        if (retrieveButtonUI != null)
            retrieveButtonUI.SetActive(false);
    }
}