class Program
{
    static void Main(string[] agrs)
    {
       Battlefield battlefield = new Battlefield();
       battlefield.RunAction();
    }
}

class Country
{
    public Country(string name)
    {
        Name = name;
        AllySquad = new Squad();
    }

    public string Name {get; private set;}   
    public Squad AllySquad {get; private set;}

    public bool IsSquadExist() => AllySquad.GetNumberOfSoldiers > 0;
}

class Squad
{
    private List<Soldier> _soldiers;

    public Squad()
    {
        _soldiers = new List<Soldier>();
        FillSoldiers();
    }

    public int GetNumberOfSoldiers => _soldiers.Count;

    public Soldier GetSoldierById(int index) => _soldiers[index];

    public void FillSoldiers()
    {
        int numberOfSoldiers = 4;
        ClassesMenu menu = new ClassesMenu();
        
        int minClassIndex = 0;
        int maxClassIndex = menu.GetNumberOfClasses;

        for(int i = 0; i < numberOfSoldiers; i++)
        {
            Soldier soldier = menu.GetSoldier(UserUtils.GenerateRandomValue(minClassIndex, maxClassIndex));    
            _soldiers.Add(soldier);
        }
    }

    public void AttackEnemies(Squad squad)
    {
        int minSoldierIndex = 0;
        int maxEnemySoldierIndex = squad.GetNumberOfSoldiers;
        int maxAllySoldierIndex = GetNumberOfSoldiers;

        Soldier allySoldier = _soldiers[UserUtils.GenerateRandomValue(minSoldierIndex, maxAllySoldierIndex)];
        Soldier enemySoldier = squad.GetSoldierById(UserUtils.GenerateRandomValue(minSoldierIndex, maxEnemySoldierIndex));

        Soldier teammate = _soldiers[UserUtils.GenerateRandomValue(minSoldierIndex, maxAllySoldierIndex)];

        Console.WriteLine($"Атакует {allySoldier.Name}");

        allySoldier.Attack(enemySoldier);

        if (allySoldier is Medic medic)
        {
            medic.Heal(teammate);
        }
        else if (allySoldier is Engineer engineer)
        {
            engineer.LaunchRocket(enemySoldier);
        }
        else if (allySoldier is Support support)
        {
            support.ThrowSmokeToTeammate(teammate);
        }
    }

    public void ShowSoldiers()
    {
        foreach(var soldier in _soldiers)
            Console.WriteLine($"{soldier.Name}\tЗдоровье: {soldier.Health}\tАтака: {soldier.Damage}");

        Console.WriteLine();
    }

    public void RemoveDiedSoldiers()
    {
        for (int i = _soldiers.Count - 1; i >= 0; i--)
            if (_soldiers[i].IsAlive() == false)
                _soldiers.RemoveAt(i);
    }
}

abstract class Soldier
{
    public Soldier(string name, int health, int damage)
    {
        HitPercentByItself = 1;
        Name = name;
        Health = health;
        Damage = damage;
    }

    public double HitPercentByItself {get; protected set;} 
    public string Name {get; protected set;}
    public int Health {get; protected set;}
    public int Damage {get; protected set;}

    public bool IsHit(double enemyWeaponHitProbability = 1)
    {
        int minProbabilityPercent = 0;
        int maxProbabilityPercent = 100;

        double hitChance = UserUtils.GenerateRandomValue(minProbabilityPercent, maxProbabilityPercent);

        hitChance /= maxProbabilityPercent;

        return  hitChance <= HitPercentByItself * enemyWeaponHitProbability;
    }

    public virtual void Attack(Soldier enemySoldier)
    {
        HitPercentByItself = 1;

        if(IsHit())
        {
            enemySoldier.TakeDamage(Damage);
            Console.WriteLine($"Выстрел по {enemySoldier.Name}!\nНанесено {Damage} Урона\nУ противника осталось {enemySoldier.Health} жизней\n");
        }
        else
        {
            Console.WriteLine("Промах!");
        }
    }

    public virtual void TakeDamage(int enemyDamage) => Health -= enemyDamage;

    public void TakeHeal(int healPower) => Health += healPower;

    public void SetHitPercent(double hitPercent) => HitPercentByItself = hitPercent;

    public bool IsAlive() => Health > 0;

    public abstract Soldier Clone();
}

class Medic : Soldier
{
    private int _healPower = 15;

    public Medic(string name, int health, int damage) : base(name, health, damage) {}

    public void Heal(Soldier allySoldier)
    {
        allySoldier.TakeHeal(_healPower);
        Console.WriteLine($"Медик вылечил {allySoldier.Name}!\nТеперь у него {allySoldier.Health} жизней!\n");
    }
    
    public override Soldier Clone() => new Medic(Name, Health, Damage);
}

class Engineer : Soldier
{
    private int _rocketCoolDown = 3;
    private int _rocketDamage = 50;
    private double _rocketHitProbability = 0.6;
    private int _currentStep = 0;
    private int _stepToShoot = 0;

