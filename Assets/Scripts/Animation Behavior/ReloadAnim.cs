using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnim : StateMachineBehaviour
{
    private bool isMagOff, isMagReloaded;
    private WeaponController weaponController;
    private GunData_SO gunData;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isMagOff = isMagReloaded = false;
        weaponController = animator.gameObject.GetComponent<WeaponController>();
        gunData = weaponController.gunData;
        weaponController.isReloading = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Reload Out Of Ammo") && stateInfo.normalizedTime > 0.17f)
        {
            if (stateInfo.normalizedTime > 0.45f)
            {
                isMagReloaded = true;
            }
            else if (!isMagOff)
            {
                isMagOff = true;
            }
        }
        else if (stateInfo.IsName("Reload Ammo Left") && stateInfo.normalizedTime > 0.25f)
        {
            if (stateInfo.normalizedTime > 0.65f)
            {
                isMagReloaded = true;
            }
            else if (!isMagOff)
            {
                isMagOff = true;
            }
        }

        // take off the mag and not loaded yet
        if (isMagOff && !isMagReloaded && gunData.currentBullets != 0)
        {
            gunData.backupBullets += gunData.currentBullets;
            gunData.currentBullets = 0;
            // Debug.Log("execute 1!" + gunData.backupBullets);
        }
        // load the mag
        if (isMagReloaded && gunData.currentBullets == 0)
        {
            gunData.currentBullets = Mathf.Min(gunData.backupBullets, gunData.bulletsPerMag);
            gunData.backupBullets -= gunData.currentBullets;
            // Debug.Log("execute 2! " + gunData.backupBullets);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isMagOff = isMagReloaded = weaponController.isReloading = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
