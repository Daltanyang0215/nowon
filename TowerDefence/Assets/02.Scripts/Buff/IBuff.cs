using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff<T>
{
    void OnActive(T target);
    void OnDeaction(T target);
    void OnDuration(T target);

}
