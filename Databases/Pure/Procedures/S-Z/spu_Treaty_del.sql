SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Treaty_del'
GO


CREATE PROCEDURE spu_Treaty_del
    @treaty_id int,
    @is_deleted tinyint = 1,
	@UserId int,  
	@UniqueId varchar(50),  
	@ScreenHierarchy varchar(500)
AS

    Update  Treaty
    Set     is_deleted = @is_deleted,
	        UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy = @ScreenHierarchy
    Where   treaty_id = @treaty_id

GO