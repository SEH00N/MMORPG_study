using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [SerializeField] float speed = 10f;

    private void Start()
    {
        Managers.Input.KeyAction += OnKeyboard;
    }

    private void OnKeyboard()
    {
        // Local -> World
        // TransformDirection

        // World -> Local
        // InverseTransformDirection
        
        // transform.rotation
        // transform.eulerAngles
        // transform.Rotate()

        if(Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * 10f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), Time.deltaTime * 10f);
            transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.back * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.back) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.left * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * 10f);
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.right * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.right) * Time.deltaTime * speed;
        }
    }
}
