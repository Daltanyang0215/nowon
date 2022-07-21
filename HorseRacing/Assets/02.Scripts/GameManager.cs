using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public List<Horse> horses;
    public Text text_t;
    public Text text;

    [SerializeField] private Transform goalPoint;
    [SerializeField] private Transform platform1GradePoint;
    [SerializeField] private Transform platform2GradePoint;
    [SerializeField] private Transform platform3GradePoint;
    private List<Transform> horsesFinished = new List<Transform>();

    public void Play()
    {
        foreach (var horse in horses)
        {
            horse.StartMove(goalPoint.position.z - horse.transform.position.z);
        }
    }

    public List<Transform> GetHorseTransforms()
    {
        List<Transform> tmpList = new List<Transform>();
        foreach (var item in horses)
        {
            tmpList.Add(item.transform);
        }
        return tmpList;
    }

    private void Update()
    {

        for (int i = horses.Count-1; i > -1; i--)
        {
            if (horses[i].isFinish)
            {
                horsesFinished.Add(horses[i].transform);
                horses.Remove(horses[i]);

                if (horses.Count == 0)
                {
                    OnGameFinish();
                }

            }
        }

    }

    void OnGameFinish()
    {
        horsesFinished[0].position = platform1GradePoint.position;
        horsesFinished[1].position = platform2GradePoint.position;
        horsesFinished[2].position = platform3GradePoint.position;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other != null)
    //    {
    //        if( other.gameObject.tag == "Player")
    //        {
    //            horses.Add(other.gameObject.GetComponent<Horse>());
    //        }
    //        if(horses.Count >= 5)
    //        {   
    //            text_t.gameObject.SetActive( true);
    //            text.gameObject.SetActive(true);
    //            text.text = $"1등 : {horses[0].name} \n2등 : {horses[1].name} \n3등 : {horses[2].name} \n4등 : {horses[3].name} \n5등 : {horses[4].name} \n";
    //        }
    //    }
    //}

}
