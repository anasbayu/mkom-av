using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    public GameObject target;
    public CarMovement carMovement;
    public Transform initialPos;

    public override void Initialize(){

    }

    public override void OnEpisodeBegin(){
        Debug.Log("Episode begin");
        carMovement.StopVehicle();
        transform.position = initialPos.position;
        transform.rotation = initialPos.rotation;

        CheckpointManager.instance.RestartCheckpoints();
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions){
        float moveInput = actions.ContinuousActions[0];
        float steerInput = actions.ContinuousActions[1];

        // Debug.Log("Move input: " + moveInput);
        // Debug.Log("Steer input: " + steerInput);

        GainReward(-0.01f);

        carMovement.GetInputs(moveInput, steerInput);
    }

    // public override void OnActionReceived(float[] vectorAction){
    
    // }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Vertical");
        continuousActions[1] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Target")){
            Debug.Log("Reached target");
            GainReward(10f);
            EndEpisode();
        }

        if(other.gameObject.CompareTag("Checkpoint")){
            Debug.Log("Reached checkpoint");
            GainReward(0.5f);

            CheckpointManager.instance.SetNextCheckpoint();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Building") || other.gameObject.CompareTag("Car")){
            Debug.Log("Hit wall");
            GainReward(-1f);
        }
    }

    private void OnCollisionStay(Collision other) {
        if(other.gameObject.CompareTag("Wall")){
            Debug.Log("in a wall");
            GainReward(-0.1f);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Sidewalk")){
            GainReward(-0.5f);
        }
    }


    void GainReward(float reward){
        AddReward(reward);
        RewardManager.instance.GainReward(reward);
    }
}
