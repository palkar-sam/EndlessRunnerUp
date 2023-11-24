using TMPro;
using UnityEngine;

public class BulletInventoryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI redColorText;
    [SerializeField] private TextMeshProUGUI greenColorText;
    [SerializeField] private TextMeshProUGUI blueColorText;

    public int RedBulletCount => _redCount;
    public int GreenBulletCount => _redCount;
    public int BlueBulletCount => _redCount;

    private int _redCount;
    private int _greenCount;
    private int _blueCount;

    public void AddBullet(CollectibleType collType, int quantity, bool isAdd)
    {
        switch (collType)
        {
            case CollectibleType.Red:
                if (isAdd)
                    _redCount += quantity;
                else
                    _redCount -= quantity;

                redColorText.text = _redCount.ToString();
                break;
            case CollectibleType.Green:

                if(isAdd)
                    _greenCount += quantity;
                else
                    _greenCount -= quantity;

                greenColorText.text = _greenCount.ToString();
                break;
            case CollectibleType.Blue:
                if(isAdd)
                    _blueCount += quantity;
                else
                    _blueCount -= quantity;

                blueColorText.text = _blueCount.ToString();
                break;
            default:
                break;
        }



    }
}