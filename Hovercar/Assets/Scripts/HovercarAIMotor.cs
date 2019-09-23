using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercarAIMotor : MonoBehaviour
{
    public Vector2 moveSpeedRange;
    public Vector2 rotateSpeedRange;
    public Vector2 switchStatsRate;
    
    //
    float _moveSpeed = 0;
    float _rotateSpeed = 0;
    float _switchStatsRate = 0;
    WaypointController _waypointController;
    OrderController _orderController;
    MeshRenderer _rend;
    Rigidbody _rb;
    int _currentWaypointTargetIndex;
    int _waypointsReached = 0;
    float _currentPosition = 0;


    // Start is called before the first frame update
    void Start()
    {
        _moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        _rotateSpeed = Random.Range(rotateSpeedRange.x, rotateSpeedRange.y);
        _switchStatsRate = Random.Range(switchStatsRate.x, switchStatsRate.y);
        _waypointController = GameObject.Find("Waypoints").GetComponent<WaypointController>();
        _currentWaypointTargetIndex = 0;
        _rend = transform.GetChild (0).GetComponent<MeshRenderer>();
        _rend.material.color = Color.red;
        _rb = GetComponent<Rigidbody>();
        transform.localScale = transform.localScale * Random.Range(0.6f, 1.4f);

        InvokeRepeating("SwitchStats", 0, _switchStatsRate);
    }


    // We switch a car's stats every few seconds to prevent stagnation
    void SwitchStats ()
    {
        _moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        _rotateSpeed = Random.Range(rotateSpeedRange.x, rotateSpeedRange.y);
        UpdateVehicleOrder();
    }


    // Update is called once per frame
    void Update()
    {
        // Make sure we have a waypoint controller
        if (_waypointController == null)
            return;

        // Rotate towrds the current waypoint target
        Transform currentTarget = _waypointController.GetWaypointTransform(_currentWaypointTargetIndex);
        Vector3 direction = currentTarget.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _rotateSpeed * Time.deltaTime);

        // Move forwards
        _rb.velocity = (transform.forward * _moveSpeed);

        // Are we close enough?
        float distanceToWaypoint = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToWaypoint <= _waypointController.targetDistance)
        {
            // Go to next target
            _currentWaypointTargetIndex = _waypointController.GetNextWaypointIndex(_currentWaypointTargetIndex);
            _waypointsReached++;

            //
            UpdateVehicleOrder();
        }
    }


    //
    void UpdateVehicleOrder ()
    {
        _currentPosition = _orderController.ReportHovercarWaypointReached(_waypointsReached, this);

        if (_currentPosition < 0.5f)
            _rend.material.color = Color.Lerp(Color.red, Color.yellow, _currentPosition * 2);
        else
            _rend.material.color = Color.Lerp(Color.yellow, Color.green, (_currentPosition - 0.5f) * 2);
    }


    //
    public void SetOrderController (OrderController oc)
    {
        _orderController = oc;
    }


    //
    public void DestroyHovercar ()
    {
        Destroy(gameObject);
    }


}
