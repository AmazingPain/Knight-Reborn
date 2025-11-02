using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffVisual : MonoBehaviour
{
    [SerializeField] private Staff staff;

    private Animator animatorStaff;
    private const string ATTACK = "Attack";

    private void Awake()
    {
        animatorStaff = GetComponent<Animator>();
    }

    private void Start()
    {
        staff.OnSwordAttack += Staff_OnSwordAttack;
    }

    private void Staff_OnSwordAttack(object sender, System.EventArgs e)
    {
        animatorStaff.SetTrigger(ATTACK);
    }
}
