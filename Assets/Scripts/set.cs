using UnityEngine;
using System.Collections;

public class set : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int m_queues = 3000;
	
	protected void Awake() {
		Material[] materials = GetComponent<Renderer>().materials;
		for (int i = 0; i < materials.Length; ++i) {
			materials[i].renderQueue = m_queues;
		}
	}
}
