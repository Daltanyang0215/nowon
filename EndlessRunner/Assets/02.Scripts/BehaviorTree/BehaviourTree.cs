using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BT
{
    

    public abstract class BehaviorTree
    {
        public virtual RootNode root { get; set; }
        public abstract void Init();
        public abstract ReturnType Tick();

        public enum ReturnType
        {
            Success,
            Failure,
            OnRunning
        }

        public abstract class Node
        {
            public abstract ReturnType Invoke();
        }

        public class RootNode : Node
        {
            public Node child;

            public override ReturnType Invoke()
            {
                return child.Invoke();
            }

            public void SetChild(Node child) => this.child = child;
        }

        public class Execution : Node
        {
            public Func<ReturnType> function;
            public Execution(Func<ReturnType> function) => this.function = function;

            public override ReturnType Invoke()
            {
                return function.Invoke();
            }
        }

        public abstract class CompositeNode : Node
        {
            public List<Node> children;
            public void Addchild(Node child)
            {
                children.Add(child);
            }

            public IEnumerable<Node> GetChildren() => children;
        }

        public class Selector : CompositeNode
        {
            private ReturnType _tmpResult;
            public override ReturnType Invoke()
            {
                foreach (var child in GetChildren())
                {
                    _tmpResult = child.Invoke();
                    if (_tmpResult != ReturnType.Failure)
                    {
                        return _tmpResult;
                    }
                }
                return ReturnType.Failure;
            }
        }
        public class RandomSelector : CompositeNode
        {
            private ReturnType _tmpResult;
            public override ReturnType Invoke()
            {
                foreach (var child in GetChildren().OrderBy(node => Guid.NewGuid()))
                {
                    _tmpResult = child.Invoke();
                    if (_tmpResult != ReturnType.Failure)
                    {
                        return _tmpResult;
                    }
                }
                return ReturnType.Failure;
            }
        }

        public class Sequence : CompositeNode
        {
            private ReturnType _tmpResult;
            public override ReturnType Invoke()
            {
                foreach (var child in GetChildren())
                {
                    _tmpResult = child.Invoke();
                    if (_tmpResult != ReturnType.Success)
                    {
                        return _tmpResult;
                    }
                }
                return ReturnType.Success;
            }
        }

        public class ConditionNode : Node
        {
            public Node child;
            public event Func<bool> condition;

            public ConditionNode(Func<bool> condition) => this.condition = condition;
            public void SetChild(Node child) => this.child = child;

            public override ReturnType Invoke()
            {
                if (condition.Invoke())
                {
                    return child.Invoke();
                }
                return ReturnType.Failure;
            }
        }

        public abstract class Decorator : Node
        {
            public Node child;
            public void SetChild(Node child) => this.child = child;
            public override ReturnType Invoke()
            {
                return Decorate(child.Invoke());
            }
            protected abstract ReturnType Decorate(ReturnType childreturn);
        }

        public class Repeater : Decorator
        {
            public event Func<bool> condition;
            protected override ReturnType Decorate(ReturnType childreturn)
            {
                if (condition.Invoke())
                {
                    return ReturnType.OnRunning;
                }
                return childreturn;
            }
        }
        public class Inverter : Decorator
        {
            protected override ReturnType Decorate(ReturnType childreturn)
            {
                switch (childreturn)
                {
                    case ReturnType.Success:
                        return ReturnType.Failure;
                    case ReturnType.Failure:
                        return ReturnType.Success;
                    case ReturnType.OnRunning:
                        return ReturnType.OnRunning;
                    default:
                        throw new Exception("inverter : not retrun type");
                }
            }
        }
    }
    
}