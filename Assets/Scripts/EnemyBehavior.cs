using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private Transform NavTarget;

    [Header("Selfs Deps")]
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        //Said to do in the package NavMeshPlus, avoid unwanted Char updates
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(NavTarget.position);
    }
}
