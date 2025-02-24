namespace FoxStevenle.API.Enums;

public enum GuessResult
{
    Success,
    Skip,       // Technically not needed, but it's here for frontend compatibility + could be useful if accounts get implemented
    Fail
}