using RPG.Dialogue;
using RPG.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class DialogueUI : MonoBehaviour
  {
    PlayerConversation _playerConversation;
    [SerializeField] TextMeshProUGUI _text, _headerText;
    [SerializeField] Button _nextBtn, _quitBtn;
    [SerializeField] Transform _choiceRoot;
    [SerializeField] GameObject _choicePrefab, _response;
    void Start()
    {
      _playerConversation = SceneMgr.Self.Player.GetComponent<PlayerConversation>();
      _playerConversation.OnConversationUpdate += UpdateUI;
      _nextBtn.onClick.AddListener(_playerConversation.Next);
      _quitBtn.onClick.AddListener(_playerConversation.Quit);
      UpdateUI();
    }
    void UpdateUI()
    {
      gameObject.SetActive(_playerConversation.IsActive);
      if (!_playerConversation.IsActive)
        return;
      _headerText.text = _playerConversation.CurConversationName;
      _response.SetActive(!_playerConversation.IsChoosing);
      _choiceRoot.gameObject.SetActive(_playerConversation.IsChoosing);
      if (_playerConversation.IsChoosing)
      {
        BuildChoices();
      }
      else
      {
        _text.text = _playerConversation.Text;
        _nextBtn.gameObject.SetActive(_playerConversation.HasNext);
      }
    }

    private void BuildChoices()
    {
      _choiceRoot.transform.DestroyAllChildren();
      foreach (var choice in _playerConversation.Choices)
      {
        var choiceInstance = Instantiate(_choicePrefab, _choiceRoot);
        choiceInstance.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;
        choiceInstance.GetComponentInChildren<Button>().onClick.AddListener(() => _playerConversation.SelectChoice(choice));
      }
    }
  }
}