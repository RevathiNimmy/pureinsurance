EXEC DDLDropProcedure 'spu_Get_CollectionDate_Override_Authority'
GO

CREATE PROCEDURE spu_Get_CollectionDate_Override_Authority
@user_id int
AS
	SELECT can_backdate_collection_date 
	FROM user_authorities 
	WHERE user_id=@user_id
GO