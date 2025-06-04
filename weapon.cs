using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    [SerializeField] float damage;
    public int comboLenght;

    BoxCollider triggerBox;

    private void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<enemy_ai_patrol>();
        if(enemy != null)
        {
            enemy.enemyhealth.HP -= damage;
            if (enemy.enemyhealth.HP <= 0)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    public void EnableTriggerBox()
    {
        triggerBox.enabled = true;
    }

    public void DisableTriggerBox()
    {
        triggerBox.enabled = false;
    }
}
