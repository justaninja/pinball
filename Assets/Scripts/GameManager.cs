﻿using System.Collections;
using ConstantineSpace.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConstantineSpace.PinBall
{
    public class GameManager : Singleton<GameManager>
    {
        private int _score;
        public bool UseAI;

        // All available game states.
        public enum GameState
        {
            Menu,
            InGame
        }

        // The current state of the game.
        public GameState CurrentState { get; private set; }

        /// <summary>
        ///     Initialization.
        /// </summary>
        private void Start()
        {
            ScreenManager.Instance.SetHomeScreen();
            GuiManager.Instance.SetHomeScoreTexts();
            SetGameState(GameState.Menu);
        }

        /// <summary>
        ///     Start the level or gameplay.
        /// </summary>
        /// <param name="useAI">True for AI mode.</param>
        public void StartLevel(bool useAI)
        {
            UseAI = useAI;
            ScreenManager.Instance.SetGameScreen();
            SetGameState(GameState.InGame);
            SetTouchSender(!useAI);
            _score = 0;
            GuiManager.Instance.SetScoreText(_score);
            if (useAI)
            {
                StartCoroutine(RandomForceLaunch(0.5f));
                
            }
        }

        /// <summary>
        ///     Go to the Home screen.
        /// </summary>
        public void GoToHome()
        {
            ScreenManager.Instance.HideGameScreen();
            ScreenManager.Instance.SetHomeScreen();
            SetGameState(GameState.Menu);

        }

        /// <summary>
        ///     Sets the game state.
        /// </summary>
        /// <param name="state">The new state.</param>
        private void SetGameState(GameState state)
        {
            CurrentState = state;
        }

        /// <summary>
        ///     Sets sender state.
        /// </summary>
        /// <param name="state">The new state.</param>
        private void SetTouchSender(bool state)
        {
            GetComponent<TouchManager>().enabled = state;
        }

        /// <summary>
        ///     Makes level end.
        /// </summary>
        public void EndLevel()
        {
            ScoreManager.SaveScore(_score);
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        ///     Updates score.
        /// </summary>
        /// <param name="score">Additional score.</param>
        public void UpdateScore(int score)
        {
            _score += score;
            GuiManager.Instance.SetScoreText(_score);
        }

        /// <summary>
        ///     Launches the ball with random force.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RandomForceLaunch(float delay)
        {
            var touchTime = Random.Range(1.0f, 5.0f);
            yield return new WaitForSeconds(delay);
            FindObjectOfType<Launcher>().LaunchBall(touchTime);
            
        }
    }
}