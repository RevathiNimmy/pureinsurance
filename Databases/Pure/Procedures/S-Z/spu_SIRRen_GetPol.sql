SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_GetPol'
GO


CREATE PROCEDURE spu_SIRRen_GetPol
    @insurance_file_cnt int
AS

/* AK - 31072001 To get Policy Details for Claims/Renewal Integration purpose */
BEGIN

    SELECT I.Insurance_Ref, P.Party_Id
    FROM Insurance_File I, Party P
    WHERE I.Insurance_File_Cnt = @Insurance_File_Cnt
    AND P.Party_Cnt = I.Lead_Insurer_Cnt

END
GO


