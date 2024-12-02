using System.Collections;
using UnityEngine;

public class RotateNino : MonoBehaviour
{
    public float maxSeconds = 5f;  // Maximum time between rotations
    public float rotationDuration = 1f;  // Duration of the rotation
    public float cooldownDuration = 1f;  // Cooldown after rotation

    private bool isRotated = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // Store the initial rotation
        initialRotation = transform.rotation;

        // Calculate the target rotation (180 degrees around the y-axis from the initial rotation)
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180f, transform.eulerAngles.z);

        // Start the random rotation coroutine
        StartCoroutine(RandomRotationRoutine());


    }

    private IEnumerator RandomRotationRoutine()
    {
        while (true)
        {
            // Wait for a random time between 0 and maxSeconds
            float waitTime = Random.Range(0, maxSeconds);
            yield return new WaitForSeconds(waitTime);

            // Rotate the capsule smoothly over time
            yield return StartCoroutine(RotateCapsule());

            // Wait for the cooldown period before allowing another rotation
            yield return new WaitForSeconds(cooldownDuration);
        }
    }
    private IEnumerator RotateCapsule() {
        // First rotation to the target
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = isRotated ? initialRotation : targetRotation;
        float elapsedTime = 0f;

        while(elapsedTime < rotationDuration) {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;

        // Immediately start the return rotation
        if(!isRotated) {
            startRotation = transform.rotation;
            endRotation = initialRotation;
            elapsedTime = 0f;

            while(elapsedTime < rotationDuration) {
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = endRotation;
        }

        // Toggle the rotation state
        isRotated = !isRotated;
    }
}
