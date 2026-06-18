SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_find_claim_details_sfu'
GO

CREATE PROCEDURE spu_find_claim_details_sfu  
     @ClaimNumber varchar(30),  
        @PolicyNumber varchar(30),  
        @ClientShortName varchar(20),  
        @LossDateFrom datetime = NULL,  
        @LossDateTo datetime = NULL,  
        @IncludeClosed int,  
        @UserID int,  
        @SourceID int,  
        @CaseID int =NULL,  
   @AgentCnt int = NULL,  
   @TPA varchar(255)= NULL  
   AS  

 /*
	Created By:		???
	Creation Date:  ???
	Descriptoin:	Returns claims and case information dependant on the passed base case id

	Amendments

	Amended By		Date		Description
	-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	George Harris	12 Dec 2018	Changes the select statement to first insert the details into a temp table for perfrmance reasons				
*/

    BEGIN  
  
 DECLARE
            @sql varchar(8000) ,
            @where varchar(8000)

 IF @CaseID=0
    SELECT @CaseID=NULL

 IF @TPA = ''
 SELECT @TPA=NULL
 --SELECT @sql = 'SET QUOTED_IDENTIFIER OFF' + CHAR(13) + CHAR(10)

        SELECT  @sql =
            'SELECT c.policy_id  InsuranceFileKey,
  c.claim_id  ClaimKey,
  c.description  ClaimDescription,
  c.claim_number  ClaimNumber,
  c.policy_number  InsuranceRef,
  c.client_short_name  ClientShortName,
  prd.description  ProductDescription,
  c.loss_from_date LossDateFrom,' +
  --c.client_name ClientName,  --PN67011
  'p.resolved_name,
  c.claim_status_id ClaimStatusID,
  ISNULL(h.description,'''') ClaimHandlerDescription,
  c.insurer_claim_number InsurerClaimNumber,
                c.client_claim_number ClientClaimNumber,
  c.client_tel_no ClientTelephoneNumber,
  c.client_tel_no_off ClientTelephoneNumberOffice,
                ISNULL(pc.description,'''') PrimaryCauseDescription,
  ISNULL(sc.description,'''') SecondaryCauseDescription,
  ISNULL(ps.description,'''') ProgressStatusDescription,
  0, 0, -- This is just a placeholder
                cu.iso_code CurrencyISOCode,
  s.is_deleted IsDeleted,
  s.closed_allow_claims IsAllowedClosedClaims,
  ISNULL(c.other_party_id,0) OtherPartyID ' +
            'FROM Claim c ' +
            'INNER JOIN insurance_file ifi ON ifi.insurance_file_cnt = c.Policy_id ' +
            'INNER JOIN product prd ON ifi.product_id = prd.product_id ' +
            'INNER JOIN source s ON ifi.source_id = s.source_id ' +
            'LEFT JOIN Handler h ON c.handler_id = h.handler_id ' +
            'LEFT JOIN primary_cause pc ON c.primary_cause_id = pc.primary_cause_id ' +
            'LEFT JOIN secondary_cause sc ON c.secondary_cause_id = sc.secondary_cause_id ' +
            'LEFT JOIN progress_status ps ON c.progress_status_id = ps.progress_status_id ' +
            'LEFT JOIN currency cu ON cu.currency_id = c.currency_id '   +
     'INNER JOIN (SELECT MAX(Version_id) as version_id,MAX(Claim_Id) as claim_id, base_claim_id FROM claim ' +
      ' WHERE is_dirty = 0 GROUP BY base_claim_id ) claim_version ON ' +
     ' c.claim_id = claim_version.claim_id ' +
     'INNER JOIN party p ON p.party_cnt = ifi.insured_cnt ' +
     'LEFT JOIN party tpa ON tpa.party_cnt = c.other_party_id '


--PRINT @sql

        SELECT @where = 'WHERE '

        IF @IncludeClosed = 1
        BEGIN
            SELECT @sql = @sql + @where + 'c.claim_status_id IN (1,2,4) '
            SELECT @where = 'AND '
        END

        IF @claimnumber IS NOT NULL
        BEGIN
            SELECT @sql = @sql + @where + 'c.claim_number like ''' + @claimnumber + ''' '
            SELECT @where = 'AND '
        END

        IF @PolicyNumber IS NOT NULL
        BEGIN
            SELECT @sql = @sql + @where + 'c.policy_number like ''' + @PolicyNumber + ''' '
            SELECT @where = 'AND '
        END

        IF @ClientShortName IS NOT NULL
        BEGIN
            SELECT @sql = @sql + @where + 'c.client_short_name like ''' + @ClientShortName + ''' '
            SELECT @where = 'AND '
        END

       IF @TPA IS NOT NULL
     BEGIN
            SELECT @sql = @sql + @where + 'ISNULL(tpa.shortname,"") like ''' + @TPA + ''' '
          SELECT @where = 'AND '
     END

        IF @LossDateFrom IS NOT NULL
        BEGIN
            SELECT @sql = @sql + @where + 'c.loss_from_date > ''' + Convert(varchar(20), @LossDateFrom) + ''' '
            SELECT @where = 'AND '
        END

 IF @LossDateTo IS NOT NULL
        BEGIN
   --Changes Done by : Krishna nand
   --Purpose: to display the records including mentioned Claim To Date
   --PN: 67176
   --Dated: 04/02/2010
      SELECT @sql = @sql + @where + 'c.loss_from_date < ''' + Convert(varchar,DateAdd(dd,1,@LossDateTo)) + ''' '
            --SELECT @sql = @sql + @where + 'c.loss_from_date < "' + Convert(varchar(20), @LossDateTo) + '" '
            --End of Changes done by Krishna Nand on 04/02/2010 for PN: 67176
            SELECT @where = 'AND '
        END

 IF @CaseID IS NOT NULL
        BEGIN
     SELECT @sql = @sql + @where + 'c.base_case_id = (SELECT base_case_id FROM [case] WHERE case_id = ' + CONVERT(VARCHAR(20),@CaseID) + ')'
            SELECT @where = 'AND '
        END

     IF EXISTS (SELECT NULL FROM hidden_options WHERE option_number = 16 AND value = '1')
     BEGIN
      /*Multi-Ledger : Only allow claims from logged in branch to be selected.*/
            SELECT @sql = @sql + @where + 'ifi.source_id = ' + CONVERT(VARCHAR(20),@SourceID) + ' '
            SELECT @where = 'AND '
        END
        ELSE
        BEGIN
         /*Single-Ledger : Only allow claims from branches that the user has permissions for.*/
            SELECT @sql = @sql + @where + 'ifi.source_id NOT IN (SELECT source_id FROM pmuser_source WHERE user_id = ' + CONVERT(VARCHAR(20),@UserID) + ')'
            SELECT @where = 'AND '
     END
     IF @AgentCnt IS NOT NULL
     BEGIN
            SELECT @sql = @sql + @where + 'ifi.lead_agent_cnt = ''' + CONVERT(VARCHAR(20),@AgentCnt) + ''' '
          SELECT @where = 'AND '
     END

        SELECT @sql = @sql + 'ORDER BY c.claim_number '

 --SELECT @sql = @sql +  CHAR(13) + CHAR(10) + 'SET QUOTED_IDENTIFIER ON'

 --PRINT @sql
 EXEC (@sql)

END


GO
