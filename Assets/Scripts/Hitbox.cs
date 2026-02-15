using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [HideInInspector] public bool isOffensive;

    [HideInInspector] public bool isParry;

    [SerializeField] private GameState _gameState;

    private Collider2D thisCollider;

    private void Start()
    {
        thisCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hurtbox" && isOffensive)
        {
            Hurtbox hurtbox = collision.gameObject.GetComponent<Hurtbox>();

            if (hurtbox._isPlayer && transform.parent.name == "Bot")
            {
                _gameState.DamagePlayer(false);
                GameManager.instance.PlaySound(GameManager.instance.hurt);
            }
            else if (!hurtbox._isPlayer && transform.parent.name == "Player")
            {
                _gameState.DamageBot(false);
                GameManager.instance.PlaySound(GameManager.instance.hurt);
            }
        }
        else if (collision.gameObject.name == "Hitbox" && !isOffensive && collision.gameObject.GetComponent<Hitbox>().isOffensive)
        {
            if (isParry)
            {
                if (collision.transform.parent.name == "Player")
                {
                    PlayerController playerController = collision.transform.parent.GetComponent<PlayerController>();

                    if (BotController.state == BotController.State.OverheadBlock && PlayerController.state == PlayerController.State.OverheadAttack)
                    {
                        BotController.animator.SetTrigger("Overhead_Parry");
                        playerController.animator.SetTrigger("Parried");
                        PlayerController.state = PlayerController.State.Parried;

                        BotController.stopBlockingDelegate();

                        Instantiate(_gameState.parrySpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y + 1.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.parry);
                    }
                    else if (BotController.state == BotController.State.MiddleBlock && PlayerController.state == PlayerController.State.MiddleAttack)
                    {
                        BotController.animator.SetTrigger("Middle_Parry");
                        playerController.animator.SetTrigger("Parried");
                        PlayerController.state = PlayerController.State.Parried;

                        BotController.stopBlockingDelegate();

                        Instantiate(_gameState.parrySpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y + 1f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.parry);
                    }
                    else if (BotController.state == BotController.State.LowBlock && PlayerController.state == PlayerController.State.LowAttack)
                    {
                        BotController.animator.SetTrigger("Low_Parry");
                        playerController.animator.SetTrigger("Parried");
                        PlayerController.state = PlayerController.State.Parried;

                        BotController.stopBlockingDelegate();

                        Instantiate(_gameState.parrySpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y - 0.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.parry);
                    }
                }
                else if (collision.transform.parent.name == "Bot")
                {
                    PlayerController playerController = transform.parent.GetComponent<PlayerController>();

                    if (PlayerController.state == PlayerController.State.OverheadBlock && BotController.state == BotController.State.OverheadAttack)
                    {
                        playerController.animator.SetTrigger("Overhead_Parry");
                        BotController.animator.SetTrigger("Parried");
                        Instantiate(_gameState.parrySpark, new Vector2(collision.transform.parent.position.x + collision.offset.x, collision.transform.parent.position.y + collision.offset.y + 1.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.parry);
                    }
                    else if (PlayerController.state == PlayerController.State.MiddleBlock && BotController.state == BotController.State.MiddleAttack)
                    {
                        playerController.animator.SetTrigger("Middle_Parry");
                        BotController.animator.SetTrigger("Parried");
                        Instantiate(_gameState.parrySpark, new Vector2(collision.transform.parent.position.x + collision.offset.x, collision.transform.parent.position.y + collision.offset.y + 1f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.parry);
                    }
                    else if (PlayerController.state == PlayerController.State.LowBlock && BotController.state == BotController.State.LowAttack)
                    {
                        playerController.animator.SetTrigger("Low_Parry");
                        BotController.animator.SetTrigger("Parried");
                        Instantiate(_gameState.parrySpark, new Vector2(collision.transform.parent.position.x + collision.offset.x, collision.transform.parent.position.y + collision.offset.y - 0.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.parry);
                    }
                }
            }
            else
            {
                if (collision.transform.parent.name == "Player")
                {
                    if (BotController.state == BotController.State.LowBlock && PlayerController.state == PlayerController.State.LowAttack)
                    {
                        _gameState.DamageBot(true);
                        collision.transform.parent.GetComponent<PlayerController>().animator.SetTrigger("Blocked");
                        BotController.stopBlockingDelegate();
                        Instantiate(_gameState.blockSpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y - 0.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.block);
                    }
                    else if (BotController.state == BotController.State.MiddleBlock && PlayerController.state == PlayerController.State.MiddleAttack)
                    {
                        _gameState.DamageBot(true);
                        collision.transform.parent.GetComponent<PlayerController>().animator.SetTrigger("Blocked");
                        BotController.stopBlockingDelegate();
                        Instantiate(_gameState.blockSpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y + 1f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.block);
                    }
                    else if (BotController.state == BotController.State.OverheadBlock && PlayerController.state == PlayerController.State.OverheadAttack)
                    {
                        _gameState.DamageBot(true);
                        collision.transform.parent.GetComponent<PlayerController>().animator.SetTrigger("Blocked");
                        BotController.stopBlockingDelegate();
                        Instantiate(_gameState.blockSpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y + 1.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.block);
                    }
                }
                else if (collision.transform.parent.name == "Bot")
                {
                    if (PlayerController.state == PlayerController.State.LowBlock && BotController.state == BotController.State.LowAttack)
                    {
                        _gameState.DamagePlayer(true);
                        BotController.animator.SetTrigger("Blocked");
                        Instantiate(_gameState.blockSpark, new Vector2(collision.transform.parent.position.x + collision.offset.x - 1.5f, collision.transform.parent.position.y + collision.offset.y - 0.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.block);
                    }
                    else if (PlayerController.state == PlayerController.State.MiddleBlock && BotController.state == BotController.State.MiddleAttack)
                    {
                        _gameState.DamagePlayer(true);
                        BotController.animator.SetTrigger("Blocked");
                        Instantiate(_gameState.blockSpark, new Vector2(collision.transform.parent.position.x + collision.offset.x - 1.5f, collision.transform.parent.position.y + collision.offset.y + 1f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.block);
                    }
                    else if (PlayerController.state == PlayerController.State.OverheadBlock && BotController.state == BotController.State.OverheadAttack)
                    {
                        _gameState.DamagePlayer(true);
                        BotController.animator.SetTrigger("Blocked");
                        Instantiate(_gameState.blockSpark, new Vector2(collision.transform.parent.position.x + collision.offset.x - 1.5f, collision.transform.parent.position.y + collision.offset.y + 1.5f), Quaternion.identity);
                        GameManager.instance.PlaySound(GameManager.instance.block);
                    }
                }
            }
        }
        else if (collision.gameObject.name == "Hitbox" && isOffensive && collision.gameObject.GetComponent<Hitbox>().isOffensive)
        {
            if (collision.transform.parent.name == "Player")
            {
                PlayerController player = collision.transform.parent.GetComponent<PlayerController>();

                if (BotController.state == BotController.State.LowAttack && PlayerController.state == PlayerController.State.LowAttack)
                {
                    BotController.animator.SetTrigger("Clash");
                    player.animator.SetTrigger("Clash");
                    Instantiate(_gameState.blockSpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y - 0.5f), Quaternion.identity);
                    GameManager.instance.PlaySound(GameManager.instance.clash);
                }
                else if (BotController.state == BotController.State.MiddleAttack && PlayerController.state == PlayerController.State.MiddleAttack)
                {
                    BotController.animator.SetTrigger("Clash");
                    player.animator.SetTrigger("Clash");
                    Instantiate(_gameState.blockSpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y + 1f), Quaternion.identity);
                    GameManager.instance.PlaySound(GameManager.instance.clash);
                }
                else if (BotController.state == BotController.State.OverheadAttack && PlayerController.state == PlayerController.State.OverheadAttack)
                {
                    BotController.animator.SetTrigger("Clash");
                    player.animator.SetTrigger("Clash");
                    Instantiate(_gameState.blockSpark, new Vector2(thisCollider.transform.parent.position.x + thisCollider.offset.x - 1.5f, thisCollider.transform.parent.position.y + thisCollider.offset.y + 1.5f), Quaternion.identity);
                    GameManager.instance.PlaySound(GameManager.instance.clash);
                }
            }
        }
    }
}
