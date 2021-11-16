using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine.UI;

public class fetcher : MonoBehaviour
{
    public List<string> data, designation, dataToText;
    public GameObject exit;
    public Image screen;
    public List<Text> text_Base, text_Data;
    public readonly string url = "https://nnedigitaldesignstorage.blob.core.windows.net/candidatetasks/Metadata.csv?sp=r&st=2021-03-15T09:12:39Z&se=2024-11-05T17:12:39Z&spr=https&sv=2020-02-10&sr=b&sig=oyj3Qyg4W42%2BO0d7YqmjxmKk0k%2BLVmE243ixdLaq3gk%3D";
    string receiver;

    public void Conveyor(string selection){
        GetCSV(selection,url);
    }

    void Update(){
        //Function for exiting the game, weird place to have the function, but it is the most universal relative to the player
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    public string GetCSV(string selection, string url)
    {
        //Pulls a temporary download based on the URL
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

        StreamReader sr = new StreamReader(resp.GetResponseStream());
        string results = sr.ReadToEnd();
        //Stores the raw text under Receiver to be parsed in Once()
        receiver = results;
        sr.Close();
        Once(selection);
        return results;
    }

    void Once(string selection){
        string[] tempstr;
        //Parse the raw text based on ; and line ends
        tempstr = receiver.Split(';', '\n');
        
        //Compile the parsed data into a single list which allows us to deliver requested parts
        foreach(string item in tempstr){
            if(!string.IsNullOrWhiteSpace(item)){
                data.Add(item);
            }
        }

        {
            //This solution is a bit messy, but does the trick
            if(selection == "RedData"){
                designation = data.GetRange(0,6);
                dataToText = data.GetRange(6,6);
            }
            else if(selection == "BlueData"){
                designation = data.GetRange(0,6);
                dataToText = data.GetRange(12,6);
            }
            else if(selection == "GreenData"){
                designation = data.GetRange(0,6);
                dataToText = data.GetRange(18,6);
            }
            else{
                designation = data.GetRange(0,6);
                dataToText = data.GetRange(24,6);
            }
        }
        
        //Allocate the texts to their appropriate locations on the HUD
        foreach(Text t0 in text_Base){
            t0.text = designation[text_Base.IndexOf(t0)];
        }
        foreach(Text t1 in text_Data){
            t1.text = dataToText[text_Data.IndexOf(t1)];
        }
        //Here to enable a pleasant reading experience for the HUD
        screen.enabled = true;
        exit.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        transform.parent.GetComponent<p_Movement>().enabled = false;
        transform.Find("rayCaster").GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Reset(){
        //Here for when you are done reading
        screen.enabled = false;
        exit.SetActive(false);
        foreach(Text t in text_Base){
            t.text = "";
        }
        foreach(Text t1 in text_Data){
            t1.text = "";
        }
        Cursor.lockState = CursorLockMode.Locked;
        transform.parent.GetComponent<p_Movement>().enabled = true;
        transform.Find("rayCaster").GetComponent<SpriteRenderer>().enabled = true;
    }
}
