using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Represents a single step in an action chain: either a callback or a delay.
    /// </summary>
    internal struct ChainNode<T>
    {
        public Action<T> Callback { get; }
        public float DelayDuration { get; }
        public bool IsDelay { get; }

        public ChainNode(Action<T> callback)
        {
            Callback = callback;
            DelayDuration = 0f;
            IsDelay = false;
        }

        public ChainNode(float delayDuration)
        {
            Callback = null;
            DelayDuration = delayDuration;
            IsDelay = true;
        }
    }

    /// <summary>
    /// Builds and runs a sequence of actions with optional delays.
    /// </summary>
    public sealed class ActionChain<T> where T : class
    {
        private readonly T _target;
        private readonly List<ChainNode<T>> _nodes = new();

        public ActionChain(T target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Adds an action to run immediately.
        /// </summary>
        public ActionChain<T> Then(Action<T> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            _nodes.Add(new ChainNode<T>(action));
            return this;
        }

        /// <summary>
        /// Adds a delay before the next action.
        /// </summary>
        public ActionChain<T> Wait(float seconds)
        {
            if (seconds > 0f)
                _nodes.Add(new ChainNode<T>(seconds));

            return this;
        }

        /// <summary>
        /// Starts executing the chain as a coroutine.
        /// </summary>
        /// <param name="coroutineTag">Optional tag for managing the coroutine.</param>
        public CoroutineHandle Run(string coroutineTag = null)
        {
            var handle = Timing.RunCoroutine(ExecutePipelineRoutine());

            if (!string.IsNullOrEmpty(coroutineTag))
                handle.Tag = coroutineTag;

            return handle;
        }

        private IEnumerator<float> ExecutePipelineRoutine()
        {
            foreach (var node in _nodes)
            {
                // Stop if the target is no longer valid.
                if (_target is Player p && !p.IsReady)
                    yield break;

                if (_target is null)
                    yield break;

                if (node.IsDelay)
                {
                    yield return Timing.WaitForSeconds(node.DelayDuration);
                }
                else
                {
                    node.Callback?.Invoke(_target);
                }
            }
        }
    }

    /// <summary>
    /// Creates a new action chain for the target.
    /// </summary>
    public static class ActionChainExtensions
    {
        public static ActionChain<T> CreateChain<T>(this T target) where T : class
            => new ActionChain<T>(target);
    }
}
