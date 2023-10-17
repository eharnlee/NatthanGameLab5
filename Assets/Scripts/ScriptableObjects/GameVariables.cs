using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameVariables : ScriptableObject
{
    // lives
    public int maxLives;

    // Mario's movement
    public int speed;
    public int maxSpeed;
    public int upSpeed;
    public int deathImpulse;
    public int stompImpulse;
    public Vector3 marioStartingPosition;

    public string currentLevel;

    // Goomba's movement
    public float goombaPatrolTime;
    public float goombaMaxOffset;
}