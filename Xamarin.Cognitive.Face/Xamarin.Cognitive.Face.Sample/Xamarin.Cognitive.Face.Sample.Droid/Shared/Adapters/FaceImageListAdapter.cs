﻿using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Xamarin.Cognitive.Face.Droid.Extensions;

namespace Xamarin.Cognitive.Face.Sample.Droid.Shared.Adapters
{
	public class FaceImageListAdapter : BaseAdapter<Model.Face>
	{
		readonly List<Bitmap> faceThumbnails;
		Bitmap highlightedBitmap;

		public List<Model.Face> Faces { get; private set; }

		public int SelectedIndex { get; private set; } = -1;

		public FaceImageListAdapter (List<Model.Face> faces, Bitmap photo)
		{
			Faces = faces;

			if (faces != null && photo != null)
			{
				faceThumbnails = faces.GenerateThumbnails (photo);
			}
		}


		protected override void Dispose (bool disposing)
		{
			highlightedBitmap?.Dispose ();
			faceThumbnails?.ForEach (b => b.Dispose ());

			base.Dispose (disposing);
		}


		public override int Count => Faces?.Count ?? 0;


		public override Model.Face this [int position] => Faces [position];


		public Model.Face SelectedFace => SelectedIndex > -1 ? Faces [SelectedIndex] : null;


		public Bitmap GetThumbnailForFace (Model.Face face)
		{
			var index = Faces.IndexOf (face);

			if (index > -1 && faceThumbnails.Count > index)
			{
				return faceThumbnails [index];
			}

			return null;
		}


		public void SetSelectedIndex (int index)
		{
			SelectedIndex = index;

			highlightedBitmap?.Dispose ();

			NotifyDataSetChanged ();
		}


		public override long GetItemId (int position) => position;


		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			if (convertView == null)
			{
				var layoutInflater = (LayoutInflater) Application.Context.GetSystemService (Context.LayoutInflaterService);
				convertView = layoutInflater.Inflate (Resource.Layout.item_face, parent, false);
			}

			convertView.Id = position;

			var thumbnailToShow = faceThumbnails [position];

			if (SelectedIndex == position)
			{
				highlightedBitmap = ImageHelper.HighlightSelectedFaceThumbnail (thumbnailToShow);
				thumbnailToShow = highlightedBitmap;
			}

			convertView.FindViewById<ImageView> (Resource.Id.image_face).SetImageBitmap (thumbnailToShow);

			return convertView;
		}
	}
}