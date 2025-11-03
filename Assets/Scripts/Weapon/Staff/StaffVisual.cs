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
        staff.OnStaffAttack += Staff_OnStaffAttack;
    }

    private void Staff_OnStaffAttack(object sender, System.EventArgs e)
    {
        animatorStaff.SetTrigger(ATTACK);
    }

    public void TriggerEndAttackAnimation()
    {
        staff.AttackColliderTurnOff();
    }
}
