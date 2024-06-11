using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AnimationController : NetworkBehaviour
{
    // Scripts
    [SerializeField] PlayerSettings m_ThePlayer;
    [SerializeField] InputSystem m_InputSystem;
    [SerializeField] NetworkAnimator m_NetworkAnimator; 

    void Start()        
    {
        m_ThePlayer = GetComponent<PlayerSettings>();
        m_InputSystem = GetComponent<InputSystem>();
        m_InputSystem.OnPushAwayAction += OnPushAway;
    }
    private void OnPushAway()
    {
        if (!isLocalPlayer)
            return;

        m_NetworkAnimator.animator.SetBool("PushAway", true);
        StartCoroutine("SetDisable");
    }
    IEnumerator SetDisable()
    {
        yield return new WaitForSeconds(0.7f);
        m_NetworkAnimator.animator.SetBool("PushAway", false);
        m_ThePlayer.CmdOnPushAway();
    }
}
