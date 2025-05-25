using Milking.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Milking.Domain.ValueObjects;

public record MilkVolume(
    [Range(0, double.MaxValue, ErrorMessage = "Volume cannot be negative.")]
    double Amount,
    VolumeUnit Unit);
