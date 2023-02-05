using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : MonoBehaviour
{
	public bool isBackground;
	public float speed;
	public float maxMoveLength;

	private FlyLittlePlayer flylittle;

    private float startPosX;
    private float x;

    // Use this for initialization
    void Start()
	{
		startPosX = transform.position.x; //41.5
		flylittle = GameObject.Find("player").GetComponent<FlyLittlePlayer>();
    }

	// Update is called once per frame
	void Update()
	{
		if (flylittle.isgamestart)
		{
			x = transform.position.x; //41.5
			x += speed * Time.deltaTime; //41.5--
			transform.position = new Vector3(x, transform.position.y, transform.position.z); //<--

            if (x < -12 && !isBackground) //pipe & parts out of camera -> Destory
            {
                Destroy(gameObject);
            }

            if (x < maxMoveLength && isBackground) //Beckground Repeat
			{
				x = startPosX;
				transform.position = new Vector3(x, transform.position.y, transform.position.z);
			}
		}
	}
}
