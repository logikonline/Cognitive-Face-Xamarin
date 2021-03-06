﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using NomadCode.UIExtensions;
using UIKit;
using Xamarin.Cognitive.Face.Model;

namespace Xamarin.Cognitive.Face.Sample.iOS
{
	public partial class GroupingResultCollectionViewController : BaseCollectionViewController
	{
		List<FaceGroup> Results;
		Func<Model.Face, UIImage> thumbnailProvider;

		public GroupingResultCollectionViewController (IntPtr handle) : base (handle)
		{
		}


		protected override void Dispose (bool disposing)
		{
			thumbnailProvider = null;

			base.Dispose (disposing);
		}


		public void SetFaceGroupResults (GroupResult groupResult, Func<Model.Face, UIImage> thumbnailProvider)
		{
			this.thumbnailProvider = thumbnailProvider;

			if (groupResult.MessyGroup?.Faces.Count > 0)
			{
				Results = groupResult.Groups.Union (new [] { groupResult.MessyGroup }).ToList ();
			}
			else
			{
				Results = groupResult.Groups;
			}

			CollectionView.ReloadData ();
		}


		public override nint NumberOfSections (UICollectionView collectionView) => Results?.Count ?? 0;


		public override UICollectionReusableView GetViewForSupplementaryElement (UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			var groupResult = Results [indexPath.Section];
			var header = collectionView.Dequeue<SimpleCVHeader> (UICollectionElementKindSection.Header, indexPath);

			header.SetTitle (groupResult.Title);

			return header;
		}


		public override nint GetItemsCount (UICollectionView collectionView, nint section) => Results? [(int) section]?.Faces?.Count ?? 0;


		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.Dequeue<FaceCVC> (indexPath);
			var face = Results [indexPath.Section].Faces [indexPath.Row];
			var thumbnail = thumbnailProvider (face);

			cell.SetFaceImage (face, thumbnail);

			return cell;
		}
	}
}