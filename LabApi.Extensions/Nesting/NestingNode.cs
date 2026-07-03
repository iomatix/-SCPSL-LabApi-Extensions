using System;

namespace LabApi.Extensions.Nesting
{

    /// <summary>
    /// A generic structural composition node that eliminates nesting boilerplate by dynamically binding a logic layer to its parent context.
    /// </summary>
    /// <typeparam name="TContext">The parent or execution framework context type (e.g., the primary Plugin class).</typeparam>
    /// <typeparam name="TLogic">The encapsulated subsystem or manager logic type.</typeparam>
    public sealed class NestingNode<TContext, TLogic>
    {
        /// <summary>
        /// Gets the encapsulated runtime logic instance context.
        /// </summary>
        public TLogic Logic { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestingNode{TContext, TLogic}"/> class.
        /// </summary>
        /// <param name="context">The parent framework context instance to bind.</param>
        /// <param name="logicFactory">The factory delegate used to instantiate the logic layer, injecting the context safely.</param>
        /// <exception cref="ArgumentNullException">Thrown if the context or logicFactory is null.</exception>
        public NestingNode(TContext context, Func<TContext, TLogic> logicFactory)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (logicFactory is null) throw new ArgumentNullException(nameof(logicFactory));

            // Execute the factory loop and bind the instance natively
            Logic = logicFactory.Invoke(context);
        }
    }
}