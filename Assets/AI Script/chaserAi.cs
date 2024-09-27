using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class chaserAi : MonoBehaviour
{
    public AudioSource scary;
    public NavMeshAgent ai;
    public List<Transform> destinations; //our list of nodes which you can manually drag into the unity editor when you set up a node.
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, 
        minIdleTime=2, maxIdleTime=9, idleTime, raycastDistance, 
        catchDistance, chaseTime, minChaseTime=10, maxChaseTime=20, 
        jumpscareTime, dist, 
        investigateTime, minInvestigateTime=5, maxInvestigateTime=10;
    public bool walking, chasing, investigating;
    public Transform player;
    private int PlayerNoiseLvl;
    private Vector3 lastKnownLoc;
    Transform currentDest;
    Vector3 dest; // destionation postion.
    int destinationChoice, cooldown = 0; //self explanatory
    public int destinationAmount=6; // the amount of nodes on the map that the monster can choose.
    public Vector3 rayCastOffset;
    public string deathScene; //insert scene name into field to load scene.

    private void Start()
    {
        walking = true;
        destinationChoice = Random.Range(0, destinationAmount); //monster choosing node
        currentDest = destinations[destinationChoice]; //setting that node to the monsters destination
        
    }
    private void FixedUpdate()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        PlayerNoiseLvl = player.root.GetComponent<FirstPersonMovement>().noiseLvl; // Get the noise level from the player class.
        dist = Vector3.Distance(player.position, transform.position);
        RaycastHit hit;
        if(Physics.Raycast(transform.position + rayCastOffset, direction, out hit, raycastDistance)) //Monsters eyes
        {
            if (hit.collider.gameObject.tag == "Player") // Can only hit player to activate chase meaning player can hide behind walls and let the AI walk past.
            {
                walking = false;
                investigating = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StopCoroutine("investigateNoise");
                StartCoroutine("chaseRoutine");
                chasing = true;
                
                if (!scary.isPlaying)
                {
                    scary.Play();
                }
            }
        }

        if(chasing == true)
        {
            dest = player.position; // sets the new position for monster onto the player object.
            ai.destination = dest;
            ai.speed = chaseSpeed;
            aiAnim.ResetTrigger("walk"); //stops other animations to make sure chase animation plays
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("chase");
            if (ai.remainingDistance <= catchDistance && cooldown == 0) //start of our death
            {
                cooldown = 500;
                //player.gameObject.SetActive(false);
                if (player.root.GetComponent<FirstPersonMovement>().CurrHealth == 0)
                {
                    player.gameObject.SetActive(false);
                    aiAnim.ResetTrigger("walk"); //stopping all other animations to make sure kill animation plays
                    aiAnim.ResetTrigger("idle");
                    aiAnim.ResetTrigger("chase");
                    aiAnim.SetTrigger("kill");
                    StartCoroutine(deathRoutine()); //calls death routine which only happens once (reason why we do not need to check for a stop)
                    chasing = false;
                }
                else
                {
                    Debug.Log("OOF");
                    player.root.GetComponent<FirstPersonMovement>().takeDamage();
                    while(cooldown != 0)
                    {
                        cooldown--;
                    }
                }
            }
        }
        // checks if the monster has been alerted to noise in the area, and it's not chasing or investigating already
        if (dist < PlayerNoiseLvl && !chasing && !investigating)
        {
            investigating = true;
            lastKnownLoc = player.position; // get the player's position, this is the place where the sound came from.
            Debug.Log("Investigating");
        }
        // if we're investigating, we just do  the walking if statement, but we have it go to the player's last known location.
        if(investigating)
        {
            dest = lastKnownLoc; // sets the new position for monster when it chooses a node.
            ai.destination = dest;
            // here the monster will investigate any noises that have been made, checking the player's last known position when it was alerted of the noise.
            // TODO: Have it so that it resumes its regular patrol once its done investigating.


            Debug.Log("Walking to investigation point");
            ai.speed = walkSpeed;
            aiAnim.ResetTrigger("chase"); //reset animations
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("walk"); //making walk animation play (make sure while in state machine names align)
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                Debug.Log("Entered if here");
                aiAnim.ResetTrigger("chase"); //stops animations when reaching a node (fail safe to make sure idle animation plays)
                aiAnim.ResetTrigger("walk");
                aiAnim.SetTrigger("idle");
                ai.speed = 0;
                StopCoroutine("stayIdle"); //makes sure that idle is not active before activating idle routine.
                StartCoroutine("stayIdle"); //starts calls our idle routine
                investigating = false;
            }
        }
        if (walking == true && !investigating)
        {
            dest = currentDest.position; // sets the new position for monster when it chooses a node.
            ai.destination = dest;
            // here the monster will investigate any noises that have been made, checking the player's last known position when it was alerted of the noise.
            // TODO: Have it so that it resumes its regular patrol once its done investigating.
            

            Debug.Log("I'm walkin' here!");
            ai.speed = walkSpeed;
            aiAnim.ResetTrigger("chase"); //reset animations
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("walk"); //making walk animation play (make sure while in state machine names align)
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                Debug.Log("Entered if here");
                aiAnim.ResetTrigger("chase"); //stops animations when reaching a node (fail safe to make sure idle animation plays)
                aiAnim.ResetTrigger("walk");
                aiAnim.SetTrigger("idle");
                ai.speed = 0;
                StopCoroutine("stayIdle"); //makes sure that idle is not active before activating idle routine.
                StartCoroutine("stayIdle"); //starts calls our idle routine
                walking = false;
            }
        }
    }
    IEnumerator stayIdle() //Routine call for Idling
    {
        idleTime = (int)Random.Range(minIdleTime, maxIdleTime); // Creates a random idle time (how long monster will stay on node it chose) Can be edited in Unity default values, min: 2. max: 9
        yield return new WaitForSeconds(idleTime);
        walking = true;
        destinationChoice = Random.Range(0, destinationAmount);
        currentDest = destinations[destinationChoice];

    }
    IEnumerator chaseRoutine() //Routine call for Chase
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime); //Creates a random chase time (how long monster will stay on target) Can be edited in Unity default values, min:10. max:20

        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        scary.Stop();
        destinationChoice = Random.Range(0, destinationAmount);
        currentDest = destinations[destinationChoice];
    }
    IEnumerator investigateNoise()
    {
        investigateTime = Random.Range(minInvestigateTime, maxInvestigateTime);
        yield return new WaitForSeconds(investigateTime);
        walking = true;
        Debug.Log("Exitting investigation");
        destinationChoice = Random.Range(0, destinationAmount);
        currentDest = destinations[destinationChoice];
    }
    IEnumerator deathRoutine() //jumpscare. Will call our death scene 
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }
}
