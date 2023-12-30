// TestAudioPlayer.cs

using UnityEngine;

public class TestAudioPlayer : MonoBehaviour
{
    public AudioClip testSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource.PlayClipAtPoint(testSound, transform.position);
        }
    }
}
