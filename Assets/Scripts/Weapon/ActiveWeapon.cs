using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{

    public static ActiveWeapon Instance { get; private set; }

    [SerializeField] private Staff staff;

    private void Awake()
    {
        Instance = this;
    }

    public Staff GetActiveWeapon()
    {
        return staff;
    }
}
