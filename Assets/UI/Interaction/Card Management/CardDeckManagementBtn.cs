using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDeckManagementBtn : MonoBehaviour
{
	private CardScriptableObj cardScriptableObj;
	private CardManagementCanvas cardManagementCanvas;
	public Button btn { get; private set; }
	private int btnId;

	public void Initialize(CardScriptableObj _cardScrObj, CardManagementCanvas _cardCanvas, int _btnId)
	{
		cardScriptableObj = _cardScrObj;
		cardManagementCanvas = _cardCanvas;
		btnId = _btnId;

		btn = GetComponent<Button>();
		btn.onClick.AddListener(() => cardManagementCanvas.ClickedCardIcon(btnId));

		TextMeshProUGUI text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
		text.text = cardScriptableObj._CardName;

		GetComponent<Image>().sprite = cardScriptableObj._CardImage;
	}

	private void OnDestroy()
	{
		btn.onClick.RemoveAllListeners();
	}

	// end
}
