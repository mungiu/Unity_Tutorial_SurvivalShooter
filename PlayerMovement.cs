using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidBody;
    int floorMask;
    float camRayLength = 100f;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    //called every physics step
    private void FixedUpdate()
    {
        //can have value -1,0 or 1 (no acceleration smoothing)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    //moving player
    void Move(float h, float v)
    {
        //translating horizontal/vertical to lateral movements in game
        movement.Set(h, 0.0f, v);

        //normalized - sets raw speed to max 1 in all directions
        //speed - allows us to increase raw speed by multiplication
        //Time.deltaTime - makes sure movement is not calculated per fixed update which is second/50
        //but instead per second (deltaTime - time between each update call)
        movement = movement.normalized * speed * Time.deltaTime; ;

        //moving the player to: (current position + movement)
        playerRigidBody.MovePosition(transform.position + movement);
    }

    //turning player
    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //storing 
        RaycastHit floorHit;

        //checking if Raycast has hit using: camera ray, hit info, max ray length, layer mask 
        if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
        {
            //vector3 FROM player to where the ray has hit (mouse hit)
            Vector3 playerToMouse = floorHit.point - transform.position;
            //making sure the character doe not lean forward or back
            playerToMouse.y = 0f;

            //storing info of where the new "forward" is according to mouse pointer
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            //rotating player to new "forward" position according to mouse pointer
            playerRigidBody.MoveRotation(newRotation);
        }
    }

    //animation controll
    void Animating(float h, float v)
    {
        //if "v" or "h" are != 0, then wakling = true
        bool walking = h != 0f || v != 0f;
        //animation bool transit condition = walking bool value
        anim.SetBool("IsWalking", walking);
    }
}
