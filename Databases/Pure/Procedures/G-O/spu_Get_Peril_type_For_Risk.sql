SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Peril_type_For_Risk'
GO

CREATE PROCEDURE spu_Get_Peril_type_For_Risk  
    @SiriusProduct varchar(2),  
    @ClaimId integer,  
    @Risk integer,  
    @Policy integer,  
    @ShowAll int = 0  
AS  
Begin  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
-- CMG/PB       PB      13/09/2002  get everything if lossSchedule is enabled  
--              DC      14/05/2003  added check for deleted perils (dont get them)  
-- RVH          RH      18/01/2005  Changed to use new ShowAll parameter  
--*******************************************************************************************  
if @SiriusProduct = 'A'  
    --BROKING  
    Begin  
        select  Peril_Type_Id, Description  
        from    Peril_Type  
        where   Peril_Type_Id not in (select Peril_Type_Id  
                from Claim_Peril  
                where Claim_Id = @ClaimID)  
        and     is_deleted = 0  
    End  
else  
    --UNDERWRITING  
    Begin  
    if (@ShowAll = 1)  
        Begin  
            --Show All Peril Types  
            SELECT DISTINCT Peril.Peril_Type_ID,  
                    Peril_type.Description  
            FROM    Peril,  
                    Peril_type  
            WHERE   Peril.Peril_type_Id = Peril_type.Peril_type_Id  
            AND     risk_cnt = @Risk 
            AND     Peril_type.is_levy_tax = 0
        End  
    else  
        Begin  
         SELECT DISTINCT Peril.Peril_Type_ID,  
                 Peril_type.Description  
         FROM    Peril,  
                 Peril_type  
         WHERE   Peril.Peril_type_Id = Peril_type.Peril_type_Id  
         AND     risk_cnt = @Risk  
         AND     Peril.Peril_Type_ID NOT IN  
                 (  
                     SELECT Peril_Type_Id  
                     FROM Claim_Peril  
                     WHERE Claim_Id = @ClaimID  
                 )  
	AND 	Peril_type.is_levy_tax = 0                 
 End  
    End  
End  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
