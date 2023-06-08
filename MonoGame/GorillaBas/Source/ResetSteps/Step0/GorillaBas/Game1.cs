using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GorillaBas.GameCode;
using GorillaBas.MonoGameHelpers;
using Microsoft.Xna.Framework.Audio;

namespace GorillaBas
{
	public partial class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private GameSettings GameSettings;
		private GameFunctions GameFunctions;
		private LoadedContent LoadedContent;
		private BuildingPainter BuildingPainter;
		private SpriteFont Font;
		private Pixel Pixel;

		private GorillaData Player1;
		private GorillaData Player2;
		private (GorillaData CurrentPlayer, GorillaData NextPlayer) Players;

		private float Gravity = 9.8f; // meters per second squared.
		private bool Firing = false;
		private ExplosionData Explosion;
		private BananaData Banana;
		private List<(int X, int Y)> previousBananas;

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Pixel = new Pixel(GraphicsDevice);

			Font = Content.Load<SpriteFont>("fonts/Monoisome-Regular");
			LoadedContent = new LoadedContent();
			LoadedContent.ExplosionImage = Content.Load<Texture2D>("sprites/explosion");
			LoadedContent.GorillaImage = Content.Load<Texture2D>("sprites/gorilla");
			LoadedContent.BananaImage = Content.Load<Texture2D>("sprites/bigbanana");
			LoadedContent.GuideArrow = Content.Load<Texture2D>("sprites/guidearrow");
			LoadedContent.Gorillas = GameFunctions.CreateGorillas(GameSettings);
			LoadedContent.ExplosionSound = Content.Load<SoundEffect>("sounds/explosion");
			LoadedContent.FireSound = Content.Load<SoundEffect>("sounds/fire");
			LoadedContent.GorillaSound = Content.Load<SoundEffect>("sounds/gorilla");
			Player1 = LoadedContent.Gorillas.LeftGorilla;
			Player2 = LoadedContent.Gorillas.RightGorilla;
			Players = (Player1, Player2);

			NewPlayfield();
		}

		protected override void Update(GameTime gameTime)
		{
			// Close if ESC is pressed.
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (!Firing && !Explosion.Active)
			{
				var inputState = GameFunctions.ReadInput();

				if (inputState.LeftArrowHeld) Players.CurrentPlayer.Angle -= 1;
				if (inputState.RightArrowHeld) Players.CurrentPlayer.Angle += 1;
				if (inputState.UpArrowHeld) Players.CurrentPlayer.Velocity += 1;
				if (inputState.DownArrowHeld) Players.CurrentPlayer.Velocity -= 1;

				if (inputState.SpacePressed)
					FireBanana();
			}
			else if (Firing && !Explosion.Active)
			{
				Banana.ApplyGravity();

				// If the banana goes out of bounds, end the turn.  Don't let it fall for eternity!
				bool bananaIsInBounds =
				(
					Banana.Area.Top < GameSettings.ScreenSize.Height
					&& Banana.Area.Left > 0
					&& Banana.Area.Left < GameSettings.ScreenSize.Width
				);

				if (!bananaIsInBounds)
				{
					Firing = false;
					NextTurn();
				}

				if (GameSettings.Debug)
					if (previousBananas.Count < 10000)
						previousBananas.Add(((int)Banana.Position.X, (int)Banana.Position.Y));

				CheckForImpact(gameTime);
			}

			if (Explosion.Active)
				Explosion.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			BuildingPainter.Draw();
			DrawGorillas();

			if (Firing)
			{
				DrawBanana();
				DrawBananaGuide();
			}

			if (Explosion.Active)
			{
				DrawExplosion();
			}

			DrawText();
			DrawDebugText();
			spriteBatch.End();

			base.Draw(gameTime);
		}

		private void DrawBananaGuide()
		{
			if (Banana.Position.Y < 0)
			{
				var image = LoadedContent.GuideArrow;
				var size = GameSettings.GuideArrowSize;
				Rectangle destRect = new Rectangle((int)Banana.Position.X - size / 2, 0, size, size);
				spriteBatch.Draw(image, destRect, Color.White);
			}
		}

		private void DrawGorillas()
		{
			if (GameSettings.Debug)
			{
				spriteBatch.Draw(Pixel.OfColor(Color.BlueViolet), LoadedContent.Gorillas.LeftGorilla.Area, Color.White);
				spriteBatch.Draw(Pixel.OfColor(Color.BlueViolet), LoadedContent.Gorillas.RightGorilla.Area, Color.White);
			}

			spriteBatch.Draw(LoadedContent.GorillaImage, LoadedContent.Gorillas.LeftGorilla.Area, Color.White);
			spriteBatch.Draw(LoadedContent.GorillaImage, LoadedContent.Gorillas.RightGorilla.Area, Color.White);
		}

		private void FireBanana()
		{
			previousBananas.Clear();
			Firing = true;
			(int X, int y) position = (Players.CurrentPlayer.Area.Center.X, Players.CurrentPlayer.Area.Top);

			Banana = new BananaData(
				position,
				GameSettings.BananaSize,
				Players.CurrentPlayer.Angle,
				Players.CurrentPlayer.DirectionModifier,
				Players.CurrentPlayer.Velocity,
				Gravity
			);
			LoadedContent.FireSound.Play();
		}

		private void CheckForImpact(GameTime gameTime)
		{
			// Did the banana touch the opposing gorilla?
			bool isOverlappedWithGorilla = GameFunctions.BananaCollidedWith(Banana.Area, Players.NextPlayer.Area);
			bool isOverlappedWithBuilding = LoadedContent.Buildings.Any(
				building => GameFunctions.BananaCollidedWith(Banana.Area, building.Area));

			bool isOverlapped = isOverlappedWithGorilla || isOverlappedWithBuilding;
			if (isOverlapped)
			{
				Firing = false;

				if (isOverlappedWithGorilla)
				{
					// The player has scored by hitting their opponent.
					// When finished exploding, create a new playfield.
					Explosion.AfterExplosion = () => NewPlayfield();
					LoadedContent.GorillaSound.Play();
				}
				Explosion.Activate(gameTime);
				LoadedContent.ExplosionSound.Play();


				if (isOverlappedWithGorilla)
				{
					Players.CurrentPlayer.Score++;
					if (Players.CurrentPlayer.Score == GameSettings.MaxScore)
					{
						// GAME OVER!
					}
				}
				NextTurn();
			}
		}
	}
}
