using UnityEngine;
using UnityEngine.UI;

public sealed class OptionsWindow : MonoBehaviour
{
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Text _spriteNameHolder;

    private bool _isInit = false;

    public void Init() {
        var gc = GameController.Instance;

        _settingsBtn.onClick.AddListener(gc.PauseGame);
        _closeBtn.onClick.AddListener(gc.ResumeGame);

        _isInit = true;
    }

    public void Show(string spriteName) {
        if (!_isInit) {
            Init();
        }

        _spriteNameHolder.text = spriteName;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
