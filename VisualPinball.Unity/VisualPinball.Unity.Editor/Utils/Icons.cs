﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VisualPinball.Unity.Editor
{
	public enum IconSize
	{
		Large, Small
	}

	public enum IconColor
	{
		Gray, Green, Orange, Blue
	}

	public class Icons
	{
		private readonly struct IconVariant
		{
			private readonly string _name;
			private readonly IconSize _size;
			private readonly IconColor _color;

			public IconVariant(string name, IconSize size, IconColor color)
			{
				_name = name;
				_size = size;
				_color = color;
			}
		}

		private const string BumperName = "bumper";
		private const string FlipperName = "flipper";
		private const string GateName = "gate";
		private const string KickerName = "kicker";
		private const string LightName = "light";
		private const string PlungerName = "plunger";
		private const string PrimitiveName = "primitive";
		private const string RampName = "ramp";
		private const string RubberName = "rubber";
		private const string SpinnerName = "spinner";
		private const string SurfaceName = "surface";
		private const string HitTargetName = "target";
		private const string TriggerName = "trigger";

		private static readonly string[] Names = {
			BumperName, FlipperName, GateName, KickerName, LightName, PlungerName, PrimitiveName, RampName, RubberName,
			SpinnerName, SurfaceName, HitTargetName, TriggerName
		};

		private readonly Dictionary<IconVariant, Texture2D> _icons = new Dictionary<IconVariant,Texture2D>();
		private static readonly MethodInfo CopyMonoScriptIconToImporters = typeof(MonoImporter).GetMethod("CopyMonoScriptIconToImporters", BindingFlags.Static|BindingFlags.NonPublic);
		private static readonly MethodInfo SetIconForObject = typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.Static|BindingFlags.NonPublic);
		private static readonly MethodInfo SetGizmoEnabled = Assembly.GetAssembly(typeof(UnityEditor.Editor))?.GetType("UnityEditor.AnnotationUtility")?.GetMethod("SetGizmoEnabled", BindingFlags.Static | BindingFlags.NonPublic);
		private static readonly MethodInfo SetIconEnabled = Assembly.GetAssembly(typeof(UnityEditor.Editor))?.GetType("UnityEditor.AnnotationUtility")?.GetMethod("SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic);

		// see https://docs.unity3d.com/Manual/ClassIDReference.html
		private static readonly int MonoBehaviourClassID = 114;

		private static Icons _instance;
		private static Icons Instance => _instance ?? (_instance = new Icons());


		private Icons()
		{
			const string iconPath = "Packages/org.visualpinball.engine.unity/VisualPinball.Unity/VisualPinball.Unity.Editor/Resources/Icons";
			foreach (var name in Names) {
				foreach (var size in Enum.GetValues(typeof(IconSize)).Cast<IconSize>()) {
					foreach (var color in Enum.GetValues(typeof(IconColor)).Cast<IconColor>()) {
						var variant = new IconVariant(name, size, color);
						var path = $"{iconPath}/{size.ToString().ToLower()}_{color.ToString().ToLower()}/{name}.png";
						if (File.Exists(path)) {
							_icons[variant] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
						}
					}
				}
			}
		}

		public static Texture2D Bumper(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(BumperName, size, color);
		public static Texture2D Flipper(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(FlipperName, size, color);
		public static Texture2D Gate(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(GateName, size, color);
		public static Texture2D Kicker(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(KickerName, size, color);
		public static Texture2D Light(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(LightName, size, color);
		public static Texture2D Plunger(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(PlungerName, size, color);
		public static Texture2D Primitive(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(PrimitiveName, size, color);
		public static Texture2D Ramp(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(RampName, size, color);
		public static Texture2D Rubber(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(RubberName, size, color);
		public static Texture2D Spinner(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(SpinnerName, size, color);
		public static Texture2D Surface(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(SurfaceName, size, color);
		public static Texture2D Target(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(HitTargetName, size, color);
		public static Texture2D Trigger(IconSize size = IconSize.Large, IconColor color = IconColor.Gray) => Instance.GetItem(TriggerName, size, color);

		public static Texture2D ByComponent<T>(T mb, IconSize size = IconSize.Large, IconColor color = IconColor.Gray)
			where T : MonoBehaviour
		{
			if (typeof(T) == typeof(BumperAuthoring)) return Bumper(size, color);
			if (typeof(T) == typeof(FlipperAuthoring)) return Flipper(size, color);
			if (typeof(T) == typeof(GateAuthoring)) return Gate(size, color);
			if (typeof(T) == typeof(KickerAuthoring)) return Kicker(size, color);
			if (typeof(T) == typeof(LightAuthoring)) return Light(size, color);
			if (typeof(T) == typeof(PlungerAuthoring)) return Plunger(size, color);
			if (typeof(T) == typeof(PrimitiveAuthoring)) return Primitive(size, color);
			if (typeof(T) == typeof(RampAuthoring)) return Ramp(size, color);
			if (typeof(T) == typeof(RubberAuthoring)) return Rubber(size, color);
			if (typeof(T) == typeof(SpinnerAuthoring)) return Spinner(size, color);
			if (typeof(T) == typeof(SurfaceAuthoring)) return Surface(size, color);
			if (typeof(T) == typeof(HitTargetAuthoring)) return Target(size, color);
			if (typeof(T) == typeof(TriggerAuthoring)) return Trigger(size, color);

			return null;
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		public static void OnScriptsReloaded()
		{
			DisableGizmo<BumperAuthoring>();
			DisableGizmo<FlipperAuthoring>();
			DisableGizmo<GateAuthoring>();
			DisableGizmo<HitTargetAuthoring>();
			DisableGizmo<KickerAuthoring>();
			DisableGizmo<LightAuthoring>();
			DisableGizmo<PlungerAuthoring>();
			DisableGizmo<PrimitiveAuthoring>();
			DisableGizmo<RampAuthoring>();
			DisableGizmo<RubberAuthoring>();
			DisableGizmo<SpinnerAuthoring>();
			DisableGizmo<SurfaceAuthoring>();
			DisableGizmo<TriggerAuthoring>();
		}

		public static void ApplyToComponent<T>(Object target, Texture2D tex) where T : MonoBehaviour
		{
			if (target == null || tex == null) {
				throw new ArgumentNullException();
			}
			SetIconForObject.Invoke(null, new object[]{ target, tex });
			DisableGizmo<T>();

			var monoScript = target as MonoScript;
			if (monoScript) {
				CopyMonoScriptIconToImporters.Invoke(null, new object[]{ monoScript });
			}
		}

		private Texture2D GetItem(string name, IconSize size, IconColor color)
		{
			var variant = new IconVariant(name, size, color);
			if (!_icons.ContainsKey(variant)) {
				variant = new IconVariant(name, IconSize.Large, color);
			}

			if (!_icons.ContainsKey(variant)) {
				variant = new IconVariant(name, IconSize.Large, IconColor.Gray);
			}

			return _icons[variant];
		}

		private static void DisableGizmo<T>() where T : MonoBehaviour
		{
			var className = typeof(T).Name;
			SetGizmoEnabled?.Invoke(null, new object[] { MonoBehaviourClassID, className, 0, false });
			SetIconEnabled?.Invoke(null, new object[] { MonoBehaviourClassID, className, 0 });
		}
	}
}