    public Engineer(string name, int health, int damage) : base(name, health, damage) {}

    public void LaunchRocket(Soldier enemySoldier)
    {
        if(_currentStep == _stepToShoot)
        {
            Console.WriteLine("Инженер выстрелил из ракетницы!");
            _currentStep = _rocketCoolDown;

            if(enemySoldier.IsHit(_rocketHitProbability))
            {
                enemySoldier.TakeDamage(_rocketDamage);
                Console.WriteLine($"Успешное попадание по {enemySoldier.Name}!\nТеперь у него {enemySoldier.Health} жизней!");
            }
            else
            {
                Console.WriteLine($"Он промахнулся!");
            }
        }
        else
        {
            _currentStep--;
        }
    }

    public override Soldier Clone() => new Engineer(Name, Health, Damage);
}

class Support : Soldier
{
    private int _smokeCoolDown = 3;
    private double _hitPercentThroughSmoke = 0.4;
    private int _currentStep = 0;
    private int _stepToShoot = 0;

    public Support(string name, int health, int damage) : base(name, health, damage) {}

    public void ThrowSmokeToTeammate(Soldier allySoldier)
    {
        if(_currentStep == _stepToShoot)
        {
            _currentStep = _smokeCoolDown;
            allySoldier.SetHitPercent(_hitPercentThroughSmoke);
            Console.WriteLine($"Поддержка кинула дымовую гранату в союзника {allySoldier.Name}!\nТеперь процент попадания по нему {allySoldier.HitPercentByItself}!\n");
        }
        else
        {
            _currentStep--;
        }
    }

    public override Soldier Clone() => new Support(Name, Health, Damage);
}

class Sniper : Soldier
{
    private int _criticalDamageCoolDown = 3;
    private int _criticalDamageCoefficient = 3;
    private int _currentStep = 0;
    private int _stepToShoot = 0;
    
    public Sniper(string name, int health, int damage) : base(name, health, damage) {}

    public override void Attack(Soldier enemySoldier)
    {
        if(_currentStep == _stepToShoot)
        {
            _currentStep = _criticalDamageCoolDown;
            enemySoldier.TakeDamage(Damage * _criticalDamageCoefficient);
            Console.WriteLine($"Критический выстрел по {enemySoldier.Name}!\nНанесено {Damage * _criticalDamageCoefficient} Урона\nУ противника осталось {enemySoldier.Health} жизней\n");
        }
        else
        {
            _currentStep--;
            base.Attack(enemySoldier);
        }
    }

    public override Soldier Clone() => new Sniper(Name, Health, Damage);
}

class ClassesMenu
{
    private List<Soldier> _soldiers;

    public ClassesMenu()
    {
        _soldiers = new List<Soldier>();

        _soldiers.Add(new Medic("Медик", 100, 15));
        _soldiers.Add(new Engineer("Инженер", 97, 18));
        _soldiers.Add(new Support("Поддержка", 110, 11));
        _soldiers.Add(new Sniper("Снайпер", 80, 20));
    }

    public int GetNumberOfClasses => _soldiers.Count;

    public Soldier GetSoldier(int index) => _soldiers[index].Clone();
}

class Battlefield
{
    private Country _country1;
    private Country _country2;
    private bool _firstPlayerTurn = true;

    public Battlefield()
    {
        _country1 = new Country("Британия");
        _country2 = new Country("Италия");
    }

    public void ShowCountrySquads()
    {
        _country1.AllySquad.ShowSoldiers();
        _country2.AllySquad.ShowSoldiers();
        Console.ReadKey();
    }

    public void ShowInfo() => Console.WriteLine
    (
        $"У первой страны осталось {_country1.AllySquad.GetNumberOfSoldiers} солдат\n" + 
        $"У второй страны осталось {_country2.AllySquad.GetNumberOfSoldiers} солдат\n"
    );
                

    public void RunAction()
    {
        ShowCountrySquads();

        while(_country1.IsSquadExist() && _country2.IsSquadExist())
        {
            if(_firstPlayerTurn)
            {
                Console.WriteLine("Ход за первой страной!");
                ShowInfo();
                _country1.AllySquad.AttackEnemies(_country2.AllySquad);
                _firstPlayerTurn = false;
            }
            else
            {
                Console.WriteLine("Ход за второй страной!");
                ShowInfo();
                _country2.AllySquad.AttackEnemies(_country1.AllySquad);
                _firstPlayerTurn = true;
            }

            _country1.AllySquad.RemoveDiedSoldiers();
            _country2.AllySquad.RemoveDiedSoldiers();

            Console.WriteLine();
            Console.ReadKey();
        }

        DetectResult();
    }

    public void DetectResult()
    {
        if(_country1.IsSquadExist())
            Console.WriteLine("Победа за первой страной!");
        else
            Console.WriteLine("Победа за второй страной!");
    }
}

static class UserUtils
{
    private static Random s_random;

    static UserUtils() => s_random = new Random();

    static public int GenerateRandomValue(int min, int max)
    {
        return s_random.Next(min, max);
    }
}