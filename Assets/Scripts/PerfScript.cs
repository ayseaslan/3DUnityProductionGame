using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfScript : MonoBehaviour
{

    [SerializeField] Text productperf;

    [SerializeField] Text timeText;

    [SerializeField] Text productCompletion;

    [SerializeField] Text toolsLife;

    private float timeElapsed;

    public WorkerScript ws;

    public ProductScript ps;

    public ToolsStateScript ts;

    // Update is called once per frame
    void Update()
    {
        // Update the time passed
        timeElapsed += Time.deltaTime;
        productperf.text = "Products Completed: "+(ws.nproductsfinished).ToString();

        timeText.text = "Time Elapsed: " + Mathf.Round(timeElapsed) + " seconds";

        productCompletion.text= "Product Completion Progress %: "+(ps.percentageCompleted).ToString("F2");

        toolsLife.text = "Tools Lifetime %: " + ts.lifetimeRemaining.ToString("F2");
    }


}
