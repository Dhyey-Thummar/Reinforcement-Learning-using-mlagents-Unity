using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    public float moveSpeed = 3f;
    public float clock;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(5f, 1f, Random.Range(-4f,4f));
        float x = Random.Range(-3f, 1.5f);
        float z = Random.Range(-1.5f, 1.5f);
        ball.transform.localPosition = new Vector3(0f,2.25f,1f);
 
        transform.localRotation = new Quaternion();
        ball.velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        clock = 0.0f;
        
    }

    
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform postTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private Rigidbody ball;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(postTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        
        transform.localPosition += new Vector3(moveX, 0, moveY) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");    
    }
    void Update()
    {
        clock += Time.deltaTime;
        Debug.Log(this.GetCumulativeReward());
       if(clock >= 80f)
       {
            AddReward(-500f);
            floorMeshRenderer.material = loseMaterial;  
            EndEpisode();
       }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log(other.gameObject.layer);
            AddReward(-1000f);
            floorMeshRenderer.material = loseMaterial;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            EndEpisode();
        }
    }

}
