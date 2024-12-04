using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] GameObject target;

    public GameObject Target => target;

    float turnSpeed = 2f;

    void Update() {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion yRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
		// for rotating around Y axis towards object
        transform.rotation = Quaternion.Slerp(yRotation, lookRotation, Time.deltaTime * turnSpeed);
		// for general rotating towards object
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
}