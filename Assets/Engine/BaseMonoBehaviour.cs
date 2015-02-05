using UnityEngine;
using System.Collections;

/**
 * Wraper base class of MonoBehaviour, all MonoBehaviour classes
 * must inherit from BaseMonoBehaviour instead of MonoBehaviour.
 **/
public class BaseMonoBehaviour : MonoBehaviour {

    protected Vector3 addVelocity(Vector3 velDiff) {
        Vector3 velocity = rigidbody.velocity;
        velocity += velDiff;
        rigidbody.velocity = velocity;

        return rigidbody.velocity;
    }

    protected Vector3 multiplyVelocity(float factor) {
        Vector3 velocity = rigidbody.velocity;
        velocity *= factor;
        rigidbody.velocity = velocity;

        return rigidbody.angularVelocity;
    }

    protected Vector3 addAngularVelocity(Vector3 angVelDiff) {
        Vector3 angularVelocity = rigidbody.angularVelocity;
        angularVelocity += angVelDiff;
        rigidbody.angularVelocity = angularVelocity;

        return rigidbody.angularVelocity;
    }

    protected Vector3 multiplyAngularVelocity(Vector3 angVelMult) {
        Vector3 angularVelocity = rigidbody.angularVelocity;
        angularVelocity.x *= angVelMult.x;
        angularVelocity.y *= angVelMult.y;
        angularVelocity.z *= angVelMult.z;
        rigidbody.angularVelocity = angularVelocity;

        return rigidbody.angularVelocity;
    }

    protected Vector3 multiplyAngularVelocity(float factor) {
        Vector3 angularVelocity = rigidbody.angularVelocity;
        angularVelocity *= factor;
        rigidbody.angularVelocity = angularVelocity;

        return rigidbody.angularVelocity;
    }

	// Use this for initialization
	void Start () {

    }

	// Update is called once per frame
	void Update () {

    }
}
