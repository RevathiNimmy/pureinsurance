SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Model_del'
GO


CREATE PROCEDURE spu_RI_Model_del
    @ri_model_id int,
    @is_deleted tinyint = 1,
    @UserId int,  
    @UniqueId varchar (50),  
    @ScreenHierarchy varchar(500)
AS

    Update  RI_Model
    Set     is_deleted = @is_deleted,
            UserId = @UserId,  
            UniqueId = @UniqueId,  
            ScreenHierarchy = @ScreenHierarchy 
    Where   ri_model_id = @ri_model_id

GO


