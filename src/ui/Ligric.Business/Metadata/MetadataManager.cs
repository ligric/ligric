﻿using System;
using Grpc.Core;

namespace Ligric.Business.Metadata
{
	public sealed class MetadataManager : IMetadataManager
	{
		public Grpc.Core.Metadata? CurrentMetadata { get; private set; }

		public event EventHandler<Grpc.Core.Metadata?>? MetadataChanged;

		public void SetMetadata(Grpc.Core.Metadata metadata)
		{
			if (CurrentMetadata == metadata)
				return;

			CurrentMetadata = metadata;
			MetadataChanged?.Invoke(this, metadata);
		}

		public void CleanMetadata()
		{
			CurrentMetadata = null;
			MetadataChanged?.Invoke(this, null);
		}
	}
}
