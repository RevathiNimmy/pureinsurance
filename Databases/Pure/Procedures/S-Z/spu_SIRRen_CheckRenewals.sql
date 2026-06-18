SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_CheckRenewals'
GO


CREATE PROCEDURE spu_SIRRen_CheckRenewals
    @insurance_file_cnt int
AS

/* AK - 31072001 - To check if the passed insurance file falls in Renewal cycle */
BEGIN
    Declare @Insurance_Folder_Cnt int
    Declare @Renewal_Frequency_id int

    /* AK 251001 - need to exclude PreRenSel status from Claims update warning */
    Declare @RenStatus int

    Select @RenStatus = Renewal_Status_Type_Id from Renewal_Status_Type where code = 'PRERENSEL' or code = 'RENEWED'
    Select @Insurance_Folder_Cnt = Insurance_Folder_Cnt, @Renewal_Frequency_id=Renewal_Frequency_id
    FROM Insurance_File
    WHERE Insurance_File_Cnt = @Insurance_File_Cnt

    If (Select count(*) from Renewal_Control
    WHERE Insurance_Folder_Cnt = @Insurance_Folder_Cnt AND Renewal_Status_Type_Id <> @RenStatus) > 0

    Select @Insurance_Folder_Cnt, @Renewal_Frequency_id
    Else
    Select 0,0

END
GO


