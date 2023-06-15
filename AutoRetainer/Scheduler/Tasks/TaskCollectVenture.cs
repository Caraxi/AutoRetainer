﻿using AutoRetainer.Scheduler.Handlers;

namespace AutoRetainer.Scheduler.Tasks;

internal static class TaskCollectVenture
{
    internal static void Enqueue()
    {
        P.TaskManager.Enqueue(YesAlready.WaitForYesAlreadyDisabledTask);
        if (P.config.RetainerMenuDelay > 0)
        {
            TaskWaitSelectString.Enqueue(P.config.RetainerMenuDelay);
        }
        P.TaskManager.Enqueue(RetainerHandlers.SelectViewVentureReport);
        P.TaskManager.Enqueue(() => RetainerHandlers.EnforceSelectString(RetainerHandlers.SelectViewVentureReport));
        P.TaskManager.Enqueue(RetainerHandlers.ClickResultConfirm);
    }
}
