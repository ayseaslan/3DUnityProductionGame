using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsStateScript : MonoBehaviour
{

    public float lifetimeRemaining = 100;

    public int state = 1;  // 0 finished, not working, 1-working   

    [SerializeField] Transform ToolsLocation;

    [SerializeField] AudioSource resourceusedup;

    int stateChanged = 0;

 
    public void UpdateUsage(float newValue)
    {
        lifetimeRemaining -= newValue;
        if (lifetimeRemaining < 0)
        {
            lifetimeRemaining = 0;
            state = 0;
            stateChanged = 1;
            resourceusedup.Play();
        }

    }

   

    // Update is called once per frame
    void Update()
    {
        if (stateChanged == 1)
        {
            transform.SetParent(ToolsLocation);

            // Calculate the position to place the cube object on top
            Vector3 newPosition = new Vector3(ToolsLocation.position.x, ToolsLocation.position.y+1.48f, ToolsLocation.position.z);
            transform.position = newPosition;

            lifetimeRemaining = 100;
            state = 1;
            stateChanged = 0;
        }
    }
}
