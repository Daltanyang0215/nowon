using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class StateMachineBase<T> where T : Enum
{
    public GameObject owner;
    public bool isReady;
    public T currentType;
    public IState<T> current;
    protected Dictionary<T, IState<T>> states;
    protected Dictionary<T, T> canExecuteCounditionMasks;
    protected Dictionary<T, T> transitionPairs;

    public StateMachineBase()
    {
        InitStates();
    }
    public StateMachineBase(GameObject owner, Dictionary<T, T> canExecuteConditionMasks, Dictionary<T, T> transitionPairs)
    {
        InitStates();
        this.owner = owner;
        this.canExecuteCounditionMasks = canExecuteConditionMasks;
        this.transitionPairs = transitionPairs;
    }

    public void ChangeStaet(T newType)
    {
        if (EqualityComparer<T>.Default.Equals(currentType, newType)) return;

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
        states = new Dictionary<T, IState<T>>();
        // 제네릭을 이름으로 클래스화 하는 법
        Array values = Enum.GetValues(typeof(T));
        foreach (T value in values)
        {
            string typeName = "State" + value.ToString();
            Debug.Log($"{typeName}<{typeof(T)}> // {value}");
            //Type ganeric = Type.GetType(typeName + "<>");
            //Assembly assembly = Assembly.GetAssembly(ganeric);
            Assembly stateTypeAssemble = typeof(T).Assembly;
            Type statetype = Type.GetType($"{typeName}`1[[{typeof(T)}{stateTypeAssemble}]]");
            ConstructorInfo constructorInfo = statetype.GetConstructor(new Type[] { typeof(StateMachineBase<T>),
                                                                                    typeof(T),
                                                                                    typeof(T),
                                                                                    typeof(T)});
            if (constructorInfo != null)
            {
                IState<T> state = constructorInfo.Invoke(new object[] { this,
                                                                        value,
                                                                        canExecuteCounditionMasks[value],
                                                                        transitionPairs[value]}) as IState<T>;
                states.Add(value, state);
            }
        }
        isReady = true;
    }


}
