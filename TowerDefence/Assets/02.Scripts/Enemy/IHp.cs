using System;

public interface IHp
{
    int HP { get; set; }

    event Action<int> OnHPChanged;
}
