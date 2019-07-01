using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;//单实例类

    public AudioSource musicPlayer;//播放背景音乐的组件
    public AudioSource soundPlayer;//播放音效的组件
    private GameObject[] ASS;//当前场景所有播放组件

    //AudioClip clip;
    public static AudioManager Instance
    {
        get
        {
            return AudioManager.instance;
        }
    }

    void Start()
    {
        //Debug.Log("start");

        //Instance = this;//初始化该实例类

    }
    private void Awake()
    {
        ASS = GameObject.FindGameObjectsWithTag("AS");
    }

    //播放背景音乐
    public void PlayMusic(string name)
    {
        //如果当前背景音乐没有播放，播放给定的背景音乐（循环播放）
        if (!musicPlayer.isPlaying)
        {
            //给定的音乐资源必须在Resource文件夹中
            musicPlayer.clip = ResourceManager.GetInstance().getClip(name);
            if(musicPlayer.clip !=null)
            {
                musicPlayer.Play();
            }
           else
            {
                Debug.Log("Not Found Music:"+name);
            }
        }
    }

    //停止播放背景音乐
    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    //播放音效
    public void PlaySound(string name)
    {

        //给定的音效资源必须在Resource文件夹中
        //Debug.Log("play");
        if (name == "Music/Sounds/Walk" || name == "Music/Sounds/Swimming")
        {
            if (!soundPlayer.isPlaying)
            {
                AudioClip c = ResourceManager.GetInstance().getClip(name);
                if (c != null)
                    soundPlayer.PlayOneShot(c);
                else
                    Debug.Log("Not Found Music:" + name);
            }
            return;

        }
        AudioClip clip = ResourceManager.GetInstance().getClip(name);
        if (clip != null)
            soundPlayer.PlayOneShot(clip);
        else
            Debug.Log("Not Found Music:" + name);
        
        return;

    }

    public void setMusciVolume(float mv)
    {
        musicPlayer.volume = mv;
        
    }
    public void setSoundVolume(float sv)
    {
        foreach(GameObject AS in ASS)
        {
            AS.GetComponent<AudioSource>().volume = sv;
        }
        soundPlayer.volume = sv;
    }
}

