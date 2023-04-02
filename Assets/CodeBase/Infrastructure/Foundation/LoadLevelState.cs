// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using CodeBase.EditorStructures;
using CodeBase.Infrastructure.Foundation.States;
using CodeBase.Infrastructure.Foundation.States.Intefaces;

namespace CodeBase.Infrastructure.Foundation
{
    public class LoadLevelState : IPayloadState<string>
    {
        private const string GameScene = "Game";
        private readonly StateMachine _stateMachine;

        public LoadLevelState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter(string payload)
        {
            var config = LoadConfig(payload);
            _stateMachine.LevelData = config;
            _stateMachine.Enter<SceneLoadState, string>(GameScene);
            _stateMachine.Enter<GameState>();
        }

        private LevelConfig LoadConfig(string levelName)
        {
            var path = Path.Combine($"{Application.streamingAssetsPath}/level{levelName}.json");
#if UNITY_ANDROID
            var reader = new WWW(path);
            while (!reader.isDone)
            {
            }
            var data = reader.text;
#elif UNITY_EDITOR
            StreamReader reader = new StreamReader(path);
            var data = reader.ReadToEnd();
            reader.Close();
#endif
            return JsonConvert.DeserializeObject<LevelConfig>(data);
        }

        public void Exit()
        {
        }
    }
}