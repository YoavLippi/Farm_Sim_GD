namespace Data
{
    public struct StatBlock
    {
        private float attention;
        private float hunger;
        private float energy;
        private float cleanliness;
        private float health;

        public StatBlock(float attention, float hunger, float energy, float cleanliness, float health)
        {
            this.attention = attention;
            this.hunger = hunger;
            this.energy = energy;
            this.cleanliness = cleanliness;
            this.health = health;
        }

        public float Attention
        {
            get => attention;
            set => attention = value;
        }

        public float Hunger
        {
            get => hunger;
            set => hunger = value;
        }

        public float Energy
        {
            get => energy;
            set => energy = value;
        }

        public float Cleanliness
        {
            get => cleanliness;
            set => cleanliness = value;
        }

        public float Health
        {
            get => health;
            set => health = value;
        }
    }
}