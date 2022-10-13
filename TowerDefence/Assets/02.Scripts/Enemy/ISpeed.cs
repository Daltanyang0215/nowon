using System;

public interface ISpeed
{
    
    float Speed { get; set; }
    float SpeedOrigin { get; }

    event Action<float> OnSpeedChanged;
}
