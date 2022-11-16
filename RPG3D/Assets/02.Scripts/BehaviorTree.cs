using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    public enum Status
    {
        Success,
        Failure,
        Running
    }
    public class BegaviorTree
    {

    }
    public abstract class Behavior
    {
        public abstract Status Invoke(out Behavior leaf);
    }
    public class Root : Behavior
    {
        public Behavior child { get; private set; }
        public Behavior runningLeaf { get; private set; }
        private Status _tmpResult;

        public void SetChild(Behavior child) => this.child = child;
        public Status Invoke()
        {
            _tmpResult = Invoke(out Behavior leaf);
            if (_tmpResult == Status.Running)
                runningLeaf = leaf;
            else
                runningLeaf = null;
            return _tmpResult;
        }
        public override Status Invoke(out Behavior leaf)
        {
            return child.Invoke(out leaf);
        }
        public Status InvokeRunningLeaf()
        {
            _tmpResult = runningLeaf.Invoke(out Behavior leaf);
            if (_tmpResult == Status.Running)
                runningLeaf = leaf;
            else
                runningLeaf = null;
            return _tmpResult;
        }
    }
    public class Condition : Behavior
    {
        public Behavior child { get; private set; }
        private Func<bool> _condition;
        public Condition(Func<bool> condition)
        {
            _condition = condition;
        }
        public void SetConidtion(Func<bool> condition) => _condition = condition;

        public override Status Invoke(out Behavior leaf)
        {
            leaf = this;
            if (_condition.Invoke())
                return child.Invoke(out leaf);
            else
                return Status.Failure;
        }
    }
    public abstract class Composite : Behavior
    {
        protected List<Behavior> _children = new List<Behavior>();
        public IEnumerable<Behavior> Children => _children;
        public void AddChild(Behavior child) => _children.Add(child);
        public void RemoveChild(Behavior child) => _children.Remove(child);
        public void ClearChild() => _children.Clear();
    }
    public class Sequence : Composite
    {
        public Status _tmpResult;

        public override Status Invoke(out Behavior leaf)
        {
            leaf = this;
            foreach (Behavior child in _children)
            {
                _tmpResult = child.Invoke(out leaf);
                if(_tmpResult != Status.Success)
                {
                    leaf = child;
                    return _tmpResult;
                }
            }
            return Status.Success;
        }
    }
    public class Filter : Sequence
    {
        public void AddCondition(Condition condition) => _children.Insert(0, condition);
    }
    public class Selector : Composite
    {
        public Status _tmpResult;

        public override Status Invoke(out Behavior leaf)
        {
            leaf = this;
            foreach (Behavior child in _children)
            {
                _tmpResult = child.Invoke(out leaf);
                if (_tmpResult != Status.Failure)
                {
                    leaf = child;
                    return _tmpResult;
                }
            }
            return Status.Failure;
        }
    }
    public class Pararell : Composite
    {
        public enum Policy
        {
            RequoreOne,
            RequoreAll
        }
        private Policy _successPolicy;
        private Policy _failurePolicy;
        private Status _tmpResult;
        public Pararell(Policy successPolicy, Policy failurePolicy)
        {
            _successPolicy = successPolicy;
            _failurePolicy = failurePolicy;
        }

        public override Status Invoke(out Behavior leaf)
        {
            leaf = this;
            int successCount = 0;
            int failureCount = 0;

            foreach (Behavior child in _children)
            {
                _tmpResult = child.Invoke(out leaf);
                switch (_tmpResult)
                {
                    case Status.Success:
                        successCount++;
                        break;
                    case Status.Failure:
                        failureCount++;
                        break;
                    case Status.Running:
                        return _tmpResult;
                    default:
                        break;
                }
            }
            if(_successPolicy == Policy.RequoreOne && successCount > 0) 
                return Status.Success;
            if(_successPolicy == Policy.RequoreAll && successCount >= _children.Count)
                return Status.Success;
            if (_failurePolicy == Policy.RequoreOne && failureCount > 0)
                return Status.Failure;
            if(_failurePolicy == Policy.RequoreAll && failureCount >= _children.Count)
                return Status.Failure;

            throw new Exception("behavior tree pararell fail");

        }
    }
    public class Monitor : Pararell
    {
        public Monitor(Policy successPolicy, Policy failurePolicy) : base(successPolicy, failurePolicy)
        {
        }
    }
    public class Execution : Behavior
    {
        private Func<Status> _execute;
        public Execution(Func<Status> execute)
        {
            _execute = execute;
        }


        public void SetExecute(Func<Status> execute)
        {
            _execute = execute;
        }
        public override Status Invoke(out Behavior leaf)
        {
            leaf = this;
            return _execute.Invoke();
        }
    }
}
