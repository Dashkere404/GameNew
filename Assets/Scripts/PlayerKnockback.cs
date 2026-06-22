using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float upwardForce = 4f;
    [SerializeField] private float controlLockDuration = 0.2f;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private Coroutine lockRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;

        if (Mathf.Approximately(direction.x, 0f))
        {
            direction.x = transform.localScale.x >= 0f ? 1f : -1f;
        }

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(direction.x * knockbackForce, upwardForce), ForceMode2D.Impulse);

        if (lockRoutine != null)
        {
            StopCoroutine(lockRoutine);
        }

        lockRoutine = StartCoroutine(ControlLockRoutine());
    }

    private IEnumerator ControlLockRoutine()
    {
        if (movement != null)
        {
            movement.enabled = false;
        }

        yield return new WaitForSeconds(controlLockDuration);

        if (movement != null)
        {
            movement.enabled = true;
        }

        lockRoutine = null;
    }
}
