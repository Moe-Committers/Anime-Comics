namespace anime_comics.Models;

public class RefreshTokens : BaseEntity
{
    public long UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpireAt { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpireAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;
    public virtual Users User { get; set; }
}