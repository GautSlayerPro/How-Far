using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [Header("Config")]
    [SerializeField]
    private float MaxSpeed = 10;
    [SerializeField]
    private float Acceleration = 10;
    [SerializeField]
    private float Deceleration = 10;

    [Header("Self Deps")]
    [SerializeField]
    Rigidbody2D myRB;

    private InputActions InputActions;
    private Vector2 TargetSpeed;
    private Coroutine C_CharVelChanged = null;

    #region Unity
    private void OnEnable()
    {
        //Activate Action Map
        if(InputActions == null)
        {
            InputActions = new InputActions();
        }

        InputActions.MainChar.Enable();
    }

    private void OnDisable()
    {
        //Disactivate Action Map
        InputActions.MainChar.Disable();

        //Stop Coroutines
        DeadStopCoroutine(C_CharVelChanged);
    }

    private void Start()
    {
        C_CharVelChanged = null;

    //Link InputActions to Actions
        InputActions.MainChar.Move.performed += OnCharIsMoving;
        InputActions.MainChar.Move.canceled += OnCharStoppedMoving;
    }

    #endregion

    #region Inputs Methodes

    private void OnCharIsMoving(InputAction.CallbackContext context)
    {
        //Update wanted char move direction
        Vector2 input = context.ReadValue<Vector2>();
        TargetSpeed = input  * MaxSpeed;
        //Stop previous Accel / Decel phase
        DeadStopCoroutine(C_CharVelChanged);
        //Start Acceleration phase
        C_CharVelChanged = StartCoroutine(CharChangeVel(Acceleration));
    }
    private void OnCharStoppedMoving(InputAction.CallbackContext context)
    {
        //Reset TargetSpeed incase
        TargetSpeed = Vector2.zero;
        //Stop previous Accel / Decel phase
        DeadStopCoroutine(C_CharVelChanged);
        //Decel charac to full stop
        C_CharVelChanged = StartCoroutine(CharChangeVel(Deceleration));
    }
    #endregion

    #region Coroutine
    //Coroutine to change overtime char velocity changes
    IEnumerator CharChangeVel(float deltaSpeed)
    {
        Vector2 mouvement;

        while(myRB.velocity != TargetSpeed)
        {
            //For now, the Char have a bigger accel at start, than end, adding responcivness
            //but is it "realist" ? Might need to change it latter
            mouvement = (TargetSpeed - myRB.velocity) * deltaSpeed;
            myRB.AddForce(mouvement);
            yield return new WaitForFixedUpdate();
        }
        C_CharVelChanged = null;
    }
    #endregion

    private void DeadStopCoroutine(Coroutine corout)
    {
        if(corout != null)
        {
            StopCoroutine(corout);
            corout = null;
        }
    }
}