using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PeopleStates { normal, happy };

public class States : MonoBehaviour
{
    private PeopleStates currentState;
    public PeopleStates State { get { return currentState; } }

    private void Start()
    {
        currentState = PeopleStates.normal;
    }

    public void normal()
    {
        currentState = PeopleStates.normal;
    }

    public void happy()
    {
        currentState = PeopleStates.happy;
    }
}
