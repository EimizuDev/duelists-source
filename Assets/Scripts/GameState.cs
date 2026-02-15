using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private int _playerHP = 10;
    private bool _isPlayerSplit = false;

    [SerializeField] private HeartState[] playerHearts;

    private int _botHP = 10;
    private bool _isBotSplit = false;

    [SerializeField] private HeartState[] botHearts;

    public ParticleSystem parrySpark;
    public ParticleSystem blockSpark;

    static public bool fightBegan = false;

    [SerializeField] private TextMeshProUGUI centerText;
    [SerializeField] private GameObject _guide;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);

        centerText.text = "2";

        yield return new WaitForSeconds(1f);

        centerText.text = "1";

        yield return new WaitForSeconds(1f);

        centerText.fontSize = 175;
        centerText.text = "Fight!";
        GameState.fightBegan = true;
        _guide.SetActive(false);

        yield return new WaitForSeconds(1f);

        centerText.gameObject.SetActive(false);
    }

    public void DamagePlayer(bool isGlaceDamage)
    {
        if (isGlaceDamage && _isPlayerSplit)
        {
            _isPlayerSplit = false;
            _playerHP = Mathf.Clamp(_playerHP - 1, 0, 10);
            DamageHeart(2, 0);
        }
        else if (isGlaceDamage)
        {
            _isPlayerSplit = true;
            DamageHeart(1, 0);
        }
        else
        {
            _playerHP = Mathf.Clamp(_playerHP - 1, 0, 10);
            DamageHeart(_isPlayerSplit ? 3 : 0, 0);
        }
    }

    public void DamageBot(bool isGlaceDamage)
    {
        if (isGlaceDamage && _isBotSplit)
        {
            _isBotSplit = false;
            _botHP = Mathf.Clamp(_botHP - 1, 0, 10);
            DamageHeart(2, 1);
        }
        else if (isGlaceDamage)
        {
            _isBotSplit = true;
            DamageHeart(1, 1);
        }
        else
        {
            _botHP = Mathf.Clamp(_botHP - 1, 0, 10);
            DamageHeart(_isPlayerSplit ? 3 : 0, 1);
        }
    }

    private void DamageHeart(int typeOfDamage, int whoToDamage)
    {
        if (whoToDamage == 0)
        {
            for (int i = playerHearts.Length - 1; i >= 0; i--)
            {
                if (!playerHearts[i].isGone)
                {
                    if (typeOfDamage == 0)
                    {
                        playerHearts[i].BreakHeart();
                        playerHearts[i].isGone = true;

                        if (i == 0) break;
                    }
                    else if (typeOfDamage == 1)
                    {
                        playerHearts[i].SplitHeart();
                        _isPlayerSplit = true;
                    }
                    else if (typeOfDamage == 2)
                    {
                        playerHearts[i].gameObject.SetActive(false);
                        _isPlayerSplit = false;
                        playerHearts[i].isGone = true;

                        if (i == 0) break;
                    }
                    else if (typeOfDamage == 3)
                    {
                        playerHearts[i].gameObject.SetActive(false);
                        playerHearts[i].isGone = true;

                        if (i - 1 >= 0)
                        {
                            playerHearts[i - 1].SplitHeart();
                        }
                        else
                        {
                            break;
                        }
                    }

                    return;
                }
            }
        }
        else if (whoToDamage == 1)
        {
            for (int i = botHearts.Length - 1; i >= 0; i--)
            {
                if (!botHearts[i].isGone)
                {
                    if (typeOfDamage == 0)
                    {
                        botHearts[i].BreakHeart();
                        botHearts[i].isGone = true;

                        if (i == 0) break;
                    }
                    else if (typeOfDamage == 1)
                    {
                        botHearts[i].SplitHeart();
                        _isBotSplit = true;
                    }
                    else if (typeOfDamage == 2)
                    {
                        botHearts[i].gameObject.SetActive(false);
                        _isBotSplit = false;
                        botHearts[i].isGone = true;

                        if (i == 0) break;
                    }
                    else if (typeOfDamage == 3)
                    {
                        botHearts[i].gameObject.SetActive(false);
                        botHearts[i].isGone = true;

                        if (i - 1 >= 0)
                        {
                            botHearts[i - 1].SplitHeart();
                        }
                        else
                        {
                            break;
                        }
                    }

                    return;
                }
            }
        }

        fightBegan = false;

        centerText.text = whoToDamage == 0 ? "Defeat" : "Victory";

        centerText.fontSize = 175;

        centerText.outlineColor = whoToDamage == 0 ? new Color32(255, 81, 81, 137) : new Color32(9, 209, 20, 137);

        centerText.gameObject.SetActive(true);

        StartCoroutine(ReturnToMainMenu());
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(0);
    }
}
