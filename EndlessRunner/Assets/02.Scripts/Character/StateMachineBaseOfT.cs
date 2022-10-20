using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachineBase<T> where T : Enum
{

    public GameObject owner;
    public bool isReady;
    public T currentType;
    public IState<T> current;
    protected Dictionary<T, IState<T>> states;

    public StateMachineBase()
    {
        InitStates();
    }
    public StateMachineBase(GameObject owner)
    {
        InitStates();
        this.owner = owner;
    }

    public void ChangeStaet(T newType)
    {
        if(EqualityComparer<T>.Default.Equals(currentType, newType)) return;

        if (states[newType].canExecute)
        {
            current.Reset();
            states[newType].Execute();
            current = states[newType];
            currentType = newType;
        }
    }

    public void Update()
    {
        if (isReady)
            ChangeStaet(current.Update());
    }

    private void InitStates()
    {
        // 제네릭을 이름으로 클래스화 하는 법
        Array values = Enum.GetValues(typeof(T));
        foreach (T value in values)
        {
            string typeName = "State" + value.ToString();
            //Type ganeric = Type.GetType(typeName + "<>");
            //Assembly assembly = Assembly.GetAssembly(ganeric);
            Assembly stateTypeAssemble = typeof(T).Assembly;
            Type statetype = Type.GetType($"{typeName}`1[{typeof(T)}{stateTypeAssemble}]");
            ConstructorInfo constructorInfo = statetype.GetConstructor(new Type[] { this.GetType(), typeof(T) });
            if (constructorInfo != null)
            {
                constructorInfo.Invoke(new object[] { this, value });
            }
        }
        isReady = true;
    }


}
