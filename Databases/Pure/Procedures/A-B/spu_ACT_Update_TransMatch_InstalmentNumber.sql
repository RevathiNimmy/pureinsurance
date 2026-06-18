SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_TransMatch_InstalmentNumber'
GO

CREATE PROCEDURE spu_ACT_Update_TransMatch_InstalmentNumber  
    @transmatch_id int,  
    @InstalmentNumber int  
AS  
UPDATE transmatch SET InstalmentNumber=@InstalmentNumber  
WHERE transmatch_id=@transmatch_id

GO


