using System.Collections.Generic;
using UnityEngine;

public class Distribute : MonoBehaviour
{
    public List<Transform> gifts;
    void Update(){
        foreach(GameObject t in GameObject.FindGameObjectsWithTag("Target")){
            t.GetComponent<rayCastForward>().alignLib[0] = gifts[0];
            t.GetComponent<rayCastForward>().alignLib[1] = gifts[1];
            t.GetComponent<rayCastForward>().alignLib[2] = gifts[2];
            t.GetComponent<rayCastForward>().alignLib[3] = gifts[3];
        }
    }
}
