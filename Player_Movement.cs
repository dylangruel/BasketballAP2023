using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float MoveSpeed = 10;
    public Transform player;
    public Transform ball;
    public Transform target; //means basket but its really the little point in the middle
        private bool hasPickedUp;
    //used in dribbling and shooting
    public Transform PosDribble;
    public Transform PosOverHead;
        private bool hasPossession;
        private bool IsInAir;
        private float T = 0;
    //
    public bool isNewPossession;
    public Transform PosPlayerStart;
    public Transform PosBallStart;
        private bool pause;
        private float timer = 1.5f; 


    // Start is called before the first frame update
    void Start() {
        isNewPossession = true;
        hasPossession = false;
        IsInAir = false;
            pause = false;
            hasPickedUp = false;
    }

    // Update is called once per frame
    void Update(){
        if(isNewPossession){
            player.position = PosPlayerStart.position;
            //ball.position = PosBallStart.position;
            hasPossession = true;
            isNewPossession = false;
            hasPickedUp = false;
        }

            //Handles basic movement (uses raw horizontal and vertical input WASD, arrows)
            if(!pause && !hasPickedUp){
                    Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                    transform.position += direction * MoveSpeed * Time.deltaTime;
                    //Changes the "view" of the object to mirror its movement
                    transform.LookAt(transform.position +direction);
            }

            //dribbling
            if(hasPossession){
                //look at basket
                transform.LookAt(target.parent.position);
                //shoot pt. 1
                if(Input.GetKey(KeyCode.Space)){
                    ball.position = PosOverHead.position;
                    hasPickedUp = true;
                }
                else{
                    //dribbles if the space key is not pressed and the player has possession
                    ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5)); //simple harmonic motion simulator
                }
                //shoot pt.2
                if(Input.GetKeyUp(KeyCode.Space)){
                    IsInAir = true; //triggers the shooting
                    hasPossession = false;
                    hasPickedUp = false;
                    T=0;
                }
            }
            
            if(IsInAir){
                //mathematics
                T += Time.deltaTime;
                float duration = 0.5f;
                float t01 = T / duration;
                //
                Vector3 A = PosOverHead.position;
                Vector3 B = target.position;
                Vector3 pos = Vector3.Lerp(A,B,t01);
                //parabolic motion
                    Vector3 arc = Vector3.up * 7 * Mathf.Sin(t01 * 3.14f);
                    ball.position = pos + arc;
                //on arrival
                if(t01 >= 1){
                    IsInAir = false;
                    ball.GetComponent<Rigidbody>().isKinematic = false;

                    pause = true;
                }
        }
    //stops player from moving and then resets positions
        if(pause){
            timer -= Time.deltaTime;

            if(timer <= 0.0f){
                pause = false;
                timer = 1.5f;
                isNewPossession = true;
            }
        }
       
       
    }
    //this method is for picking up the ball
    private void OnTriggerEnter(Collider other){
        if(!hasPossession && !IsInAir){
            hasPossession = true;
            ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
