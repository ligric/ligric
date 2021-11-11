using BoardModels.AbstractBoardNotifications.Abstractions;
using BoardModels.BitZlato.Entities;

namespace BoardModels.BitZlato
{
    public class BitZlatoBoardWithTimer<T> : AbstractBoardWithTimerNotifications<T>
        where T : BitZlatoAdDto
    {
        public BitZlatoBoardWithTimer()
        {

        }
    }
}
