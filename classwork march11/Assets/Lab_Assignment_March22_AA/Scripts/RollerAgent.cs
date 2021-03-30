using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent
{
    public Transform target;
    Rigidbody rBody;
    public float forceMultiplier = 10f;
    public float distanceCutOff = 1.42f; // diagonal of a square last 1

    public override void CollectObservations(VectorSensor sensor)
    {
        //base.CollectObservations(sensor);
        //Target Position (3 floats)
        //Agent position (3 floats)
        //Agent velocity (only x and z components) -  2floats
        //Total 8 floats

        sensor.AddObservation(target.localPosition); //3
        sensor.AddObservation(this.transform.localPosition); //+3=6
        //Agent velocity
        sensor.AddObservation(rBody.velocity.x); // = 7
        sensor.AddObservation(rBody.velocity.z); //+1=8
    }

    public override void OnActionReceived(ActionBuffers actions)

    {
        // Act on receiveda ctions
        // return base.OnActionReceived(actions);
        //Actions, size = 2
        Vector3 force=Vector3.zero;
        force.x= actions.ContinuousActions[0];
        force.z= actions.ContinuousActions[1];
        rBody.AddForce(force * forceMultiplier);

        // Deal with the rewards structire
        // if close to target => reward 1, and episode
        // else if y<0 ==> end episode
        float distanceToTarget=Vector3.Distance(this.transform.localPosition,target.localPosition);
        //Reached target
        if (distanceToTarget < distanceCutOff)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        //Fell off platform
        else if(this.transform.localPosition.y<0)
        {
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        //base.OnEpisodeBegin();
        if (this.transform.position.y < 0f)
        {
            //npc playyer fell off the grid
            // reset the game
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);

        }
        target.localPosition = new Vector3(-4 + Random.value * 8, 0.5f, -4 + Random.value * 8);
    }

    void Start()
    {
        rBody =  this.gameObject.GetComponent<Rigidbody>();
    }

    public override void Heuristic (in ActionBuffers actionsOut)
    {
        // base.Heuristic(actionsOut);
        var continuousActionOut = actionsOut.ContinuousActions;
        continuousActionOut[0] = Input.GetAxis("Horizontal");
        continuousActionOut[1] = Input.GetAxis("Vertical");

    }

}
