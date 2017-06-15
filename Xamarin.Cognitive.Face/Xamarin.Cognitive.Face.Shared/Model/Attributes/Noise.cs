﻿namespace Xamarin.Cognitive.Face.Model
{
	public class Noise : Attribute
	{
		public NoiseLevel NoiseLevel { get; set; }


		public float Value { get; set; }


		public override string ToString ()
		{
			return $"Noise Level: {NoiseLevel} ({Value})";
		}
	}
}