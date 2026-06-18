SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Premium_Tolerances'
GO

CREATE PROCEDURE spu_SIRRen_Get_Premium_Tolerances 
    @insurance_folder_cnt int,
    @gis_scheme_id        int

AS

SELECT    I.this_premium existing_premium,
          S.max_change_num max_tolerance,
          S.min_change_num min_tolerance,
          RC.party_cnt
FROM      Renewal_Control RC,
          Insurance_File  I,
          GIS_Scheme      S
WHERE     RC.insurance_folder_cnt = @insurance_folder_cnt
AND       I.insurance_file_cnt = RC.old_insurance_file_cnt
AND       S.gis_scheme_id = @gis_scheme_id 



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

