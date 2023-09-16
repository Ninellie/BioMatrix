using UnityEngine;

public interface CardInfoDisplayer
{
    void SetActiveCardInfo(string deckTitle, string cardTitle, string cardDescription);
    void SetSelectedCardInfo(string deckTitle, string cardTitle, string cardDescription);
    void Select();
    void Deselect();
}

public class CardInfoUIPanel : MonoBehaviour, CardInfoDisplayer
{
    private string _panelTitle;

    private string _activeCardDeckTitle;
    private string _activeCardTitle;
    private string _activeCardDescription;

    //private string activeCard
    //private string selectedCard


    public void SetActiveCardInfo(string deckTitle, string cardTitle, string cardDescription)
    {
    }

    public void SetSelectedCardInfo(string deckTitle, string cardTitle, string cardDescription)
    {
    }

    public void Select()
    {

    }

    public void Deselect()
    {
    }
}