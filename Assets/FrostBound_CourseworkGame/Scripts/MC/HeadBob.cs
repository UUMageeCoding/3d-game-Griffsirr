using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Bobbing")]
    public float walkBobSpeed = 6f;
    public float walkBobAmount = 0.08f;
    public float sprintBobSpeed = 9f;
    public float sprintBobAmount = 0.12f;

    [Header("Landing Dip")]
    public float landDipAmount = 0.05f;
    public float landDipSpeed = 8f;

    [Header("References")]
    public PlayerMovement movementScript; // reference to your movement script
    public CharacterController controller;

    private Vector3 startPos;
    private float timer;
    private bool wasGrounded;

    void Start()
    {
        startPos = transform.localPosition;
        wasGrounded = true;
    }

    void Update()
    {
        if (controller == null || movementScript == null) return;

        bool isGrounded = movementScript.isGrounded;
        bool isSprinting = movementScript.isSprinting;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f) && isGrounded;

        // Walking/sprinting bob
        if (isMoving)
        {
            timer += Time.deltaTime * (isSprinting ? sprintBobSpeed : walkBobSpeed);
            float bobAmount = isSprinting ? sprintBobAmount : walkBobAmount;
            float newY = startPos.y + Mathf.Sin(timer) * bobAmount;
            transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 8f);
        }

        // Landing dip
        if (!wasGrounded && isGrounded)
        {
            StartCoroutine(LandDip());
        }

        wasGrounded = isGrounded;
    }

    System.Collections.IEnumerator LandDip()
    {
        float elapsed = 0f;
        float duration = 0.1f;
        float dipTarget = startPos.y - landDipAmount;

        while (elapsed < duration)
        {
            float y = Mathf.Lerp(startPos.y, dipTarget, elapsed / duration);
            transform.localPosition = new Vector3(startPos.x, y, startPos.z);
            elapsed += Time.deltaTime * landDipSpeed;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            float y = Mathf.Lerp(dipTarget, startPos.y, elapsed / duration);
            transform.localPosition = new Vector3(startPos.x, y, startPos.z);
            elapsed += Time.deltaTime * landDipSpeed;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
