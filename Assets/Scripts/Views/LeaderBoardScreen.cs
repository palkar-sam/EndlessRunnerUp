using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardScreen : MonoBehaviour
{
    [SerializeField] private LeaderBoardItemPanel leaderBoardItem;
    [SerializeField] private Transform itemContainer;

    private List<string> userList = new List<string>();

    public void SetData(List<KeyValuePair<string, string>> itemList)
    {

        foreach (KeyValuePair<string, string> pair in itemList)
        {

            if (!userList.Contains(pair.Key))
            {
                LeaderBoardItemPanel item = Instantiate(leaderBoardItem, itemContainer, false);
                item.SetData(pair.Key, pair.Value);
                userList.Add(pair.Key);
            }
        }

        if (itemList.Count > 0)
            gameObject.SetActive(true);
    }

    public void CloseLeaderBoard()
    {
        gameObject.SetActive(false);
    }
}
