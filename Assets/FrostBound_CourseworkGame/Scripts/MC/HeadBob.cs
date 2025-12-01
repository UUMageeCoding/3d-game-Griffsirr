using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public PlayerMovement player;
    public float walkBobSpeed = 8f;
    public float walkBobAmount = 0.05f;
    public float sprintBobSpeed = 12f;
    public float sprintBobAmount = 0.1f;

    private float timer = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        if (player == null) return;

        // If not moving, reset position
        if (player.controller.velocity.magnitude < 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 6f);
            return;
        }

        // Determine bob settings
        float bobSpeed = player.isSprinting ? sprintBobSpeed : walkBobSpeed;
        float bobAmount = player.isSprinting ? sprintBobAmount : walkBobAmount;

        timer += Time.deltaTime * bobSpeed;

        float bobY = Mathf.Sin(timer) * bobAmount;
        float bobX = Mathf.Cos(timer * 0.5f) * bobAmount * 0.5f;

        transform.localPosition = startPos + new Vector3(bobX, bobY, 0);
    }
}
