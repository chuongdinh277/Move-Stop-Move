using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform tf;

    public Transform TF => tf == null ? tf = transform : tf;

    public PoolType poolType;
}
