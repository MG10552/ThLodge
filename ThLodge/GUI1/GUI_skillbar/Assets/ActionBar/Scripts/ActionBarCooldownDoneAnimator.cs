using UnityEngine;
using System.Collections;

public class ActionBarCooldownDoneAnimator : MonoBehaviour
{
    float startTime;
    Color color;

    public float Duration = 0.5f;
    public float ScaleFrom = 0f;
    public float ScaleTo = 3f;
    public float Rotations = 1f;

    void Start()
    {
        startTime = Time.time;

        GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material.name = GetComponent<Renderer>().material.name + " (Copy)";
        color = GetComponent<Renderer>().material.GetColor("_TintColor");

        GameObject.Destroy(gameObject, Duration);
    }

    void Update()
    {
        float t = ((Time.time - startTime) / Duration);
        float s = Mathf.Lerp(ScaleFrom, ScaleTo, t);

        transform.rotation = Quaternion.Euler(0, 0, 360f * Rotations * t);
        transform.localScale = new Vector3(s, s, 1);

        GetComponent<Renderer>().material.SetColor("_TintColor", new Color(color.r, color.b, color.g, 1 - t));
    }
}
