using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameManager
{
    [CreateAssetMenu(fileName = "NewGameSettings",menuName = "Game Settings")]
    public class GameSettingsScriptableObject : ScriptableObject
    {
        public float moveSpeed = 1.5f;
    }
}
