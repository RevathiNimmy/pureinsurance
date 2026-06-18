SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Update_BG_Status'
GO

CREATE PROCEDURE spu_Update_BG_Status
       	@BG_id    		INT,
	@Status_Id 		INT,
	@Status_Date 		DateTime,
	@Insurance_File_Cnt 	INT = NULL


AS


UPDATE 	 Bank_Guarantee
SET      BG_Status_Id = @Status_Id
Where 	 BG_Id = @BG_Id


Update	Insurance_File_BG_Link
             Set BG_Status_Id = @Status_Id,
                   BG_Status_Date = @Status_Date
Where   BG_Id = @BG_Id
        AND 
	(
		(@Insurance_File_Cnt IS NULL)
	OR
		(@Insurance_File_Cnt IS NOT NULL AND Insurance_File_BG_Link.Insurance_File_Cnt = @Insurance_File_Cnt)
	)
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
