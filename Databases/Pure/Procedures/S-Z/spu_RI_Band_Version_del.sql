SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_RI_Band_Version_del'
GO
CREATE PROCEDURE spu_RI_Band_Version_del  
         @RI_Band_id int,
	 @userid INT = NULL,
	 @uniqueid VARCHAR(50) = NULL,
	 @screenhierarchy VARCHAR(100) = NULL
AS  
BEGIN
   
   UPDATE rbv  SET 
        userid=@UserId,
        uniqueid=@uniqueid,
		screenhierarchy=@screenhierarchy + '/' + description + '/' + CAST(convert(date,effective_date,103) AS VARCHAR)
    FROM RI_Band_Version rbv 

    WHERE
	    ri_band_id = @RI_Band_id

   DELETE FROM  RI_Band_Version where ri_band_id = @RI_Band_id
   
END
GO
