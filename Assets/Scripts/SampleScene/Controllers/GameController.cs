using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public enum GameStage {
        Play,
        Pause
    }

    private GameStage _gameStage;

    [SerializeField] private OptionsWindow _optionsWindow;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Another instance of GameController already exists!");
            return;
        }

        Instance = this;
        _gameStage = GameStage.Play;
    }

    void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start() {
        InitWindows();
    }

    private void InitWindows() {
        _optionsWindow?.Init();
    }

    public void ResumeGame() {
        _gameStage = GameStage.Play;
        _optionsWindow?.Hide();
    }

    public void PauseGame() {
        _gameStage = GameStage.Pause;
        _optionsWindow?.Show(GetCellName());
    }

    public GameStage CurentGameStage() {
        return _gameStage;
    }

    private string GetCellName() {
        Vector3 cameraCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 1f, Camera.main.transform.position.z));
        RaycastHit2D hit = Physics2D.Raycast(cameraCorner, transform.forward);

        if (hit) {
            return hit.collider.gameObject.name;
        }

        return "No hit detected";
    }
}
