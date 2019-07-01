using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    // Start is called before the first frame update
    Transform apple;
    Transform key;
    void Start()
    {
        apple = transform.Find("apple");
        key = transform.Find("key");
        updateAchievement();
    }

    void updateAchievement()
    {
        PlayerData playerData = (PlayerData)SavePlayerData.GetData("Save/PlayerData.sav", typeof(PlayerData));
        if(playerData!=null)
        {
            apple.Find("count").GetComponent<Text>().text= "x"+playerData.numOfFace;
            key.Find("count").GetComponent<Text>().text = "x" + playerData.numOfKey;
        }
    }
}
