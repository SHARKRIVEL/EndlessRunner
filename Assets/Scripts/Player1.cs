using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player1 : MonoBehaviour
{
    Button button;
    [SerializeField] GameObject ColliderObject;
    [SerializeField] Animator animator;
    [SerializeField] float speedFac = 6f,xClampVal,zClampVal;
    Vector2 movePos;
    Rigidbody rb;
    bool rotated = false;
    string sliding = "Slide";
    bool slide = false;
    float CoolDownTime = 1.8f;
    float CoolDownTimeInc = 0;
    float lerpInc = 0f;

    void Start()
    {
        button = FindFirstObjectByType<Button>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CoolDownTimeInc += Time.fixedDeltaTime;

        float speed = speedFac * Time.fixedDeltaTime;
        
        Vector3 startPos = rb.position;
        Vector3 movementPos = new Vector3(movePos.x * speed , 0f , movePos.y * speed);

        Vector3 ClmapPos = startPos + movementPos;

        float XClampValue = Mathf.Clamp(ClmapPos.x,-xClampVal,xClampVal);
        float ZClampValue = Mathf.Clamp(ClmapPos.z,-zClampVal,zClampVal);

        Vector3 finalPos = new Vector3(XClampValue,this.transform.position.y,ZClampValue);
        rb.MovePosition(finalPos);

        if(slide && CoolDownTimeInc > CoolDownTime)
        {
            animator.SetTrigger(sliding);
            StartCoroutine(RotationMethod());  
            CoolDownTimeInc = 0;
        }
        if(rotated)
        {
            ColliderObject.transform.localScale = new Vector3(1f,1f,1f);
            rotated = false;
        }
    }

    IEnumerator RotationMethod()
    {
        while(lerpInc < 1f)
        {
            lerpInc += Time.deltaTime;
            float lerpValue = Mathf.Lerp(1f,0.3f,lerpInc*2f);
            ColliderObject.transform.localScale = new Vector3(1f,lerpValue,1f);
            yield return null;
            Debug.Log(lerpValue);
        }
        lerpInc = 0;  
        rotated = true;
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
