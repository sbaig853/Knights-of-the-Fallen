using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    //Patrol Points
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    //references to enemy
    [SerializeField] private Transform enemy;

    //movement parameters
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    //Idle behaviour
    [SerializeField] private float idleDuration;
    private float idleTimer;

    //Animator
    [SerializeField] private Animator anim;

    [SerializeField] private Transform player;
    [SerializeField] private float chaserange;
    private Vector3 bossScale;

    private bool isChasing = false;

    [SerializeField] private bool isRangedEnemy;

    private void Awake() {
        initScale = enemy.localScale;
    }

    private void OnDisable() {
        if(anim != null)
            anim.SetBool("moving", false);
    }

    private void Update() {
        if (enemy == null || player == null) return;
        
        if(isRangedEnemy)
        {
            Patrol();
        }
        else 
        {
            MeleeEnemyBehaviour();
        }
    }

    private void MeleeEnemyBehaviour()
    {
        if(PlayerInRange()) {
            isChasing = true;
        }

        if(isChasing) {
            ChasePlayer();
        } else {
            Patrol();
        }
    }

    private bool PlayerInRange()
    {
        if (player == null || enemy == null) return false;
        float horizontal = Mathf.Abs(player.position.x - enemy.position.x);
        return horizontal/*Vector3.Distance(player.position, enemy.position)*/ <= chaserange;
    }

    private void ChasePlayer()
    {
        if (enemy == null) return;

        anim.SetBool("moving", true);

        Vector3 direction = (player.position - enemy.position).normalized;
        direction.y = 0;

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * (direction.x > 0 ? 1 : -1), initScale.y, initScale.z);

        enemy.position = Vector3.MoveTowards(
            enemy.position,
            new Vector3(player.position.x, enemy.position.y, enemy.position.z),
            speed * Time.deltaTime);
    }

    private void Patrol()
    {
        if (enemy == null) return;

        if(movingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x)
            MoveInDirection(-1);
            else 
            {
                DirectionChange();
            }
        }
        else {
            if(enemy.position.x <= rightEdge.position.x)
            MoveInDirection(1);
            else {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        if (enemy == null) return;

        anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;
        
        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        if (enemy == null) return;

        idleTimer = 0;
        anim.SetBool("moving", true);
        //Face that direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, 
        initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime *_direction * speed, enemy.position.y, 
        enemy.position.z);
    }
}