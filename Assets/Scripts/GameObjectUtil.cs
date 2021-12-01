using System;
using UnityEngine;

internal static class GameObjectUtil
{
    private static GameManager _gameManager;

    internal static void KeepAliveOneCopy(string myTag, GameObject gameObject)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(myTag);

        if (objs.Length > 1)
        {
            UnityEngine.Object.Destroy(gameObject);
        }

        UnityEngine.Object.DontDestroyOnLoad(gameObject);
    }


    internal static GameManager GetGameManager()
    {
        if (_gameManager == null)
        {
            GameObject objs = GameObject.Find("GameManager");
            if (objs != null)
            {
                _gameManager = objs.GetComponent<GameManager>();
            }
            else
            {
                GameObject gameObjectManager = new GameObject("GameManager");
                _gameManager = gameObjectManager.AddComponent<GameManager>();
                _gameManager.RefreshSceneObjects();
            }
        }

        return _gameManager;
    }



}