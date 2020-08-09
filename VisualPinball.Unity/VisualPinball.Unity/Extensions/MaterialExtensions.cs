// ReSharper disable StringLiteralTypo

using System;
using System.Text;
using NLog;
using VisualPinball.Engine.VPT;
using VisualPinball.Unity.Import;
using VisualPinball.Unity.Import.Material;
using VisualPinball.Unity.Patcher.Matcher;
using VisualPinball.Unity.VPT.Table;
using Logger = NLog.Logger;

namespace VisualPinball.Unity.Extensions
{
	public static class MaterialExtensions
	{

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Material Converter instance for the current graphics pipeline
		/// </summary>
		public static IMaterialConverter MaterialConverter => CreateMaterialConverter();

		/// <summary>
		/// Create a material converter depending on the graphics pipeline
		/// </summary>
		/// <returns></returns>
		private static IMaterialConverter CreateMaterialConverter()
		{
			switch (RenderPipeline.Current)
			{
				case RenderPipelineType.BuiltIn:
					return new StandardMaterialConverter();
				case RenderPipelineType.Hdrp:
					return new HdrpMaterialConverter();
				case RenderPipelineType.Urp:
					return new UrpMaterialConverter();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static UnityEngine.Material ToUnityMaterial(this PbrMaterial vpxMaterial, TableBehavior table, StringBuilder debug = null)
		{
			if (table != null)
			{
				var existingMat = table.GetMaterial(vpxMaterial);
				if (existingMat != null)
				{
					return existingMat;
				}
			}

			var unityMaterial = MaterialConverter.CreateMaterial(vpxMaterial, table, debug);

			if (table != null)
			{
				table.AddMaterial(vpxMaterial, unityMaterial);
			}

			return unityMaterial;
		}

		public static string GetUnityFilename(this PbrMaterial vpMat, string folderName)
		{
			return $"{folderName}/{vpMat.Id}.mat";
		}

	}
}
