SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetRiskDetails'
GO


CREATE PROCEDURE spu_GetRiskDetails
    @SiriusProduct varchar(1),
    @Risk integer,
    @Policy integer
AS

if @SiriusProduct = 'A'
    Begin
        select Risk_Code_Id, Description
        from Risk_Code
        where Risk_Code_Id = @Risk
    end
    else
    Begin
        select  Risk.Risk_Type_Id, 
                Risk_type.Description,
                Risk_type.claims_is_post_taxes
        from    Risk_type
        join    Risk
                on Risk_type.Risk_type_Id = Risk.Risk_type_Id
        and Risk_cnt = @Risk
        --and Insurance_File_Cnt = @Policy
    End

GO


