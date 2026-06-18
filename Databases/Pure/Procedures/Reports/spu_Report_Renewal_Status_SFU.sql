SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Renewal_Status_SFU'
GO

CREATE PROCEDURE spu_Report_Renewal_Status_SFU  
	@product varchar(255),  
	@agent varchar(255),  
	@start_date datetime,  
	@end_date datetime,  
	@branch_id int,  
	@AgentGroupCode Varchar(30)
AS  
Set Nocount ON  
  
declare @ibranchid int  
IF @branch_id IS NULL  
  SELECT @iBranchID = 0  
 ELSE  
  SELECT @iBranchID = @branch_id  
  
CREATE TABLE #tempRSARen_Status  
(  
    Client varchar(255) NULL,  
    PolicyNumber varchar(30) NULL,  
    Agent varchar (255) NULL,  
    Product varchar (255) NULL,  
    RenewalDate datetime NULL,  
    Premium numeric(19,4) NULL,  
    RenewalStatus varchar(255) NULL, 
    NoOfClaims int NULL
)  
INSERT INTO #tempRSARen_Status  
        SELECT pHolder.resolved_name,  
                ifi.insurance_ref,  
                isnull(pAgent.resolved_name,' Direct'),  
                p.description,  
                ifi.cover_start_date,  
                ifi.this_premium,  
                rst.description, 
		 (select COUNT(distinct claim_number) from claim where Policy_Number=ifi.insurance_ref and ISNull(is_dirty,0)=0)  NoOfClaims  
        FROM renewal_status rs  
        JOIN insurance_file ifi ON ifi.insurance_file_cnt = rs.renewal_insurance_file_cnt  
        JOIN renewal_status_type rst ON rst.renewal_status_type_id = rs.renewal_status_type_id  
        JOIN product p ON rs.product_id = p.product_id  
        JOIN party pHolder ON rs.insurance_holder_cnt = pHolder.party_cnt 
        LEFT OUTER JOIN party pAgent ON rs.lead_agent_cnt = pAgent.party_cnt  
        LEFT JOIN (select poilcy_number Policy_Number, count(*) noofclaims from 
				( select distinct claim_number, policy_number poilcy_number  from claim group by claim_number ,policy_number ) CL 
				group by poilcy_number ) Claim ON ifi.Insurance_ref = Claim.Policy_Number 
        WHERE ifi.cover_start_date >= @start_date  
        AND ifi.cover_start_date <= @end_date  
AND ( @iBranchID= 0  
              or    (   @iBranchID <> 0 and ifi.branch_id = @iBranchID ))  

--Get the CurrencySymbol
Declare @sCurrencySymbol char(4) 
SELECT @sCurrencySymbol=cb.Symbol 
from Currency cb 
JOIN pmSystem ps on  cb.currency_id =ps.currency_id 
WHERE System_id=1



Set Nocount OFF  

set @sCurrencySymbol = ''

IF LOWER(@AgentGroupCode) = 'all'
BEGIN
 PRINT 'ENTER1.1'

	IF isnull(@product, 'ALL') = 'ALL' AND isnull(@agent, 'ALL') = 'ALL'  
	        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
	ELSE IF isnull(@product, 'ALL') <> 'ALL' AND isnull(@agent, 'ALL') <> 'ALL'  
		BEGIN  
		        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
		        WHERE Product = @product  
		        AND Agent = @agent  
		END  
	ELSE IF isnull(@product, 'ALL') <> 'ALL'  
		BEGIN  
		        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
		        WHERE Product = @product  
		END  
	ELSE  
		BEGIN  
		        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
		        WHERE Agent = @agent  
		END  
END

IF LOWER(@AgentGroupCode) <> 'all'
BEGIN
 PRINT 'ENTER2.1'

	IF isnull(@product, 'ALL') = 'ALL' AND isnull(@agent, 'ALL') = 'ALL'  
	        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
			Where Agent IN(
			select trading_name from party_agent where linked_account_group = (
			select  party_cnt from party where shortname = @AgentGroupCode) )
	ELSE IF isnull(@product, 'ALL') <> 'ALL' AND isnull(@agent, 'ALL') <> 'ALL'  
		BEGIN  
	        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
	        WHERE Product = @product  
	        AND Agent = @agent  
			AND Agent IN(
			select trading_name from party_agent where linked_account_group = (
			select  party_cnt from party where shortname = @AgentGroupCode) )
		END  
	ELSE IF isnull(@product, 'ALL') <> 'ALL'  
		BEGIN  
	        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
	        WHERE Product = @product  
			AND Agent IN(
			select trading_name from party_agent where linked_account_group = (
			select  party_cnt from party where shortname = @AgentGroupCode) )
		END  
	ELSE  
		BEGIN  
	        SELECT *,@sCurrencySymbol CurrencySymbol from #tempRSARen_Status  
	        WHERE Agent = @agent  
			AND Agent IN(
			select trading_name from party_agent where linked_account_group = (
			select  party_cnt from party where shortname = @AgentGroupCode) )
		END  
END

DROP TABLE #tempRSARen_Status  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

