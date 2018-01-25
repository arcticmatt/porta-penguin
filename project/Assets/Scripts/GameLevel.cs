using UnityEngine;
﻿using System;
using System.Linq;

namespace NSPoop
{
	public class GameLevel
	{
		public float[] scrollSpeeds;
		public SpawnTime[] spawnTimes;

		public GameLevel (float[] _scrollSpeeds, SpawnTime[] _spawnTimes)
		{
			scrollSpeeds = _scrollSpeeds;
			spawnTimes = _spawnTimes;
		}
	}

	public static class GameLevelMaker
	{
		private static float noSpawnTime = 99999f;

		private static bool initialized = false;
		private static int numLevels = 5;
		private static GameLevel[] levels;
		private static GameLevel level0;
		private static GameLevel level1;
		private static GameLevel level2;
		private static int[] levelMarks = { 0, 10, 20, 30, 40 };
		// private static int[] levelMarks = { 0, 1, 2, 3, 4 };
		private static SpawnTime[] characterSpawnTimes;
		private static SpawnTime[] obstacleSpawnTimes;
		private static SpawnTime[] powerSpawnTimes;

		public static void InitLevels ()
		{
			if (initialized) {
				return;
			}
			initialized = true;

			// Put all these here for ease of editing. Scroll speeds are easier to
			// edit so I left them in the invidiual level creation methods.
			characterSpawnTimes = new SpawnTime[numLevels];
			obstacleSpawnTimes = new SpawnTime[numLevels];
			powerSpawnTimes = new SpawnTime[numLevels];

			characterSpawnTimes[0] = new SpawnTime (1f, 3f);
			characterSpawnTimes[1] = new SpawnTime (1f, 3f);
			characterSpawnTimes[2] = new SpawnTime (1f, 3f);
			characterSpawnTimes[3] = new SpawnTime (1f, 3f);
			characterSpawnTimes[4] = new SpawnTime (1f, 2.5f);

			obstacleSpawnTimes[0] = new SpawnTime (noSpawnTime, noSpawnTime);
			obstacleSpawnTimes[1] = new SpawnTime (3f, 6f);
			obstacleSpawnTimes[2] = new SpawnTime (2f, 5f);
			obstacleSpawnTimes[3] = new SpawnTime (1f, 4f);
			obstacleSpawnTimes[4] = new SpawnTime (0.5f, 3f);

			powerSpawnTimes[0] = new SpawnTime (noSpawnTime, noSpawnTime);
			powerSpawnTimes[1] = new SpawnTime (5f, 15f);
			powerSpawnTimes[2] = new SpawnTime (5f, 15f);
			powerSpawnTimes[3] = new SpawnTime (5f, 15f);
			powerSpawnTimes[4] = new SpawnTime (5f, 15f);

			levels = new GameLevel[numLevels];
			levels [0] = Level0 ();
			levels [1] = Level1 ();
			levels [2] = Level2 ();
			levels [3] = Level3 ();
			levels [4] = Level3 ();
		}

		public static GameLevel Level0 ()
		{
			GameLevel level = EmptyLevel ();
			level.scrollSpeeds [(int)ObjectType.Character] = level.scrollSpeeds [(int)ObjectType.Obstacle] = level.scrollSpeeds [(int)ObjectType.Power] = -3f;
			level.scrollSpeeds [(int)ObjectType.Background] = -1f;

			level.spawnTimes [(int)ObjectType.Character] = characterSpawnTimes[0];
			level.spawnTimes [(int)ObjectType.Obstacle] = obstacleSpawnTimes[0];
			level.spawnTimes [(int)ObjectType.Power] = powerSpawnTimes[0];

			return level;
		}

		public static GameLevel Level1 ()
		{
			GameLevel level = EmptyLevel ();
			// Same as level0
			level.scrollSpeeds [(int)ObjectType.Character] = level.scrollSpeeds [(int)ObjectType.Obstacle] = level.scrollSpeeds [(int)ObjectType.Power] = -3f;
			level.scrollSpeeds [(int)ObjectType.Background] = -1f;

			level.spawnTimes [(int)ObjectType.Character] = characterSpawnTimes[1];
			level.spawnTimes [(int)ObjectType.Obstacle] = obstacleSpawnTimes[1];
			level.spawnTimes [(int)ObjectType.Power] = powerSpawnTimes[1];

			return level;
		}

		public static GameLevel Level2 ()
		{
			GameLevel level = EmptyLevel ();
			// Same as level0
			level.scrollSpeeds [(int)ObjectType.Character] = level.scrollSpeeds [(int)ObjectType.Obstacle] = level.scrollSpeeds [(int)ObjectType.Power] = -3f;
			level.scrollSpeeds [(int)ObjectType.Background] = -1f;

			level.spawnTimes [(int)ObjectType.Character] = characterSpawnTimes[2];
			level.spawnTimes [(int)ObjectType.Obstacle] = obstacleSpawnTimes[2];
			level.spawnTimes [(int)ObjectType.Power] = powerSpawnTimes[2];

			return level;
		}

		public static GameLevel Level3 ()
		{
			GameLevel level = EmptyLevel ();
			// Same as level0
			level.scrollSpeeds [(int)ObjectType.Character] = level.scrollSpeeds [(int)ObjectType.Obstacle] = level.scrollSpeeds [(int)ObjectType.Power] = -3.5f;
			level.scrollSpeeds [(int)ObjectType.Background] = -1f;

			level.spawnTimes [(int)ObjectType.Character] = characterSpawnTimes[3];
			level.spawnTimes [(int)ObjectType.Obstacle] = obstacleSpawnTimes[3];
			level.spawnTimes [(int)ObjectType.Power] = powerSpawnTimes[3];

			return level;
		}

		public static GameLevel Level4 ()
		{
			GameLevel level = EmptyLevel ();
			// Same as level0
			level.scrollSpeeds [(int)ObjectType.Character] = level.scrollSpeeds [(int)ObjectType.Obstacle] = level.scrollSpeeds [(int)ObjectType.Power] = -4f;
			level.scrollSpeeds [(int)ObjectType.Background] = -1f;

			level.spawnTimes [(int)ObjectType.Character] = characterSpawnTimes[4];
			level.spawnTimes [(int)ObjectType.Obstacle] = obstacleSpawnTimes[4];
			level.spawnTimes [(int)ObjectType.Power] = powerSpawnTimes[4];

			return level;
		}

		public static GameLevel GetLevel (int score)
		{
			for (int i = 0; i < numLevels - 1; i++) {
				if (score >= levelMarks [i] && score < levelMarks [i + 1]) {
					return levels [i];
				}
			}
			return levels [numLevels - 1];
		}

		private static GameLevel EmptyLevel ()
		{
			int numObjects = System.Enum.GetNames (typeof(ObjectType)).Length;
			float[] scrollSpeeds = new float[numObjects];
			SpawnTime[] spawnTimes = new SpawnTime[numObjects];

			return new GameLevel (scrollSpeeds, spawnTimes);
		}
	}
}
