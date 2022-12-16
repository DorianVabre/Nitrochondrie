using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadManager : MonoBehaviour
{
    private bool reloadActive;

    void Start(){
        reloadActive = false;
    }

    void Update(){
    }

    public void ActivateReload() {
        reloadActive = true;
    }

    public void OnReload() {
        if (reloadActive) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
