using Otter;

using OtterTutorial.Entities;

using System;
using System.IO;

namespace OtterTutorial.Scenes
{
    public class GameScene : Scene
    {
        public Music gameMusic = new Music(Assets.MUSIC_GAME);

        // Our Tilemap's calculated width and height
        public const int WIDTH = Global.GAME_WIDTH * 3;
        public const int HEIGHT = Global.GAME_HEIGHT * 2;
        public const float HALF_SCRENE_X = Global.GAME_WIDTH / 2;
        public const float HALF_SCRENE_Y = Global.GAME_HEIGHT / 2;
        public Tilemap tilemap = null;
        public GridCollider grid = null;

        // Our new constructor takes in the new J,I coordinates, and a Player object
        public GameScene(Player player = null)
            : base()
        {
            // If a Player object isn't passed in, start at the default x,y position of 100,100
            if (player == null)
            {
                Global.player = new Player(100, 100);
            }
            else
            {
                Global.player = player;
            }
            // Create and load our Tilemap and GridCollider
            tilemap = new Tilemap(Assets.TILESET, WIDTH, HEIGHT, Global.GRID_WIDTH, Global.GRID_HEIGHT);
            grid = new GridCollider(WIDTH, HEIGHT, Global.GRID_WIDTH, Global.GRID_HEIGHT);

            string mapToLoad = Assets.MAP_WORLD;
            string solidsToLoad = Assets.MAP_SOLID;
            LoadWorld(mapToLoad, solidsToLoad);
            // Since we are constantly switching Scenes we need to do some checking,
            // ensuring that the music doesn't get restarted.
            // We should probably add an isPlaying boolean to the Music class. I will do this soon.
            if (Global.gameMusic == null)
            {
                Global.gameMusic = new Music(Assets.MUSIC_GAME);
                Global.gameMusic.Play();
                Global.gameMusic.Volume = 0.40f;
            }
        }

        // We now add our Entities and Graphics once the Scene has been switched to
        public override void Begin()
        {
            Entity gridEntity = new Entity(0, 0, null, grid);
            Add(gridEntity);
            AddGraphic(tilemap);
            // Ensure that the player is not null
            if (Global.player != null)
            {
                Add(Global.player);
                // Never should be paused once transitioning is complete
                Global.paused = false;
            }
            Add(Global.camShaker);
            // This is rather crude, as we re-add the Enemy every time we switch screens
            // A good task beyond these tutorials would be ensuring that non-player
            // Entities retain their state upon switching screens
            Add(new Enemy(500, 400));
            Add(new Enemy(800, 400));
            Add(new Enemy(500, 800));
        }

        private void LoadWorld(string map, string solids)
        {
            // Get our CSV map in string format and load it via our tilemap
            string newMap = CSVToString(map);
            tilemap.LoadCSV(newMap);

            // Get our csv solid map and load it into our GridCollider
            string newSolids = CSVToString(solids);
            grid.LoadCSV(newSolids);
        }

        // Add this method to your GameScene.cs class
        private static string CSVToString(string csvMap)
        {
            string ourMap = "";
            using (var reader = new StreamReader(csvMap))
            {
                // Read each line, adding a line-break to the end of each
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ourMap += line;
                    ourMap += "\n";
                }
            }

            return ourMap;
        }

        public override void Update()
        {
            if (Global.paused)
            {
                return;
            }
            this.CameraX = Global.player.X - HALF_SCRENE_X;
            this.CameraY = Global.player.Y - HALF_SCRENE_Y;
        }
    }
}