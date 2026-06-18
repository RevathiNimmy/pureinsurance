SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetMultiPolicyClaimsByRiskIndex'
GO

CREATE PROCEDURE spu_SAM_GetMultiPolicyClaimsByRiskIndex
 @RiskIndex VARCHAR(255),
 @ClaimNumber VARCHAR(30),
 @PolicyNumber VARCHAR(30),
 @ClientShortName VARCHAR(20),
 @LossDateFrom DATETIME = NULL,
 @LossDateTo DATETIME = NULL,
 @IncludeClosed INT,
 @UserID INT,
 @SourceID INT,
 @ClaimStatus INT  = 1,
 @TransactionType VARCHAR(10),
 @RiskKey INT =NULL ,
    @agent_group_cnt INT =0,
    @MaxRowsToFetch INT=-1,
    @TPA   VARCHAR(255)=NULL,
    @CaseNumber VARCHAR(50)= NULL,
    @Description varchar(1000) =NULL,
    @RetrieveAssociates TINYINT=0
AS

-- ******************************************************************************************************
-- Stored Procedure spu_SAM_GetMultiPolicyClaimsByRiskIndex
-- ******************************************************************************************************
-- Revision             Description of Modification             Date        Who
-- --------             ---------------------------             ----        ---
-- 1.0                  Created                                 02/06/2006  Ramakant Singh
-- ******************************************************************************************************

BEGIN

SET NOCOUNT ON

DECLARE @sSQL NVARCHAR(Max)

DECLARE @GIS_Data_Model__code VARCHAR(10)

DECLARE @nParty_cnt as INT


DECLARE @tmpGISSearchRisk__insurance_file_cnt INT,
   @tmpGISSearchRisk__gis_policy_link_id INT,
   @tmpGISSearchRisk__object_name VARCHAR(70),
   @tmpGISSearchRisk__property_name VARCHAR(70),
   @tmpGISSearchRisk__value VARCHAR(255),
   @tmpGISSearchRisk__claim_id INT

IF (@UserID<>0)

 BEGIN

    select @nParty_cnt= party_cnt from Pmuser where user_id=@UserID
 END

 SELECT @nParty_cnt=ISNULL(@nParty_cnt,0)

IF (@RiskIndex='')
 SELECT @RiskIndex='%'

--IF (NOT RIGHT(@RiskIndex,1)='%')
-- SELECT @RiskIndex=@RiskIndex + '%'

--Create Temporary Tables
CREATE TABLE #tmpGISSearchFind (
  insurance_file_cnt INT,
    gis_policy_link_id INT,
    object_name VARCHAR(70),
    property_name VARCHAR(70),
    value VARCHAR(255),
  insurance_ref VARCHAR(255),
    --Prakash: This sp was not working because spu_gis_search_property_find returns claim_id also.
    claim_id int
   )

CREATE TABLE #tmpGISSearchRisk (
  insurance_file_cnt INT,
    gis_policy_link_id INT,
    object_name VARCHAR(70),
    property_name VARCHAR(70),
    value VARCHAR(255),
    --Prakash: This sp was not working because spu_gis_search_property_find returns claim_id also.
    claim_id int
   )

CREATE TABLE #tmpSearchResult (
  Policy_id INT,
  Claim_id INT,
  Description VARCHAR(255),
  Claim_Number VARCHAR(255),
  Policy_Number VARCHAR(255),
  client_short_name VARCHAR(255),
  CoverFrom DATETIME,
  CoverTo DATETIME,
  LeadAgentName VARCHAR (255),
  product VARCHAR(255),
  loss_from_date DATETIME,
  client_name VARCHAR(255),
  claim_status_id INT,
  Handler VARCHAR(255),
  insurer_claim_number VARCHAR(255),
  client_claim_number VARCHAR(255),
  client_tel_no VARCHAR(255),
  client_tel_no_off VARCHAR(255),
  Primary_Cause VARCHAR(255),
  Secondary_Cause VARCHAR(255),
  Progress_Status VARCHAR(255),
  Payments NUMERIC(18,2),
  Reserve NUMERIC(18,2),
  iso_code VARCHAR(255),
  is_deleted TINYINT,
  closed_allow_claims TINYINT,
  Case_number VARCHAR(50),
  SearchResultsCol1 VARCHAR(255),
  ClaimStatus		VARCHAR(255),
  ClaimRiskField	VARCHAR(255),
  RiskDescription VARCHAR(255)
  )

