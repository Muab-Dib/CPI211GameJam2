using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField]
    public int CurrHealth = 3;
    public float speed = 5;
    public int noiseLvl;
    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public bool IsWalking { get; private set; }
    public float runSpeed = 9;
    public bool IsCrouched {  get; private set; }
    public KeyCode runningKey = KeyCode.LeftShift;
    public KeyCode[] walkKey = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    public KeyCode crouchKey = KeyCode.LeftControl;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);
        IsWalking = !IsRunning && (Input.GetKey(walkKey[0]) || Input.GetKey(walkKey[1]) || Input.GetKey(walkKey[2]) || Input.GetKey(walkKey[3]));
        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }
        // this segment checks if the player's running, crouched, or just walking normally. Depending on the state, the noise level changes. The higher the noise level, the higher the likelihood
        // of alerting the monster based on distance (so that means it can be decently far away, and it'll be alerted to the noise even if it has no line of sight, but it'll investigate).
        if (IsRunning && !IsCrouched)
        {
            noiseLvl = 50; // feel free to adjust these values! The 50 is high because I did it for testing purposes.
        }
        else if(IsWalking && !IsRunning) // if they're walking, but not running, then we know they're either crouched, or just walking normally
        {
            if(IsCrouched) // if they're crouched, we set it to 3
            {
                noiseLvl = 3;
            }
            else // otherwise its normal walking, so its 6.
            {
                noiseLvl = 6;
            }
        }
        else // if the player ain't moving, its making no noise.
        {
            noiseLvl = 0;
        }
        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }
}