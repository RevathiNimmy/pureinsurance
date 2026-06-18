
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Sir_GetExpiryDate'
GO

CREATE PROCEDURE spu_Sir_GetExpiryDate

    @insurance_folder_cnt int,
	@Cover_Start_Date Date

AS

BEGIN
SELECT min(cover_start_date) as cover_start_date 
FROM insurance_file IFI
JOIN Insurance_File_Type IFT on IFI.insurance_file_type_id=IFT.insurance_file_type_id
WHERE IFT.code='MTA PERM' and insurance_folder_cnt = @insurance_folder_cnt and cover_start_date > @Cover_Start_Date

 
END    

GO
  