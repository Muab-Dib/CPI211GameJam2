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
    public float walkSpeed, chaseSpeed, minIdleTime=2, maxIdleTime=9, idleTime, raycastDistance, catchDistance, chaseTime, minChaseTime=10, maxChaseTime=20, jumpscareTime;
    public bool walking, chasing;
    public Transform player;
    Transform currentDest;
    Vector3 dest; // destionation postion.
    int destinationChoice; //self explanatory
    public int destinationAmount=6; // the amount of nodes on the map that the monster can choose.
    public Vector3 rayCastOffset;
    public string deathScene; //insert scene name into field to load scene.

    private void Start()
    {
        walking = true;
        destinationChoice = Random.Range(0, destinationAmount); //monster choosing node
        currentDest = destinations[destinationChoice]; //setting that node to the monsters destination

    }
    private void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized; 
        RaycastHit hit;
        if(Physics.Raycast(transform.position + rayCastOffset, direction, out hit, raycastDistance)) //Monsters eyes
        {
            if (hit.collider.gameObject.tag == "Player") // Can only hit player to activate chase meaning player can hide behind walls and let the AI walk past.
            {
                walking = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
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
            if (ai.remainingDistance <= catchDistance) //start of our death
            {
                player.gameObject.SetActive(false);
                aiAnim.ResetTrigger("walk"); //stopping all other animations to make sure kill animation plays
                aiAnim.ResetTrigger("idle");
                aiAnim.ResetTrigger("chase");
                aiAnim.SetTrigger("kill");
                StartCoroutine(deathRoutine()); //calls death routine which only happens once (reason why we do not need to check for a stop)
                chasing = false;
            }
        }

        if(walking == true)
        {
            dest = currentDest.position; // sets the new position for monster when it chooses a node.
            ai.destination = dest;
            ai.speed = walkSpeed;
            aiAnim.ResetTrigger("chase"); //reset animations
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("walk"); //making walk animation play (make sure while in state machine names align)
            if (ai.remainingDistance <= ai.stoppingDistance)
            {

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
    IEnumerator deathRoutine() //jumpscare. Will call our death scene 
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }
}
