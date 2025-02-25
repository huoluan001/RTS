using Animancer;
using UnityEngine;

namespace Script.Element
{
    public class Soldier : Army
    {
        private SoldierFsmSystem _fsmSystem;

        [Header("Motion")] 
        public ClipTransition idleMotion;
        public ClipTransition moveMotion;
        
        public ClipTransition aimMotion;
        public ClipTransition fireMotion;
        public ClipTransition bulletLoadMotion;
        public ClipTransition deathMotion;
        

        protected void Start()
        {
            animancer = GetComponent<AnimancerComponent>();
            currentWeapon = armySo.Weapons[0];
            
            MoveTarget = null;
            _fsmSystem = new SoldierFsmSystem(armySo, this);
            
        }

        public void Update()
        {
            _fsmSystem.CheckTransition();
            _fsmSystem.CurrentSoldierState.Run();
        }

        public bool AttackTargetInRange()
        {
            var distance = Vector3.Distance(transform.position, attackTarget.transform.position);
            return distance < currentWeapon.Range;
        }

        public void AddAttackTarget(Unit target)
        {
            attackTarget = target;
            MoveTarget = null;
        }

        public void AddMoveTarget(Vector3 target)
        {
            MoveTarget = target;
            attackTarget = null;
        }
    }
}