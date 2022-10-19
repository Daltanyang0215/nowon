using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;

public class StateMachineBase<T> where T : Enum
{
    public bool isReady;
    public T currentType;
    public StateBase<T> current;
    protected Dictionary<T, StateBase<T>> states;

    public StateMachineBase()
    {
        InitStates();
    }

    public void ChangeStaet(T newType)
    {
        if (Enum.Equals(newType, currentType))
            return;

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
            Type ganeric = Type.GetType(typeName + "<>");
            Assembly assembly = Assembly.GetAssembly(ganeric);
            Type statetype = assembly.GetType($"{typeName}`1[{value}]");
            ConstructorInfo constructorInfo = statetype.GetConstructor(new Type[] { this.GetType(), typeof(T) });
            if(constructorInfo != null)
            {
                constructorInfo.Invoke(new object[] { this,value} );
            }
        }
        isReady = true;
    }


}
