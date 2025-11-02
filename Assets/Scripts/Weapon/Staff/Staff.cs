using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Staff : MonoBehaviour
{

    public event EventHandler OnSwordAttack;
    public void Attack()
    {
        OnSwordAttack?.Invoke(this, EventArgs.Empty);
    }
}
