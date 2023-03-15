using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        public bool isDebug = false;

        GameSettingsScriptableObject gameSettings;
        GameManagerSound managerSound;
        GameManagerUI managerUI;
        GameManagerCamera managerCamera;
        GameManagerScene managerScene;

        public static GameManager instance { get; private set; }
        [ShowIf("isDebug")] public GameObject firstObj;
        [ShowIf("isDebug")] public GameObject lastObj;
        [ShowIf("isDebug")][SerializeField] Transform horizontalPos;
        [ShowIf("isDebug")][SerializeField] Transform verticalPos;
        [ShowIf("isDebug")][SerializeField] GameObject spawnReferance;
        [ShowIf("isDebug")][SerializeField] Color startColor;
        [ShowIf("isDebug")][SerializeField] Color endColor;
        [ShowIf("isDebug")][SerializeField] Color currentColor;
        [ShowIf("isDebug")][SerializeField] float colorFade = 0;
        [ShowIf("isDebug")][SerializeField] int cameraMoveCount = 0;
        [ShowIf("isDebug")][SerializeField] Material fogMaterial;
        [ShowIf("isDebug")][SerializeField] int score;
        GameObject m_spawnedBlock;
        /***/
        [ShowIf("isDebug")][SerializeField] Camera camera;
        [ShowIf("isDebug")][SerializeField] AudioClip cutEffect;
        [ShowIf("isDebug")][SerializeField] AudioClip startEffect;
        [ShowIf("isDebug")][SerializeField] AudioClip failEffect;
        [ShowIf("isDebug")][SerializeField] AudioSource effectSource;
        [ShowIf("isDebug")][SerializeField] GameObject particle;
        [ShowIf("isDebug")][SerializeField] GameObject startPanel;
        [ShowIf("isDebug")][SerializeField] GameObject gamePanel;
        [ShowIf("isDebug")][SerializeField] GameObject failPanel;
        [ShowIf("isDebug")][SerializeField] GameObject fogObj;
        [ShowIf("isDebug")][SerializeField] TextMeshProUGUI scoreText;
        [ShowIf("isDebug")][SerializeField] TextMeshProUGUI startPanelScoreText;

        private void Awake()
        {
            if (!instance)
                instance = this;

            gameSettings = Resources.Load<GameSettingsScriptableObject>("GameSettings");
        }
        private void Start()
        {
            Application.targetFrameRate = 60;

            startColor = Random.ColorHSV();
            endColor = new Color(1 - startColor.r, 1 - startColor.g, 1 - startColor.b);
            fogMaterial.SetColor("_Color", new Color(startColor.r + .1f, startColor.g + .1f, startColor.b + .1f));

            lastObj.GetComponent<MeshRenderer>().material.color = startColor;
            currentColor = lastObj.GetComponent<MeshRenderer>().material.color;
            horizontalPos.transform.position = new Vector3(lastObj.transform.localScale.x + .1f, gameSettings.blockThickness, horizontalPos.transform.position.z);
            verticalPos.transform.position = new Vector3(verticalPos.transform.position.x, gameSettings.blockThickness, lastObj.transform.localScale.z + .1f);
            horizontalPos.gameObject.SetActive(System.Convert.ToBoolean(Random.Range(0, 1)));
            verticalPos.gameObject.SetActive(!horizontalPos.gameObject.activeSelf);

            /***/
            managerUI = new GameManagerUI(startPanel, gamePanel, failPanel, scoreText, startPanelScoreText);
            managerSound = new GameManagerSound(effectSource);
            managerCamera = new GameManagerCamera(camera);
            managerScene = new GameManagerScene();
            managerUI.GetBestScore();
        }
        public void StartGame()
        {
            PlayOneShot(startEffect);
            particle.SetActive(false);
            managerUI.StartGame();
            Spawn();
        }
        public void FailGame()
        {
            managerUI.FailGame();
            CheckScore();

        }
        public void CameraFailGame()
        {
            managerCamera.FailGame(LoadScene);
        }
        public void LoadScene()
        {
            managerScene.LoadScene(managerScene.GetSceneIndex());
        }
        public void CutEffect()
        {
            PlayOneShot(cutEffect);
        }
        public void PlayOneShot(AudioClip audioClip)
        {
            managerSound.PlayOneShot(audioClip);
        }
        public void SetLastObj(GameObject _lastObj)
        {
            lastObj = _lastObj;
        }
        public void Spawn()
        {
            horizontalPos.gameObject.SetActive(!horizontalPos.gameObject.activeSelf);
            verticalPos.gameObject.SetActive(!verticalPos.gameObject.activeSelf);
            horizontalPos.transform.localPosition = new Vector3(horizontalPos.transform.localPosition.x, horizontalPos.transform.localPosition.y, lastObj.transform.position.z);
            verticalPos.transform.localPosition = new Vector3(lastObj.transform.position.x, verticalPos.transform.localPosition.y, verticalPos.transform.localPosition.z);

            m_spawnedBlock = Instantiate(spawnReferance, Vector3.zero, Quaternion.identity);
            m_spawnedBlock.transform.localScale = new Vector3(lastObj.transform.localScale.x, gameSettings.blockThickness, lastObj.transform.localScale.z);
            currentColor = Color.Lerp(startColor, endColor, colorFade += gameSettings.colorFade);

            var block = m_spawnedBlock.GetComponent<Block.Block>();
            block.SetColor(currentColor);
            block.SetLastObj(lastObj);
            block.SetDirection(horizontalPos.gameObject.activeSelf);
            m_spawnedBlock.transform.position = block.directionType == Block.Block.Direction.Horizontal ? horizontalPos.transform.position : verticalPos.transform.position;
            block.SetMoveTime(gameSettings.moveTime);
            block.Move();
            if (currentColor == endColor)
            {
                colorFade = 0;
                SetRandomColor();
            }
            cameraMoveCount++;
        }
        public void AddPosition()
        {
            horizontalPos.transform.localPosition += new Vector3(0, gameSettings.blockThickness);
            verticalPos.transform.localPosition += new Vector3(0, gameSettings.blockThickness);
            if (cameraMoveCount > 2)
            {
                managerCamera.AddPosition(new Vector3(0, gameSettings.blockThickness, 0), .2f, Ease.Linear);
                fogObj.transform.DOMoveY(fogObj.transform.localPosition.y + gameSettings.blockThickness, .2f).SetEase(Ease.Linear);
            }
        }
        public void SetRandomColor()
        {
            startColor = currentColor;
            endColor = Random.ColorHSV();
            var fogColor = new Color(endColor.r + .1f, endColor.g + .1f, endColor.b + .1f);

            fogMaterial.DOColor(fogColor, "_Color", 1);
        }
        public void AddScore()
        {
            score++;
            managerUI.AddScore(score);
        }
        public void CheckScore()
        {
            if(score > PlayerPrefs.GetInt("BestScore"))
            {
                PlayerPrefs.SetInt("BestScore", score);
            }
        }
    }
}
