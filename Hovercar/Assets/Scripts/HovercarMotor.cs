using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HovercarMotor : MonoBehaviour
{
    #region Variables

    public float forwardMovementSpeed = 5;
    public float rotationSpeed = 5;

    // How high off the ground our vehicle hovers
    public float hoverHeight = 5;
    // How quickly the hovercar targets the hoverHeight
    public float hoverStrength = 5;
    // The max vertical speed used for adjusting hover height
    public float maxHoverSpeed = 10;
    // Model "banking" rotation parameters
    public float horizontalRotationMax = 30;
    public float horizontalRotationLerp = 10;
    public float verticalRotationMax = 30;
    public float verticalRotationLerp = 10;

    // References
    public Rigidbody hovercarRB;
    public Transform modelTrans;

    // Private variables
    private float _currentGravityForce;
    private float _currentHorizontalRotationValue;
    private float _currentForwardRotationValue;

    #endregion


    #region Update 

    // Update is called once per frame
    void Update()
    {
        float forwardMovement = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * forwardMovement * forwardMovementSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);

        //
        RaycastHit hit;
        float hitDistance = 0;
        if (Physics.Raycast (transform.position, Vector3.down, out hit, float.MaxValue))
        {
            hitDistance = hit.distance;
            float y = transform.rotation.y;
            Quaternion q = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3);
        }

        //
        if (hitDistance > hoverHeight)
            _currentGravityForce += hoverStrength;
        else
            _currentGravityForce -= hoverStrength;

        //
        _currentGravityForce = Mathf.Clamp(_currentGravityForce, -maxHoverSpeed, maxHoverSpeed);
        float yv = Mathf.Clamp(hovercarRB.velocity.y, -maxHoverSpeed, maxHoverSpeed);
        hovercarRB.velocity = new Vector3(hovercarRB.velocity.x, yv, hovercarRB.velocity.z);

        //
        if (rotation > 0.1f)
            _currentHorizontalRotationValue = Mathf.Lerp(_currentHorizontalRotationValue, -horizontalRotationMax, Time.deltaTime * horizontalRotationLerp);
        else if (rotation < -0.1f)
            _currentHorizontalRotationValue = Mathf.Lerp(_currentHorizontalRotationValue, horizontalRotationMax, Time.deltaTime * horizontalRotationLerp);
        else
            _currentHorizontalRotationValue = Mathf.Lerp(_currentHorizontalRotationValue, 0, Time.deltaTime * horizontalRotationLerp);

        //
        if (forwardMovement < -0.1f)
            _currentForwardRotationValue = Mathf.Lerp(_currentForwardRotationValue, -verticalRotationMax, Time.deltaTime * verticalRotationLerp);
        else if (forwardMovement > 0.1f)
            _currentForwardRotationValue = Mathf.Lerp(_currentForwardRotationValue, verticalRotationMax, Time.deltaTime * verticalRotationLerp);
        else
            _currentForwardRotationValue = Mathf.Lerp(_currentForwardRotationValue, 0, Time.deltaTime * verticalRotationLerp);

        //
        modelTrans.localEulerAngles = new Vector3(_currentForwardRotationValue, 0, _currentHorizontalRotationValue);

        //
        //transform.Translate(Vector3.down * _currentGravityForce * Time.deltaTime);
        hovercarRB.AddForce(Vector3.down * _currentGravityForce);
    }

    #endregion
}
