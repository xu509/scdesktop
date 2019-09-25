using System.Collections.Generic;
/// <summary>
///     数据服务接口
/// </summary>

namespace scdesktop
{
    public interface IDaoService
    {
        BallData GetItem();

        void prepareData();

    }
}