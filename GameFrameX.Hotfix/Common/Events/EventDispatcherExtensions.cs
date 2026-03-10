using GameFrameX.Apps.Common.Event;
using GameFrameX.Core.Abstractions.Agent;
using GameFrameX.Core.Abstractions.Events;
using GameFrameX.Foundation.Extensions;
using GameFrameX.Hotfix.Logic.Server;

namespace GameFrameX.Hotfix.Common.Events;

public static class EventDispatcherExtensions
{
    /// <summary>
    /// Dispatch event
    /// </summary>
    /// <param name="agent">Agent object</param>
    /// <param name="eventId">Event ID</param>
    /// <param name="gameEventArgs">Event arguments, can be null</param>
    public static void Dispatch(this IComponentAgent agent, int eventId, GameEventArgs gameEventArgs = null)
    {
        // Handle locally
        SelfHandle(agent, eventId, gameEventArgs);

        if ((EventId)eventId > EventId.RoleSeparator && agent.OwnerType > GlobalConst.ActorTypeSeparator)
        {
            // Global non-player event, broadcast to all players
            agent.Tell(()
                           =>
                       {
                           return ServerComponentAgent.OnlineRoleForeach(role
                                                                             =>
                                                                         {
                                                                             role.Dispatch(eventId, gameEventArgs);
                                                                         });
                       });
        }
    }

    private static void SelfHandle(IComponentAgent agent, int evtId, GameEventArgs evt)
    {
        agent.Tell(async () =>
        {
            // Events must execute within this actor; multi-threaded execution is not allowed, so Task.WhenAll cannot be used
            var listeners = HotfixManager.FindListeners(agent.OwnerType, evtId);
            if (listeners.IsNullOrEmpty())
            {
                // Log.Warn($"Event: {(EventID)evtId} no listeners found");
                return;
            }

            foreach (var listener in listeners)
            {
                var comp = await agent.GetComponentAgent(listener.AgentType);
                try
                {
                    await listener.HandleEvent(comp, evt);
                }
                catch (Exception exception)
                {
                    LogHelper.Error(exception);
                }
            }
        });
    }

    /// <summary>
    /// Dispatch event
    /// </summary>
    /// <param name="agent">Agent object</param>
    /// <param name="eventId">Event ID</param>
    /// <param name="args">Event arguments</param>
    public static void Dispatch(this IComponentAgent agent, EventId eventId, GameEventArgs args = null)
    {
        Dispatch(agent, (int)eventId, args);
    }
}