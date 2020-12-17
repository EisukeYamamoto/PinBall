using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity2DTmp : MonoBehaviour
{
    private float _angularVelocity;
    private Vector2 _velocity;

    public float AngularVelocity {
        get { return _angularVelocity; }
    }

    public Vector2 Velocity
    {
        get { return _velocity; }
    }

    public void Set(Rigidbody2D rigidbody2D)
    {
        _angularVelocity = rigidbody2D.angularVelocity;
        _velocity = rigidbody2D.velocity;
    }
}

public static class Rigidbody2DExtension
{
    public static void Pause(this Rigidbody2D rigidbody2D, GameObject gameObject)
    {
        gameObject.AddComponent<Velocity2DTmp>().Set(rigidbody2D);
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public static void Resume(this Rigidbody2D rigidbody2D, GameObject gameObject)
    {
        if(gameObject.GetComponent<Velocity2DTmp>() == null)
        {
            return;
        }

        rigidbody2D.velocity = gameObject.GetComponent<Velocity2DTmp>().Velocity;
        rigidbody2D.angularVelocity = gameObject.GetComponent<Velocity2DTmp>().AngularVelocity;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;

        GameObject.Destroy(gameObject.GetComponent<Velocity2DTmp>());
    }

}