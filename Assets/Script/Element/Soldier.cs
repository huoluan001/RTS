using System.Collections.Generic;

namespace Script.Element
{
    public class Soldier : Army
    {
        public ISoldierActionModel CurrentSoldierActionModel;

        private Dictionary<SoldierActionModel, ISoldierActionModel> _actionStateMachine =
            new Dictionary<SoldierActionModel, ISoldierActionModel>()
            {
                { SoldierActionModel.Idle, new IdleSoldierAction() },
                { SoldierActionModel.Move, new MoveSoldierAction() },
                { SoldierActionModel.Attack, new AttackSoldierAction() }
            };

        public void Start()
        {
            SwitchActionModel(SoldierActionModel.Idle);
        }

        public void SwitchActionModel(SoldierActionModel newActionModel)
        {
            CurrentSoldierActionModel = _actionStateMachine[newActionModel];
            CurrentSoldierActionModel.Init();
        }
    }
}