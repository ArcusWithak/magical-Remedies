using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneController : MonoBehaviour
{
    GameObject text;

    VideoPlayer videoPlayer;

    GameManager manager;

    private void Awake()
    {
        manager = GameManager.instance;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    private void Start()
    {
        if (GameManager.instance.currentLevelIndex == 3)
        {
            text = transform.parent.parent.GetChild(1).GetChild(0).gameObject;
            StartCoroutine(DisapearingText());
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log(manager.saveLoaded);
        Debug.Log(manager.currentLevel);

        if (!manager.saveLoaded)
        {

            if (manager.currentLevelIndex == 3)
            {
                manager.LoadLevel(manager.NumberOfMenuLevels);
            }
            else if (manager.currentLevel <= 4)
            {
                manager.LoadLevel(manager.NumberOfMenuLevels + 1);
            }
            else
            {
                manager.LoadLevel(manager.NumberOfMenuLevels + 2);
            }
        }
        else
        {
            if (manager.saveLoaded)
            {
                if (manager.currentLevel <= 1)
                {
                    manager.LoadLevel(manager.NumberOfMenuLevels);
                }
                else if (manager.currentLevel < 5)
                {
                    manager.LoadLevel(manager.NumberOfMenuLevels + 1);
                }
                else
                {
                    manager.LoadLevel(manager.NumberOfMenuLevels + 2);
                }
            }
        }

    }

    private void Update()
    {
        if (manager.currentLevelIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (manager.saveLoaded)
                {
                    if (manager.currentLevel <= 1)
                    {
                        manager.LoadLevel(manager.NumberOfMenuLevels);
                    }
                    else if (manager.currentLevel < 5)
                    {
                        manager.LoadLevel(manager.NumberOfMenuLevels + 1);
                    }
                    else
                    {
                        manager.LoadLevel(manager.NumberOfMenuLevels + 2);
                    }
                }
                else
                {
                    manager.LoadLevel(manager.NumberOfMenuLevels);
                }
            }
        }
    }

    private IEnumerator DisapearingText()
    {
        text.SetActive(false);
        yield return new WaitForSeconds(1);
        text.SetActive(true);
        yield return new WaitForSeconds(5);
        text.SetActive(false);
    }
}
