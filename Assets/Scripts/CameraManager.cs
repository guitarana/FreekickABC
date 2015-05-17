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
	public static CameraManager instance;
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 20;
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
	public float angle;
	public GameObject parentCam;



	void Start(){
		instance = this;
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
	

		angle =  GoalOrientation.instance.angle;
		Debug.Log("angle : "+ angle);
		Debug.Log("normal : "+ target2.transform.forward);

		if(GameState.instance.isCelebrating){
			transform.LookAt(target.transform);
			GetDesiredPosition3();
			return;
		}

		if (isDamping) {
			//transform.localPosition = Vector3.Lerp(transform.localPosition,new Vector3(target.localPosition.x, height, target.localPosition.z),Time.deltaTime);
			parentCam.transform.position =Vector3.Lerp(parentCam.transform.localPosition,new Vector3(target.localPosition.x, height-5, target.localPosition.z),Time.deltaTime);

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
				//transform.position = GetDesiredPosition();
				GetDesiredPosition2();
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
//		Debug.Log("a: "+a);
//		Debug.Log("b: "+b);
//		Debug.Log("d: "+d);

		return Vector3.Lerp(transform.position,new Vector3(target.position.x +c, target.position.y +7, target.position.z-b),Time.deltaTime*2);
	}

	public void GetDesiredPosition2(){
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, target2.rotation * Quaternion.Euler(0,180,0), Time.deltaTime * 100f);
		transform.localPosition = new Vector3(0,height,-distance);
		parentCam.transform.position = Vector3.Lerp(parentCam.transform.position,target.transform.position,Time.deltaTime*2);
		parentCam.transform.rotation = Quaternion.RotateTowards(parentCam.transform.rotation, target.transform.rotation, Time.deltaTime * 100f);
	}

	public void GetDesiredPosition3(){
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, target2.rotation * Quaternion.Euler(0,180,0), Time.deltaTime * 100f);
		transform.localPosition = new Vector3(0,8,8);
		parentCam.transform.position = Vector3.Lerp(parentCam.transform.position,target.transform.position,Time.deltaTime*100);
		parentCam.transform.rotation = Quaternion.RotateTowards(parentCam.transform.rotation, target.transform.rotation, Time.deltaTime * 1000f);
	}

	float Angle360(Vector3 v1, Vector3 v2, Vector3 n)
	{
		//  Acute angle [0,180]
		float angle = Vector3.Angle(v1,v2);
		
		//  -Acute angle [180,-179]
		float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(v1, v2)));
		float signed_angle = angle * sign;
		
		//  360 angle
		return (signed_angle <= 0) ? 360 + signed_angle : signed_angle;
	}
	
}
