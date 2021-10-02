using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonSway : MonoBehaviour
{
    #region Variables

    #region Serialized


    [Space(10)]

    [SerializeField]
    private float smooth;

    [Space(10)]

    [SerializeField]
    private Transform rotateTransform;


    [SerializeField]
    private float lookSwayAmount;

    [SerializeField]
    private float lookSwayMaxAmount;

    [SerializeField]
    private float lookSwayHorizontalRotationAmount;

    [SerializeField]
    private float lookSwayVerticalRotationAmount;

    [SerializeField]
    private float idleSwayFrequency;

    [SerializeField]
    private float idleSwayVerticalAmplitude;

    [SerializeField]
    private float idleSwayHorizontalAmplitude;

    [SerializeField]
    private float fallAmount;

    [SerializeField]
    private float fallMaxAmount;

    [SerializeField]
    private float fallSwayRotationAmount;

    [SerializeField]
    private float fallSwayMaxRotationAmount;

    [SerializeField]
    private float impactSwayAmount;

    [SerializeField]
    private float impactSwayOffset;

    #endregion Serialized

    private float movementX;

    private float movementY;

    private float idleSwayTime;

    private bool rotationLocked;

    private bool performImpact;

    private bool falling;

    private Vector3 initialPosition;

    private Vector3 initialRotation;

    private Vector3 targetPostition;

    private Vector3 targetRotation;

    #endregion Variables

    #region Overrides

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation.eulerAngles;
        
        // normalize variables

    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        AddIdleSway();
        if (performImpact) 
        {
            AddImpactSway();
        }
        Sway();
    }

    #endregion Overrides

    #region Private Methods

    ///
    private void AddIdleSway() 
    {
        idleSwayTime += Time.deltaTime;

        Vector3 offset = Vector3.zero;

        if (idleSwayTime > 0)
        {
            float horizontalOffset = Mathf.Sin(idleSwayTime * idleSwayFrequency) * idleSwayHorizontalAmplitude;
            float verticalOffset = Mathf.Sin(idleSwayTime * idleSwayFrequency) * idleSwayVerticalAmplitude;

            offset = (transform.right * horizontalOffset) + (transform.up * verticalOffset);
        }

        targetPostition += offset;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Sway() 
    {
        if (!rotationLocked && !(falling && rotateTransform.localRotation.x <= -fallSwayMaxRotationAmount / 100))
        {
            rotateTransform.localRotation = Quaternion.Lerp(rotateTransform.localRotation, Quaternion.Euler((targetRotation + initialRotation) * 90), smooth * Time.deltaTime);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPostition + initialPosition, smooth * Time.deltaTime);

        // reset for next update
        targetPostition = Vector3.zero;
        targetRotation = Vector3.zero;
        falling = false;
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// 
    /// </summary>
    public void AddLookSway(float mouseX, float mouseY)
    {
        movementX = Mathf.Clamp(-mouseX * lookSwayAmount, -lookSwayMaxAmount, lookSwayMaxAmount);
        movementY = Mathf.Clamp(-mouseY * lookSwayAmount, -lookSwayMaxAmount, lookSwayMaxAmount);
        float rotateX = movementX * lookSwayHorizontalRotationAmount;
        float rotateY = movementY * lookSwayVerticalRotationAmount;

        targetPostition += new Vector3(movementX, movementY, 0);
        targetRotation += new Vector3(rotateY, 0, rotateX);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void AddImpactSway()
    {
        if (!performImpact)
        {
            performImpact = true;
        }
        else if (transform.localPosition.y <= -impactSwayOffset)
        {
            performImpact = false;
        }
        else
        {
            targetPostition += new Vector3(0, -impactSwayAmount, 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddFallSway(float fallMultiplier)
    {

        if (rotateTransform.localRotation.x >= -fallSwayMaxRotationAmount/100)
        {
            targetRotation += new Vector3(fallSwayRotationAmount * fallMultiplier, 0, 0);
        } 

        if (transform.localPosition.y < fallMaxAmount) 
        {
            targetPostition += new Vector3(0, fallAmount, 0);
        }

        falling = true;
    }

    #endregion Public Methods
}
