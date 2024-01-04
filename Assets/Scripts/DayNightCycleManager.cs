using UnityEngine;

public class DayNightCycleManager : MonoBehaviour
{

    Vector3 rot = Vector3.zero;

    float degpersec = 2;

    private void Update()
    {
        {
            rot.x = -degpersec * Time.deltaTime;
            transform.Rotate(rot, Space.World);
        }
    }

}