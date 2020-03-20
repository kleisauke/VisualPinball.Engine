using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VisualPinball.Engine.PinMame;

namespace VisualPinball.Unity.PinMame
{
	[AddComponentMenu("Visual Pinball/PinMAME")]
	[ExecuteInEditMode]
	public class PinMameBehavior : MonoBehaviour
	{

		public string GameName = "mm_109c";
		public Engine.PinMame.PinMame PinName { get; private set; }

		private Texture2D _texture;
		private DmdDimensions _dmdDimensions;

		private static readonly Color Tint = new Color(1, 0.18f, 0);

		private Dictionary<byte, Color> _map = new Dictionary<byte, Color> {
			{0x0, Color.Lerp(Color.black, Tint, 0)},
			{0x14, Color.Lerp(Color.black, Tint, 0.33f)},
			{0x21, Color.Lerp(Color.black, Tint, 0.66f)},
			{0x43, Color.Lerp(Color.black, Tint, 1f)},
			{0x64, Color.Lerp(Color.black, Tint, 1f)}
		};

		public void Init()
		{
			PinName = Engine.PinMame.PinMame.Instance();
		}

		public void StartGame()
		{
			Task.Run(async () => {
				if (!PinName.IsRunning) {
					await PinName.StartGame(GameName, showConsole: true);
				}
			});
		}

		private void Start()
		{
			Init();
		}

		// Update is called once per frame
		private void Update()
		{
			if (PinName != null && PinName.IsRunning && PinName.NeedsDmdUpdate()) {
				if (_texture == null) {
					_dmdDimensions = PinName.GetDmdDimensions();
					_texture = new Texture2D(_dmdDimensions.Width, _dmdDimensions.Height);
					GetComponent<Renderer>().sharedMaterial.mainTexture = _texture;
				}

				var frame = PinName.GetDmdPixels();
				if (frame.Length == _dmdDimensions.Width * _dmdDimensions.Height) {
					for (var y = 0; y < _dmdDimensions.Height; y++) {
						for (var x = 0; x < _dmdDimensions.Width; x++) {
							var pixel = frame[y * _dmdDimensions.Width + x];
							_texture.SetPixel(x, y, _map.ContainsKey(pixel) ? _map[pixel] : Color.magenta);
						}
					}
				}
				_texture.Apply();
			}
		}

		private void OnDestroy()
		{
			PinName?.StopGame();
			PinName?.ResetGame();
		}
	}
}
