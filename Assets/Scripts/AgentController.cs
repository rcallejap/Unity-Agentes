// TC2008B. Sistemas Multiagentes y Gráficas Computacionales
// C# client to interact with Python
// Sergio. Julio 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AgentController : MonoBehaviour
{
    List<List<float>> positions;
    List<List<float>> pastPositions;
    public GameObject agent1Prefab;
    public GameObject agent2Prefab;
    public GameObject agent3Prefab;

    public GameObject SemaforoPrefav;
    GameObject[] semaforos;
    int statusSemaforo = 0;


    GameObject[] agents;
    public float timeToUpdate = 5.0f;
    private float timer;
    float dt;

    // IEnumerator - yield return
    IEnumerator SendData(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585";
        using (UnityWebRequest www = UnityWebRequest.Post(url, form)) {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("Content-Type", "text/html");
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();          // Talk to Python
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                List<List<float>> newPositions = new List<List<float>>();
                string txt = www.downloadHandler.text.Replace('\'', '\"');
                txt = txt.TrimStart('"', '{', 'd', 'a', 't', 'a', ':', '[');
                txt = "{\"" + txt;
                txt = txt.TrimEnd(']', '}');
                txt = txt + '}';
                string[] strs = txt.Split(new string[] { "}, {" }, StringSplitOptions.None);
                //Debug.Log("strs.Length:"+strs.Length);
                
                for (int i = 0; i < strs.Length; i++){
                
                    //Debug.Log(i);
                    string[] subStrs =  strs[i].Split(new string[] { ":" , ",", "}" }, StringSplitOptions.None);
                    //Debug.Log("Id:" + subStrs[1] + "x" + subStrs[3] + "y" + subStrs[5] + "z" + subStrs[7]);

                    if (i ==0){
                        int sem  = int.Parse(subStrs[1]);
                        statusSemaforo = sem; 
                    }
                    else {
                        int id  = int.Parse(subStrs[1]);
                        float x = float.Parse(subStrs[3]);
                        float y = float.Parse(subStrs[5]);
                        float z = float.Parse(subStrs[7]);
        
                        //Debug.Log("__________");
                        //Debug.Log(newPositions[street][car]);
                        List<float> pos = new List<float>(){id, x, y, z};
                        newPositions.Add(pos);
                        
                        //Debug.Log(pos[0]);
                    }
                }
                pastPositions = positions;
                positions= newPositions;
            }
        }
    }

    // Start is called before the first frame update
    void Start(){
        agents = new GameObject[500];
        for(int i = 0; i < 500; i++){
            if (i%4 == 0)
            {
                agents[i] = Instantiate(agent2Prefab, Vector3.zero, Quaternion.identity);
            }
            else if (i%3 == 0){
                agents[i] = Instantiate(agent3Prefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                agents[i] = Instantiate(agent1Prefab, Vector3.zero, Quaternion.identity);
            }
            agents[i].transform.localScale = new Vector3(50, 50, 50);
        }
        semaforos = new GameObject[4];

        semaforos[0] = Instantiate(SemaforoPrefav, new Vector3(21f, .4f, 24.3f), Quaternion.identity);
        semaforos[0].transform.localScale = new Vector3(.3f, .3f, .3f);
        semaforos[0].transform.Rotate(-90, 0, -30);
        semaforos[0].transform.Find("Green").gameObject.SetActive(true);
        semaforos[0].transform.Find("Red").gameObject.SetActive(true);

        semaforos[1] = Instantiate(SemaforoPrefav, new Vector3(-6.81f, .4f, -3.48f), Quaternion.identity);
        semaforos[1].transform.localScale = new Vector3(.3f, .3f, .3f);
        semaforos[1].transform.Rotate(-90, 0, 45);
        semaforos[1].transform.Find("Green").gameObject.SetActive(true);
        semaforos[1].transform.Find("Red").gameObject.SetActive(true);
        

        semaforos[2] = Instantiate(SemaforoPrefav, new Vector3(-56f, .4f, 35.4f), Quaternion.identity);
        semaforos[2].transform.localScale = new Vector3(.3f, .3f, .3f);
        semaforos[2].transform.Rotate(-90, 0, 140);
        semaforos[2].transform.Find("Green").gameObject.SetActive(true);
        semaforos[2].transform.Find("Red").gameObject.SetActive(true);
        
        
        semaforos[3] = Instantiate(SemaforoPrefav, new Vector3(-3.39f, .4f, 71.35f), Quaternion.identity);
        semaforos[3].transform.localScale = new Vector3(.3f, .3f, .3f);
        semaforos[3].transform.Rotate(-90, 0, 235);
        semaforos[3].transform.Find("Green").gameObject.SetActive(true);
        semaforos[3].transform.Find("Red").gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if  (statusSemaforo == 1){

            semaforos[0].transform.Find("Green").gameObject.SetActive(true);
            semaforos[0].transform.Find("Red").gameObject.SetActive(false);

            semaforos[1].transform.Find("Green").gameObject.SetActive(false);
            semaforos[1].transform.Find("Red").gameObject.SetActive(true);

            semaforos[2].transform.Find("Green").gameObject.SetActive(true);
            semaforos[2].transform.Find("Red").gameObject.SetActive(false);

            semaforos[3].transform.Find("Green").gameObject.SetActive(false);
            semaforos[3].transform.Find("Red").gameObject.SetActive(true);
        }
        else{
            semaforos[0].transform.Find("Green").gameObject.SetActive(false);
            semaforos[0].transform.Find("Red").gameObject.SetActive(true);

            semaforos[1].transform.Find("Green").gameObject.SetActive(true);
            semaforos[1].transform.Find("Red").gameObject.SetActive(false);

            semaforos[2].transform.Find("Green").gameObject.SetActive(false);
            semaforos[2].transform.Find("Red").gameObject.SetActive(true);

            semaforos[3].transform.Find("Green").gameObject.SetActive(true);
            semaforos[3].transform.Find("Red").gameObject.SetActive(false);

        }


#if UNITY_EDITOR
        timer = timeToUpdate; // reset the timer
        Vector3 fakePos = new Vector3(3.44f, 0, -15.707f);
        string json = EditorJsonUtility.ToJson(fakePos);
        StartCoroutine(SendData(json));
#endif

        for(int i = 0; i < 500; i++){
            agents[i].transform.localPosition  =new Vector3(0,100,0);
            
        } 
        Debug.Log("lenght " + pastPositions.Count);
        for (int i = 0; i < pastPositions.Count; i++)
        {

            Debug.Log(pastPositions[i][0]+ " " + pastPositions[i][1] + " " + pastPositions[i][2] + " " + pastPositions[i][3]);
            int car = (int)pastPositions[i][0];
            agents[car].transform.localPosition = new Vector3(pastPositions[i][1], pastPositions[i][2], pastPositions[i][3]);
        }
        for (int i = 0; i < positions.Count; i++)
        {
            int car = (int)positions[i][0];
            //agents[car].transform.eulerAngles = new Vector3(positions[i][1], positions[i][2], positions[i][3]);
            //Rotacion
            Vector3 targetPos = new Vector3(positions[i][1], positions[i][2], positions[i][3]);
            Vector3 currPost = agents[car].transform.localPosition;
            if (targetPos!=currPost)
            {
                Vector3 direction = (targetPos - currPost).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation *= Quaternion.Euler(0,-90,0);
                
                agents[car].transform.rotation = Quaternion.Slerp(agents[car].transform.rotation, targetRotation,Time.deltaTime * 2);
            }
        }
    }
}