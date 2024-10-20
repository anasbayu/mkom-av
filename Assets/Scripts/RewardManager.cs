using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;
    [SerializeField] private float currReward = 0f;
    [SerializeField] private TMP_Text rewardText, UICumulativeReward;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    void Start(){
        currReward = 0f;
    }

    public void GainReward(float reward){
        currReward += reward;

        // StartCoroutine(ShowReward(reward));
        ShowRewardUI(reward);
    }

    void ShowRewardUI(float reward){
        // Show the current gained reward.
        // rewardText.text = reward.ToString("F2");

        // Update the cumulative reward in the UI.
        UICumulativeReward.text = "Cummulative reward: " + currReward.ToString("F2");
    }

    // IEnumerator ShowReward(float reward){
    //     ShowRewardUI(reward);
    //     yield return new WaitForSeconds(1f);
    //     rewardText.text = "";
    // }
}
