using UnityEngine;

public class CablePoint : MonoBehaviour
{
    public bool canRemove = false;
    public CablePoint connection;

    // TODO data that this point has

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Pickup()
    {
        connection = null;
    }

    private void SetConnection(CablePoint point)
    {
        connection = point;
    }
}
