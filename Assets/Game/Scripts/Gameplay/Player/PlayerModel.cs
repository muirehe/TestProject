using Configs;
using Gameplay.Combat;
using Infrastructure.ConfigSystem;

namespace Gameplay.Player
{
    public class PlayerModel
    {
        public Unit Unit { get; }

        public PlayerModel(ConfigProvider configProvider) =>
            Unit = new Unit(configProvider.GetSingle<PlayerConfig>());
    }
}