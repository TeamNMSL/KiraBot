using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Mirai_CSharp.Example
{
    public partial class ExamplePlugin : IFriendMessage, // 你想处理什么事件就实现什么事件对应的接口
                                 IGroupMessage,
                                 INewFriendApply,
                                 IGroupApply,
                                 IBotInvitedJoinGroup,
                                 IDisconnected, ITempMessage
    {
        
    }

}
