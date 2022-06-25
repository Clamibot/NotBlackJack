using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider thickCloud;
    private float leftBoundary;
    [SerializeField] private float speed = 0.0625f;
    [SerializeField] private float angle = 28f;

    void Start()
    {
        thickCloud = gameObject.GetComponent<Collider>();
        gameObject.transform.Rotate(Vector3.up, Random.Range(-1, 2) * angle);
    }

    private void FixedUpdate()
    {
        //Start to move the object to the left
        this.transform.position = new Vector3(
            this.transform.position.x - speed,
            this.transform.position.y,
            this.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndCloud")
        {
            thickCloud.enabled = false;
            CloudGenerator.numClouds--;
            StartCoroutine(DisappearAfterTime(gameObject, 4f));
        }
    }

    public IEnumerator DisappearAfterTime(GameObject cloud, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(cloud);
    }

}
