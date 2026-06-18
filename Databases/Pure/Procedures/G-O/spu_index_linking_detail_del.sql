SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_index_linking_detail_del'
GO


CREATE PROCEDURE spu_index_linking_detail_del
    @index_linking_id int,
	@UserId INT = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL
AS

UPDATE index_linking_detail SET UserId = @UserId,ScreenHierarchy = @ScreenHierarchy,UniqueId = @UniqueId
WHERE   index_linking_id = @index_linking_id

DELETE  index_linking_detail
WHERE   index_linking_id = @index_linking_id
GO


