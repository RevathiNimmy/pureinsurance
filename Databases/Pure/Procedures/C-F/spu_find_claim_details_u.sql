  
  SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_find_claim_details_u'
GO

CREATE PROCEDURE spu_find_claim_details_u    
  @ClaimNumber varchar(30), 
  @PolicyNumber varchar(30),  
  @ClientShortName varchar(20),  
  @LossDateFrom datetime = NULL,  
  @LossDateTo datetime = NULL,  
  @IncludeClosed int,  
  @UserID int,  
  @SourceID int,  
  @CaseID int      =NULL,  
  @RiskKey int =NULL,  
  @agent_group_cnt INT =0 ,  
  @MaxRowsToFetch INT=-1,  
  @TPA varchar(255) =NULL,  
        @CaseNumber VARCHAR(50)= NULL,  
  @Description varchar(1000) =NULL,  
  @AgentKey INT = NULL,  
  @RetrieveAssociates TINYINT=0  
  
    AS  
    BEGIN  
  
    DECLARE @Claim_number VARCHAR(30),  
            @Insurance_ref VARCHAR(30),  
            @ShortName VARCHAR(20)  
  
    SELECT @Claim_number =@ClaimNumber , @Insurance_ref =@PolicyNumber , @ShortName =@ClientShortName  
  
        DECLARE  
        @sql NVARCHAR(MAX),  
  @where VARCHAR(8000),  
  @Where2 VARCHAR(4000),  
  @Where3 VARCHAR(4000),  
  @SQL2 VARCHAR(1000),  
        @sqlClaimVersion  NVARCHAR(4000)  
  
     IF @CaseID=0  
        SELECT @CaseID=NULL  
  
 IF @RiskKey=0  
        SELECT @RiskKey=NULL  
  
 IF @TPA=''  
        SELECT @TPA=NULL  
  
 SELECT @sql = ''--'SET QUOTED_IDENTIFIER OFF' + CHAR(13) + CHAR(10)  
  
	
 SET @Where2 =''  
    CREATE TABLE #tempClaims(version_id INT,claim_id INT,base_claim_id INT)  
 SELECT @sqlClaimVersion =   'Insert INTO #tempClaims SELECT MAX(Version_id) as version_id,MAX(Claim_Id) as claim_id, base_claim_id  FROM claim WITH (NOLOCK) WHERE '  + CHAR(13)  
  
        IF @ClaimNumber IS NOT NULL  
        BEGIN  
            IF @WHERE2 <>''  
    SELECT @Where2 = @Where2 + ' AND '  
    IF CHARINDEX('%', @ClaimNumber) > 0  
     SELECT @Where2 = @Where2 +   ' claim.claim_number like ''' + @ClaimNumber + ''''  
    ELSE  
     SELECT @Where2 = @Where2 +   ' claim.claim_number = ''' + @ClaimNumber + ''''  
  END  
  
        IF @PolicyNumber IS NOT NULL  
        BEGIN  
            IF @WHERE2 <>''  
    SELECT @Where2 = @Where2 + ' AND '  
                IF CHARINDEX('%', @PolicyNumber) > 0  
    SELECT @Where2 = @Where2 +   ' claim.policy_number like ''' + @PolicyNumber + ''''  
                ELSE  
     SELECT @Where2 = @Where2 +   ' claim.policy_number = ''' + @PolicyNumber + ''''  
  END  
  
        IF @ClientShortName IS NOT NULL  
        BEGIN  
     IF @WHERE2 <>''  
   SELECT @Where2 = @Where2 + ' AND '  
            IF CHARINDEX('%', @ClientShortName) > 0  
            SELECT @Where2 = @Where2 + ' claim.client_short_name like ''' + @ClientShortName + ''''  
            ELSE  
    SELECT @Where2 = @Where2 + ' claim.client_short_name = ''' + @ClientShortName + ''''  
        END  
    IF @WHERE2 <>''  
     SELECT @Where2 = @Where2 + ' AND is_dirty = 0'  
  ELSE  
     SELECT @Where2 = @Where2 + '  is_dirty = 0 '  
  
   SELECT @sqlClaimVersion= @sqlClaimVersion + @Where2  
   SELECT @sqlClaimVersion = @sqlClaimVersion + 'GROUP BY base_claim_id '  
   EXEC(@sqlClaimVersion)  
  
CREATE TABLE  #Claims  
(  
    ID INT                      IDENTITY,  
    InsuranceFileKey            INT,  
    ClaimKey                    INT,  
    ClaimDescription            VARCHAR(1000),  
    ClaimNumber                 VARCHAR(30),  
    InsuranceRef                VARCHAR(255),  
    ClientShortName             VARCHAR(255),  
    ProductDescription          VARCHAR(255),  
    LossDateFrom                DATETIME,  
    ClientName                  VARCHAR(255),  
    ClaimStatusID               INT,  
    ClaimHandlerDescription     VARCHAR(255),  
    InsurerClaimnumber          VARCHAR(20),  
    ClientClaimNumber           VARCHAR(20),  
    ClientTelephoneNumber       VARCHAR(50),  
    ClientTelephoneNumberOffice VARCHAR(50),  
    PrimaryCauseDescription     VARCHAR(255),  
    SecondaryCauseDescription   VARCHAR(255),  
    ProgressStatusDescription   VARCHAR(255),  
    Payments                    NUMERIC(19,2),  
    Reserve                     NUMERIC(19,2),  
     CurrencyISOCode             VARCHAR(10),
    IsDeleted                   INT,
    IsAllowedClosedClaims       INT,
    Insured_cnt                 INT,
    insurance_folder_cnt        INT,
    RiskKey                     INT,
    RiskDescription				VARCHAR(255),
    ReportedDate                DateTime,
    LastModifiedDate            DateTime,
    BaseclaimKey                INT,  
  
    CoverFrom                   DATETIME,  
    CoverTo                     DATETIME,  
    LeadAgentName               VARCHAR(255),  
    NotificationDate            DATETIME,  
    CatastropheCode             VARCHAR(10),  
    CaseNumber                  VARCHAR(255),  
	SearchResultsCol1			VARCHAR(255),
	ClaimStatus					VARCHAR(255),
	AgentKey					INT,
    AssociatedClients           XML,
	ClaimRiskField				VARCHAR(255)
)	
  
  IF @MaxRowsToFetch<>-1  
 BEGIN  
  SELECT @sql = @sql +'SET NOCOUNT ON' + CHAR(13) + CHAR(10)  
  SELECT @sql = @sql +'SET ROWCOUNT '  
  SELECT @sql = @sql + CONVERT(VARCHAR(5),@MaxRowsToFetch) + CHAR(13) + CHAR(10)  
 END  
  
SELECT @sql = @SQL  +  'INSERT INTO #Claims(  
    InsuranceFileKey,  
    ClaimKey,  
    ClaimDescription,  
    ClaimNumber,  
    InsuranceRef,  
    ClientShortName,  
    ProductDescription,  
    LossDateFrom,  
    ClientName,  
    ClaimStatusID,  
    ClaimHandlerDescription,  
    InsurerClaimnumber,  
    ClientClaimNumber,  
    ClientTelephoneNumber,  
    ClientTelephoneNumberOffice,  
    PrimaryCauseDescription,  
    SecondaryCauseDescription,  
    ProgressStatusDescription,  
    Payments,  
    Reserve,  
    CurrencyISOCode,  
    IsDeleted,  
    IsAllowedClosedClaims,  
    Insured_cnt,  
    insurance_folder_cnt,  
    RiskKey,  
    RiskDescription,  
    ReportedDate,  
    LastModifiedDate,  
    BaseclaimKey,  
    CoverFrom,  
    CoverTo,  
    LeadAgentName,  
    NotificationDate,  
    CatastropheCode,  
    CaseNumber,  
 SearchResultsCol1,  
 ClaimStatus,  
 AgentKey,  
    AssociatedClients,  
 ClaimRiskField)  '  
  
        SELECT @sql =  @sql +  
        'SELECT c.policy_id  InsuranceFileKey,  
  c.claim_id  ClaimKey,  
  c.description  ClaimDescription,  
  c.claim_number  ClaimNumber,  
  c.policy_number  InsuranceRef,  
  c.client_short_name  ClientShortName,  
  prd.description  ProductDescription,  
  c.loss_from_date LossDateFrom,  
  c.client_name ClientName,  
  c.claim_status_id ClaimStatusID,  
  ISNULL(h.description,'''') ClaimHandlerDescription,  
  c.insurer_claim_number InsurerClaimNumber,  
  c.client_claim_number ClientClaimNumber,  
  c.client_tel_no ClientTelephoneNumber,  
  c.client_tel_no_off ClientTelephoneNumberOffice,  
  ISNULL(pc.description,'''') PrimaryCauseDescription,  
  ISNULL(sc.description,'''') SecondaryCauseDescription,  
  ISNULL(ps.description,'''') ProgressStatusDescription,  
  0.00 AS Payments,  
        0.00 AS Reserve,  
  
  cu.iso_code CurrencyISOCode,  
  s.is_deleted IsDeleted,  
  s.closed_allow_claims IsAllowedClosedClaims,  
  ifi.insured_cnt,  
  ifi.insurance_folder_cnt,  
  c.risk_type_id as RiskKey,  
        risk.description as RiskDescription,  
  c.reported_date ReportedDate,  
  c.last_modified_date LastModifiedDate,  
  c.base_claim_id BaseClaimKey,  
  
  ifi.cover_start_date AS CoverFrom,  
  ifi.expiry_date AS CoverTo,  
  ag.name AS LeadAgentName,  
  c.reported_date AS NotificationDate,  
  cc.code AS CatastropheCode,  
  
  [CASE].Case_number CaseNumber,  
  c.SearchResultsCol1 AS SearchResultsCol1,  
  cs.description AS ClaimStatus,  
  ifi.lead_agent_cnt,  
  
         CASE '+ Cast(@RetrieveAssociates As Varchar(10)) +'  
         WHEN 1 THEN (SELECT  P.resolved_name +'' (''+ AT.description + '')''  as Name  
                              FROM insurance_file_associates Associate  
                              JOIN Association_Type AT ON Associate.Association_Type_id=AT.Association_Type_id  
                              INNER JOIN party P ON Associate.party_cnt = P.party_cnt  
                              WHERE Associate.Insurance_file_cnt=ifi.insurance_file_cnt AND ISNUll(Associate.Is_Deleted,0) <> 1  
                              FOR XML AUTO, TYPE )  
         ELSE ''''  
         END As AssociatedClients, ' + CHAR(13) +  
   ''''''  +  
  
  
        'FROM Claim c WITH (NOLOCK) ' +  
        'INNER JOIN Risk risk WITH (NOLOCK) ON risk.risk_cnt = c.risk_type_id ' +    CHAR(13) +  
        'INNER JOIN insurance_file ifi WITH (NOLOCK) ON ifi.insurance_file_cnt = c.Policy_id ' +    CHAR(13) +  
        'INNER JOIN product prd WITH (NOLOCK) ON ifi.product_id = prd.product_id ' +  
        'INNER JOIN source s WITH (NOLOCK) ON ifi.source_id = s.source_id ' +  CHAR(13) +  
        'LEFT JOIN Handler h WITH (NOLOCK) ON c.handler_id = h.handler_id ' +  
        'LEFT JOIN primary_cause pc WITH (NOLOCK) ON c.primary_cause_id = pc.primary_cause_id ' +  CHAR(13) +  
        'LEFT JOIN secondary_cause sc WITH (NOLOCK) ON c.secondary_cause_id = sc.secondary_cause_id ' +  
        'LEFT JOIN progress_status ps WITH (NOLOCK) ON c.progress_status_id = ps.progress_status_id ' +  CHAR(13) +  
        'LEFT JOIN currency cu WITH (NOLOCK) ON cu.currency_id = c.currency_id '   +  
     'LEFT JOIN catastrophe_code cc WITH(NOLOCK) ON cc.catastrophe_code_id = c.catastrophe_code_id ' +  CHAR(13) +  
     'LEFT JOIN party ag WITH(NOLOCK) ON ag.party_cnt = ifi.lead_agent_cnt ' +  CHAR(13) +  
     'LEFT JOIN claim_status cs WITH(NOLOCK) ON cs.claim_status_id = c.claim_status_id ' + CHAR(13) +  
     'LEFT JOIN party tpa WITH(NOLOCK) ON tpa.party_cnt = c.other_party_id ' + CHAR(13)  
  
      -- 'INNER JOIN (SELECT MAX(Version_id) as version_id,MAX(Claim_Id) as claim_id, base_claim_id FROM claim WITH (NOLOCK) ' +  
      -- 'WHERE is_dirty = 0 '  
  
 
-- Include the same Where Clause on the sub-query for performance  
     SELECT @sql = @sql + 'INNER JOIN  #tempClaims ON c.claim_id =  #tempClaims.claim_id '  
       SELECT @sql = @sql + 'LEFT JOIN [case] WITH (NOLOCK) ON [case].case_id = c.base_case_id '  
  
     IF @agent_group_cnt <>0  
     BEGIN  
         SELECT @sql = @sql + 'INNER JOIN party p WITH (NOLOCK) ON p.party_cnt=ifi.insured_cnt '  
     END  
  
        SELECT @where = 'WHERE '  
  
        IF @IncludeClosed = 1  
        BEGIN  
            SELECT @sql = @sql + @where + 'c.claim_status_id IN (1,2,4) '  
            SELECT @where = 'AND '  
        END  
   
        IF @claimnumber IS NOT NULL  
        BEGIN  
		
        IF CHARINDEX('%', @claimnumber) > 0  			
            SELECT @sql = @sql + @where + 'c.claim_number like ''' + @claimnumber + ''' '  
        ELSE
			SELECT @sql = @sql + @where + 'c.claim_number = ''' + @claimnumber + ''' '    
            SELECT @where = 'AND '  
        END  
  
  IF @CaseNumber  IS NOT NULL  
        BEGIN  
        IF CHARINDEX('%', @CaseNumber) > 0  
            SELECT @sql = @sql + @where + '[case].case_number like ''' + @CaseNumber  + ''' '  
            ELSE  
    SELECT @sql = @sql + @where + '[case].case_number = ''' + @CaseNumber  + ''' '  
  
            SELECT @where = 'AND '  
        END  
  
        IF @PolicyNumber IS NOT NULL  
        BEGIN  
        IF CHARINDEX('%', @PolicyNumber) > 0  
            SELECT @sql = @sql + @where + 'c.policy_number like ''' + @PolicyNumber + ''' '  
            ELSE  
    SELECT @sql = @sql + @where + 'c.policy_number = ''' + @PolicyNumber + ''' '  
  
            SELECT @where = 'AND '  
        END  
  
        IF @ClientShortName IS NOT NULL  
        BEGIN  
        IF CHARINDEX('%', @ClientShortName) > 0  
            SELECT @sql = @sql + @where + 'c.client_short_name like ''' + @ClientShortName + ''' '  
            ELSE  
    SELECT @sql = @sql + @where + 'c.client_short_name = ''' + @ClientShortName + ''''  
  
            SELECT @where = 'AND '  
        END  
  
        IF @TPA IS NOT NULL  
        BEGIN  
        IF CHARINDEX('%', @TPA) > 0  
            SELECT @sql = @sql + @where + 'ISNULL(tpa.shortname,'''') like ''' + @TPA + ''' '  
            ELSE  
    SELECT @sql = @sql + @where + 'ISNULL(tpa.shortname,'''') = ''' + @TPA + ''' '  
  
            SELECT @where = 'AND '  
        END  
  
    IF @Description IS NOT NULL  
        BEGIN  
        IF CHARINDEX('%', @Description) > 0  
            SELECT @sql = @sql + @where + 'c.description like ''' + @Description + ''' '  
      ELSE  
    SELECT @sql = @sql + @where + 'c.description = ''' + @Description + ''' '  
  
            SELECT @where = 'AND '  
        END  
  
        IF @LossDateFrom IS NOT NULL  
        BEGIN  
          SELECT @sql = @sql + @where + 'CONVERT(DateTIME,Convert(varchar,c.loss_from_date,106)) > ''' + Convert(varchar,DATEADD(d,-1,@LossDateFrom ),106) + ''' '  
            SELECT @where = 'AND '  
        END  
        IF @LossDateTo IS NOT NULL  
        BEGIN  
            SELECT @sql = @sql + @where + 'c.loss_to_date <  ''' +  Convert(varchar,DATEADD(d,1,@LossDateTo ), 106) + ''' '  
            SELECT @where = 'AND '  
        END  
       -- End - Sankar - PN 63381  
     IF @CaseID IS NOT NULL  
        BEGIN  
            SELECT @sql = @sql + @where + 'c.base_case_id = (SELECT base_case_id FROM [case] WITH (NOLOCK) WHERE case_id = ' + CONVERT(VARCHAR(20),@CaseID) + ')'  
            SELECT @where = 'AND '  
        END  
  
IF @RiskKey IS NOT NULL  
        BEGIN  
            SELECT @sql = @sql + @where + 'c.risk_type_id =' + CONVERT(VARCHAR(20),@RiskKey)  
           SELECT @where = 'AND '  
        END  
  
        IF @agent_group_cnt <>0  
        BEGIN  
            SELECT @sql = @sql + @where + '(p.Agent_Cnt IN (SELECT party_cnt FROM party_agent WITH (NOLOCK) WHERE linked_account_group =' + CONVERT(VARCHAR(20),@agent_group_cnt) + ')'  
            + 'OR ifi.lead_agent_cnt IN (SELECT party_cnt FROM party_agent WITH (NOLOCK) WHERE linked_account_group =' +CONVERT(VARCHAR(20),@agent_group_cnt) + '))'  
            SELECT @where = 'AND '  
        END  
  
     IF EXISTS (SELECT NULL FROM hidden_options WHERE option_number = 16 AND value = '1' AND branch_id = @SourceID)  
     BEGIN  
      /*Multi-Ledger : Only allow claims from logged in branch to be selected.*/  
            SELECT @sql = @sql + @where + 'ifi.source_id = ' + CONVERT(VARCHAR(20),@SourceID) + ' '  
            SELECT @where = 'AND '  
        END  
        ELSE  
        BEGIN  
         /*Single-Ledger : Only allow claims from branches that the user has permissions for.*/  
            SELECT @sql = @sql + @where + 'ifi.source_id NOT IN (SELECT source_id FROM pmuser_source WITH (NOLOCK) WHERE user_id = ' + CONVERT(VARCHAR(20),@UserID) + ')'  
            SELECT @where = 'AND '  
     END  
  
        SELECT @sql = @sql + 'ORDER BY c.claim_number '  
  
  
	 
IF @MaxRowsToFetch <>-1  
     BEGIN  
         SELECT @sql = @sql +  CHAR(13) + CHAR(10) + 'SET ROWCOUNT 0'  
         SELECT @sql = @sql +  CHAR(13) + CHAR(10) + 'SET NOCOUNT OFF'  
     END  
  
    EXEC (@SQL)  
  
 UPDATE #Claims SET Payments = CPT.Payments , Reserve = cpt.Reserve
 From #Claims C 
 JOIN (SELECT Claim_id,SUM(R.paid_to_date) AS Payments,SUM(r.revised_reserve) + SUM(r.initial_reserve) AS Reserve 
 FROM Claim_Peril CP 
 JOIN Reserve R ON R.claim_Peril_id = CP.Claim_Peril_id
 WHERE Cp.Claim_id IN (SELECT ClaimKey FROM #Claims)
 GROUP BY CP.Claim_id) CPT ON CPT.Claim_id = C.ClaimKey																   
  

  --********************************************************************************  
-- FILTER OUT POLICIES WHICH DO NOT MATCH OUR SPECIFIED AGENTS  
--********************************************************************************  
  
--If @agent_group_cnt is provided, do not filter by @AgentKey  
IF @agent_group_cnt  <> 0  
BEGIN  
    SET @AgentKey=0  
 END  
  
IF ISNULL(@AgentKey,0) <> 0  
DELETE FROM #Claims  
WHERE AgentKey <> @AgentKey OR AgentKey IS NULL  
  
																			 IF EXISTS (SELECT 1 FROM GIS_Property WHERE ISNULL(is_claim360display,0) = 1)
BEGIN 							 
Declare @claimkey1 INT  
DECLARE @squery NVarchar(4000)  
DECLARE @sql1 NVARCHAR(4000)  
		
        DECLARE CLaimCursor CURSOR  
      FOR  
      SELECT CLaimkey  
      FROM #Claims  
  
      OPEN CLaimCursor  
  
       FETCH NEXT FROM CLaimCursor INTO  
      @claimkey1  
         WHILE @@FETCH_STATUS = 0  
      BEGIN  
  
  
Set @sql1=N'Select @DynamicQuery = +''Select top 1 '' + RTrim(LTrim(GP.Property_Name)) + '' From ''+ RTrim(LTrim(GDM.code)) +''_'' + GB.object_name +'' where '' +RTrim(LTrim(GDM.code))+''_Policy_Binder_id=''+ CAST(GPL.gis_policy_link_id as Varchar(10))
from GIS_Policy_link GPL JOIN  
GIS_Data_Model GDM ON  GDM.gis_data_model_id=gPL.gis_data_model_id  
JOIN GIS_Object GB ON GB.gis_data_model_id= GDM.gis_data_model_id  
JOIN GIS_Property GP ON GP.gis_object_id=GB.gis_object_id  
JOIN Claim clm ON clm.claim_id=GPL.claim_id  
where clm.claim_id='+CAST(@claimkey1 as varchar(10))+' and GP.is_claim360display =1 and GP.is_claim360display IS NOT NULL'  
  
EXEC sp_executesql @sql1,N'@DynamicQuery Varchar(4000) OUTPUT',@DynamicQuery =@squery OUTPUT  
  
if @squery<>''  
BEGIN  
Set @sql1='update #Claims set ClaimRiskField=SUBSTRING((' + @squery  +'),0,30)  where ClaimKey= ' + CAST(@claimkey1 as varchar(10))  
  
  
EXEC(@sql1)  
  
END  
         FETCH NEXT FROM CLaimCursor INTO  
      @claimkey1  
End  
CLOSE CLaimCursor  
      DEALLOCATE CLaimCursor  
END	 
	SELECT  
    InsuranceFileKey,  
    ClaimKey,  
    ClaimDescription,  
    ClaimNumber,  
    InsuranceRef,  
    ClientShortName,  
    ProductDescription,  
    LossDateFrom,  
    ClientName,  
    ClaimStatusID,  
    ClaimHandlerDescription,  
    InsurerClaimnumber,  
    ClientClaimNumber,  
    ClientTelephoneNumber,  
    ClientTelephoneNumberOffice,  
    PrimaryCauseDescription,  
    SecondaryCauseDescription,  
    ProgressStatusDescription,  
    Payments,  
    Reserve,  
    CurrencyISOCode,  
    IsDeleted,  
    IsAllowedClosedClaims,  
    Insured_cnt,  
    insurance_folder_cnt,  
    RiskKey,  
    RiskDescription,  
    ReportedDate,  
    LastModifiedDate,  
    BaseclaimKey,  
    CoverFrom,  
    CoverTo,  
    LeadAgentName,  
    NotificationDate,  
    CatastropheCode,  
    CaseNumber,  
 SearchResultsCol1,  
    ClaimStatus,  
    AssociatedClients,  
 ClaimRiskField  
    FROM #Claims  
  
	  
    DROP TABLE #tempClaims  
   
    DROP TABLE #Claims  
END  
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


