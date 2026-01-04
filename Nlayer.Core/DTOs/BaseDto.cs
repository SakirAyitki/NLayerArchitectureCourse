namespace Nlayer.Core.DTOs;

// DTO'lar bir entity'de client'e göndermek istemediğimiz property'leri filtrelediğimiz yerdir. 
public abstract class BaseDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
}