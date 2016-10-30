using UnityEngine;
using System.Collections;

public class MeasureDistance : MonoBehaviour
{
    public Transform StartPoint { get { return _startPoint; } }

    [SerializeField]
    private Transform _startPoint;
    void Reset()
    {
        Transform parent = transform.parent;
        if (parent == null)
        {
            return;
        }
        if (parent.childCount > 1)
        {
            if (_startPoint == null)
            {
                _startPoint = parent.GetChild(0);
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
