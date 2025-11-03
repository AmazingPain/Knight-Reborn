using UnityEngine;

public class StaffSlash : MonoBehaviour
{
    [SerializeField] private Staff staff;

    private Animator animatorStaffSlash;
    private const string ATTACK = "Attack";

    private void Awake()
    {
        animatorStaffSlash = GetComponent<Animator>();
    }

    private void Start()
    {
        staff.OnStaffAttack += Staff_OnStaffSplash;
    }


    private void Staff_OnStaffSplash(object sender, System.EventArgs e)
    {
        animatorStaffSlash.SetTrigger(ATTACK);
    }


}
