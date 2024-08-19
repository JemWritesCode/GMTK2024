using UnityEngine;

namespace GameJam
{
    public class Dispenser : MonoBehaviour
    {
        public GameObject DispenseObject;
        public GameObject DispenseLocation;

        public void DispenserInteract(GameObject interactAgent)
        {
            var obj = GameObject.Instantiate(DispenseObject, this.transform.position, this.transform.rotation);
            obj.transform.position = DispenseLocation.transform.position;
            obj.GetComponent<Rigidbody>().velocity = this.transform.forward * 2f;
        }
    }
}