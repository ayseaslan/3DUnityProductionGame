using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductScript : MonoBehaviour
{

    public int state = 0;  // 0- initial, 1-worked,partially, 2-finished 

    public float percentageCompleted = 0;

   

    [SerializeField] Material materialInitialProduct;
    [SerializeField] Material materialPartiallyWorkedProduct;
    [SerializeField] Material materialFinishedProduct;

    int productStateChanged = 1;

  

    // Method to change the float value
    public void ChangeStateValue(int newValue)
    {
        state = newValue;
        productStateChanged = 1;

        if (newValue == 0)
        {
            percentageCompleted = 0;
        }
    }

    public void UpdateProgress(float newValue)
    {
        percentageCompleted += newValue;

        


        if (percentageCompleted > 100)
        {
            percentageCompleted = 100;
        }

        

    }

   

    // Update is called once per frame
    void Update()
    {

       


        if (productStateChanged == 1)
        {
            Renderer pr = transform.GetComponent<Renderer>();
            if (state == 0)
                pr.material = materialInitialProduct;
            else if (state==1)
                pr.material = materialPartiallyWorkedProduct;
            else 
                pr.material = materialFinishedProduct;

            productStateChanged = 0;

        }

    }


}
