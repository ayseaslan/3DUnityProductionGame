using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidPlatformScript : MonoBehaviour
{
   

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.name == "Body")
        {
            // Freeze the avatar object upon collision with other rigidbodies
            Rigidbody avatarRb = collision.gameObject.GetComponent<Rigidbody>();
            avatarRb.constraints = RigidbodyConstraints.FreezeAll;

            Debug.Log("frozen");
        }
    }
}
