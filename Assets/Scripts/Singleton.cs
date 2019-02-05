using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    public static T Instance;
    private bool doNotDestroy = false;

    protected virtual void Awake() {
        if (Instance == null) {
            Instance = (T)this;
        } else {
            Destroy(gameObject);
        }
    }

    protected void SetDontDestroy() {
        DontDestroyOnLoad(gameObject);
        doNotDestroy = true;
    }

    void OnDestroy() {
        if(!doNotDestroy) 
            Instance = null;
    }
}
