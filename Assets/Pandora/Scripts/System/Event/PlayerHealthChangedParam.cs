namespace Pandora.Scripts.System.Event
{
    public struct PlayerHealthChangedParam
    {
        public float CurrentHealth;
        public float MaxHealth;
        public int PlayerCharacterId;
        
        public PlayerHealthChangedParam(float currentHealth, float maxHealth, int playerCharacterId)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            PlayerCharacterId = playerCharacterId;
        }
    }
}