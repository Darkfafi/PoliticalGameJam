using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HappyState : MonoBehaviour
{
    private States states;
    [SerializeField] private Transform player;
    private float x;
    private float y;
    private float dir;
    private float zRot;
    private float rotS;

    private void Start ()
	{
	    states = GetComponent<States>();
	    dir = 1;
	    zRot = 0;
	    rotS = 1;
        StartCoroutine(jTimer());
    }

    private void Update()
    {
        x = player.transform.position.x;
        y = player.transform.position.y;
        player.transform.localEulerAngles = new Vector3(0,0,0 + zRot);
        if (states.State == PeopleStates.happy)
        {
            player.position = new Vector3(x,y + dir * Time.deltaTime);
            if (zRot >= 13 || zRot <= -13)
            {
                rotS *= -1;
            }
            zRot += rotS;
        }
        /*
      if (Input.GetKey(KeyCode.A))
            {
                if (states.State == PeopleStates.normal)
                {
                    states.happy();
                }
                else
                {
                    states.normal();
                }
            }
            */
    }

    private IEnumerator jTimer()
    {
        yield return new WaitForSeconds(0.2f);
        dir *= -1;
        StartCoroutine(jTimer());
    }
}
