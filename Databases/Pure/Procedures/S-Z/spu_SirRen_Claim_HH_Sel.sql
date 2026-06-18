SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Claim_HH_Sel'
GO


CREATE PROCEDURE spu_SirRen_Claim_HH_Sel
    @Insurance_File_Cnt int
AS

/* AK 30072001 - To get Claims data for Household */
BEGIN

    SELECT C.Claim_Id, C.Loss_From_Date, Code=Substring(P.Code, 7, 3), C.Likely_Claim, C.Description
    From Claim C, Primary_Cause P, Insurance_File I
    Where I.Insurance_File_Cnt = @Insurance_File_Cnt
    AND C.Policy_Id in (Select Insurance_File_Cnt from Insurance_File F
                WHERE F.Insurance_Folder_Cnt = I.Insurance_Folder_cnt)
    /* Need to select all the claims for that Insurance Folder */
    And P.Primary_Cause_Id = C.Primary_Cause_Id
END
GO


