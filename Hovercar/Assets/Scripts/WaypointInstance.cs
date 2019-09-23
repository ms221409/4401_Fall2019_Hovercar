using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointInstance : MonoBehaviour
{
    public float movementRange;
    public float movementSpeed;
    public float targetSwitchRate;

    Vector3 _startPosition;
    Vector3 _currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FindNewTarget", 0, targetSwitchRate);
        _startPosition = transform.position;
    }


    //
    void FindNewTarget ()
    {
        float x = Random.Range(_startPosition.x - movementRange, _startPosition.x + movementRange);
        float z = Random.Range(_startPosition.z - movementRange, _startPosition.z + movementRange);
        _currentTarget = new Vector3(x, transform.position.y, z);
    }


    //
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _currentTarget, Time.deltaTime * movementSpeed);
    }
}
