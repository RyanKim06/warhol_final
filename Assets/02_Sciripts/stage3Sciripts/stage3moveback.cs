using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage3moveback : MonoBehaviour
{
	public float speed;
	private float x;
	public float PontoDeDestino;
	public float PontoOriginal;

	public FlyLittlePlayer flylittle;

	// Use this for initialization
	void Start()
	{
		//PontoOriginal = transform.position.x;
	}

	// Update is called once per frame
	void Update()
	{

		if (flylittle.isgamestart)
		{
			x = transform.position.x;
			x += speed * Time.deltaTime;
			transform.position = new Vector3(x, transform.position.y, transform.position.z);

			if (x <= PontoDeDestino)
			{

				x = PontoOriginal;
				transform.position = new Vector3(x, transform.position.y, transform.position.z);
			}
		}
	}
}