--Retrieve all the GIS_Data_Model__code for GIS_Data_Model_Type is 'CLAIM'
DECLARE GIS_Data_Model__Code_Cursor CURSOR FAST_FORWARD FOR
Select  LTRIM(RTRIM(GDM.Code)) from gis_data_model GDM
	JOIN GIS_Data_Model_Type T ON GDM.GIS_Data_Model_Type_Id = T.GIS_Data_Model_Type_Id
	WHERE T.CODE IN('CLAIM','RISK') AND GDM.is_deleted = 0


OPEN GIS_Data_Model__Code_Cursor
FETCH NEXT FROM GIS_Data_Model__Code_Cursor INTO @GIS_Data_Model__code
WHILE @@FETCH_STATUS = 0
BEGIN
 IF(@TransactionType='C_CP' OR @TransactionType='C_CR' OR @TransactionType ='' )
 BEGIN
  INSERT #tmpGISSearchRisk EXECUTE spu_gis_search_property_risk   @gis_data_model_code = @GIS_Data_Model__code,
       @search_object_name = NULL, @search_value = @RiskIndex
 END

 ELSE
 BEGIN
  INSERT #tmpGISSearchFind EXECUTE spu_gis_search_property_find   @gis_data_model_code = @GIS_Data_Model__code,
       @search_object_name = NULL, @search_value = @RiskIndex,
       @is_insurance_ref_reqd = 1

 END

 FETCH NEXT FROM GIS_Data_Model__Code_Cursor INTO @GIS_Data_Model__code
END
CLOSE GIS_Data_Model__Code_Cursor
DEALLOCATE GIS_Data_Model__Code_Cursor

IF(@TransactionType='C_CP' OR @TransactionType='C_CR' OR @TransactionType= '')
 DECLARE tmpGISSearch_Cursor CURSOR FAST_FORWARD FOR
   SELECT
    insurance_file_cnt,
    0,
      object_name,
      property_name,
    value,
	claim_id
   FROM #tmpGISSearchRisk

ELSE
 DECLARE tmpGISSearch_Cursor CURSOR FAST_FORWARD FOR
   SELECT
    insurance_file_cnt,
    gis_policy_link_id,
      object_name,
      property_name,
      value,
      claim_id
   FROM #tmpGISSearchFind

OPEN tmpGISSearch_Cursor
FETCH NEXT FROM tmpGISSearch_Cursor
  INTO  @tmpGISSearchRisk__insurance_file_cnt,
     @tmpGISSearchRisk__gis_policy_link_id,
     @tmpGISSearchRisk__object_name,
     @tmpGISSearchRisk__property_name,
     @tmpGISSearchRisk__value,
     @tmpGISSearchRisk__claim_id
