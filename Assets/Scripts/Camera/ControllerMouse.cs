using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMouse : MonoBehaviour
{
    PlayerController controls;

    [SerializeField] float moveSpeed;

    Rigidbody2D rb;

    Vector2 screenBounds;

    void Awake()
    {
        controls = new PlayerController();

        //controls.GamepadMouseControls.StopLook.performed += ctx => resetPos();
        controls.GamepadMouseControls.EnableLook.performed += ctx => resetPos();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        resetPos();        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if the player if using a controller
        if (Gamepad.all.Count >= 1)
        {
            controls.GamepadMouseControls.Enable();

            Vector2 move = controls.GamepadMouseControls.Movement.ReadValue<Vector2>();

            rb.velocity = move * moveSpeed;

            Vector3 clampPos = this.transform.position;
            clampPos.x = Mathf.Clamp(clampPos.x, -screenBounds.x + .5f, screenBounds.x - .5f);
            clampPos.y = Mathf.Clamp(clampPos.y, -screenBounds.y + .5f, screenBounds.y - .5f);

            this.transform.position = clampPos;
        }
        else
        {
            controls.GamepadMouseControls.Disable();
        }
    }

    void resetPos()
    {
        this.transform.position = Player.instance.transform.position;

        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, this.transform.position.z));
    }
}
