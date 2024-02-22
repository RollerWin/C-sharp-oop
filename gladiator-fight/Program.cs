class Program
{
    static void Main(string[] args)
    {
        Arena arena = new Arena();
        arena.RunFight();
    }
}

abstract class Fighter
{
    public Fighter(int health, int damage, string name)
    {
        Name = name;
        Health = health;
        Damage = damage;
    }

    public string Name {get; protected set;}
    public int Health {get; protected set;}
    public int Damage {get; protected set;}

    public virtual void TakeDamage(int enemyDamage) => Health -= enemyDamage;

    public virtual void Attack(Fighter enemy) => enemy.TakeDamage(Damage);

    public abstract Fighter Clone();
}

class Witcher : Fighter
{
    const int PotionCoolDown = 3;

    private int _stepCounter = 0;
    private int _potionDamage = 40;

    public Witcher(int health, int damage, string name) : base(health, damage, name) {}

    public int ThrowPoison() => _potionDamage;

    public override void Attack(Fighter enemy)
    {
        int damageOutput = 0;

        if(_stepCounter == PotionCoolDown)
        {
            _stepCounter = 0;
            damageOutput = ThrowPoison();
        }
        else
        {
            _stepCounter++;
            damageOutput = Damage;
        }

        enemy.TakeDamage(damageOutput);
    }

    public override Fighter Clone() => new Witcher(Health, Damage, Name);
}

class Ninja : Fighter
{
    const int DodgePercent = 30;

    public Ninja(int health, int damage, string name) : base(health, damage, name) {}

    private bool CanDodge()
    {
        Random random = new Random();
        int minProbabilityPercent = 0;
        int maxProbabilityPercent = 100;

        int dodgeChance = random.Next(minProbabilityPercent, maxProbabilityPercent);

        return  dodgeChance <= DodgePercent;
    }

    public override void TakeDamage(int enemyDamage)
    {
        if(CanDodge())
        {
            Console.WriteLine("Ниндзя уклонился от атаки!");
        }
        else
        {
            base.TakeDamage(enemyDamage);
        }
    }

    public override Fighter Clone() => new Ninja(Health, Damage, Name);
}

class Knight : Fighter
{
    private int _blockDamage = 10;

    public Knight(int health, int damage, string name) : base(health, damage, name) {}

    public override void TakeDamage(int enemyDamage) => base.TakeDamage(enemyDamage - _blockDamage);

    public override Fighter Clone() => new Knight(Health, Damage, Name);
}

class Medic : Fighter
{
    private int _healPower = 10;

    public Medic(int health, int damage, string name) : base(health, damage, name) {}

    public override void TakeDamage(int enemyDamage)
    {
        base.TakeDamage(enemyDamage);
        Heal();
    }

    public void Heal() => Health += _healPower;

    public override Fighter Clone() => new Medic(Health, Damage, Name);
}

class Berserk : Fighter
{
    private const int LowerHealthThreashold = 30;
    private const int DamagePowerUpCoefficient = 2;

    private bool _isBerserkActivated = false;

    public Berserk(int health, int damage, string name) : base(health, damage, name) {}

    public override void TakeDamage(int enemyDamage)
    {
        base.TakeDamage(enemyDamage);

        if(IsInBerserkMode() && _isBerserkActivated == false)
        {
            ActivateBerserkMode();
            _isBerserkActivated = true;
        }
    }

    public bool IsInBerserkMode()
    {
        return Health <= LowerHealthThreashold;
    }

    public override Fighter Clone() => new Berserk(Health, Damage, Name);

    public void ActivateBerserkMode() => Damage *= DamagePowerUpCoefficient;
}

class Barrack
{
    private List<Fighter> _fighters;

    public Barrack()
    {
        _fighters = new List<Fighter>();

        _fighters.Add(new Witcher(100, 15, "Ведьмак"));
        _fighters.Add(new Ninja(90, 20, "Ниндзя"));
        _fighters.Add(new Knight(150, 5, "Рыцарь"));
        _fighters.Add(new Medic(100, 10, "Медик"));
        _fighters.Add(new Berserk(110, 15, "Берсерк"));
    }

    public void ShowFighters()
    {
        for(int i = 0; i < _fighters.Count; i++)
            Console.WriteLine($"{i+1} {_fighters[i].Name} Здоровье: {_fighters[i].Health} Урон: {_fighters[i].Damage}");
    }

    public Fighter GetFighter(int index) => _fighters[index];

    public int GetNumberOfFighters() => _fighters.Count;
}

class Arena
{
    private Barrack _barrack;
    private Fighter _fighter1;
    private Fighter _fighter2;

    public Arena() => _barrack = new Barrack();

    private Fighter ChooseFighter(Barrack barrack)
    {
        Console.Write("Выберите бойца по индексу: ");
        int fighterIndex = ReadCorrectIndex(barrack.GetNumberOfFighters());
        Fighter chosenFighter = barrack.GetFighter(fighterIndex - 1);

        return chosenFighter.Clone();
    }

    private int ReadCorrectIndex(int maxValue)
    {
        string userInput;
        bool isCorrect = false;
        int correctNumber = 0;

        while(isCorrect != true)
        {
            userInput = Console.ReadLine();

            if(int.TryParse(userInput, out correctNumber) && correctNumber > 0 && correctNumber <= maxValue)
            {
                isCorrect = true;
            }
            else
            {
                Console.WriteLine("Некорректный ввод! Попробуйте ещё раз");
                Console.ReadKey();
                Console.Clear();
            }
        }

        return correctNumber;
    }

    public void RunFight()
    {
        _barrack.ShowFighters();
        _fighter1 = ChooseFighter(_barrack);
        _fighter2 = ChooseFighter(_barrack);

        while(_fighter1.Health > 0 &&  _fighter2.Health > 0)
        {
            _fighter1.Attack(_fighter2);
            _fighter2.Attack(_fighter1);

            Console.WriteLine("Игрок 1 наносит урон! Здоровье игрока 2: " + _fighter2.Health);
            Console.WriteLine("Игрок 2 наносит урон! Здоровье игрока 1: " + _fighter1.Health);

            Console.ReadKey();
            Console.WriteLine();
        }

        DetectResult(_fighter1, _fighter2);
    }

    public void DetectResult(Fighter fighter1, Fighter fighter2)
    {
        if(fighter1.Health <= 0 && fighter2.Health <= 0)
        {
            Console.WriteLine("Ничья!");
        }
        else if(fighter1.Health <= 0)
        {
            Console.WriteLine("Игрок 2 победил!");
        }
        else
        {
            Console.WriteLine("Игрок 1 победил!");
        }
    }    
}