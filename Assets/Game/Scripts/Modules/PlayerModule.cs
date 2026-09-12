using Configs;
using Infrastructure.ConfigSystem;
using Infrastructure.Input;
using Infrastructure.SaveSystem;
using MessagePack;

namespace Modules
{
    [MessagePackObject]
    public class PlayerData
    {
        [Key(0)] public string Name { get; set; } = "Player";
    }

    public class PlayerModule : ModuleBase<PlayerData>
    {
        private readonly GameInput _gameInput;
        private readonly InputSettings _inputSettings;

        public PlayerModule(ConfigProvider configProvider, GameInput gameInput)
        {
            _inputSettings = configProvider.GetSingle<InputSettings>();
            _gameInput = gameInput;
        }
    }
}