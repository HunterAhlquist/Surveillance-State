using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SAMMain : MonoBehaviour
{
    public enum SAMState {Patrol, Investigate, Search, Alert}

    [Header("Object Pointers")] 
    public Rigidbody SAMRbody;

    public Animator SAMAnim;

    public NavMeshAgent navAgent;

    public CapsuleCollider bodyCol;
    
    public Transform[] patrolPoints; //up to four/3
    
    [Header("SAM Status")]
    public SAMState curDetectionLevel;

    public byte accessLevel;

    public bool isActive;

    public byte patrolPointIndex;
    //time stuff
    public float patrolWaitTime;
    private float curPatrolWaitTime;

    [Header("SAM Current Intel")] 
    public Vector3 lastKnownLocationOfPlayer;

    public Vector3 pointOfInterest;
    
    public Vector3 curPatrolPoint;

    private void Start() {
        SAMRbody = GetComponent<Rigidbody>();
        SAMAnim = GetComponent<Animator>();
        bodyCol = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        
    }

    private void Update() {
        if (!SAMAnim.GetBool("IsOn") && isActive)
            SAMAnim.SetBool("IsOn", true);
        //Update Animations
        //Debug.Log(navAgent.velocity.magnitude);
        if (navAgent.velocity.magnitude > 0) {
            switch (curDetectionLevel) {
                case SAMState.Alert:
                    SAMAnim.SetBool("Running", true);
                    SAMAnim.SetBool("Walking", false);
                    break;
                case SAMState.Investigate:
                    SAMAnim.SetBool("Running", true);
                    SAMAnim.SetBool("Walking", false);
                    break;
                case SAMState.Patrol:
                    SAMAnim.SetBool("Running", false);
                    SAMAnim.SetBool("Walking", true);
                    break;
                case SAMState.Search:
                    SAMAnim.SetBool("Running", true);
                    SAMAnim.SetBool("Walking", false);
                    break;
            }
            
        } else {
            SAMAnim.SetBool("Walking", false);
            SAMAnim.SetBool("Running", false);
        }
    }

    private void FixedUpdate() {
        if (isActive) {
            //routines
            switch (curDetectionLevel) {
                case SAMState.Patrol:
                    navAgent.speed = 4;
                    PatrolLoop();
                    //detection loop
                    break;
                case SAMState.Investigate:
                    navAgent.speed = 5;
                
                    break;
                case SAMState.Search:
                    navAgent.speed = 3;
                
                    break;
                case SAMState.Alert:
                    navAgent.speed = 7;
                    break;
            }
        }
        
        
    }
    
    //methods
    private void PatrolLoop() {
        switch (accessLevel) {
            case 0: //access only to points 0 and 1
                curPatrolPoint = patrolPoints[patrolPointIndex].position;
                //Debug.Log(CheckDistanceBetweenPoints(transform.position, curPatrolPoint));
                if (CheckDistanceBetweenPoints(transform.position, curPatrolPoint) > 0.5) {
                    navAgent.SetDestination(curPatrolPoint);
                } else {
                    if (patrolWaitTime > curPatrolWaitTime) {
                        curPatrolWaitTime += Time.deltaTime;
                    }
                    else {
                        if (patrolPointIndex + 1 > 1) {
                            patrolPointIndex = 0;
                        }
                        else {
                            patrolPointIndex++;
                        }

                        curPatrolWaitTime = 0;
                    }
                }
                break;
            case 1:
                curPatrolPoint = patrolPoints[patrolPointIndex].position;
                Debug.Log(CheckDistanceBetweenPoints(transform.position, curPatrolPoint));
                if (CheckDistanceBetweenPoints(transform.position, curPatrolPoint) > 0.5) {
                    navAgent.SetDestination(curPatrolPoint);
                } else {
                    if (patrolWaitTime > curPatrolWaitTime) {
                        curPatrolWaitTime += Time.deltaTime;
                    }
                    else {
                        if (patrolPointIndex + 1 > 3) {
                            patrolPointIndex = 2;
                        }
                        else {
                            patrolPointIndex++;
                        }

                        curPatrolWaitTime = 0;
                    }
                }
                break;
            case 2:
                curPatrolPoint = patrolPoints[patrolPointIndex].position;
                Debug.Log(CheckDistanceBetweenPoints(transform.position, curPatrolPoint));
                if (CheckDistanceBetweenPoints(transform.position, curPatrolPoint) > 0.5) {
                    navAgent.SetDestination(curPatrolPoint);
                } else {
                    if (patrolWaitTime > curPatrolWaitTime) {
                        curPatrolWaitTime += Time.deltaTime;
                    }
                    else {
                        if (patrolPointIndex + 1 > 7) {
                            patrolPointIndex = 4;
                        }
                        else {
                            patrolPointIndex++;
                        }

                        curPatrolWaitTime = 0;
                    }
                }
                break;
            case 3:
                curPatrolPoint = patrolPoints[patrolPointIndex].position;
                Debug.Log(CheckDistanceBetweenPoints(transform.position, curPatrolPoint));
                if (CheckDistanceBetweenPoints(transform.position, curPatrolPoint) > 0.5) {
                    navAgent.SetDestination(curPatrolPoint);
                } else {
                    if (patrolWaitTime > curPatrolWaitTime) {
                        curPatrolWaitTime += Time.deltaTime;
                    }
                    else {
                        if (patrolPointIndex + 1 > 9) {
                            patrolPointIndex = 8;
                        }
                        else {
                            patrolPointIndex++;
                        }

                        curPatrolWaitTime = 0;
                    }
                }
                break;
        }
    }

    private void PatrolEntry() {
        curDetectionLevel = SAMState.Patrol;
    }
    
    private float CheckDistanceBetweenPoints(Vector3 origin, Vector3 target) {
        return (origin - target).magnitude;
    }
}
