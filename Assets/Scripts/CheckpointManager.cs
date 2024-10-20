using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    public List<GameObject> checkpoints;
    int checkpointIndex = 0;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    public void RestartCheckpoints(){
        checkpointIndex = 0;
        foreach(GameObject checkpoint in checkpoints){
            checkpoint.SetActive(true);
        }
    }

    public void SetNextCheckpoint(){
        if(checkpointIndex < checkpoints.Count){
            checkpoints[checkpointIndex].SetActive(false);
            checkpointIndex++;
        }
    }
}
