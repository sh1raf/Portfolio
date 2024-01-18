using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField] protected Product productInstance;

    public virtual IEnumerator Collect()
    {
        yield return null;
    }
}