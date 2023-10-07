using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace chess
{
	public enum PieceColor
	{
		white = 0,
		black
	}

	public class Move
	{
		public Vector2 position { get; set; }
		public bool isCapture { get; set; }

		public int isCastle { get; set; }

		public Move(Vector2 position, Board board, PieceColor color)
		{
			this.position = position;
			isCapture = false;
			Piece testPiece = board.GetPiece(position);
			if (testPiece != null && testPiece.color != color)
			{
				isCapture = true;
			}

		}
	}
	public class Piece
	{
		public string type { get; set; }
		public Vector2 position { get; set; }
		bool hasMoved = false;

		public PieceColor color;

		public Piece(string _type, Vector2 _position, PieceColor _color)
		{
			type = _type;
			position = _position;
			color = _color;
		}

		public List<Move> GetMoves(Board board)
		{
			List<Move> moves = new List<Move>();
			int direction = color == PieceColor.white ? -1 : 1;
			switch (type)
			{
				default: moves.Add(new Move(position, board, color)); break;
				case "Pawn":
					if (board.GetPiece(position + new Vector2(1, direction)) != null)
					{
						moves.Add(new Move(position + new Vector2(1, direction), board, color));
					}
					else if (board.GetPiece(position + new Vector2(-1, direction)) != null)
					{
						moves.Add(new Move(position + new Vector2(-1, direction), board, color));
					}
					if (board.GetPiece(position + (Vector2.UnitY * direction)) != null)
					{
						break;
					}
					if (!hasMoved)
					{

						moves.Add(new Move(position + (Vector2.UnitY * 2 * direction), board, color));
					}
					else
					{
						moves.Add(new Move(position + (Vector2.UnitY * direction), board, color));
					}


					break;
				case "Rook":

					for (int x = (int)position.X + 1; x < 8; x++)
					{
						Vector2 newPosition = new Vector2(x, position.Y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}
					for (int x = (int)position.X - 1; x >= 0; x--)
					{
						Vector2 newPosition = new Vector2(x, position.Y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}

					for (int y = (int)position.Y + 1; y < 8; y++)
					{
						Vector2 newPosition = new Vector2(position.X, y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}
					for (int y = (int)position.Y - 1; y >= 0; y--)
					{
						Vector2 newPosition = new Vector2(position.X, y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}
					break;
				case "Knight":
					moves.Add(new Move(position + new Vector2(2, 1), board, color));
					moves.Add(new Move(position + new Vector2(-2, 1), board, color));
					moves.Add(new Move(position + new Vector2(-2, -1), board, color));
					moves.Add(new Move(position + new Vector2(2, -1), board, color));
					moves.Add(new Move(position + new Vector2(1, 2), board, color));
					moves.Add(new Move(position + new Vector2(-1, 2), board, color));
					moves.Add(new Move(position + new Vector2(-1, -2), board, color));
					moves.Add(new Move(position + new Vector2(1, -2), board, color));
					break;
				case "Bishop":
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X + i, position.Y + i);
						if (newPosition.X < 8 || newPosition.Y < 8)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X - i, position.Y + i);
						if (newPosition.X > 0 || newPosition.Y < 8)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X - i, position.Y - i);
						if (newPosition.X > 0 || newPosition.Y > 0)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X + i, position.Y - i);
						if (newPosition.X < 8 || newPosition.Y > 0)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					break;
				case "Queen":
					for (int x = (int)position.X + 1; x < 8; x++)
					{
						Vector2 newPosition = new Vector2(x, position.Y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}
					for (int x = (int)position.X - 1; x >= 0; x--)
					{
						Vector2 newPosition = new Vector2(x, position.Y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}

					for (int y = (int)position.Y + 1; y < 8; y++)
					{
						Vector2 newPosition = new Vector2(position.X, y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}
					for (int y = (int)position.Y - 1; y >= 0; y--)
					{
						Vector2 newPosition = new Vector2(position.X, y);
						Piece piece = board.GetPiece(newPosition);
						if (piece != null)
						{
							if (piece.color != color) moves.Add(new Move(newPosition, board, color));
							break;
						}
						moves.Add(new Move(newPosition, board, color));
					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X + i, position.Y + i);
						if (newPosition.X < 8 || newPosition.Y < 8)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X - i, position.Y + i);
						if (newPosition.X > 0 || newPosition.Y < 8)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X - i, position.Y - i);
						if (newPosition.X > 0 || newPosition.Y > 0)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 newPosition = new Vector2(position.X + i, position.Y - i);
						if (newPosition.X < 8 || newPosition.Y > 0)
						{
							Piece piece = board.GetPiece(newPosition);
							if (piece != null && piece != this)
							{
								if (piece.color != color) moves.Add(new Move(newPosition, board, color));
								break;
							}
							moves.Add(new Move(newPosition, board, color));
						}

					}
					break;
				case "King":
					for (int y = -1; y <= 1; y++)
					{
						for (int x = -1; x <= 1; x++)
						{
							Vector2 newPosition = new Vector2(position.X + x, position.Y + y);
							if (newPosition.X >= 8 || newPosition.Y >= 8 || newPosition.X < 0 || newPosition.Y < 0) continue;
							if (board.GetPiece(newPosition) == null)
								moves.Add(new Move(newPosition, board, color));
							else
							{
								if (board.GetPiece(newPosition).color != color) moves.Add(new Move(newPosition, board, color));
							}
						}
					}
					if (!hasMoved)
					{
						Move move = new Move(position + new Vector2(2, 0), board, color);
						move.isCastle = 1;
						if(board.GetPiece(position + new Vector2(1, 0)) == null)
							moves.Add(move);
						move = new Move(position + new Vector2(-2, 0), board, color);
						move.isCastle = -1;
						if (board.GetPiece(position + new Vector2(-1, 0)) == null && board.GetPiece(position + new Vector2(-3, 0)) == null)
							moves.Add(move);
					}
					break;
			}
			return moves;
		}

		public void Move(Vector2 newPosition)
		{
			position = newPosition;
			if (!hasMoved) hasMoved = true;
		}
	}

	public class Board
	{
		Dictionary<string, Texture2D> whitePieceTextures = new Dictionary<string, Texture2D>();
		Dictionary<string, Texture2D> blackPieceTextures = new Dictionary<string, Texture2D>();
		Texture2D lightTile;
		Texture2D darkTile;
		public List<Piece> whitePieces = new List<Piece>(16);
		public List<Piece> blackPieces = new List<Piece>(16);
		public Board(Game1 game)
		{
			#region textureLoading
			whitePieceTextures["Bishop"] = game.Content.Load<Texture2D>("Pieces/White/Cropped/Bishop");
			whitePieceTextures["King"] = game.Content.Load<Texture2D>("Pieces/White/Cropped/King");
			whitePieceTextures["Knight"] = game.Content.Load<Texture2D>("Pieces/White/Cropped/Knight");
			whitePieceTextures["Pawn"] = game.Content.Load<Texture2D>("Pieces/White/Cropped/Pawn");
			whitePieceTextures["Queen"] = game.Content.Load<Texture2D>("Pieces/White/Cropped/Queen");
			whitePieceTextures["Rook"] = game.Content.Load<Texture2D>("Pieces/White/Cropped/Rook");

			blackPieceTextures["Bishop"] = game.Content.Load<Texture2D>("Pieces/Black/Cropped/Bishop");
			blackPieceTextures["King"] = game.Content.Load<Texture2D>("Pieces/Black/Cropped/King");
			blackPieceTextures["Knight"] = game.Content.Load<Texture2D>("Pieces/Black/Cropped/Knight");
			blackPieceTextures["Pawn"] = game.Content.Load<Texture2D>("Pieces/Black/Cropped/Pawn");
			blackPieceTextures["Queen"] = game.Content.Load<Texture2D>("Pieces/Black/Cropped/Queen");
			blackPieceTextures["Rook"] = game.Content.Load<Texture2D>("Pieces/Black/Cropped/Rook");

			lightTile = game.Content.Load<Texture2D>("Tiles/Cropped/Light");
			darkTile = game.Content.Load<Texture2D>("Tiles/Cropped/Dark");
			#endregion
			#region blackPieceInitialization
			blackPieces.Add(new Piece("Rook", new Vector2(0, 0), PieceColor.black));
			blackPieces.Add(new Piece("Knight", new Vector2(1, 0), PieceColor.black));
			blackPieces.Add(new Piece("Bishop", new Vector2(2, 0), PieceColor.black));
			blackPieces.Add(new Piece("Queen", new Vector2(3, 0), PieceColor.black));
			blackPieces.Add(new Piece("King", new Vector2(4, 0), PieceColor.black));
			blackPieces.Add(new Piece("Bishop", new Vector2(5, 0), PieceColor.black));
			blackPieces.Add(new Piece("Knight", new Vector2(6, 0), PieceColor.black));
			blackPieces.Add(new Piece("Rook", new Vector2(7, 0), PieceColor.black));

			blackPieces.Add(new Piece("Pawn", new Vector2(0, 1), PieceColor.black));
			blackPieces.Add(new Piece("Pawn", new Vector2(1, 1), PieceColor.black));
			blackPieces.Add(new Piece("Pawn", new Vector2(2, 1), PieceColor.black));
			blackPieces.Add(new Piece("Pawn", new Vector2(3, 1), PieceColor.black));
			blackPieces.Add(new Piece("Pawn", new Vector2(4, 1), PieceColor.black));
			blackPieces.Add(new Piece("Pawn", new Vector2(5, 1), PieceColor.black));
			blackPieces.Add(new Piece("Pawn", new Vector2(6, 1), PieceColor.black));
			blackPieces.Add(new Piece("Pawn", new Vector2(7, 1), PieceColor.black));
			#endregion
			#region whitePieceInitialization

			whitePieces.Add(new Piece("Pawn", new Vector2(0, 6), PieceColor.white));
			whitePieces.Add(new Piece("Pawn", new Vector2(1, 6), PieceColor.white));
			whitePieces.Add(new Piece("Pawn", new Vector2(2, 6), PieceColor.white));
			whitePieces.Add(new Piece("Pawn", new Vector2(3, 6), PieceColor.white));
			whitePieces.Add(new Piece("Pawn", new Vector2(4, 6), PieceColor.white));
			whitePieces.Add(new Piece("Pawn", new Vector2(5, 6), PieceColor.white));
			whitePieces.Add(new Piece("Pawn", new Vector2(6, 6), PieceColor.white));
			whitePieces.Add(new Piece("Pawn", new Vector2(7, 6), PieceColor.white));

			whitePieces.Add(new Piece("Rook", new Vector2(0, 7), PieceColor.white));
			whitePieces.Add(new Piece("Knight", new Vector2(1, 7), PieceColor.white));
			whitePieces.Add(new Piece("Bishop", new Vector2(2, 7), PieceColor.white));
			whitePieces.Add(new Piece("Queen", new Vector2(3, 7), PieceColor.white));
			whitePieces.Add(new Piece("King", new Vector2(4, 7), PieceColor.white));
			whitePieces.Add(new Piece("Bishop", new Vector2(5, 7), PieceColor.white));
			whitePieces.Add(new Piece("Knight", new Vector2(6, 7), PieceColor.white));
			whitePieces.Add(new Piece("Rook", new Vector2(7, 7), PieceColor.white));
			#endregion

			//blackPieces.Add(new Piece("King", new Vector2(1, 3), PieceColor.black));
		}

		public void Draw(Game1 game, GameTime gameTime, SpriteBatch _spriteBatch, List<Move> tileHighlight)
		{
			#region boardRender

			_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, game.boardTransform);
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					Color tileColor = Color.White;
					foreach (Move move in tileHighlight)
					{
						if (move.position == new Vector2(x, y))
						{
							if (move.isCapture) tileColor = Color.Red;
							else tileColor = Color.Green;
						}
					}
					int currentTile = x + (y * 7);
					if (currentTile % 2 == 0) _spriteBatch.Draw(lightTile, new Vector2(x * game.tileSize, y * game.tileSize), tileColor);
					else _spriteBatch.Draw(darkTile, new Vector2(x * game.tileSize, y * game.tileSize), tileColor);
				}
			}
			_spriteBatch.End();
			#endregion
			#region pieceRender
			_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, game.pieceTransform);
			foreach (Piece piece in whitePieces)
			{
				_spriteBatch.Draw(whitePieceTextures[piece.type], piece.position * game.tileSize, Color.White);
			}
			foreach (Piece piece in blackPieces)
			{
				_spriteBatch.Draw(blackPieceTextures[piece.type], piece.position * game.tileSize, Color.White);
			}
			_spriteBatch.End();
			#endregion
		}

		public Piece GetPiece(Vector2 position)
		{
			foreach (Piece piece in whitePieces)
			{
				if (piece.position == position) return piece;
			}
			foreach (Piece piece in blackPieces)
			{
				if (piece.position == position) return piece;
			}
			return null;
		}
	}

	public partial class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		public int tileSize = 64;
		public int boardSize = 512;

		Board board;

		public Matrix boardTransform;
		public Matrix pieceTransform;

		List<Move> tileHighlight = new List<Move>();
		int selectedPiece = -1;

		PieceColor turn = PieceColor.white;
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			boardSize = tileSize * 8;
			_graphics.PreferredBackBufferWidth = boardSize;
			_graphics.PreferredBackBufferHeight = boardSize + tileSize;

			boardTransform = Matrix.CreateTranslation(0, tileSize, 0);
			pieceTransform = Matrix.CreateTranslation(0, tileSize / 3, 0);
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			board = new Board(this);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{

				Vector2 gridPosition = new Vector2((int)(Mouse.GetState().X / tileSize), (int)((Mouse.GetState().Y - tileSize) / tileSize));

				//Debug.WriteLine(gridPosition);

				Piece pieceAtMouse = board.GetPiece(gridPosition);


				if (pieceAtMouse != null && pieceAtMouse.color == turn)
				{
					tileHighlight = pieceAtMouse.GetMoves(board);
					if (pieceAtMouse.color == turn)
					{
						if (turn == PieceColor.white)
							selectedPiece = board.whitePieces.IndexOf(pieceAtMouse);
						else
							selectedPiece = board.blackPieces.IndexOf(pieceAtMouse);
						Debug.WriteLine(pieceAtMouse.type);
					}
					else selectedPiece = -1;
				}
				else
				{
					Debug.WriteLine("Moving");

					Debug.WriteLine(selectedPiece);
					if (selectedPiece != -1)
					{

						foreach (Move move in tileHighlight)
						{
							if (move.position == gridPosition)
							{

								if (turn == PieceColor.white)
								{
									if (move.isCapture) board.blackPieces.Remove(board.GetPiece(gridPosition));
									board.whitePieces[selectedPiece].Move(move.position);
									if (move.isCastle != 0)
									{
										if(move.isCastle == 1)
										{
											Piece rook = board.GetPiece(new Vector2(7, 7));
											rook.Move(board.whitePieces[selectedPiece].position - (Vector2.UnitX * move.isCastle));
										}
										else
										{
											Piece rook = board.GetPiece(new Vector2(0, 7));
											rook.Move(board.whitePieces[selectedPiece].position - (Vector2.UnitX * move.isCastle));
										}
									}
									turn = PieceColor.black;
								}
								else
								{
									if (move.isCapture) board.whitePieces.Remove(board.GetPiece(gridPosition));
									board.blackPieces[selectedPiece].Move(move.position);
									turn = PieceColor.white;
								}

								break;
							}
						}
						selectedPiece = -1;
						tileHighlight.Clear();
					}
				}
			}
			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.DarkBlue);

			// TODO: Add your drawing code here
			board.Draw(this, gameTime, _spriteBatch, tileHighlight);

			base.Draw(gameTime);
		}
	}
}