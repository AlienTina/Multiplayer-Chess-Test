using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

using WebSocketSharp;
using WebSocketSharp.Server;
using System.Reflection.Emit;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using System.IO;
using System.Net;
using Microsoft.Xna.Framework.Audio;

namespace chess
{
	
	public enum PieceColor
	{
		white = 0,
		black
	}

	public enum GameState
	{
		menu = 0,
		playing,
		waitingForMatch,
		playingMultiplayer,
		end
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
						if (board.GetPiece(position + new Vector2(1, 0)) == null)
						{
							if (!board.whiteSide)
							{
								if (board.GetPiece(position + new Vector2(3, 0)) == null) moves.Add(move);
							}
							else moves.Add(move);
						}
						move = new Move(position + new Vector2(-2, 0), board, color);
						move.isCastle = -1;
						if (board.GetPiece(position + new Vector2(-1, 0)) == null)
						{
							if (board.whiteSide)
							{
								if(board.GetPiece(position + new Vector2(-3, 0)) == null) moves.Add(move);
							}
							else moves.Add(move);
						}
					}
					break;
			}
			return moves;
		}
		public void Move(Vector2 newPosition, MultiplayerManager manager, Game1 game)
		{
			if (manager != null && manager.secondPlayer.IsAlive)
			{
				string message = $"{7 - position.X},{7 - position.Y} {7 - newPosition.X},{7 - newPosition.Y}";
				manager.secondPlayer.Send(message);
			}
			position = newPosition;
			int direction = color == PieceColor.white ? -1 : 1;
			if (type == "Pawn")
			{
				if (color == PieceColor.white)
				{
					if (newPosition.Y == 0) type = "Queen";
				}
				if (color == PieceColor.black)
				{
					if (newPosition.Y == 7) type = "Queen";
				}
			}
			if (!hasMoved) hasMoved = true;
			game.moveSound.Play();
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

		public bool whiteSide {  get; set; }
		public Board(Game1 game, bool whiteSide)
		{
			this.whiteSide = whiteSide;
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
			if (whiteSide)
			{
				blackPieces.Add(new Piece("Queen", new Vector2(3, 0), PieceColor.black));
				blackPieces.Add(new Piece("King", new Vector2(4, 0), PieceColor.black));
			}
			else 
			{
				blackPieces.Add(new Piece("Queen", new Vector2(4, 0), PieceColor.black));
				blackPieces.Add(new Piece("King", new Vector2(3, 0), PieceColor.black));
			}
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
			if (whiteSide)
			{
				whitePieces.Add(new Piece("Queen", new Vector2(3, 7), PieceColor.white));
				whitePieces.Add(new Piece("King", new Vector2(4, 7), PieceColor.white));
			}
			else
			{
				whitePieces.Add(new Piece("Queen", new Vector2(4, 7), PieceColor.white));
				whitePieces.Add(new Piece("King", new Vector2(3, 7), PieceColor.white));
			}
			whitePieces.Add(new Piece("Bishop", new Vector2(5, 7), PieceColor.white));
			whitePieces.Add(new Piece("Knight", new Vector2(6, 7), PieceColor.white));
			whitePieces.Add(new Piece("Rook", new Vector2(7, 7), PieceColor.white));
			#endregion


			/*blackPieces.Add(new Piece("King", new Vector2(2, 4), PieceColor.black));
			whitePieces.Add(new Piece("King", new Vector2(2, 4), PieceColor.white));*/
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
			_spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, game.pieceTransform);
			foreach (Piece piece in blackPieces)
			{
				_spriteBatch.Draw(blackPieceTextures[piece.type], piece.position * game.tileSize, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, piece.position.Y / 7.0f);
			}
			foreach (Piece piece in whitePieces)
			{
				_spriteBatch.Draw(whitePieceTextures[piece.type], piece.position * game.tileSize, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, piece.position.Y / 7.0f);

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
		public Piece GetPiece(Vector2 position, PieceColor color)
		{
			if (color == PieceColor.white)
			{
				foreach (Piece piece in whitePieces)
				{
					if (piece.position == position) return piece;
				}
			}
			if (color == PieceColor.black)
			{
				foreach (Piece piece in blackPieces)
				{
					if (piece.position == position) return piece;
				}
			}
			return null;
		}
	}
	public class Match : WebSocketBehavior
	{
		private Game1 _instance;
		private MultiplayerManager manager;
		public Match(Game1 gameInstance, MultiplayerManager manager)
		{
			_instance = gameInstance;
			this.manager = manager;
		}

		protected override void OnOpen()
		{
			manager.secondPlayer = Context.WebSocket;
			_instance.turn = PieceColor.white;
			_instance.board = new Board(_instance, true);
			_instance.state = GameState.playingMultiplayer;
			base.OnOpen();
		}

		protected override void OnMessage(MessageEventArgs e)
		{
			if (e.Data == "gg")
			{
				_instance.endScreenText = "You Lose!";
				_instance.state = GameState.end;
				manager.secondPlayer.Close();
				return;
			}
			Vector2 piece;
			Vector2 move;
			string pieceData = e.Data.Split(' ')[0];
			string moveData = e.Data.Split(' ')[1];
			piece = new Vector2((float)Convert.ToDouble(pieceData.Split(',')[0]), (float)Convert.ToDouble(pieceData.Split(',')[1]));
			move = new Vector2((float)Convert.ToDouble(moveData.Split(',')[0]), (float)Convert.ToDouble(moveData.Split(',')[1]));

			Debug.WriteLine($"piece: {piece}, move {move}");
			if (_instance.board.GetPiece(move, PieceColor.white) != null) _instance.board.whitePieces.Remove(_instance.board.GetPiece(move, PieceColor.white));
			_instance.board.GetPiece(piece, PieceColor.black).Move(move, null, _instance);
			_instance.turn = PieceColor.white;
			base.OnMessage(e);
		}

		protected override void OnClose(CloseEventArgs e)
		{
			if (_instance.state != GameState.end)
				_instance.state = GameState.menu;
			Debug.WriteLine("Lost Connection.");
			base.OnClose(e);
		}
	}
	public class MultiplayerManager
	{
		public WebSocket secondPlayer { get; set; }
		public WebSocketServer me { get; set; }
		public bool isHost { get; set; }
		public Game1 game;
		public MultiplayerManager(bool isHost, Game1 game, string ip)
		{
			this.isHost = isHost;
			this.game = game;
			if (!isHost)
			{
				secondPlayer = new WebSocket($"ws://{ip}:12345/match");
				secondPlayer.OnMessage += OnMessage;
				secondPlayer.OnOpen += game.OnOpen;
				secondPlayer.OnClose += OnClose;
				secondPlayer.Connect();
				
			}
			else
			{

				me = new WebSocketServer($"ws://0.0.0.0:12345");

				me.AddWebSocketService<Match>("/match", () => new Match(game, this));

				me.Start();
			}
		}

		private void OnClose(object sender, CloseEventArgs e)
		{
			Debug.WriteLine("Lost Connection.");
			if(game.state != GameState.end)
				game.state = GameState.menu;
		}

		private void OnMessage(object sender, MessageEventArgs e)
		{
			if(e.Data == "gg")
			{
				game.endScreenText = "You Lose!";
				game.state = GameState.end;
				secondPlayer.Close();
			}
			Vector2 piece;
			Vector2 move;
			string pieceData = e.Data.Split(' ')[0];
			string moveData = e.Data.Split(' ')[1];
			piece = new Vector2((float)Convert.ToDouble(pieceData.Split(',')[0]), (float)Convert.ToDouble(pieceData.Split(',')[1]));
			move = new Vector2((float)Convert.ToDouble(moveData.Split(',')[0]), (float)Convert.ToDouble(moveData.Split(',')[1]));

			Debug.WriteLine($"piece: {piece}, move {move}");
			if (game.board.GetPiece(move, PieceColor.white) != null) game.board.whitePieces.Remove(game.board.GetPiece(move, PieceColor.white));
			game.board.GetPiece(piece, PieceColor.black).Move(move, null, game);
			game.turn = PieceColor.white;
		}

		
	}

	public partial class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		public int tileSize = 64;
		public int boardSize = 512;

		public Board board;

		public Matrix boardTransform;
		public Matrix pieceTransform;

		List<Move> tileHighlight = new List<Move>();
		int selectedPiece = -1;

		public PieceColor turn = PieceColor.white;

		public GameState state = GameState.menu;

		Texture2D menuTexture;

		MultiplayerManager multiplayer;

		public SoundEffect moveSound;

		public static string ShowDialog(string text, string caption)
		{
			Form prompt = new Form()
			{
				Width = 500,
				Height = 150,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};
			Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
			TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
			Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
			confirmation.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(textBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(textLabel);
			prompt.AcceptButton = confirmation;

			return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
		}

		SpriteFont font;

		public string endScreenText = "";

		public void OnOpen(object sender, EventArgs e)
		{
			turn = PieceColor.black;
			board = new Board(this, false);
			state = GameState.playingMultiplayer;
			Debug.WriteLine("Connected.");
		}
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
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			menuTexture = Content.Load<Texture2D>("Placeholders/menu");
			font = Content.Load<SpriteFont>("defaultfont");
			moveSound = Content.Load<SoundEffect>("move");

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (state == GameState.playing || state == GameState.playingMultiplayer)
			{
				if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				{

					Vector2 gridPosition = new Vector2((int)(Mouse.GetState().X / tileSize), (int)((Mouse.GetState().Y - tileSize) / tileSize));

					//Debug.WriteLine(gridPosition);

					Piece pieceAtMouse = board.GetPiece(gridPosition, turn);


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
										if (move.isCapture)
										{
											Piece removePiece = board.GetPiece(gridPosition, PieceColor.black);
											board.blackPieces.Remove(removePiece);
											if (removePiece.type == "King")
											{
												endScreenText = "You Win!";
												state = GameState.end;
												multiplayer.secondPlayer.Send("gg");
												multiplayer.secondPlayer.Close();
											}
										}
										board.whitePieces[selectedPiece].Move(move.position, multiplayer, this);
										if (move.isCastle != 0)
										{
											if (move.isCastle == 1)
											{
												
												Piece rook = board.GetPiece(new Vector2(7, 7), PieceColor.white);
												rook.Move(board.whitePieces[selectedPiece].position - (Vector2.UnitX * move.isCastle), multiplayer, this);
											}
											else
											{
												Piece rook = board.GetPiece(new Vector2(0, 7), PieceColor.white);
												rook.Move(board.whitePieces[selectedPiece].position - (Vector2.UnitX * move.isCastle), multiplayer, this);
											}
										}
										turn = PieceColor.black;
									}
									else if(state == GameState.playing)
									{
										if (move.isCapture) board.whitePieces.Remove(board.GetPiece(gridPosition));
										board.blackPieces[selectedPiece].Move(move.position, multiplayer, this);
										if (move.isCastle != 0)
										{
											if (move.isCastle == 1)
											{
												Piece rook = board.GetPiece(new Vector2(7, 0));
												rook.Move(board.blackPieces[selectedPiece].position - (Vector2.UnitX * move.isCastle), multiplayer, this);
											}
											else
											{
												Piece rook = board.GetPiece(new Vector2(0, 0));
												rook.Move(board.blackPieces[selectedPiece].position - (Vector2.UnitX * move.isCastle), multiplayer, this);
											}
										}
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
			}
			else if (state == GameState.menu)
			{
				if(multiplayer != null)
				{
					if(multiplayer.me != null)
						multiplayer.me.Stop();
					multiplayer = null;
				}
				if (Keyboard.GetState().IsKeyDown(Keys.Enter))
				{
					state = GameState.playing;
					board = new Board(this, true);
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.Back))
				{
					state = GameState.waitingForMatch;
					
					multiplayer = new MultiplayerManager(true, this, "");
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.Tab))
				{
					state = GameState.waitingForMatch;
					string ip = ShowDialog("Enter IP of host", "IP");
					Debug.WriteLine($"'{ip}'");
					if(ip == "") Exit();
					else multiplayer = new MultiplayerManager(false, this, ip);
				}
			}
			else if(state == GameState.end)
			{
				if (Keyboard.GetState().IsKeyDown(Keys.Enter))
				{
					state = GameState.menu;
				}
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (turn == PieceColor.black)
				GraphicsDevice.Clear(Color.DarkBlue);
			else if(turn == PieceColor.white)
				GraphicsDevice.Clear(Color.LightGray);

			//Debug.WriteLine(state);
			if (state == GameState.playing || state == GameState.playingMultiplayer)
			{
				// TODO: Add your drawing code here
				board.Draw(this, gameTime, _spriteBatch, tileHighlight);
			}
			else if (state == GameState.menu)
			{
				_spriteBatch.Begin();

				_spriteBatch.Draw(menuTexture, Vector2.Zero, Color.White);

				_spriteBatch.End();
			}
			else if(state == GameState.end)
			{
				_spriteBatch.Begin();
				_spriteBatch.DrawString(font, endScreenText, new Vector2(0, _graphics.PreferredBackBufferHeight / 2), Color.White);
				_spriteBatch.End();
			}
			else if(state == GameState.waitingForMatch)
			{
				_spriteBatch.Begin();
				_spriteBatch.DrawString(font, "Waiting for connection to begin. . .", new Vector2(0, _graphics.PreferredBackBufferHeight / 2), Color.White);
				_spriteBatch.End();
			}

			base.Draw(gameTime);
		}
	}
}