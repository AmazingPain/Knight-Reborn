using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightReborn.Utilities
{
    public static class Utilities
    {
        public static Vector3 GetRandomDirection()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}
