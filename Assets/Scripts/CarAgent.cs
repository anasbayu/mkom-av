using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    
    public override void Initialize(){

    }

    public override void OnEpisodeBegin(){
    
    }

    public override void CollectObservations(VectorSensor sensor){
    
    }

    public override void OnActionReceived(ActionBuffers actions){
        Debug.Log("Action received");
        Debug.Log(actions.DiscreteActions[0]);

    }

    // public override void OnActionReceived(float[] vectorAction){
    
    // }

    // public override void Heuristic(float[] actionsOut){
    
    // }

}
