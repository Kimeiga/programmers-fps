using UnityEngine; using System.Collections;

/// MouseLook rotates the transform based on the mouse delta. /// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character: /// - Create a capsule. /// - Add a rigid body to the capsule /// - Add the MouseLook script to the capsule. /// -> Set the mouse look to use LookX. (You want to only turn character but not tilt it) /// - Add FPSWalker script to the capsule

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform. /// - Add a MouseLook script to the camera. /// -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)

public class HeadMouseLook : MonoBehaviour {

    public bool canRotate = true;

public float sensitivityX = 2F;
public float sensitivityY = 2F;

public float minimumX = -360F;
public float maximumX = 360F;

public float minimumY = -90F;
public float maximumY = 90F;

public float rotationX = 0F;
public float rotationY = 0F;

Quaternion originalRotation;

    public MouseLook bodyMouseLook;
    public MouseLook fireMouseLook;

    public float bodyRotationX;
    public float fireRotationY;

    public bool freeLook;
    
    public float rotationXFreeLookOffset;
    public float rotationYFreeLookOffset;



    void Start()
    {
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
        originalRotation = transform.localRotation;

        float rotationXFreeLookOffset = 0;
        float rotationYFreeLookOffset = 0;

        canRotate = true;

    }

    void Update()
    {

        if (freeLook)
        {


            bodyMouseLook.canRotate = false;
            fireMouseLook.canRotate = false;

            // Read the mouse input axis
            rotationXFreeLookOffset += Input.GetAxis("Mouse X") * sensitivityX;
            rotationYFreeLookOffset += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationXFreeLookOffset = ClampAngle(rotationXFreeLookOffset, minimumX, maximumX);
            rotationYFreeLookOffset = ClampAngle(rotationYFreeLookOffset, minimumY, maximumY);


        }
        else
        {

            rotationXFreeLookOffset = Mathf.Lerp(rotationXFreeLookOffset, 0, 0.3f);
            rotationYFreeLookOffset = Mathf.Lerp(rotationYFreeLookOffset, 0, 0.3f);


            bodyMouseLook.canRotate = true;
            fireMouseLook.canRotate = true;

        }

        bodyRotationX = bodyMouseLook.rotationX;
        fireRotationY = fireMouseLook.rotationY;

        rotationX = bodyRotationX + rotationXFreeLookOffset;
        rotationY = fireRotationY + rotationYFreeLookOffset;

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;


        if (Input.GetButton("Middle Mouse Button"))
        {
            freeLook = true;
        }
        else
        {
            freeLook = false;

        }


    }

public static float ClampAngle (float angle, float min, float max)
{
	if (angle < -360F){
			angle += 360F;
		}
	if (angle > 360F){
		angle -= 360F;
		}
	return Mathf.Clamp (angle, min, max);
}
}