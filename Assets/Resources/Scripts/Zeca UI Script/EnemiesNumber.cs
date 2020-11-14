using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesNumber : MonoBehaviour
{     
    public int  Score;
    public Text ScoreText;


    // Start is called before the first frame update
    void Start()
    {
     
    }
        

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entities"))
        {
            Debug.Log("layer");
            AddScore();
            }
     
        
    }

    void AddScore()
    {
        Score++;
        ScoreText.text = Score.ToString();
    }
}
