using System.Collections.Generic;
using UnityEngine;
using ServiceLocator.Wave.Bloon;
using ServiceLocator.Player.Projectile;
using ServiceLocator.Sound;

namespace ServiceLocator.Player
{
    public class MonkeyController
    {
        private MonkeyView monkeyView;
        private MonkeyScriptableObject monkeyScriptableObject;
        private ProjectilePool projectilePool;
        private List<BloonController> bloonsInRange;

        private float attackTimer;

        public MonkeyController(MonkeyScriptableObject monkeyScriptableObject, ProjectilePool projectilePool)
        {
            this.monkeyScriptableObject = monkeyScriptableObject;
            this.projectilePool = projectilePool;
            bloonsInRange = new List<BloonController>();

            CreateMonkeyView();
            ResetAttackTimer();
        }

        public void UpdateMonkey()
        {
            if(bloonsInRange.Count > 0)
            {
                RotateTowardsTarget(bloonsInRange[0]);
                ShootAtTarget(bloonsInRange[0]);
            }
        }

        private void RotateTowardsTarget(BloonController bloonController)
        {
            Vector3 direction = bloonController.Position - monkeyView.transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 180;
            monkeyView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void ShootAtTarget(BloonController bloonController)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0f)
            {
                ProjectileController projectile = projectilePool.GetProjectile(monkeyScriptableObject.projectileType);
                projectile.SetPosition(monkeyView.transform.position);
                projectile.SetTarget(bloonController);
                SoundService.Instance.PlaySoundEffects(Sound.SoundType.MonkeyShoot);
                ResetAttackTimer();
            }
        }

        private void CreateMonkeyView()
        {
            monkeyView = Object.Instantiate(monkeyScriptableObject.Prefab);
            monkeyView.SetController(this);
            monkeyView.SetTriggerRadius(monkeyScriptableObject.Range);
        }

        public void BloonEnteredRange(BloonController bloon)
        {
            if(CanAttackBloon(bloon.GetBloonType()))
            {
                bloonsInRange.Add(bloon);
            }
        }

        public void BloonExitedRange(BloonController bloon)
        {
            if (CanAttackBloon(bloon.GetBloonType()))
            {
                bloonsInRange.Remove(bloon);
            }
        }

        public void SetPosition(Vector3 positionToSet) => monkeyView.transform.position = positionToSet;

        public bool CanAttackBloon(BloonType bloonType) => monkeyScriptableObject.AttackableBloons.Contains(bloonType);

        private void ResetAttackTimer() => attackTimer = monkeyScriptableObject.AttackRate;
    }
}