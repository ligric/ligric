DROP PROCEDURE IF EXISTS [GetUserIdsThatDontHaveTheseApi]
GO

CREATE PROCEDURE [GetUserIdsThatDontHaveTheseApi] @userApiId INT
AS
BEGIN
	DECLARE @apiId AS INT;
	SET @apiId = (select TOP 1 _userApi.[ApiId] from [UserAPIs] _userApi where _userApi.[Id] = @userApiId)

	SELECT _user.[Id]
	FROM [Users] _user
	WHERE NOT EXISTS (
		SELECT *
		FROM [UserAPIs] _userApi
		WHERE _userApi.[ApiId] = @apiId AND _userApi.[UserId] = _user.[Id])
END
GO

