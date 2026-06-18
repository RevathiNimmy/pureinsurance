SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Upd_Policy_Premium
GO

CREATE PROCEDURE spu_Upd_Policy_Premium  
    @insurance_file_cnt INT  
AS  
  
/*********************************************************************************************************  
* Name : spu_Upd_Policy_Premium  
*  
* Desc : update policy this_premium, net_premium and annual_premium from risk  
*  
* Ver  : 1.00.0000  
*  
* Note : need to change slightly when we have tax details  
*  
* Hist : 19/02/2001 Created - Tinny  
*        27/09/2001 Amended - Tom - do not include unchanged risks  
*        03/10/2002 Amended - PW - do not included unselected risks  
*        21/11/2002 Amended - PW - write the risk tax (PS411)  
**********************************************************************************************************/  
BEGIN  
  
    DECLARE @this_premium MONEY,  
            @annual_premium MONEY,  
            @tax_amount MONEY  
  
    SELECT  
        @tax_amount=SUM(value)  
    FROM  
        Tax_Calculation rt  
    WHERE  
        insurance_file_cnt = @insurance_file_cnt  
        AND rt.transtype IN ('TTR','TTIF')  
		AND ISNULL(rt.is_not_applied_to_client,0)=0
       
    SELECT  
        @this_premium = SUM(r.total_this_premium)  
    FROM  
        insurance_file_risk_link ifrl  
        INNER JOIN risk r  
            ON ifrl.risk_cnt = r.risk_cnt  
            AND ifrl.insurance_file_cnt = @insurance_file_cnt  
            AND ifrl.status_flag NOT IN ('U','R') 
        WHERE r.is_risk_selected = 1  
  
    IF @@ERROR <> 0  
        RETURN  
  
    SELECT  
        @annual_premium = SUM(ISNULL(annual_premium,0))  
    FROM  
        rating_section rs  
        INNER JOIN insurance_file_risk_link ifrl  
            ON rs.risk_cnt = ifrl.risk_cnt  
        INNER JOIN risk r  
            ON ifrl.risk_cnt = r.risk_cnt  
    WHERE  
        ifrl.insurance_file_cnt = @insurance_file_cnt  
        AND ifrl.risk_cnt = r.risk_cnt  
        AND r.is_risk_selected = 1  
        AND rs.original_flag = 0  
  
    IF @@ERROR <> 0  
        RETURN  
  
    UPDATE insurance_file  
       SET this_premium = ISNULL(@this_premium,0),  
           net_premium = ISNULL(@this_premium,0),  
           annual_premium = ISNULL(@annual_premium,0),  
           tax_amount = ISNULL(@tax_amount,0)  
     WHERE insurance_file_cnt = @insurance_file_cnt  
  
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

