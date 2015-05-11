// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden

using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	/*
     This camera smoothes out rotation around the y-axis and height.
     Horizontal Distance to the target is always fixed.
     
     There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.
     
     For every of those smoothed values we calculate the wanted value and the current value.
     Then we smooth it using the Lerp function.
     Then we apply the smoothed values to the transform's position.
     */
	
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public bool isDamping;
	public bool isStop;
	public float fov;
	private Camera mainCam;
	public Transform target2;
	public Transform lookTarget;


	void Start(){
		mainCam = GetComponent<Camera> ();
		fov = mainCam.fieldOfView;
		target2 = GameManager.instance.goal;
	}

	void  LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target)
			return;
		if(GameState.instance.isCameraStatic)
			isStop = true;

		if (isDamping) {
			transform.localPosition = Vector3.Lerp(transform.localPosition,new Vector3(target.localPosition.x, height, target.localPosition.z),Time.deltaTime);

			if (fov <= 90){
				fov += Time.deltaTime*50;

			}
			if(Vector3.Distance(transform.position,GameManager.instance.goal.transform.position)<15){
				GameState.instance.isCameraDamping = false;
				isStop = true;
			}

		} else {
			if(!isStop){
				transform.LookAt(GameManager.instance.goal.transform);
				transform.position = GetDesiredPosition();
				if (fov >= 60){
					fov -= Time.deltaTime*50;
				}
			}
		}

		mainCam.fieldOfView = fov;
	}

	float a,b,c,d;
	public Vector3 GetDesiredPosition(){
		c=distance;
		a = Mathf.Abs(target.position.z-target2.position.z);
		d = Mathf.Abs((target.position.x)-target2.position.x);
		b = (a/d)*(c);
		Debug.Log("a: "+a);
		Debug.Log("b: "+b);
		Debug.Log("d: "+d);

		return Vector3.Lerp(transform.position,new Vector3(target.position.x +c, target.position.y +7, target.position.z-b),Time.deltaTime*2);
	}
	
}
