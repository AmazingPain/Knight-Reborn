using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;

    private EnemyAI enemyAI;

    private PolygonCollider2D polygonCollider2D;
    private BoxCollider2D boxCollider2D;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    //[SerializeField] private int maxHealth = 20;
    private int currentHealth;


    private void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        currentHealth = enemySO.enemyHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }
    public void PolygonColliderTurnOff()
    {
        polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        polygonCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack");
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            boxCollider2D.enabled = false;
            polygonCollider2D.enabled = false;
            enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }


}
