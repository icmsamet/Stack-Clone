using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameManager
{
    [CreateAssetMenu(fileName = "NewGameSettings",menuName = "Game Settings")]
    public class GameSettingsScriptableObject : ScriptableObject
    {
        public float moveTime = 1.5f;
        public float colorFade = 0.02f;
        public float blockThickness = .25f;
    }
}
