using UnityEngine;

public class CardDeckManagementBtn : MonoBehaviour
{
	private CardScriptableObj cardScriptableObj;
	private CardManagementCanvas cardManagementCanvas;

	public void Initialize(CardScriptableObj _cardScrObj, CardManagementCanvas _cardCanvas)
	{
		cardScriptableObj = _cardScrObj;
		cardManagementCanvas = _cardCanvas;
	}

	// end
}
