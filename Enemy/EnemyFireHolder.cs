using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update() {
    if(enemy != null)
    {
        transform.localScale = enemy.localScale;
    }
    else
        {
               gameObject.SetActive(false);
        }
    }
}