WHILE @@FETCH_STATUS = 0
BEGIN

        --Build the SQL select statement according to the parameters passed
        --Select statement to select all details relating to values entered
        SELECT @sSQL = 'SELECT Claim.Policy_id InsuranceFileKey,
   Claim.Claim_id ClaimKey,
   Claim.Description ClaimDescription,
   Claim.Claim_Number ClaimNumber,
   Claim.Policy_Number InsuranceRef,
   Claim.client_short_name ClientShortName,
   ifl.cover_start_date  CoverFrom,
   ifl.Expiry_date  CoverTo,
   Claim.insurer_contact LeadAgentName,
   (SELECT  prd.description FROM product prd,insurance_file ifi
    Where ifi.insurance_file_cnt = claim.Policy_id
    AND ifi.product_id = prd.product_id
     ) ProductDescription,
   Claim.loss_from_date LossDateFrom,
   Claim.client_name ClientName,
   Claim.claim_status_id ClaimStatusID,
   (SELECT ISNULL(Handler.description,'''') FROM Handler WHERE handler_id = Claim.handler_id) ClaimHandlerDescription,
   Claim.insurer_claim_number InsurerClaimNumber,
   Claim.client_claim_number ClientClaimNumber,
   Claim.client_tel_no ClientTelephoneNumber,
   Claim.client_tel_no_off ClientTelephoneNumberOffice,
   (SELECT ISNULL(Primary_Cause.description,'''')
    FROM Primary_Cause
    WHERE primary_cause_id = Claim.primary_cause_id) PrimaryCauseDescription,
       (SELECT ISNULL(Secondary_Cause.description,'''')
    FROM Secondary_Cause
    WHERE secondary_cause_id = Claim.secondary_cause_id) SecondaryCauseDescription,
   (SELECT ISNULL(Progress_Status.description,'''')
    FROM Progress_Status
    WHERE progress_status_id = Claim.progress_status_id) ProgressStatusDescription,
           isnull((select sum(isnull(r.paid_to_date,0))
    from reserve r
        join claim_peril cp on
    r.claim_peril_id = cp.claim_peril_id
        and claim_id=claim.claim_id),0) Payments,
                 ((select isnull(sum(isnull(r.revised_reserve,0)),0) + isnull(sum(isnull(r.initial_reserve,0)),0)
          from reserve r
    join claim_peril cp on
     r.claim_peril_id = cp.claim_peril_id
        and claim_id=claim.claim_id)) Reserve,
   (select  cu.iso_code from currency cu Where cu.currency_id = claim.currency_id) CurrencyISOCode,
   (select s.is_deleted
    from source s
    inner join insurance_file ifi ON
     ifi.source_id = s.source_id
    Where ifi.insurance_file_cnt = claim.Policy_id) IsDeleted,
   (select s.closed_allow_claims
    from source s
     inner join insurance_file ifi ON
     ifi.source_id = s.source_id
    Where ifi.insurance_file_cnt = claim.Policy_id) IsAllowedClosedClaims,
			  [CASE].Case_number,
			  SearchResultsCol1,
			  cs.description AS ClaimStatus,
			  ISNULL(R.Description,'''') AS ClaimRiskField,
			  (Select risk.description from risk risk where risk.risk_cnt = claim.risk_type_id) RiskDescription '  

 IF(@tmpGISSearchRisk__gis_policy_link_id<>0)
	BEGIN
  SELECT @sSQL =  @sSQL + ' FROM Claim, Gis_Policy_Link gpl '
		SELECT @sSQL = @sSQL + 'LEFT JOIN [case] WITH (NOLOCK) ON [case].base_case_id = Claim.base_case_id '
		SELECT @SsQL = @sSQL + ' LEFT JOIN claim_status cs WITH(NOLOCK) ON cs.claim_status_id = claim.claim_status_id '
	END
 ELSE
	BEGIN
  SELECT @sSQL =  @sSQL + ' FROM Claim '
		SELECT @sSQL = @sSQL + 'LEFT JOIN [case] WITH (NOLOCK) ON [case].base_case_id = Claim.base_case_id '
		SELECT @SsQL = @sSQL + ' LEFT JOIN claim_status cs WITH(NOLOCK) ON cs.claim_status_id = claim.claim_status_id '
	END
	SELECT @sSQL =  @sSQL +'LEFT JOIN risk R WITH (NOLOCK) ON Claim.risk_type_id = R.risk_cnt '
  IF (@TPA IS NOT NULL)
	SELECT @sSQL =  @sSQL + ' LEFT JOIN party tpa WITH(NOLOCK) ON tpa.party_cnt = claim.other_party_id'


 IF @agent_group_cnt <>0
 BEGIN
  SELECT @sSQL = @sSQL + ',insurance_file ifl,party p '
 END
 ELSE
 SELECT @sSQL = @sSQL + ',insurance_file ifl '


  -- start (rajeev) (agent association)

  set @nParty_cnt=ISNULL(@nParty_cnt,0)
  IF @nParty_cnt  <>0
   BEGIN
     SELECT @sSQL = @sSQL + ',party p '
   END
  -- End (rajeev)
 --Prakash: Restructuring the code

 SELECT @sSQL=@sSQL + ' WHERE 1=1'
 IF (@ClaimNumber IS NOT NULL)
 BEGIN
  SELECT @sSQL=@sSQL + ' AND Claim.Claim_Number Like ''' + @ClaimNumber + ''''
 END

   IF (@CaseNumber  IS NOT NULL)
  BEGIN
   SELECT @sSQL=@sSQL + ' AND [case].case_number Like ''' + @CaseNumber  + ''''
  END

 IF (@PolicyNumber IS NOT NULL)
 BEGIN
   SELECT @sSQL=@sSQL + ' AND Claim.Policy_Number Like ''' + @PolicyNumber + ''''
 END
	IF (@Description IS NOT NULL)
	BEGIN
		SELECT @sSQL=@sSQL + ' AND Claim.Description Like ''' + @Description + ''''
	END

 IF (@ClientShortName IS NOT NULL)
 BEGIN
   SELECT @sSQL=@sSQL + ' AND Claim.Client_Short_Name Like ''' + @ClientShortName + ''''
 END

 IF (@LossDateFrom IS NOT NULL)
 BEGIN
   SELECT @sSQL=@sSQL + ' AND CONVERT(DateTIME,Convert(varchar,claim.loss_from_date,106)) >=''' + CONVERT(varchar, @LossDateFrom ,106) + ''''
 END

 IF (@LossDateTo IS NOT NULL)
 BEGIN

    SELECT @sSQL=@sSQL + ' AND CONVERT(DateTIME,Convert(varchar,claim.loss_to_date,106)) <=''' + CONVERT(varchar, @LossDateTo ,106) + ''''
 END

 IF (@ClaimStatus=1 AND @IncludeClosed=0) 
 BEGIN
   SELECT @sSQL=@sSQL + ' AND (Claim.Claim_Status_id IN (1,2,4))'
 END

 IF (@ClaimStatus=1 AND @IncludeClosed=1)  
 BEGIN  
   SELECT @sSQL=@sSQL + ' AND (Claim.Claim_Status_id IN (1,2,3,4))'  
 END  

 IF (@tmpGISSearchRisk__gis_policy_link_id<>0)
 BEGIN
   SELECT @sSQL=@sSQL + ' AND gpl.gis_policy_link_id = ' + CAST(@tmpGISSearchRisk__gis_policy_link_id AS NVARCHAR) + '
              AND gpl.claim_id = Claim.claim_id'
 END
 ELSE
 BEGIN
  IF (@tmpGISSearchRisk__insurance_file_cnt<>0)
  BEGIN
    SELECT @sSQL=@sSQL + ' AND Claim.Policy_id = ' + CAST(@tmpGISSearchRisk__insurance_file_cnt AS NVARCHAR)
  END
IF (@tmpGISSearchRisk__claim_id<>0)
  BEGIN
    SELECT @sSQL=@sSQL + ' AND Claim.claim_id = ' + CAST(@tmpGISSearchRisk__claim_id AS NVARCHAR)
  END
  END

 IF @agent_group_cnt <>0
 BEGIN
     SELECT  @sSQL=@sSQL+ ' AND ifl.insurance_file_cnt=Claim.Policy_id AND p.party_cnt=ifl.insured_cnt'
            + ' AND (p.Agent_Cnt IN (SELECT party_cnt FROM party_agent WHERE linked_account_group =' + CONVERT(VARCHAR(20),@agent_group_cnt) + ')'
            + ' OR ifl.lead_agent_cnt IN (SELECT party_cnt FROM party_agent WHERE linked_account_group =' +CONVERT(VARCHAR(20),@agent_group_cnt) + '))'
 END
 ELSE
  SELECT  @sSQL=@sSQL+ ' AND ifl.insurance_file_cnt=Claim.Policy_id'
  -- start (rajeev) ( for Agent Association)
  IF @nParty_cnt <>0

   BEGIN

       SELECT  @sSQL=@sSQL+ '  AND p.party_cnt=ifl.insured_cnt'

	  SELECT  @sSQL=@sSQL+ ' AND ifl.lead_agent_cnt IN (' +CONVERT(VARCHAR(20),@nParty_cnt) + ')'
   END
   -- End (rajeev)
   IF (@TPA IS NOT NULL)
     SELECT  @sSQL=@sSQL+ ' AND tpa.shortname like ''' + CAST(@TPA AS NVARCHAR) + ''''

 INSERT #tmpSearchResult EXECUTE sp_executesql @sSQL

 FETCH NEXT FROM tmpGISSearch_Cursor
  INTO  @tmpGISSearchRisk__insurance_file_cnt,
     @tmpGISSearchRisk__gis_policy_link_id,
     @tmpGISSearchRisk__object_name,
     @tmpGISSearchRisk__property_name,
     @tmpGISSearchRisk__value,
     @tmpGISSearchRisk__claim_id
END
CLOSE tmpGISSearch_Cursor
DEALLOCATE tmpGISSearch_Cursor

IF @MaxRowsToFetch<>-1
BEGIN
SET ROWCOUNT @MaxRowsToFetch
END

SELECT DISTINCT
  #tmpSearchResult.Policy_id  InsuranceFileKey,
  #tmpSearchResult.Claim_id ClaimKey,
  #tmpSearchResult.Description ClaimDescription,
  #tmpSearchResult.Claim_Number ClaimNumber,
  #tmpSearchResult.Policy_Number InsuranceRef,
  #tmpSearchResult.client_short_name ClientShortName,
  #tmpSearchResult.CoverFrom CoverFrom,
  #tmpSearchResult.CoverTo CoverTo,
  #tmpSearchResult.LeadAgentName LeadAgentName,
  #tmpSearchResult.product ProductDescription,
  #tmpSearchResult.loss_from_date LossDateFrom,
  #tmpSearchResult.client_name ClientName,
  #tmpSearchResult.claim_status_id ClaimStatusID,
  #tmpSearchResult.Handler ClaimHandlerDescription,
  #tmpSearchResult.insurer_claim_number InsurerClaimNumber,
  #tmpSearchResult.client_claim_number ClientClaimNumber,
  #tmpSearchResult.client_tel_no ClientTelephoneNumber,
  #tmpSearchResult.client_tel_no_off ClientTelephoneNumberOffice,
  #tmpSearchResult.Primary_Cause PrimaryCauseDescription,
  #tmpSearchResult.Secondary_Cause SecondaryCauseDescription,
  #tmpSearchResult.Progress_Status ProgressStatusDescription,
  #tmpSearchResult.Payments,
  #tmpSearchResult.Reserve,
  #tmpSearchResult.iso_code CurrencyISOCode,
  #tmpSearchResult.is_deleted IsDeleted,
  #tmpSearchResult.closed_allow_claims IsAllowedClosedClaims,
  #tmpSearchResult.SearchResultsCol1,
  #tmpSearchResult.RiskDescription RiskDescription,
  claim.info_only InfoOnly,
  claim.reported_date ReportedDate,
  claim.last_modified_date LastModifiedDate,
  claim.base_claim_id BaseClaimKey ,
  #tmpSearchResult.Case_number CaseNumber,
  #tmpSearchResult.SearchResultsCol1,
  #tmpSearchResult.ClaimStatus,
  #tmpSearchResult.ClaimRiskField,


    Cast((CASE @RetrieveAssociates
        WHEN 1 THEN (SELECT
                 P.resolved_name +' ('+ AT.description + ')'  as Name
                FROM insurance_file_associates Associate
                INNER JOIN party P ON P.party_cnt = Associate.party_cnt
                INNER JOIN Association_Type AT ON Associate.Association_Type_id=AT.Association_Type_id
                    Where Associate.Insurance_file_cnt=#tmpSearchResult.Policy_id And
                    (CASE  WHEN ISNUll(Associate.Is_Deleted,0) = 1  And ISNull(Associate.date_removed,Dateadd(year,-99,Getdate())) <= GETDATE() THEN 0 ELSE 1  END =1)
                        FOR XML AUTO, TYPE )
        ELSE ''
        END)As Varchar(Max)) As AssociatedClients,
		#tmpSearchResult.ClaimRiskField 

 FROM #tmpSearchResult

  INNER JOIN Claim on
  Claim.Claim_Id = #tmpSearchResult.Claim_Id

  
 -- ensure only the latest versions of the claims
 -- are returned
WHERE #tmpSearchResult.claim_id IN(Select MAX(claim_id) from claim WHERE Claim.is_dirty=0 
    GROUP by base_claim_id)

IF @MaxRowsToFetch<>-1
BEGIN
SET ROWCOUNT 0
END

DROP TABLE #tmpSearchResult
DROP TABLE #tmpGISSearchRisk
DROP TABLE #tmpGISSearchFind

SET NOCOUNT ON

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO