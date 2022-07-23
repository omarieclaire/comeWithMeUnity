 using UnityEngine;
 using System.Collections;
 
 public class SpinLogic : MonoBehaviour {
 
	 float f_lastX = 0.0f;
	 float f_difX = 0.5f;
	 float f_lastY = 0.0f;
	 float f_difY = 0.5f;
	 float f_steps = 0.0f;
	 int i_direction = 1;
	 public float verticalSpeed = 1f;
	 public float horizontalSpeed = 1f;
	 public float MaximumX;
	 public float MinimumX;
	 private Quaternion xRotationQuaternion;
	 public FloatEvent OutputX;
	 public FloatEvent OutputY;
 
	 Quaternion ClampRotationAroundXAxis(Quaternion q)
	 {
		 q.x /= q.w;
		 q.y /= q.w;
		 q.z /= q.w;
		 q.w = 1.0f;

		 float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		 angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

		 q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		 return q;
	 }
     
	 // Update is called once per frame
	 void Update () 
	 {
		 if (Input.GetMouseButtonDown(0))
		 {
			 f_difX = 0.0f;
			 f_difY = 0.0f;
		 }
		 else if (Input.GetMouseButton(0))
		 {
			 f_difX = Mathf.Abs(f_lastX - Input.GetAxis ("Mouse X"));
 
			 if (f_lastX < Input.GetAxis ("Mouse X"))
			 {
				 i_direction = -1;
				 
				 transform.Rotate(Vector3.up, -f_difX*horizontalSpeed);
			 }
 
			 if (f_lastX > Input.GetAxis ("Mouse X"))
			 {
				 i_direction = 1;
				 transform.Rotate(Vector3.up, f_difX*horizontalSpeed);
			 }
 
			 f_lastX = -Input.GetAxis ("Mouse X");
			 OutputX.Invoke(f_difX);
			 
			 
			 
			 f_difY = Mathf.Abs(f_lastY - Input.GetAxis ("Mouse Y"));
 
			 if (f_lastY < Input.GetAxis ("Mouse Y"))
			 {
				 i_direction = -1;
				 transform.Rotate(Vector3.right, -f_difY*verticalSpeed);
				 
				 
			 }
 
			 if (f_lastY > Input.GetAxis ("Mouse Y"))
			 {
				 i_direction = 1;
				 transform.Rotate(Vector3.right, f_difY*verticalSpeed);
			 }
 
			 f_lastY = -Input.GetAxis ("Mouse Y");
			 OutputY.Invoke(f_difY);
			 //xRotationQuaternion = ClampRotationAroundXAxis(transform.rotation);
			 //transform.rotation = xRotationQuaternion;
			 //Vector3 currentRotation = transform.localRotation.eulerAngles;
			 //currentRotation.y = Mathf.Clamp(currentRotation.y, minimum, maximum);
			 //currentRotation.z =Mathf.Clamp(currentRotation.z, minimum, maximum);
			 //transform.localRotation = Quaternion.Euler (currentRotation);
			 
		 }
		 else 
		 {
			 //if (f_difX > 0.5f) f_difX -= 0.05f;
			 //if (f_difX < 0.5f) f_difX += 0.05f;
 
			 //transform.Rotate(Vector3.up, f_difX * i_direction);
		 }
		 
	 }
 }