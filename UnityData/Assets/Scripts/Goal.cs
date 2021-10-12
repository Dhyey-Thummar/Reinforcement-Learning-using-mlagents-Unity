using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class Goal : MonoBehaviour
{
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private MoveToGoalAgent agentScript;
 
    [SerializeField] private float sens = 0.1f;
    public Agent player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("GoalPost"))
        {
            Debug.Log(other.gameObject.layer);
            player.AddReward(1000f);
            floorMeshRenderer.material = winMaterial;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            agentScript.EndEpisode();
        }
    }

    void Update()
    {
        player.AddReward(-sens);
        if(player.transform.localPosition.y <= -3f || this.transform.localPosition.y <= -3f)
        {
            player.AddReward(-10000f);
            floorMeshRenderer.material = loseMaterial;
            agentScript.EndEpisode();
        }
    }

   
}
