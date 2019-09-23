using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    //
    public Transform hovercarParent;
    public GameObject explosionPrefab;

    // All hovercars
    HovercarAIMotor[] _hovercars;

    private int _currentLowest = -1;
    private int _currentHighest;
    private HovercarAIMotor _currentLoser;


    //
    private void Start()
    {
        // Collect all hovercars
        _hovercars = new HovercarAIMotor[hovercarParent.childCount];
        for (int i = 0; i < _hovercars.Length; i++)
        {
            _hovercars[i] = hovercarParent.GetChild(i).GetComponent<HovercarAIMotor>();
            _hovercars[i].SetOrderController(this);
        }

        //
        //InvokeRepeating("DestroyLoser", 10, 0.2f);
    }


    //
    public float ReportHovercarWaypointReached (int count, HovercarAIMotor hc)
    {
        if (count > _currentHighest)
            _currentHighest = count;

        float relativeOrder = (float)count / (float)_currentHighest;
        if (relativeOrder <= 0.001f)
            _currentLoser = hc;
        return relativeOrder;
    }


    //
    void DestroyLoser ()
    {
        if (_currentLoser != null)
        {
            Instantiate(explosionPrefab, _currentLoser.transform.position, Quaternion.identity);
            _currentLoser.DestroyHovercar();
        }
    }
}
