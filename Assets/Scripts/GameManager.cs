using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private AudioSource _audioPrefab;

    public AudioClip hurt;
    public AudioClip clash;
    public AudioClip block;
    public AudioClip parry;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource newSound = Instantiate(_audioPrefab,  new Vector2(0, 0), Quaternion.identity);

        newSound.clip = clip;

        newSound.volume = 1;

        newSound.Play();

        Destroy(newSound.gameObject, newSound.clip.length);
    }
}
