using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using System;

public static class Scores
{
    public static Action OnEventCalculateMoney;

    public const float LOW_PULLING_STRENGHT_MULTIPLY = 0.9f;
    public const float LOW_MID_PULLING_STRENGHT_MULTIPLY = 1f;
    public const float HIGH_MID_PULLING_STRENGHT_MULTIPLY = 1.1f;
    public const float HIGH_PULLING_STRENGHT_MULTIPLY = 1.2f;

    public const int MAIN_TARGET_SCORE_VALUE = 100;
    public const int SECOND_TARGET_SCORE_VALUE = 50;

    public const int SMALL_OBSTACLE_SCORE_VALUE = 10;
    public const int MEDIUM_OBSTACLE_SCORE_VALUE = 20;
    public const int LARGE_OBSTACLE_SCORE_VALUE = 50;

    public const int SNIPER_SCORE_VALUE = -1000;

    public const int DONT_KILL_SCORE_VALUE = -100;
    public const int DONT_DESTROY_SCORE_VALUE = -50;

    public const float HEADSHOT_MULTIPLY = 1.2f;

    public const int MIN_LEVEL_REWARD = 0;
    public const int MAX_LEVEL_REWARD = 200;

    public static void SendCalculateMoney()
    {
        OnEventCalculateMoney?.Invoke();
    }

    public static int CalculateMoneyFromPlayerScore(in GameState gameState, int currentScene)
    {
        int currentReward;
        int finalReward;
        int receivedReward;

        var Index = SaveManager.GetData().Levels.FindIndex(x => x.ID == currentScene);

        receivedReward = Mathf.CeilToInt(gameState.LevelReward * ((float)SaveManager.GetData().Levels[Index].Scores / (float)gameState.TotalMaxScoreOnLevel));
        currentReward = Mathf.CeilToInt(gameState.LevelReward * ((float)gameState.FinalPlayerScore / (float)gameState.TotalMaxScoreOnLevel));
        finalReward = currentReward - receivedReward;
        
        if(finalReward > 0) SaveManager.SaveCoins(finalReward);
        
        SendCalculateMoney();
        return Mathf.Clamp(finalReward, MIN_LEVEL_REWARD, MAX_LEVEL_REWARD);
    }

    public static int CalculateFinalScore(in GameState gameState)
    {
        return Mathf.CeilToInt(gameState.CurrentPlayerScore * GetMultiplyFromPullingStrength(gameState.PullingStrength));
    }

    public static float GetMultiplyFromPullingStrength(float pullingStrength)
    {
        if(pullingStrength < 0.25f)
        {
            return LOW_PULLING_STRENGHT_MULTIPLY;
        }
        if(pullingStrength < 0.5f)
        {
            return LOW_MID_PULLING_STRENGHT_MULTIPLY;
        }
        if (pullingStrength < 0.75f)
        {
            return HIGH_MID_PULLING_STRENGHT_MULTIPLY;
        }
        else
        {
            return HIGH_PULLING_STRENGHT_MULTIPLY;
        }
    }

    public static class Objects
    {
        public enum Type
        {
            Default = 0,

            MainTarget = 1,
            SecondTarget = 2,

            Small = 10,
            Medium = 11,
            Large = 12,

            Sniper = 20,

            DontKill = 21,
            DontDestroy = 22,

        }

        public static int GetValueFromObjectType(Type objectType)
        {
            switch (objectType)
            {
                case Type.Default:
                    return 0;
                case Type.MainTarget:
                    return MAIN_TARGET_SCORE_VALUE;
                case Type.SecondTarget:
                    return SECOND_TARGET_SCORE_VALUE;
                case Type.Small:
                    return SMALL_OBSTACLE_SCORE_VALUE;
                case Type.Medium:
                    return MEDIUM_OBSTACLE_SCORE_VALUE;
                case Type.Large:
                    return LARGE_OBSTACLE_SCORE_VALUE;
                case Type.Sniper:
                    return SNIPER_SCORE_VALUE;
                case Type.DontKill:
                    return DONT_KILL_SCORE_VALUE;
                case Type.DontDestroy:
                    return DONT_DESTROY_SCORE_VALUE;
                default:
                    return 0;
            }
        }
    }

    public static class Combo
    {
        public enum Level
        {
            D = 0,
            C = 1,
            B = 2,
            A = 3,
            S = 4,
            SS = 5,
            SSS = 6,
        }

        public static Level GetComboLevelFromComboCount(int comboCount)
        {
            switch (comboCount)
            {
                case 0:
                    return Level.D;
                case 1:
                    return Level.D;
                case 2:
                    return Level.C;
                case 3:
                    return Level.B;
                case 4:
                    return Level.A;
                case 5:
                    return Level.A;
                case 6:
                    return Level.S;
                case 7:
                    return Level.S;
                case 8:
                    return Level.SS;
                case 9:
                    return Level.SS;
                default:
                    return Level.SSS;
            }
        }

        public static float GetMultipleFromComboLevel(Level comboLevel)
        {
            switch (comboLevel)
            {
                case Level.D:
                    return 1f;
                case Level.C:
                    return 1.05f;
                case Level.B:
                    return 1.1f;
                case Level.A:
                    return 1.15f;
                case Level.S:
                    return 1.2f;
                case Level.SS:
                    return 1.25f;
                case Level.SSS:
                    return 1.3f;
                default:
                    return 1f;
            }
        }

        public static float GetMultiplyFromComboCount(int comboCount)
        {
            return GetMultipleFromComboLevel(GetComboLevelFromComboCount(comboCount));
        }
    }
}
