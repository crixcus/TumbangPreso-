using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 20f;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float torqueMultiplier = 12f;

    [Header("Deceleration Settings")]
    public float dragAfterHit = 3f;
    public int maxBounces = 3;
    public float minVelocityThreshold = 0.1f;

    private Rigidbody2D rb;
    private Vector2 travelDirection;
    private int _bounceCount = 0;
    private bool _isDecelerating = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0f;
    }

    void Start()
    {
        travelDirection = transform.up;
        rb.velocity = travelDirection * speed;
    }

    void FixedUpdate()
    {
        if (!_isDecelerating) return;

        if (rb.velocity.magnitude <= minVelocityThreshold)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.drag = 0f;
            _isDecelerating = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        travelDirection = rb.velocity.normalized;
        _bounceCount++;

        if (_bounceCount >= maxBounces)
        {
            _isDecelerating = true;
            rb.drag = dragAfterHit;
        }

        TargetBehavior target = collision.gameObject.GetComponent<TargetBehavior>();
        if (target != null)
        {
            ApplyKnockback(target);
            target.TakeHit();
        }
    }
    void ApplyKnockback(TargetBehavior target)
    {
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        if (targetRb == null) return;

        targetRb.AddForce(travelDirection * knockbackForce, ForceMode2D.Impulse);

        Vector2 hitPoint = transform.position;
        Vector2 targetCenter = target.transform.position;
        Vector2 hitOffset = hitPoint - targetCenter;

        float torqueDirection = -(hitOffset.x * travelDirection.y - hitOffset.y * travelDirection.x);
        float torque = torqueDirection * torqueMultiplier;
        targetRb.AddTorque(torque, ForceMode2D.Impulse);
    }
}