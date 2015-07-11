using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
	public bool isEnd;
	public bool shake;
	public float shakeAmount = 50;
	public float frequency = 0.5f;
	public float maxTime = 0.3f;
	public enum Type{
		GameObjectTransformation,
		FloatingPointInterpolation,
		CameraOffset
	}

	public Type type = Type.CameraOffset;

	public Vector3 originPos;
	public float vValue;
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void LateUpdate ()
	{
		if(shake){
			shake=false;
			if(type == Type.GameObjectTransformation)
				StartCoroutine(Shaker(this.gameObject,shakeAmount,maxTime,frequency));
			if(type == Type.FloatingPointInterpolation)
				StartCoroutine(ShakerV(vValue ,shakeAmount,maxTime,frequency));
			if(type == Type.CameraOffset)
				StartCoroutine(ShakerCameraOffset(shakeAmount,maxTime,frequency));
		}

		if(GameState.instance.isGoal && !isEnd){
			isEnd = true;
			shake = true;
		}

		if(!GameState.instance.isGoal){
			isEnd = false;
		}
	}
	
	IEnumerator Shaker(GameObject _goCam, float _shakeAmount, float _maxTime, float _freq){
		float timer = _maxTime;
		originPos = _goCam.transform.position;
		do{
			PosLerp(_goCam,_goCam.transform.position + new Vector3(0,Random.insideUnitSphere.y,0) * _shakeAmount/10,_freq);
			PosLerp(_goCam,originPos,_freq);

			timer-=Time.deltaTime*_freq;
			if(timer<= 0) {
				//_goCam.transform.position = originPos;
				//ObserverController.instance.syncToPlayer= true;
				yield break;
			}
			yield return null;
		}while (true);
	}

	IEnumerator ShakerV(float _float, float _shakeAmount, float _maxTime, float _freq){
		float timer = _maxTime;
		do{

			_float =Mathf.Lerp(_float,Random.insideUnitSphere.y* _shakeAmount/10,_freq);
			vValue = _float;
			//ObserverController.instance.angleV = vValue;
			timer-=Time.deltaTime*_freq;
			if(timer<= 0) {
				yield break;
			}
			yield return null;
		}while (true);
	}

	IEnumerator ShakerCameraOffset(float _shakeAmount, float _maxTime, float _freq){
		float timer = _maxTime;
		//Vector3 origin = ObserverController.instance.cameraOffset;
//		Vector3 offsetTemp = origin;
		do{
			//offsetTemp = PosVectorLerp(offsetTemp,new Vector3(Random.insideUnitSphere.x * _shakeAmount/10,Random.insideUnitSphere.y * _shakeAmount/10,offsetTemp.z),_freq);
			//ObserverController.instance.cameraOffset = offsetTemp;
			timer-=Time.deltaTime*_freq;
			if(timer<= 0) {
				//ObserverController.instance.cameraOffset = origin;
				yield break;
			}
			yield return null;
		}while (true);
	}

	public void PosLerp(GameObject src, Vector3 dst, float timeScale)
	{
		src.transform.position = new Vector3(
			Mathf.Lerp(src.transform.position.x,dst.x,Time.deltaTime * timeScale),
			Mathf.Lerp(src.transform.position.y,dst.y,Time.deltaTime * timeScale),
			Mathf.Lerp(src.transform.position.z,dst.z,Time.deltaTime * timeScale)
			);
	
	}

	Vector3  PosVectorLerp(Vector3 src, Vector3 dst, float timeScale)
	{
		src = new Vector3(
			Mathf.Lerp(src.x,dst.x,Time.deltaTime * timeScale),
			Mathf.Lerp(src.y,dst.y,Time.deltaTime * timeScale),
			Mathf.Lerp(src.z,dst.z,Time.deltaTime * timeScale)
			//src.z
			);
		return src;
	}
}

