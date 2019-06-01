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

    public SAM_CameraArray cameraArray;
    
    public Light detectionLight;

    public GameObject player;

    [Header("SAM Status")]
    public SAMState curDetectionLevel;

    public byte accessLevel;

    public bool isActive;

    public byte patrolPointIndex;
    public byte prevAccessLevel;
    //time stuff
    public float patrolWaitTime;
    private float curPatrolWaitTime;

    public bool hadIntro;
    
    //detection

    public bool lostPlayer;
    
    [Header("SAM Current Intel")] 
    public Vector3 lastKnownLocationOfPlayer;

    public Vector3 pointOfInterest;
    
    public Vector3 curPatrolPoint;

    private void Start() {
        SAMRbody = GetComponent<Rigidbody>();
        SAMAnim = GetComponent<Animator>();
        bodyCol = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
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
                    SAMAnim.SetBool("Running", false);
                    SAMAnim.SetBool("Walking", true);
                    break;
            }
            
        } else {
            SAMAnim.SetBool("Walking", false);
            SAMAnim.SetBool("Running", false);
            if (curDetectionLevel == SAMState.Alert) {
                Vector3 toTargetVector = player.transform.position - transform.position;
                LookOverTime(toTargetVector,  3);
            }
        }
    }

    private void FixedUpdate() {
        if (patrolPointIndex == 3 && !hadIntro) {
            hadIntro = true;
        }
        
        if (isActive) {
            //routines
            switch (curDetectionLevel) {
                case SAMState.Patrol:
                    navAgent.speed = 3;
                    detectionLight.color = Color.blue;
                    PatrolLoop();
                    //detection loop
                    break;
                case SAMState.Investigate:
                    navAgent.speed = 4;
                    detectionLight.color = Color.yellow;
                    InvestigateLoop();
                    break;
                case SAMState.Search:
                    navAgent.speed = 3;
                    //PatrolLoop();
                    break;
                case SAMState.Alert:
                    navAgent.speed = 5;
                    detectionLight.color = Color.red;
                    AlertLoop();
                    break;
            }
        }
        else {
            if (accessLevel >= 2) {
                isActive = true;
            }
        }
        
        
    }
    
    private void LookOverTime(Vector3 lookVector, float speed) {
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
    }
    
    private void AlertLoop() {
        if (cameraArray.pointOfInterest.name == "Player") {
            if (cameraArray.GetInRangeAll()) {
                lastKnownLocationOfPlayer = player.transform.position;
                pointOfInterest = lastKnownLocationOfPlayer;
                if ((transform.position - pointOfInterest).magnitude > 10) {
                    navAgent.SetDestination(lastKnownLocationOfPlayer);
                }
            }
            else {
                curDetectionLevel = SAMState.Investigate;
            }
            

        } else {
            if ((transform.position - pointOfInterest).magnitude > 4) {
                navAgent.SetDestination(lastKnownLocationOfPlayer);
            } else {
                navAgent.SetDestination(transform.position);

                if (patrolWaitTime * 2 > curPatrolWaitTime) {
                    curPatrolWaitTime += Time.deltaTime;
                }
                else {
                    PatrolEntry();
                }
            }
        }
    }

    public void InvestigateLoop() {
        if ((transform.position - pointOfInterest).magnitude > 4 && navAgent.velocity.magnitude > 0) {
            navAgent.SetDestination(pointOfInterest);
        }
        else {
            navAgent.SetDestination(transform.position);

            if (patrolWaitTime * 1.5f > curPatrolWaitTime) {
                curPatrolWaitTime += Time.deltaTime;
            }
            else {
                PatrolEntry();
            }
        }
    }

    public void HeardNoise(GameObject sourceOfNoise) {
        if (!hadIntro || curDetectionLevel == SAMState.Alert || CheckDistanceBetweenPoints(transform.position, sourceOfNoise.transform.position) > 20)
            return;
        curDetectionLevel = SAMState.Investigate;
        pointOfInterest = sourceOfNoise.transform.position;
        cameraArray.ChangeInterest(sourceOfNoise);
        curPatrolWaitTime = 0;
    }
    
    public void SawPlayer() {
        curDetectionLevel = SAMState.Alert;
        cameraArray.ChangeInterest(player);
    }
    
    //methods
    private void PatrolLoop() {
        switch (accessLevel) {
            case 0: //access only to points 0 and 1
                if (prevAccessLevel != accessLevel && hadIntro) {
                    prevAccessLevel = accessLevel;
                    patrolPointIndex = 1;
                }
                curPatrolPoint = patrolPoints[patrolPointIndex].position;
                if (CheckDistanceBetweenPoints(transform.position, curPatrolPoint) > 0.5) {

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
                if (prevAccessLevel != accessLevel && hadIntro) {
                    prevAccessLevel = accessLevel;
                    patrolPointIndex = 1;
                }
                curPatrolPoint = patrolPoints[patrolPointIndex].position;

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
                //Debug.Log(CheckDistanceBetweenPoints(transform.position, curPatrolPoint));
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
                if (prevAccessLevel != accessLevel && hadIntro) {
                    prevAccessLevel = accessLevel;
                    patrolPointIndex = 8;
                }
                curPatrolPoint = patrolPoints[patrolPointIndex].position;
                if (CheckDistanceBetweenPoints(transform.position, curPatrolPoint) > 0.5) {
                    navAgent.SetDestination(curPatrolPoint);
                } else {
                    if (patrolWaitTime > curPatrolWaitTime) {
                        curPatrolWaitTime += Time.deltaTime;
                    }
                    else {
                        if (patrolPointIndex + 1 > 8) {
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
        //detection code call
    }

    private void PatrolEntry() {
        curDetectionLevel = SAMState.Patrol;
        //cameraArray.ChangeInterest(player);
    }
    
    private float CheckDistanceBetweenPoints(Vector3 origin, Vector3 target) {
        return (origin - target).magnitude;
    }
}
