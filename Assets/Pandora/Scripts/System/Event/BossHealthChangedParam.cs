namespace Pandora.Scripts.System.Event
{
    public struct BossHealthChangedParam
    {
        public float CurrentHealth;
        public float MaxHealth;

        public BossHealthChangedParam(float currentHealth, float maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
}
