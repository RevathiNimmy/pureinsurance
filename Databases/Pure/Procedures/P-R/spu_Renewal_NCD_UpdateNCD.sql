SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Renewal_NCD_UpdateNCD'
GO


CREATE PROCEDURE spu_Renewal_NCD_UpdateNCD
    @newClaimedYears INT,
    @insurance_folder_cnt INT
AS

/** Query to update GIIMNCD table information based on GIIMGemPolicy_id parameter **/
/** Changes history : **/
/** Changes made to reflect updation of renewal_NCD_Year field of insurance_folder table : SSL 24/04/2001**/
BEGIN
    UPDATE Insurance_Folder
    SET renewal_NCD_Year = @newClaimedYears
    WHERE insurance_folder_cnt = @insurance_folder_cnt
END
GO


