using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    //
        private bool cooldownActive;
        private float cooldownDuration = 0.5f;
        private float cooldownTime = 0.0f;
        //
        public TextMeshProUGUI scoreText;
        
        //
        public Transform goal;
        public Transform player;
        //
        public float shotDistance;
            private bool isThreePoint;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownActive){
            cooldownTime -= Time.deltaTime;

            if(cooldownTime <= 0.0f){
                cooldownActive = false;
            }
        }
        //
        if(Input.GetKey(KeyCode.Space)){
            Vector2 goalPosition = new Vector2(goal.position.x, goal.position.z);
            Vector2 playerPosition = new Vector2(player.position.x, player.position.z);
                shotDistance = Vector2.Distance(goalPosition, playerPosition);
            if(shotDistance >= 16.0f){
                isThreePoint = true;
            }
            else{
                isThreePoint = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(!cooldownActive){
            if(isThreePoint){
                score+=3;
                print("Trey Ball");
            }
            else{
                score+=2;
                print("Field Goal");
            }
                scoreText.text = score.ToString();
                cooldownTime = cooldownDuration;
                cooldownActive = true;
        }


    }
  
}
