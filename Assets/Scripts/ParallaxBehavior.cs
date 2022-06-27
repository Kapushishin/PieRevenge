using UnityEngine;

public class ParallaxBehavior : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private GameObject target;
    [SerializeField, Range(0f, 1f)] float parallaxStr;
    private float temporary, distance;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        temporary = (target.transform.position.x * (1 - parallaxStr));
        distance = (target.transform.position.x * parallaxStr);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temporary > startPos + length)
            startPos += length;
        if (temporary < startPos - length)
            startPos -= length;
    }
}
