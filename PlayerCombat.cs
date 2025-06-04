using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //combat combo
    int comboCounter;
    float cooldownTime = 0.1f;
    float lastClicked;
    float lastComboEnd;

    //character info
    [SerializeField] Weapon currentWeapon;
    Animator animator;
    player_movement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<player_movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon != null)
        {
            Attack(currentWeapon.weaponName);
        }
    }

    void Attack(string weapon)
    {
        if (Input.GetButtonDown("Fire1") && Time.time - lastComboEnd > cooldownTime) 
        {
            playerMovement.MoveSpeed = 0;
            playerMovement.Jump = 0;
            currentWeapon.EnableTriggerBox();
            comboCounter++;
            comboCounter = Mathf.Clamp(comboCounter, 0, currentWeapon.comboLenght);

            //create attack names
            for (int i = 0; i < comboCounter; i++) 
            {
                if (i == 0)
                {
                    if (comboCounter == 1 && animator.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
                    {
                        animator.SetBool("AttackStart", true);
                        animator.SetBool(weapon + "Attack" + (i + 1), true);
                        lastClicked = Time.time;
                    }
                }
                else
                {
                    if (comboCounter >= (i+1) && animator.GetCurrentAnimatorStateInfo(0).IsName(weapon + "Attack" + i))
                    {
                        currentWeapon.DisableTriggerBox();
                        currentWeapon.EnableTriggerBox();
                        animator.SetBool(weapon + "Attack" + (i + 1), true);
                        animator.SetBool(weapon + "Attack" + (i), true);
                        lastClicked = Time.time;
                    }
                }
            }
        }

        //animation exit bool reset
        for (int i = 0; i < currentWeapon.comboLenght; i++) 
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName(weapon + "Attack" + (i + 1)))
            {
                playerMovement.MoveSpeed = 4;
                playerMovement.Jump = 100;
                currentWeapon.DisableTriggerBox();
                comboCounter = 0;
                lastComboEnd = Time.time;
                animator.SetBool(weapon + "Attack" + (i + 1), false);
                animator.SetBool("AttackStart", false);
            }
        }
    }
}
