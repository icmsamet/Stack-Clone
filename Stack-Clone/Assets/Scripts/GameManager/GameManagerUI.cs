using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace GameManager
{
    public class GameManagerUI
    {
        GameObject m_startPanel;
        GameObject m_gamePanel;
        GameObject m_failPanel;
        TextMeshProUGUI m_scoreText;
        TextMeshProUGUI m_startPanelScoreText;

        public GameManagerUI(GameObject startPanel,GameObject gamePanel,GameObject failPanel,TextMeshProUGUI scoreText,TextMeshProUGUI startPanelScoreText)
        {
            m_startPanel = startPanel;
            m_gamePanel = gamePanel;
            m_failPanel = failPanel;
            m_scoreText = scoreText;
            m_startPanelScoreText = startPanelScoreText;
        }

        public void StartGame()
        {
            m_startPanel.SetActive(false);
            m_failPanel.SetActive(false);
            m_gamePanel.SetActive(true);
        }
        public void FailGame()
        {
            m_startPanel.SetActive(false);
            m_failPanel.SetActive(true);
            m_gamePanel.SetActive(true);
        }

        public void SetText(TextMeshProUGUI textMeshPro,string text)
        {
            textMeshPro.text = text;
        }

        public void AddScore(int score)
        {
            SetText(m_scoreText, score.ToString());
        }

        public void GetBestScore()
        {
            SetText(m_startPanelScoreText, GetBestScoreText());
        }

        public string GetBestScoreText()
        {
            return PlayerPrefs.GetInt("BestScore").ToString();
        }
    }
}
