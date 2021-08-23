using Common.DtoTypes.Board;
using ViewModelEngine;

namespace ViewModelBoardAbstraction.Types
{
    public class BoardPresenter : BasePropertyChanged
    {
        private AdBoardReadOnlyStruct _adBoard;
        private double _positionX, _positionY;

        public AdBoardReadOnlyStruct AdBoard { get => _adBoard; set => SetProperty(ref _adBoard, value); }
        public double PositionX { get => _positionX; set => SetProperty(ref _positionX, value); }
        public double PositionY { get => _positionY; set => SetProperty(ref _positionY, value); }

        public BoardPresenter(AdBoardReadOnlyStruct adBoard, double positionX, double positionY)
        {
            AdBoard = adBoard; PositionX = positionX; PositionY = positionY;
        }
    }
}
