using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public enum Enum_ReturnStatus
    {
        成功=0,
        失败
    }

    public enum Enum_UserRole
    {
        管理员=0,
        普通会员,
    }

    public enum Enum_UserSex
    {
        
        男=1,
        女=2,
        保密=0,
    }

    public enum Enum_UserHobby
    {
        运动 = 1,
        阅读,
        饮食,
        旅游,
        音乐,
        饮茶,
        影视,
        服饰
    }

    public enum Enum_VideoStatus
    {
        正常=1,
        违规
    }

    public enum Enum_VideoType
    {
        视频=1,
        音乐
    }

}
