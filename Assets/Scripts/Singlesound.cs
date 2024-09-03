using UnityEngine;

public class Singlesound : MonoBehaviour {
    [SerializeField, Range(0f, 2f)]
    float pitchMin;
    [SerializeField, Range(0f, 2f)]
    float pitchMax;

    AudioSource source;

    protected void Start() {
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();
        Destroy(gameObject, source.clip.length);
    }
}
