namespace IlmAI.Api.Models.Data.Entities;

using System;
using System.ComponentModel.DataAnnotations;

public class SystemLog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(20)]
    public string Level { get; set; } = "Info"; // Info, Warning, Error

    [Required]
    public string Message { get; set; } = string.Empty;

    public string? Source { get; set; } // Controller, Service, background job

    public string? Exception { get; set; }

    public string? UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
