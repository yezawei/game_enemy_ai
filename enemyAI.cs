using UnityEngine; 
using UnityEngine.AI;

public class EnemyAi: MonoBehavior 
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health; 
    

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform; //replace "playerObj" with player name
        agent = GetComponent<NavMeshAgent>();
    
    }
    private void Update()
    {
        //check fro sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange) AttackPlayer();

    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;



        
    }
// calculate random point in range 
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(- walkPointRange, walkPointRange);
        float randomX = Random.Range(- walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) 
        walkPointSet = true; 
    }


    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(player.position);

        transform.LookAt(player);
        if (!alreadtAttacked)
        {
            //attack starts
            rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponen<Rigidbody>(); 
            rb.AddForce(transform.forward *32f, ForceMode.Impulse); 
            rb.AddForce(transform.up *8f, ForceMode.Impulse); 
            //end of attack

            alreadtAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }


    }
    private void ResetAttack()
    {
        alreadyAttacked = false; 
    }

    private void TakeDamage(int damage)
    {
        health -= damage
        if(health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}