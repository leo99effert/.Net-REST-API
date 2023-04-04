namespace Services.Fight
{
  public class FightService : IFightService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public FightService(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }
    public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
    {
      var response = new ServiceResponse<FightResultDto>();
      response.Data = new FightResultDto();
      try
      {
        // Get characters
        var characters = await _context.Characters
          .Include(c => c.Weapon)
          .Include(c => c.Skills)
          .Where(c => request.CharacterIds.Contains(c.Id))
          .ToListAsync();
        // Enter fight loop
        bool characterIsDefeated = false;
        while (characterIsDefeated is false)
        {
          foreach (var attacker in characters)
          {
            // Pick opponent
            var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
            var opponent = opponents[new Random().Next(opponents.Count)];
            // Prepare attack
            int damage = 0;
            string attackUsed = string.Empty;
            // Pick weapon (1) or skill (0)
            bool useWeapon = new Random().Next(2) == 0;
            // Attack with weapon
            if (useWeapon && attacker.Weapon is not null)
            {
              attackUsed = attacker.Weapon.Name;
              damage = WeaponAttack(attacker, opponent);
            }
            // Attack with skill
            else if (useWeapon is false && attacker.Skills is not null && attacker.Skills.Count > 0)
            {
              var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
              attackUsed = skill.Name;
              damage = SkillAttack(attacker, opponent, skill);
            }
            // After trying to use non existing weapon or skill
            else
            {
              response.Data.Log.Add($"...................................................({attacker.Name} wasnt able to attack...)");
              continue;
            }
            // Write log
            response.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} || {attackUsed}: {(damage >= 0 ? damage : 0)} damage");
            // handle defeat
            if (opponent.HitPoints <= 0)
            {
              // HighScore updates
              attacker.Victories++;
              opponent.Defeats++;
              // Write logs
              response.Message = "Battle log";
              response.Data.Log.Add($"{attacker.Name} has defeated {opponent.Name}.");
              response.Data.Log.Add("HitPoints Left...");
              characters.ForEach(c => { response.Data.Log.Add($"{c.Name}: {(c.HitPoints > 0 ? c.HitPoints : 0)}"); });
              // After game stats change
              attacker.Strength++;          // Bonus for winning
              attacker.Intelligence++;      // Bonus for winning
              opponent.Strength += 2;       // To help catch up
              opponent.Intelligence += 2;   // To help catch up
              opponent.Defense += 5;        // To help catch up
              // End Loop
              characterIsDefeated = true;
              break;
            }
          }
        }
        characters.ForEach(c =>
        {
          c.Fights++;
          c.HitPoints = 100;
        });
        // Save
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }
    public async Task<ServiceResponse<List<HighScoreDto>>> ShowHighScore()
    {
      var response = new ServiceResponse<List<HighScoreDto>>();
      try
      {
        // Get characters
        var characters = await _context.Characters
          .OrderByDescending(c => c.Victories)
          .ThenBy(c => c.Defeats)
          .ToListAsync();
        // Create response
        response.Message = "High Score List";
        response.Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList();
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      }
      return response;
    }

    private static int WeaponAttack(Models.Character attacker, Models.Character opponent)
    {
      // Get damage
      int damage = (new Random().Next(attacker.Weapon!.Damage)) + (new Random().Next(attacker.Strength));
      // Use defense
      damage -= new Random().Next(opponent.Defense);
      // Reduce hitpoints
      if (damage > 0) opponent.HitPoints -= damage;
      // Alter stats
      if(attacker.Weapon.Damage > 1) attacker.Weapon.Damage--;
      if(opponent.Intelligence > 1) opponent.Intelligence--;
      if(attacker.Intelligence > 1) attacker.Intelligence--;
      if(opponent.Strength > 1) opponent.Strength--;
      if(attacker.Defense > 1) attacker.Defense--;
      attacker.Strength++;
      opponent.Defense += 2;

      return damage;
    }
    private static int SkillAttack(Models.Character attacker, Models.Character opponent, Skill skill)
    {
      // Get damage
      int damage =  (new Random().Next(skill.Damage)) + (new Random().Next(attacker.Intelligence));
      // Use defense
      damage -= (new Random().Next(opponent.Defense)) / 2;
      // Reduce hitpoints
      if (damage > 0) opponent.HitPoints -= damage;
      // Alter stats
      if(opponent.Defense > 1) opponent.Defense--;
      attacker.Intelligence++;
      opponent.Intelligence++;

      return damage;
    }
  }
}