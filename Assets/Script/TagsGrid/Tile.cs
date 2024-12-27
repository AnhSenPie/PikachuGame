
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Item _item;
    public Item Item
    {
        get => _item;
        set
        {
            if (_item == value) return;
            _item = value;
            Icon.sprite = _item.Icon;
        }

    }

    public Image Icon;

    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(() => Board.Instance.Select(this));
    }
    public void ClearTile()
    {
        if (_item != null)
        {
            _item = null;
            Icon.gameObject.SetActive(false);
        }
        if(_item == null)
        {
            btn.interactable = false;
            Icon.gameObject.SetActive(false);
        }
    }
}
