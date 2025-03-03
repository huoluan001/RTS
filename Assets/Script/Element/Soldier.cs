using System;
using Animancer;
using UnityEngine;

namespace Script.Element
{
    public class Soldier : Army
    {
        private SoldierFsmSystem _fsmSystem;

        [Header("Motion")] [SerializeField] private ClipTransition idleMotion;
        [SerializeField] private ClipTransition moveMotion;
        [SerializeField] private ClipTransition deathMotion;
        [SerializeField] private ClipTransition prostrateMotion; // 匍匐
        [SerializeField] private ClipTransition danglingMotion; // 悬空
        [SerializeField] private ClipTransition aimMotion;
        [SerializeField] private ClipTransition fireMotion;
        [SerializeField] private ClipTransition bulletLoadMotion;
        public Action 

        protected void Start()
        {
            currentWeapon = armySo.Weapons[0];
            _fsmSystem = new SoldierFsmSystem(armySo, this);
            MotionParameterUpdate();
            TargetChangedCallBack += _fsmSystem.CheckTransition;
        }

        public void Update()
        {
            // _fsmSystem.CurrentSoldierState.Run();
        }

        public void PlayIdleMotion() => animancer.Play(idleMotion);

        public void PlayMoveMotion() => animancer.Play(moveMotion);

        // public void PlayDeathMotion() => _mainMotionLayer.Play(deathMotion);
        // public void PlayProstrateMotion() => _mainMotionLayer.Play(prostrateMotion);
        // public void PlayDanglingMotion() => _mainMotionLayer.Play(danglingMotion);
        // public void PlayAimMotion() => _mainMotionLayer.Play(aimMotion);
        public void PlayFireMotion() => animancer.Play(fireMotion);

        public void PlayBulletLoadMotion() => animancer.Play(bulletLoadMotion);
        
        public void PlayBulletLoadAnimationInUpBody(Action callback)
        {
            bulletLoadMotion.Events.Clear();
            if (callback != null)
            {
                
            }
            bulletLoadMotion.Events.Add(0.99999f, callback);
            animancer.Play(bulletLoadMotion);
        }


        private void MotionParameterUpdate()
        {
            fireMotion.Speed = fireMotion.Length / currentWeapon.FiringDuration;
            bulletLoadMotion.Speed = bulletLoadMotion.Length / currentWeapon.MagazineLoadingTime;
            if (fireMotion.Events.Count == 0)
                fireMotion.Events.Add(0.999999f, FireCallback);
            if (bulletLoadMotion.Events.Count == 0)
            {
                bulletLoadMotion.Events.Add(0.999999f, BulletLoadCallback);
            }
        }
    }
}