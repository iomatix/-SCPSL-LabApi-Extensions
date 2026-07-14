using System;

namespace LabApi.Extensions.Nesting
{
    /// <summary>
    /// A generic structural node designed to bind a logic layer to its parent context.
    /// </summary>
    /// <typeparam name="TContext">The parent context type (e.g., the primary Plugin class).</typeparam>
    /// <typeparam name="TLogic">The encapsulated subsystem or manager logic type.</typeparam>
    [Obsolete("NestingNode is deprecated and violates the KISS principle. Instantiating your managers or subsystems directly (e.g., 'public MyManager Manager { get; }') is the recommended standard. This struct is kept as a zero-allocation bridge for backward compatibility.", false)]
    public readonly struct NestingNode<TContext, TLogic>
    {
        /// <summary>
        /// Gets the encapsulated runtime logic instance.
        /// </summary>
        public TLogic Logic { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestingNode{TContext, TLogic}"/> struct safely.
        /// </summary>
        /// <param name="context">The parent framework context instance to bind.</param>
        /// <param name="logicFactory">The factory delegate used to instantiate the logic layer, injecting the context.</param>
        public NestingNode(TContext context, Func<TContext, TLogic> logicFactory)
        {
            // FIX: Unified null-checking standard (== null instead of is null).
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (logicFactory == null)
                throw new ArgumentNullException(nameof(logicFactory));

            // Execute the factory loop and bind the instance directly on the stack
            Logic = logicFactory.Invoke(context);
        }
    }
}