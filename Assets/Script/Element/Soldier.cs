using System;
using Animancer;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

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
        private AnimancerLayer _mainLayer;
        private AnimancerLayer _upBodyLayer;
        public AvatarMask upBodyMask;

        private void Start()
        {
            currentWeapon = armySo.Weapons[0];
            MotionParameterUpdate();
            _fsmSystem = new SoldierFsmSystem(armySo, this);
            

            
            TargetChangedCallBack += _fsmSystem.CheckTransition;
        }

        public void Update()
        {
            // _fsmSystem.CurrentSoldierState.Run();
        }

        public void PlayIdleMotion()
        {
            _upBodyLayer.SetWeight(0f);
            _mainLayer.Play(idleMotion);
        }

        public void PlayMoveMotion() => _mainLayer.Play(moveMotion);

        // public void PlayDeathMotion() => _mainMotionLayer.Play(deathMotion);
        // public void PlayProstrateMotion() => _mainMotionLayer.Play(prostrateMotion);
        // public void PlayDanglingMotion() => _mainMotionLayer.Play(danglingMotion);
        // public void PlayAimMotion() => _mainMotionLayer.Play(aimMotion);
        public void PlayFireMotion()
        {
            _upBodyLayer.SetWeight(0f);
            var state = _mainLayer.Play(fireMotion);
            state.Time = 0;
        }

        public void PlayBulletLoadMotion()
        {
            _upBodyLayer.SetWeight(1f);
            var state = _upBodyLayer.Play(bulletLoadMotion);
            state.Time = 0;
        }


        private void MotionParameterUpdate()
        {
            fireMotion.Speed = fireMotion.Length / currentWeapon.FiringDuration;
            bulletLoadMotion.Speed = bulletLoadMotion.Length / currentWeapon.MagazineLoadingTime;
            _mainLayer = animancer.Layers[0];
            _upBodyLayer = animancer.Layers[1];
            _upBodyLayer.SetMask(upBodyMask);
            _upBodyLayer.SetWeight(0f);
        }

        public void FireMotionAddCallBack(Action callback)
        {
            fireMotion.Events.Add(MOTION_END_TIME, callback);
        }

        public void BulletLoadMotionAddCallBack(Action callback)
        {
            bulletLoadMotion.Events.Add(MOTION_END_TIME, callback);
        }
    }
}