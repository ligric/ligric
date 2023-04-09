﻿using Ligric.Domain.Entities.Apies;

namespace Ligric.Domain.Entities.Apis
{
	public interface IApiRepository
	{
		ApiEntity GetEntityById(long id);

		ApiEntity GetEntityByUserApiId(long id);

		object Save(ApiEntity entity);

		void Delete(long id);

		void Delete(ApiEntity entity);
	}
}