﻿using System.Threading.Tasks;

namespace kmd.Activation
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    internal abstract class ActivationHandler
    {
        public abstract bool CanHandle(object args);

        public abstract Task HandleAsync(object args);
    }

    internal abstract class ActivationHandler<T> : ActivationHandler
        where T : class
    {
        public override bool CanHandle(object args)
        {
            return args is T && CanHandleInternal(args as T);
        }

        public override async Task HandleAsync(object args)
        {
            await HandleInternalAsync(args as T);
        }

        protected virtual bool CanHandleInternal(T args)
        {
            return true;
        }

        protected abstract Task HandleInternalAsync(T args);
    }
}
