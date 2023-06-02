using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Spawner
{
    public class EnemyPlacer
    {
        private readonly Circle _circle = new();
        public void PlaceEnemies(GameObject[] enemies, GroupingMode mode, Vector2 playerPoint)
        {
            var radius = _circle.GetRadiusInscribedAroundTheCamera();
            switch (mode)
            {
                case GroupingMode.Default:
                    PlaceDefault(enemies, radius, playerPoint);
                    break;
                case GroupingMode.Surround:
                    PlaceRing(enemies, radius, playerPoint);
                    break;
                case GroupingMode.Group:
                    PlaceGroup(enemies, radius, playerPoint);
                    break;
            }
        }
        private void PlaceDefault(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
        {
            foreach (var enemy in enemies)
            {
                var randomAng = _circle.GetRandomAngle();
                enemy.transform.position = _circle.GetPointOn(radius, playerPoint, randomAng);
            }
        }
        private void PlaceRing(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
        {
            var enemiesArray = enemies as GameObject[] ?? enemies.ToArray();
            var angleStep = (float)Math.PI * 2f / enemiesArray.Length;
            var nextAngle = _circle.GetRandomAngle();

            foreach (var enemy in enemiesArray)
            {
                enemy.transform.position = _circle.GetPointOn(radius, playerPoint, nextAngle);
                nextAngle += angleStep;
            }
        }
        private void PlaceGroup(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
        {
            var enemiesArray = enemies as GameObject[] ?? enemies.ToArray();
            var padding = enemiesArray.Sum(enemy => enemy.GetComponent<CircleCollider2D>().radius) / 2;
            var packCentre = _circle.GetPointOn(radius + padding, playerPoint, _circle.GetRandomAngle());

            foreach (var enemy in enemiesArray)
            {
                var v2 = new Vector2(Random.value, Random.value);
                enemy.transform.position = Random.value switch
                {
                    > 0.5f => packCentre - v2,
                    _ => packCentre + v2
                };
            }
        }
    }
}