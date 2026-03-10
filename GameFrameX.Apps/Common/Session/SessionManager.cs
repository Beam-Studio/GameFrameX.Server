// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//  
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//  
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//  
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//  
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GameFrameX.Apps.Common.Event;
using GameFrameX.Core.Actors;
using GameFrameX.Core.Events;
using GameFrameX.NetWork.Abstractions;
using GameFrameX.Utility.Setting;

namespace GameFrameX.Apps.Common.Session;

/// <summary>
/// Manages player sessions (one per player). Removed on logout; on displacement, the old channel is released and replaced.
/// </summary>
public static class SessionManager
{
    private static readonly ConcurrentDictionary<string, Session> SessionMap = new();

    /// <summary>
    /// Get the number of currently online players.
    /// </summary>
    /// <returns>The number of currently online players.</returns>
    public static int Count()
    {
        return SessionMap.Count;
    }

    /// <summary>
    /// Get a paginated list of player sessions.
    /// </summary>
    /// <param name="pageSize">Number of players per page.</param>
    /// <param name="pageIndex">Zero-based page index.</param>
    /// <returns>List of player sessions for the specified page.</returns>
    public static List<Session> GetPageList(int pageSize, int pageIndex)
    {
        var result = SessionMap.Values.OrderBy(m => m.CreateTime)
            .Where(m => ActorManager.HasActor(m.PlayerId))
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();
        return result;
    }

    /// <summary>
    /// Kick the player with the specified role ID and remove their session.
    /// </summary>
    /// <param name="roleId">The role ID of the player to kick.</param>
    public static void KickOffLineByUserId(long roleId)
    {
        var roleSession = Get(m => m.PlayerId == roleId);
        if (roleSession != null)
        {
            if (SessionMap.TryRemove(roleSession.SessionId, out var value) && ActorManager.HasActor(roleSession.PlayerId))
            {
                EventDispatcher.Dispatch(roleSession.PlayerId, (int)EventId.SessionRemove);
            }
        }
    }

    /// <summary>
    /// Get the session for the given role ID.
    /// Returns the session only if it already exists.
    /// </summary>
    /// <param name="roleId">Role ID.</param>
    /// <returns>The corresponding session, or null if not found.</returns>
    public static Session GetByRoleId(long roleId)
    {
        var roleSession = Get(m => m.PlayerId == roleId);
        if (roleSession != null && ActorManager.HasActor(roleSession.PlayerId))
        {
            return roleSession;
        }

        return roleSession;
    }

    /// <summary>
    /// Get a session by its session ID.
    /// </summary>
    /// <param name="sessionId">Session ID.</param>
    /// <returns>The corresponding session, or null if not found.</returns>
    public static Session Get(string sessionId)
    {
        SessionMap.TryGetValue(sessionId, out var value);
        return value;
    }

    /// <summary>
    /// Get a session matching the specified predicate.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>The matching session, or null if not found.</returns>
    public static Session Get(Func<Session, bool> predicate)
    {
        return SessionMap.Values.FirstOrDefault(predicate);
    }

    /// <summary>
    /// Get a list of sessions matching the specified predicate.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    /// <returns>List of matching sessions.</returns>
    public static List<Session> GetList(Func<Session, bool> predicate)
    {
        return SessionMap.Values.Where(predicate).ToList();
    }

    /// <summary>
    /// Remove the session with the specified session ID.
    /// </summary>
    /// <param name="sessionId">The session ID to remove.</param>
    /// <returns>The removed session, or null if not found.</returns>
    public static Session Remove(string sessionId)
    {
        if (SessionMap.TryRemove(sessionId, out var value) && ActorManager.HasActor(value.PlayerId))
        {
            EventDispatcher.Dispatch(value.PlayerId, (int)EventId.SessionRemove);
        }

        return value;
    }

    /// <summary>
    /// Remove all online player sessions.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task RemoveAll()
    {
        foreach (var session in SessionMap.Values)
        {
            if (ActorManager.HasActor(session.PlayerId))
            {
                EventDispatcher.Dispatch(session.PlayerId, (int)EventId.SessionRemove);
            }
        }

        SessionMap.Clear();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Get the network channel for the specified session ID.
    /// </summary>
    /// <param name="sessionId">Session ID.</param>
    /// <returns>The corresponding network channel, or null if not found.</returns>
    public static INetWorkChannel GetChannel(string sessionId)
    {
        SessionMap.TryGetValue(sessionId, out var session);
        return session?.WorkChannel;
    }

    /// <summary>
    /// Add a new session.
    /// </summary>
    /// <param name="session">The session to add.</param>
    public static void Add(Session session)
    {
        session.WorkChannel.SetData(GlobalConst.SessionIdKey, session.SessionId);
        SessionMap[session.SessionId] = session;
    }

    /// <summary>
    /// Update a session's role ID and sign.
    /// If the role ID is already logged in on another device, the old session is notified and its connection is closed.
    /// </summary>
    /// <param name="sessionId">Session ID identifying the current session</param>
    /// <param name="roleId">Role ID associated with the current session</param>
    /// <param name="sign">Sign used to verify session uniqueness</param>
    public static async void UpdateSession(string sessionId, long roleId, string sign)
    {
        // Get the old session associated with this role ID
        var oldSession = GetByRoleId(roleId);
        if (oldSession != null)
        {
            // Create a prompt message to notify the user that their account is logged in on another device
            var msg = new RespPrompt
            {
                Type = 5,
                Content = "Your account has been logged in on another device",
            };
            // Send message to the old session
            await oldSession.WriteAsync(msg);
            // Clear old session connection data and close the connection
            oldSession.WorkChannel.ClearData();
            oldSession.WorkChannel.Close();
            // Remove immediately; waiting for the Disconnected callback would introduce a delay
            Remove(oldSession.SessionId);
        }

        // Get the current session and update its role ID and sign
        var session = Get(sessionId);
        if (session.IsNull())
        {
            return;
        }
        session.SetPlayerId(roleId);
        session.SetSign(sign);
    }
}