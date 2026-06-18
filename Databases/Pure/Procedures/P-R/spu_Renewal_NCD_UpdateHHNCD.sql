SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Renewal_NCD_UpdateHHNCD'
GO


CREATE PROCEDURE spu_Renewal_NCD_UpdateHHNCD
    @newBuildingYears INT,
    @newContentsYears INT,
    @insurance_folder_cnt INT
AS


BEGIN
    UPDATE Insurance_Folder
    SET renewal_NCD_Year = @newBuildingYears,
        renewal_NCD_contents = @newContentsYears
    WHERE insurance_folder_cnt = @insurance_folder_cnt
END
GO


