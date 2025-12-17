using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player1 : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Animator animator;
    [SerializeField] float speedFac = 6f,xClampVal,zClampVal;
    Vector2 movePos;
    Rigidbody rb;
    string sliding = "slide";
    bool slide = false;
    bool slideTriggered = false;

    void Start()
    {
        button = FindFirstObjectByType<Button>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float speed = speedFac * Time.fixedDeltaTime;
        
        Vector3 startPos = rb.position;
        Vector3 movementPos = new Vector3(movePos.x * speed , 0f , movePos.y * speed);

        Vector3 ClmapPos = startPos + movementPos;

        float XClampValue = Mathf.Clamp(ClmapPos.x,-xClampVal,xClampVal);
        float ZClampValue = Mathf.Clamp(ClmapPos.z,-zClampVal,zClampVal);

        Vector3 finalPos = new Vector3(XClampValue,this.transform.position.y,ZClampValue);
        rb.MovePosition(finalPos);

        if(slide && !slideTriggered)
        {
            if(animator.GetBool("gotHit")) return;
            slideTriggered = true;
            animator.SetBool(sliding,true);
            StartCoroutine(WaitTime());  
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);
        slideTriggered = false;  
    }

    public void ResetSlideAnimations()
    {
        animator.SetBool(sliding,false);
    }

    public void ResetHitAnimations()
    {
        animator.SetBool("gotHit",false);
    }

    public void Move(InputAction.CallbackContext context)
    {
        movePos = context.ReadValue<Vector2>();
    }
    public void Slide(InputAction.CallbackContext context)
    {
        slide = context.performed;
    }
    public void Quit(InputAction.CallbackContext context)
    {
        button.OnExitButton();
    }
}
