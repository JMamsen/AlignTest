using System.Collections.Generic;
using UnityEngine;

public class rayCastForward : MonoBehaviour {

    public Transform alignBox;
    public List<string> tagSearch;
    public List<Transform> alignLib;

    void Update(){
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)){
            var selection = hit.transform;

            if(Input.GetMouseButtonDown(0)){
                //Get information for alignment
                if(selection.tag == "Blue" || selection.tag == "Black" || selection.tag == "Red" || selection.tag == "Green"){
                    Identify(selection.transform);
                }
                //Get information for data cube and start fetching process
                else if(selection.tag == "BlueData" || selection.tag == "YellowData" || selection.tag == "RedData" || selection.tag == "GreenData"){
                    transform.parent.GetComponent<fetcher>().Conveyor(selection.tag.ToString());
                }
            }
        }
    }

    void Identify(Transform e){
        foreach(Transform i in alignLib){
            if (i.tag == e.tag){
                //Find the tag to align with the clicked tag and start the process
                Align(alignBox, i, e);
            }
        }
    }

    void Align(Transform tr1P, Transform tr1C, Transform tr2C){
        //Set the model to the appropriate alignment
        tr1P.rotation = tr2C.rotation * Quaternion.Inverse(tr1C.localRotation);
        tr1P.position = tr2C.position + (tr1P.position - tr1C.position);
    }
}
