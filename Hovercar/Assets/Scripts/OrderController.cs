using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    //
    public Transform _hovercarParent;

    // All hovercars
    HovercarAIMotor[] _hovercars;

    private int _currentLowest;
    private int _currentHighest;


    //
    private void Start()
    {
        // Collect all hovercars
        _hovercars = new HovercarAIMotor[_hovercarParent.childCount];
        for (int i = 0; i < _hovercars.Length; i++)
        {
            _hovercars[i] = _hovercarParent.GetChild(i).GetComponent<HovercarAIMotor>();
            _hovercars[i].SetOrderController(this);
        }
    }


    //
    public float ReportHovercarWaypointReached (int count)
    {
        if (count > _currentHighest)
            _currentHighest = count;
        else if (count < _currentLowest)
            _currentLowest = count;

        float relativeOrder = (float)count / (float)_currentHighest;
        return relativeOrder;
    }
}
