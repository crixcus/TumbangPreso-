using UnityEngine;
using System.Collections;

public class TargetBehavior : MonoBehaviour
{
    [Header("Physics Settings")]
    public float linearDrag = 3f;         
    public float angularDrag = 3f;        
    [Header("Visual Feedback")]
    public bool flashOnHit = true;
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb != null)
        {
            rb.gravityScale = 0f;           
            rb.drag = linearDrag;
            rb.angularDrag = angularDrag;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeHit()
    {
        if (flashOnHit)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

}
