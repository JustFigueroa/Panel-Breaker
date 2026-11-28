using UnityEngine;
using UnityEngine.SceneManagement;

public class LostInSpace : MonoBehaviour
{
    public Camera cam;
    
     Animator anim;
     Vector3 startPosition;
     Vector3 endPosition;
     float startZoom;
     float time = 0f;
     float progress;
     bool zooming = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!zooming)
        return;
            time += Time.deltaTime;
            cam.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            cam.orthographicSize = Mathf.Lerp(startZoom, 3f, time);
            progress = time / 1f; 
            progress = Mathf.Clamp01(progress);
            cam.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            cam.orthographicSize = Mathf.Lerp(startZoom, 3f, progress);
            if(progress >= .90f)
            SceneManager.LoadScene("Level Lost In Space");
    }
    
    void OnMouseEnter()
    {
        anim.SetBool("Highlighted", true);
    }
    void OnMouseExit()
    {
    anim.SetBool("Highlighted", false);

    }

    void OnMouseUpAsButton()
    {
        anim.SetBool("Selected", true);
        startPosition = cam.transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        startZoom = cam.orthographicSize;
        time = 0f;
        zooming = true;
    }
}
