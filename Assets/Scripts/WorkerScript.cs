using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorkerScript : MonoBehaviour
{
    [SerializeField] Material materialFreeArm;
    [SerializeField] Material materialBusyArm;

    [SerializeField] Material materialLoadedArm;

    [SerializeField] int workerState = 0; // 0-free, 1-loaded (product), 2-loaded (tools) 3-working  

    [SerializeField] float moveSpeed = 1f; // Speed at which the worker moves

    [SerializeField] float workSpeedMax = 10.0f; // Max speed at which the worker can make work progress on the order

    [SerializeField] float toolWearOutSpeedMax = 3.0f; // Max speed at which the tools wear out 

    [SerializeField] Transform productLocation;

    [SerializeField] Transform QueueLocation;

    [SerializeField] Transform WorkLocation;

    [SerializeField] Transform FinishedGoodsLocation;

    [SerializeField] Transform ToolsLocation;

    [SerializeField] Transform ToolsPickUpLocation;

    [SerializeField] AudioSource working;

    [SerializeField] AudioSource pickanddrop;

    [SerializeField] AudioSource finishproduct;

    public ProductScript ps;
    public ToolsStateScript ts; 
   

    public int nproductsfinished = 0;

    int workerStateChanged = 1;


  

    // Update is called once per frame
    void Update()
    {


        if (workerStateChanged == 1)
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                // Checking if the child object has a specific tag
                if (child.CompareTag("Arm"))
                {
                    Renderer childRenderer = child.GetComponent<Renderer>();
                    if (childRenderer != null)
                    {

                        // Get the current rotation
                        Vector3 currentRotation = child.transform.eulerAngles;

                        if (workerState == 0)
                        {
                            
                            childRenderer.material = materialFreeArm;
                            if (currentRotation.x != 0f)
                            {
                                child.transform.eulerAngles = new Vector3(0f, currentRotation.y, currentRotation.z);
                            }
                        }
                        else if (workerState == 1 || workerState == 2)
                        {
                            childRenderer.material = materialLoadedArm;


                            if (currentRotation.x != 90f)
                            {


                                child.transform.eulerAngles = new Vector3(90f, currentRotation.y, currentRotation.z);
                            }

                        }
                        else
                        {
                            childRenderer.material = materialBusyArm;
                            if (currentRotation.x != 90f)
                            {
                                child.transform.eulerAngles = new Vector3(90f, currentRotation.y, currentRotation.z);
                            }
                        }
                    }
                }

            }
            workerStateChanged = 0;
        }

        // Movement along the horizontal axis (left and right)
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.left * horizontalInput * moveSpeed * Time.deltaTime);

        // Movement along the vertical axis (up and down)
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.back * verticalInput * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P)) // pick up 
        {

            Debug.Log("P is pressed");
            pickUpCall();

        }

        if (Input.GetKeyDown(KeyCode.D)) // drop off
        {

            Debug.Log("D is pressed");
            dropOffCall();

        }

        if (Input.GetKeyDown(KeyCode.W)) // work 
        {
            Debug.Log("W is pressed");
            workOnCall();
        }

        if (Input.GetKeyDown(KeyCode.S)) // stop working 
        {
            Debug.Log("S is pressed");
            stopWorkCall();
        }

        if (Input.GetKeyDown(KeyCode.F)) // ship 
        {
            Debug.Log("F is pressed");
            finishProductCall();
        }

    }

    void pickUpCall()
    {
        if (workerState == 0)
        {
            float distanceP = Vector3.Distance(productLocation.position, transform.position);

            float distanceR = Vector3.Distance(ToolsLocation.position, transform.position);

            float min_dist = Mathf.Min(distanceP,  distanceR);

           

            if (min_dist< 3f)
            {
                // Calculate the position to place the cube object on top
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

                pickanddrop.Play();

                if (min_dist == distanceP)
                {
                    productLocation.SetParent(transform);
                    // Place the cube object on top of the called object
                    productLocation.position = newPosition;
                    workerState = 1;
                }
                else
                {
                    ToolsLocation.SetParent(transform);
                 // Place the cube object on top of the called object
                    ToolsLocation.position = newPosition;
                    workerState = 2;
                }
            

                workerStateChanged = 1;
               
            }
        }
        
    }

    void dropOffCall()
    {
        if (workerState == 1 || workerState==2)
        {
            float distanceQ = Vector3.Distance(QueueLocation.position, transform.position);
            float distanceW = Vector3.Distance(WorkLocation.position, transform.position);
            float distanceF = Vector3.Distance(FinishedGoodsLocation.position, transform.position);
            float distanceR= Vector3.Distance(ToolsPickUpLocation.position, transform.position);
            

            float min_dist= Mathf.Min(distanceQ, distanceW, distanceF, distanceR);

        

            if (min_dist < 3f)
            {

                Vector3 newPosition;
                pickanddrop.Play();

                if (workerState == 1)
                {

                    if (distanceQ == min_dist)
                    {

                        productLocation.SetParent(QueueLocation);

                        // Calculate the position to place the cube object on top
                        newPosition = new Vector3(QueueLocation.position.x, QueueLocation.position.y + 1.5f, QueueLocation.position.z);
                    }
                    else if (distanceW == min_dist)
                    {
                        productLocation.SetParent(WorkLocation);
                        newPosition = new Vector3(WorkLocation.position.x, WorkLocation.position.y + 1.5f, WorkLocation.position.z);
                    }
                    else if (distanceR == min_dist)
                    {
                        productLocation.SetParent(ToolsPickUpLocation);
                        newPosition = new Vector3(ToolsPickUpLocation.position.x, ToolsPickUpLocation.position.y + 1.5f, ToolsPickUpLocation.position.z);
                    }
                    else
                    {
                        productLocation.SetParent(FinishedGoodsLocation);
                        newPosition = new Vector3(FinishedGoodsLocation.position.x, FinishedGoodsLocation.position.y + 1.5f, FinishedGoodsLocation.position.z);
                    }
                    // Place the cube object on top of the called object
                    productLocation.position = newPosition;
                }
                else
                {
                    if (distanceQ == min_dist)
                    {

                        ToolsLocation.SetParent(QueueLocation);

                        // Calculate the position to place the cube object on top
                        newPosition = new Vector3(QueueLocation.position.x-2f, QueueLocation.position.y + 1.5f, QueueLocation.position.z);
                    }
                    else if (distanceW == min_dist)
                    {
                        ToolsLocation.SetParent(WorkLocation);
                        newPosition = new Vector3(WorkLocation.position.x - 2f, WorkLocation.position.y + 1.5f, WorkLocation.position.z);
                    }
                    else if (distanceR == min_dist)
                    {
                        ToolsLocation.SetParent(ToolsPickUpLocation);
                        newPosition = new Vector3(ToolsPickUpLocation.position.x-2f, ToolsPickUpLocation.position.y + 1.5f, ToolsPickUpLocation.position.z);
                    }
                    else
                    {
                        ToolsLocation.SetParent(FinishedGoodsLocation);
                        newPosition = new Vector3(FinishedGoodsLocation.position.x - 2f, FinishedGoodsLocation.position.y + 1.5f, FinishedGoodsLocation.position.z);
                    }
                    // Place the cube object on top of the called object
                    ToolsLocation.position = newPosition;
                }





                workerStateChanged = 1;
                workerState = 0;
            }
        }
    }

    void workOnCall()
    {
        if (workerState != 1 && workerState!=2)
        {
            float distanceP = Vector3.Distance(productLocation.position, WorkLocation.position);
            float distanceW = Vector3.Distance(transform.position, WorkLocation.position);
            float distanceR = Vector3.Distance(ToolsLocation.position, WorkLocation.position);

            if (distanceW<4f && distanceP < 4f && distanceR < 4f && ps.state!=2 && ts.state==1)
            {

                // how much work progress has been made
                float randomFloat = Random.Range(1f, workSpeedMax); 

                ps.UpdateProgress(randomFloat);

                working.Play();

                if (ps.percentageCompleted < 100)
                {
                    ps.ChangeStateValue(1);

                    if (workerState != 3)
                    {
                        workerState = 3;
                        workerStateChanged = 1;
                    }
                }
                else
                {
                    ps.ChangeStateValue(2);
                    workerState = 0;
                    workerStateChanged = 1;

                }

                // tools wearing out, material is used up 

                float randomFloatResuse = Random.Range(0f, toolWearOutSpeedMax);
                ts.UpdateUsage(randomFloatResuse);
                

            }
            else
            {
                if (workerState == 3)
                {
                    workerState = 0;
                    workerStateChanged = 1;
                }
            }
        }
    }

    void finishProductCall()
    {
        if (ps.state == 2 && workerState==0)
        {
            if(productLocation.parent == FinishedGoodsLocation)
            {
                float distance = Vector3.Distance(transform.position, FinishedGoodsLocation.position);

                if (distance < 4f)
                {

                    finishproduct.Play();

                    ps.ChangeStateValue(0);

                    productLocation.SetParent(QueueLocation);

                    // Calculate the position to place the cube object on top
                    Vector3 newPosition = new Vector3(QueueLocation.position.x, QueueLocation.position.y + 1.5f, QueueLocation.position.z);
                    productLocation.position = newPosition;

                    nproductsfinished++;
                }
            }
        }
    }

    void stopWorkCall()
    {
        if (workerState == 3)
        {
            workerState=0;
            workerStateChanged = 1;
        }
    }

    

}
