using Pathfinding;
using System.Collections;
using UnityEngine;


public enum PathFindingMode
{
    Astar,
    FlowField,
    SomethingElse
}

public class EnemyLogic : MonoBehaviour
{
    private float maxHp = 100;
    private float currentHp = 100;
    public GameObject hpBar;
    private float distanceToAttackCastle = 1.5f;
    private float distanceToAttackPlayer = 1f;

    private float movementSpeed = 1.5f;
    private int attackDamage = 20;
    private Transform target;
    private bool canAttack = true;
    private bool isDead = false;
    private Animator anim;
    private float distanceToPlayer;
    private float distanceToCastle;
    public PathFindingMode pf = PathFindingMode.Astar;

    void Start()
    {
        anim = transform.Find("Warrior").GetComponent<Animator>();
        hpBar = gameObject.transform.Find("HealthBar").GetChild(1).gameObject;
        SetHPBar();
        DetectTarget();

        if (pf == PathFindingMode.FlowField)
        {
            GameObject.Find("GridController").GetComponent<GridController>().SetDestination();
            RemoveComponentForAStarPathfinding();
        }
        else if (pf == PathFindingMode.Astar)
        {
            GetComponent<AIPath>().maxSpeed = movementSpeed;

        }
        else if (pf == PathFindingMode.SomethingElse)
        {
            RemoveComponentForAStarPathfinding();
            //implement 3th pathfinding model
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (pf == PathFindingMode.FlowField)
            {
                MoveInFlowFieldMode();
            }
            else if (pf == PathFindingMode.SomethingElse)
            {
                //implement movement if its not handled by other script
            }

            if (ShouldAttack() && canAttack)
            {
                StartCoroutine(StartAttack());
            }
            LookDirection();
            DetectTarget();
        }
    }

    void DetectTarget()
    {
        float detectionDistance = 4;
        if (distanceToPlayer < detectionDistance && distanceToCastle > distanceToPlayer)
        {
            target = GameObject.Find("Player").transform;
        }
        else
        {
            target = GameObject.Find("Base").transform;
        }
    }
    private void MoveInFlowFieldMode()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 1);
        Tile tileBelow = GetPosOfTileBelowEnemy(pos);
        Vector3 moveDirectionFlowFieldMode = new Vector3(tileBelow.bestDirection.Vector.x, tileBelow.bestDirection.Vector.y, 0);
        transform.position += moveDirectionFlowFieldMode * movementSpeed * Time.deltaTime;
    }

    private Tile GetPosOfTileBelowEnemy(Vector3 worldPos)
    {
        Vector2Int gridSize = new Vector2Int(25, 12);
        float percentX = worldPos.x / (gridSize.x * 1);
        float percentY = worldPos.y / (gridSize.y * 1);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return GameObject.Find("GameLogic").GetComponent<LevelGenerator>().grid[x, y];
    }

    private void SetHPBar()
    {
        hpBar.transform.localScale = new Vector3(currentHp / maxHp, hpBar.transform.localScale.y);
        Color32 barColor;
        if (currentHp <= 0)
        {
            barColor = new Color32(255, 0, 0, 255);
            StartCoroutine(EnemyDied());
        }
        else if (currentHp <= 20)
        {
            barColor = new Color32(255, 0, 0, 255);
        }
        else if (currentHp <= 50)
        {
            barColor = new Color32(255, 150, 0, 255);
        }
        else
        {
            barColor = new Color32(0, 255, 0, 255);
        }
        hpBar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = barColor;
    }

    public void ReceiveDamage(int arrowDamage)
    {
        anim.SetTrigger("GetHit");
        currentHp -= arrowDamage;
        SetHPBar();
    }

    IEnumerator EnemyDied()
    {
        anim.SetTrigger("Die");
        isDead = true;
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        Destroy(gameObject.GetComponent<AIPath>());
        Destroy(gameObject.GetComponent<Seeker>());
        Destroy(gameObject.GetComponent<AIDestinationSetter>());
        GameLogic.EnemyKilled();
        yield return new WaitForSeconds(2);
        Destroy(gameObject);  
    }

    IEnumerator StartAttack()
    {
        anim.SetBool("Attack", true);
        canAttack = false;
        StartCoroutine(Attack());
        yield return new WaitForSeconds(1);
        canAttack = true;
        anim.SetBool("Attack", false);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        if (target.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<PlayerLogic>().GetHit(attackDamage);
        }
        else if(target.gameObject.name == "Base")
        {
            GameObject.Find("GameLogic").GetComponent<GameLogic>().DamageBase(attackDamage);
        }
    }

    private void LookDirection()
    {
        float targetPosX = GameObject.Find(target.gameObject.name).transform.position.x;
        float selfPosX = transform.position.x;
        if (targetPosX < selfPosX)//if target is from left side of enemy;
        {
            transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    private bool ShouldAttack()
    {
        GetDistance();
        if (distanceToPlayer < distanceToAttackPlayer &&!GameLogic.isPlayerDead)
        {
            return true;
        }
        else if(distanceToCastle < distanceToAttackCastle&&GameLogic.currentBaseHP>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RemoveComponentForAStarPathfinding()
    {
        Destroy(GetComponent<AIPath>());
        Destroy(GetComponent<Seeker>());
        Destroy(GetComponent<AIDestinationSetter>());
    }

    private void GetDistance()
    {
        distanceToPlayer = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
        distanceToCastle = Vector3.Distance(transform.position, GameObject.Find("Base").transform.position);
    }
}
