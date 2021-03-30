using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
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
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject gun in other.gameObject.GetComponent<GunController>().gunLoadout)
            {
                if (gun.activeInHierarchy)
                {
                    float secondsUsed = (float)(System.DateTime.Now - gun.GetComponent<GunBehavior>().switchTime).TotalSeconds;

                    QualtricsDataContainer.AddWeaponUseTime(gun.name, secondsUsed);
                }
            }

            QualtricsDataContainer.SendData();
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
