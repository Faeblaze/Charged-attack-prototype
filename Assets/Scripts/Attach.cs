using UnityEngine;

public class Attach : MonoBehaviour {
    public Transform target;
    private Vector3 fallback;

    public Attach SetFallbackPosition(Vector3 position)
    {
        fallback = position;

        return this;
    }

    private void LateUpdate()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
            return;
        }
        if (target)
        {
            transform.position = target.position;
            fallback = transform.position;
        }
        else
            transform.position = fallback;
    }
}
