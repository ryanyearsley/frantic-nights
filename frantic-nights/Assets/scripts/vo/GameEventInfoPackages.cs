using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class JoinPlayerInfo
    {
        public int inputIndex;
        public int playerNumber;
        public Transform trans;
        public GameObject wizardPrefab;
    }
    public class LeavePlayerInfo
    {
        public int inputIndex;
        public int playerNumber;
        public Transform trans;
    }
    public class SceneInfo
    {
        public int sceneIndex;
        public int initiatorIndex;
    }
