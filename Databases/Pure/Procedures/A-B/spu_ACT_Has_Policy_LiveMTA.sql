SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Has_Policy_LiveMTA'
GO

CREATE PROCEDURE spu_ACT_Has_Policy_LiveMTA
    @insurance_file_cnt INT,
	@HasLiveMTA SMALLINT OUTPUT
AS

BEGIN
    DECLARE @MTAInsuranceFileCnt INT

    SELECT
        @MTAInsuranceFileCnt=MIN(ifl2.insurance_file_cnt)
    FROM
        Insurance_File ifl
    JOIN
        Insurance_Folder ifld ON ifld.insurance_folder_cnt=ifl.insurance_folder_cnt
    JOIN
        Insurance_File ifl2 ON ifl2.insurance_folder_cnt=ifld.insurance_folder_cnt
    WHERE
        ifl.insurance_file_cnt=@insurance_file_cnt
--	AND ifl2.insurance_file_cnt<>@insurance_file_cnt -- RAW 08/03/2004 : CQ4263 : removed 
	AND ifl2.insurance_file_type_id=5				-- MTA Perm
	AND ISNULL(ifl2.insurance_file_status_id,0)<>4 	-- and not replaced

	IF (ISNULL(@MTAInsuranceFileCnt,0)>0)
		SELECT @HasLiveMTA=1
	ELSE
		SELECT @HasLiveMTA=0
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
