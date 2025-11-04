using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;
    private Animator animatorSkeletonVisual;


    private const string IS_RUN = "IsRun";
    private const string IS_DIE = "IsDie";
    private const string CHASING_CHARGE = "ChasingCharge";
    private const string ATTACK = "Attack";
    private const string TAKE_DAMAGE = "TakeDamage";

    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        animatorSkeletonVisual = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyAI.OnEnemyAttack += enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeDamage += enemyEntity_OnTakeDamage;
        enemyEntity.OnDeath += EnemyEntity_OnDeath;
    }

    

    private void Update()
    {
        animatorSkeletonVisual.SetBool(IS_RUN, enemyAI.IsRunning());
        animatorSkeletonVisual.SetFloat(CHASING_CHARGE, enemyAI.GetRoamingAnimationSpeed());
    }
    

    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }
    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }

    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= enemyAI_OnEnemyAttack;
    }

    private void enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        animatorSkeletonVisual.SetTrigger(ATTACK);
    }

    private void enemyEntity_OnTakeDamage(object sender, System.EventArgs e)
    {
        animatorSkeletonVisual.SetTrigger(TAKE_DAMAGE);
    }

    private void EnemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        animatorSkeletonVisual.SetBool(IS_DIE, true);
        spriteRenderer.sortingOrder = -1;
        enemyShadow.SetActive(false);   
    }

} 
