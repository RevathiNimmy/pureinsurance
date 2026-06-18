SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_edi_refresh_check'
GO


CREATE PROCEDURE spu_SIRRen_edi_refresh_check
    @insurance_folder_cnt int
AS


BEGIN
    Declare @StatusQuote int
    Select @StatusQuote = Renewal_Status_Type_Id from Renewal_Status_Type where code = 'RENQUOTED'

    Select count(*) from Renewal_Control
        where Insurance_folder_Cnt = @insurance_folder_cnt
        AND Renewal_Status_Type_Id >= @StatusQuote
END
GO


