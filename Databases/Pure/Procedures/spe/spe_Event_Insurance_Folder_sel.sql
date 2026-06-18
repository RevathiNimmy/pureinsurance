SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spe_Event_Insurance_Folder_sel'
GO

CREATE PROCEDURE spe_Event_Insurance_Folder_sel
    @insurance_folder_cnt int
AS
SELECT
    insurance_folder_cnt,
    insurance_folder_id,
    source_id,
    insurance_holder_cnt,
    code,
    description,
    inception_date,
    arc_archive_folder_id,
    quote_insurance_ref,
    next_insurance_ref,
    last_insurance_ref,
    renewal_count  
 FROM Event_Insurance_Folder
WHERE insurance_folder_cnt = @insurance_folder_cnt




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
