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
    [SerializeField] Button _nextBtn;
    [SerializeField] Transform _choiceRoot;
    [SerializeField] GameObject _choicePrefab, _response;
    void Start()
    {
      _playerConversation = SceneMgr.Self.Player.GetComponent<PlayerConversation>();
      _nextBtn.onClick.AddListener(Next);
      UpdateUI();
    }
    void Next()
    {
      _playerConversation.Next();
      UpdateUI();
    }

    void UpdateUI()
    {
      _response.SetActive(!_playerConversation.IsChoosing);
      _choiceRoot.gameObject.SetActive(_playerConversation.IsChoosing);
      if (_playerConversation.IsChoosing)
      {
        foreach (Transform item in _choiceRoot.transform)
          Destroy(item.gameObject);
        foreach (var text in _playerConversation.Choices)
        {
          var choice = Instantiate(_choicePrefab, _choiceRoot);
          choice.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
      }
      else
      {
        _text.text = _playerConversation.Text;
        _nextBtn.gameObject.SetActive(_playerConversation.HasNext);
      }
    }
  }
}