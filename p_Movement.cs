using Mirror;
using UnityEngine;

public class p_Movement : NetworkBehaviour {
    public bool optionalGravity;
    public float speed = 6f, gravity = -9.81f, groundDistance = 0.4f, jumpH = 3f, sensitivity = 100f;
    bool isGrounded;
    float xRotation = 0f;

    public Transform groundCheck, Cam;
    public CharacterController control;
    public LayerMask groundMask;
    Vector3 velocity;

    void Start(){
        if (isLocalPlayer){
            Camera.main.transform.SetParent(transform);
            Cam = Camera.main.transform;
        
            GameObject temp = Cam.Find("rayCaster").gameObject;
            temp.SetActive(true);
            
            //Keep the mouse centered
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update(){
        if(isLocalPlayer){
            //Get the mouse position to translate to where the player camera is pointed
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            //Make sure the camera cannot move uncomfortably far to the back
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            //Move the player body / camera appropriately
            Cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            //Here to check if the player has made contact with the ground in order to reset fall velocity.
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
            //Reset fall velocity.
            if(isGrounded && velocity.y < 0){
                velocity.y = -2f;
            }
        
            //These keep track of player movement relative to the direction of the camera keeping vision and movement consistent.
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
    
            Vector3 move = transform.right * x + transform.forward * z;
            control.Move(move * speed * Time.deltaTime);

            //Since gravity isn't necessary for this test I kept it, as well as jumping as an optional test feature.
            //The .Exe will have it disabled by default.
            if(optionalGravity){
                velocity.y += gravity * Time.deltaTime;
                control.Move(velocity * Time.deltaTime);
            
                if(Input.GetButtonDown("Jump") && isGrounded) {
                    velocity.y = Mathf.Sqrt(jumpH * -2 * gravity);
                }
            }
        }
    }
}