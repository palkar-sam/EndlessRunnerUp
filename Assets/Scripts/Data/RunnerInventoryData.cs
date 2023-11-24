using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Data
{
    public class RunnerInventoryData
    {
        private RunnerInventoryData()
        {
        }

        private const string USER_IDS = "User_Ids";
        private const string USER_SCORE = "User_Score";
        private const int MAX_RECORDS = 10; 

        private static RunnerInventoryData _instance;

        public int RedBullet => _redBulletCount;
        public int GreenBullet => _greenBulletCount;
        public int BlueBullet => _blueBulletCount;
        public int HighScore => _highScore;
        public string DebugData => _logString;
        public List<KeyValuePair<string, string>> LeaderBoardList => _sortedUserList != null && _sortedUserList.Count > 0 ? _sortedUserList : new List<KeyValuePair<string, string>>();

        public bool IsBulletAvailable => _redBulletCount > 0 || _greenBulletCount > 0 || _blueBulletCount > 0;

        private int _redBulletCount;
        private int _greenBulletCount;
        private int _blueBulletCount;
        private int _highScore;
        private string _userId;
        private string _storedUserIds;
        private Dictionary<string, string> _userInfo = new Dictionary<string, string>();
        private List<KeyValuePair<string, string>> _sortedUserList;
        private string _logString = string.Empty;

        public static RunnerInventoryData GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RunnerInventoryData();   
            }

            return _instance;
        }

        public void SetLeaderBoardData()
        {
            _userInfo = new Dictionary<string, string>();
            _storedUserIds = PlayerPrefs.GetString(USER_IDS);
            
            if(!string.IsNullOrEmpty(_storedUserIds))
            {
                string[] userIdArr = _storedUserIds.Split(",");
                string.Concat(_logString,"Leader Board : Stored userIds : " + _storedUserIds + " : Count : " + userIdArr.Length);
                _highScore = 0;
                int score = 0;

                for (int i = 0; i < userIdArr.Length; i++)
                {
                    _userInfo[userIdArr[i]] = PlayerPrefs.GetString(userIdArr[i]);
                    score = int.Parse(_userInfo[userIdArr[i]]);

                    if (score > _highScore)
                        _highScore = score;

                    string.Concat(_logString,$"Leader Boar data : User ID : {userIdArr[i]} : Score : {_userInfo[userIdArr[i]]}");
                }

                string.Concat(_logString,"Leader Board : Total user data : " + _userInfo.Count);
                SortUserInfoList();
            }
        }

        public void SaveLeaderBoardData()
        {
            if (_sortedUserList == null) return;

            PlayerPrefs.DeleteAll();
            string userList = string.Empty;
            string.Concat(_logString,$"Leader Board : save user data : _soreted User LIst Obj: {_sortedUserList}");

            foreach (KeyValuePair<string, string> pair in _sortedUserList)
            {
                string.Concat(_logString,$"Leader Board : save user data : User id : {pair.Key} : value score : {pair.Value}");
                string.Concat(userList, pair.Key);
                PlayerPrefs.SetString(pair.Key, pair.Value);
            }

            string.Concat(_logString,"Saved User ids : " + userList);
            _storedUserIds = userList;

            PlayerPrefs.SetString(USER_IDS, userList);
            PlayerPrefs.Save();
        }

        public void CreateCurrentRoundId()
        {
            // Get the offset from current time in UTC time
            DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
            // Get the unix timestamp in seconds
            string unixTime = dto.ToUnixTimeSeconds().ToString();
            // Get the unix timestamp in seconds, and add the milliseconds
            string unixTimeMilliSeconds = dto.ToUnixTimeMilliseconds().ToString();
            _userId = unixTimeMilliSeconds;
        } 


        public void AddRedBullet()
        {
            _redBulletCount++;
        }

        public void reduceRedBullet()
        {
            _redBulletCount--;
            if (_redBulletCount <= 0)
                _redBulletCount = 0;
        }

        public void AddGreenBullet()
        {
            _greenBulletCount++;
        }

        public void reduceGreenBullet()
        {
            _greenBulletCount--;
            if (_greenBulletCount <= 0)
                _greenBulletCount = 0;
        }

        public void AddBlueBullet()
        {
            _blueBulletCount++;
        }

        public void reduceBlueBullet()
        {
            _blueBulletCount--;
            if (_blueBulletCount <= 0)
                _blueBulletCount = 0;
        }

        public CollectibleType GetRanomBullet()
        {

            if (!IsBulletAvailable) return CollectibleType.None;

            System.Random random = new System.Random();

            Type type = typeof(CollectibleType);
            Array values = type.GetEnumValues();
            int index = random.Next(values.Length);

            CollectibleType value = (CollectibleType)values.GetValue(index);


            switch (value)
            {
                case CollectibleType.Red:

                    if (_redBulletCount <= 0)
                        value = GetRanomBullet();
                    break;

                case CollectibleType.Green:
                    if (_greenBulletCount <= 0)
                        value = GetRanomBullet();
                    break;

                case CollectibleType.Blue:
                    if (_blueBulletCount <= 0)
                        value = GetRanomBullet();
                    break;
            }

            return value;
        }

        public void SetHighestScore(int newScore)
        {
            string.Concat(_logString,"Leader Board : Set New SCore : " + newScore);
            if (newScore > _highScore)
                _highScore = newScore;

            _userInfo.Add(_userId, newScore.ToString());
            SortUserInfoList();
            if(_sortedUserList.Count > MAX_RECORDS)
            {
                _sortedUserList.RemoveRange(MAX_RECORDS, _sortedUserList.Count - MAX_RECORDS);
            }

            string.Concat(_logString,"Leader Board : affter Seting New SCore : ");

            foreach (KeyValuePair<string, string> pair in _sortedUserList)
            {
                string.Concat(_logString,$"Leader Board : New user data : User id : {pair.Key} : value score : {pair.Value}");
            }
        }

        private void SortUserInfoList()
        {
            _sortedUserList = new List<KeyValuePair<string, string>>(_userInfo);
            _sortedUserList.Sort(delegate (KeyValuePair<string, string> firstPair,KeyValuePair<string, string> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            });
        }
    }
}
