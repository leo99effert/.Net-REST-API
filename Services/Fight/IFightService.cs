namespace Services.Fight
{
  public interface IFightService
    {
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
        Task<ServiceResponse<List<HighScoreDto>>> ShowHighScore();
    }
}