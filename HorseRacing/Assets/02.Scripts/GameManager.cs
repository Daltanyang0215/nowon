using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] public List<Horse> horses;
    public Text text_t;
    public Text text;

    [SerializeField] private Transform goalPoint;

    public void Play()
    {
        foreach (var horse in horses)
        {
            horse.StartMove(goalPoint.position.z - horse.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if( other.gameObject.tag == "Player")
            {
                horses.Add(other.gameObject.GetComponent<Horse>());
            }
            if(horses.Count >= 5)
            {   
                text_t.gameObject.SetActive( true);
                text.gameObject.SetActive(true);
                text.text = $"1�� : {horses[0].name} \n2�� : {horses[1].name} \n3�� : {horses[2].name} \n4�� : {horses[3].name} \n5�� : {horses[4].name} \n";
            }
        }
    }

}
