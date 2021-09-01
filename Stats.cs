using System;

namespace GameManager
{
    class Stats : IStats
    {
        public Stats(string name)
        {
            _name = name;
            _level = GenerateLevel();

            for (int i = 0; i < _level; i++)
            {
                _health += 10;
                _damage += 2;
            }

            _maxHealth = _health;
            _armor = 1;
        }
        
        private string _name;
        private int _level;
        private float _health;
        private float _maxHealth;
        private int _damage;
        private float _armor;
        private bool _maxArmor = false;
        private int GenerateLevel()
        {
            var rng = new Random();
            int levelRng = rng.Next(1, 20);

            return levelRng;
        }

        
        public float TakeDamage(float damagePoints)
        {
            _health -= (int)(damagePoints * _armor);
            return (int)_health;
        }
        public int DoDamage() => _damage;
        
        public string GetName() => _name;
        public int GetLevel() => _level;
        public float GetHealth() => _health;

        public float SetHealth(float currHealth)
        {
            _health = currHealth;
            return _health;
        }
        public float GetMaxHealth() => _maxHealth;
        public int GetAttack() => _damage;
        public float GetArmor() => _armor;
        public bool GetMaxArmor() => _maxArmor;

        public void SetArmor(float armorUpgrade)
        {
            _armor -= armorUpgrade;
            
            if (_armor <= 0.5)
            {
                _armor = 0.5f;
                _maxArmor = true;
            }
        }
    }
}
