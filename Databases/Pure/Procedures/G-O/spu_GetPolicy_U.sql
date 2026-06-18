SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetPolicy_U'
GO

CREATE PROCEDURE spu_GetPolicy_U  
    @PolicyNo VARCHAR(30) = NULL,    
    @PartyShortName VARCHAR(20) = NULL,    
    @PostCode VARCHAR(20) = NULL,    
    @PolicyStartDate DATETIME = NULL,    
    @PolicyEndDate DATETIME = NULL,    
    @InsuranceFileCnt int = NULL,    
    @GISValue VARCHAR(100) = NULL,    
    @ClaimDate DATETIME = NULL,    
    @SourceID INT,    
    @AgentKey INT = NULL,    
    @CoverNoteSheetNumber INT = NULL,    
    @agent_group_cnt INT =0,
	@user_id INT = 0,
	@RetrieveAssociates  TINYINT =0
AS    

DECLARE @claim_Type_Basis_code AS VARCHAR(10)
DECLARE @nRisk_Id AS INT
DECLARE  @dtCoverStartDate AS DATETIME=NULL
DECLARE @dtExpiryDate  AS DATETIME =NULL   
    
DECLARE @SQL1 VARCHAR(255),    
        @SQL2 VARCHAR(255),    
        @SQL3 VARCHAR(255),    
        @StartDateSQL VARCHAR(255),    
        @EndDateSQL VARCHAR(255),    
		@ClaimDateSQL VARCHAR(511),    
        @GROUPBYSQL VARCHAR(255),    
        @PolicyNoSQL VARCHAR(255),    
        @ClientSQL VARCHAR(255),    
        @PolicyStartSQL VARCHAR(255),    
        @PolicyEndSQL VARCHAR(255),    
        @PolicyCntSQL VARCHAR(255),    
		@PolicyClaimDateSQL VARCHAR(511)   
    
-- INITIALISE SQL VARIABLES    
SELECT  @SQL1 = '',    
        @SQL2 = '',    
        @SQL3 = '',    
        @StartDateSQL = '',    
        @EndDateSQL = '',    
        @GROUPBYSQL = '',    
        @PolicyNoSQL = '',    
        @ClientSQL = '',    
        @PolicyStartSQL = '',    
        @PolicyEndSQL = '',    
        @PolicyCntSQL = '',    
		@PolicyClaimDateSQL = ''    
  
Declare @LapsedDate datetime,  
 @UnionSQL varchar(1000)   
  
Select  @UnionSQL=''  
  
IF @PolicyNo IS NOT NULL
    BEGIN
         SELECT @nRisk_Id=IFRL.risk_cnt,@dtCoverStartDate=IFL.cover_start_date,@dtExpiryDate=IFL.[expiry_date] FROM insurance_file_risk_link IFRL JOIN Insurance_File IFL ON IFRL.insurance_file_cnt=IFL.insurance_file_cnt 
		 WHERE Insurance_ref LIKE @PolicyNo 
         and @ClaimDate between IFL.cover_start_date and ifl.[expiry_date]
IF  @nRisk_Id IS  NULL 
        RETURN

 SELECT @claim_Type_Basis_code=ctb.code
 FROM   risk r JOIN
     	risk_type rt on r.risk_type_id=rt.risk_type_id
		JOIN claims_type_basis ctb
 ON     rt.claims_type_basis_id= ctb.claims_type_basis_id
 WHERE  r.risk_cnt = @nRisk_Id
		
IF @claim_Type_Basis_code='CM'  
BEGIN
  SET @PolicyStartDate=@dtCoverStartDate
  SET @PolicyEndDate=@dtExpiryDate
END
END 

IF @PolicyNo IS NOT NULL    
BEGIN    
    CREATE TABLE #TempPolicyRef    
        (    
        insurance_file_cnt INT NULL ,
        insurance_file_status_id INT NULL 
        )    
    
    INSERT INTO #TempPolicyRef    
        SELECT insurance_file_cnt,insurance_file_status_id FROM insurance_file WHERE insurance_ref LIKE @PolicyNo    
    
    -- PUT POLICY REF INTO OUR CRITERIA    
    SELECT @PolicyNoSQL = 'AND ifi.insurance_file_cnt IN ( '    
    SELECT @PolicyNoSQL = @PolicyNoSQL + 'SELECT insurance_file_cnt FROM #TempPolicyRef ) '    
END    
    
--********************************************************************************    
-- DO WE HAVE CLIENT NAME    
--********************************************************************************    
IF @PartyShortName IS NOT NULL    
BEGIN    
    CREATE TABLE #TempParty    
            (    
            party_cnt INT NULL    
            )    
    
    INSERT INTO #TempParty    
        SELECT party_cnt FROM party WHERE shortname LIKE @PartyShortName    
    
    -- PUT CLIENTS INTO OUR CRITERIA    
    SELECT @ClientSQL = 'AND ifi.insurance_folder_cnt = ifo.insurance_folder_cnt '    
    SELECT @ClientSQL = @ClientSQL + 'AND ifo.insurance_holder_cnt IN ( '    
    SELECT @ClientSQL = @ClientSQL + 'SELECT party_cnt FROM #TempParty ) '    
    
END    
    
--********************************************************************************    
-- CREATE TEMP TABLE TO STORE START AND END DATE    
-- THE CONVERT FUNCTION WILL NOT WORK    
--********************************************************************************    
IF @PolicyStartDate IS NOT NULL OR @PolicyEndDate IS NOT NULL OR @ClaimDate IS NOT NULL    
BEGIN    
    CREATE TABLE #TempDate    
        (    
        start_date DATETIME NULL,    
        end_date DATETIME NULL,    
  claim_date DATETIME NULL    
        )    
    
    -- PUT THE PASSED IN START AND END DATE IN TEMP TABLE    
    INSERT INTO #TempDate SELECT @PolicyStartDate, @PolicyEndDate, @ClaimDate    
END    
    
--********************************************************************************    
-- SQL TO DECLARE TEMP VARIABLE FOR START DATE    
-- AND USE IT IN OUR CRITERIA    
--********************************************************************************    
IF @PolicyStartDate IS NOT NULL    
    BEGIN    
        -- BUILD OUR TEMP START DATE VARIALE    
        SELECT @StartDateSQL = 'DECLARE @start_date DATETIME '    
        SELECT @StartDateSQL = @StartDateSQL + 'SELECT @start_date = start_date FROM #TempDate '    
    
        -- USE IT IN OUR CRITERIA    
        SELECT @PolicyStartSQL = 'AND ifi.cover_start_date >= @start_date '    
    END   
-- PN77530
--********************************************************************************    
-- SQL TO DECLARE TEMP VARIABLE FOR END DATE    
-- AND USE IT IN OUR CRITERIA    
--********************************************************************************    
IF @PolicyEndDate IS NOT NULL    
    BEGIN
        -- BUILD OUR TEMP END DATE VARIALE
 SELECT @EndDateSQL = 'DECLARE @end_date DATETIME '
    SELECT @EndDateSQL = @EndDateSQL + 'SELECT @end_date = end_date FROM #TempDate '    
    
    -- USE IT IN OUR CRITERIA    
    SELECT @PolicyEndSQL = 'AND ifi.expiry_date <= @end_date '    
    END
    
--********************************************************************************    
-- INCLUDE CLAIM DATE IN CRITERIA    
--********************************************************************************    
IF @ClaimDate IS NOT NULL    
  BEGIN   
	SELECT @LapsedDate = MAX(lapsed_date) FROM insurance_file 
  WHERE insurance_ref LIKE @PolicyNo and ISNULL(lapsed_date, '1900-01-01 00:00:00.000') > '1900-01-01 00:00:00.000'
 
   -- BUILD OUR TEMP START DATE VARIALE    
  SELECT @ClaimDateSQL = 'DECLARE @claim_date DATETIME '    
  SELECT @ClaimDateSQL = @ClaimDateSQL + 'SELECT @claim_date = claim_date FROM #TempDate '    

  -- USE IT IN OUR CRITERIA    
  SELECT @PolicyClaimDateSQL = 'AND ((((ifi.cover_start_date <= @claim_date AND ifi.expiry_date >= @claim_date AND ifi.insurance_file_type_id<>8 AND ISNULL(ifi.insurance_file_status_id,0)<>1) '    
  SELECT @PolicyClaimDateSQL = @PolicyClaimDateSQL + 'OR (ifi.inception_date_tpi <= @claim_date AND ifi.lapsed_date >= @claim_date AND ifi.insurance_file_type_id=8 AND ifi.insurance_file_status_id=1)) '    

  SELECT @PolicyClaimDateSQL = @PolicyClaimDateSQL + ' AND p.Valid_Policy_Version_At_Loss_Date = 1) OR p.Valid_Policy_Version_At_Loss_Date = 0)  AND ifi.cover_start_date <= @claim_date AND ifi.expiry_date >= @claim_date '
  SELECT @UnionSQL ='UNION '      
  SELECT @UnionSQL =@UnionSQL + ' SELECT ifi.insurance_file_cnt'   
  SELECT @UnionSQL =@UnionSQL + ' FROM insurance_file ifi, product p '  
  --Start Written status
  SELECT @UnionSQL =@UnionSQL + ' WHERE ifi.product_id = p.product_id AND (ifi.insurance_file_type_id IN (2,5,6,9,11) OR (p.Valid_Policy_Version_At_Loss_Date = 0 and ifi.insurance_file_type_id = 8 and @claim_date>isnull(lapsed_date,''1899-12-29 00:00:00.000'')))'  
  --Start Written status

  IF @PolicyStartSql IS NOT NULL
   SELECT @UnionSQL =@UnionSQL + @PolicyStartSql
  IF @PolicyEndSql IS NOT NULL
   SELECT @UnionSQL =@UnionSQL + @PolicyEndSql

  IF @PolicyNo IS NOT NULL   
   SELECT @UnionSQL =@UnionSQL + ' AND ifi.insurance_file_cnt IN ( SELECT insurance_file_cnt FROM #TempPolicyRef) '     
  SELECT @UnionSQL =@UnionSQL + ' AND ((ifi.cover_start_date <= @claim_date AND ifi.expiry_date >= @claim_date AND p.Valid_Policy_Version_At_Loss_Date = 1) '   
  SELECT @UnionSQL =@UnionSQL + ' OR (p.Valid_Policy_Version_At_Loss_Date = 0)) '  
  SELECT @UnionSQL =@UnionSQL + ' AND ifi.insurance_file_type_id <> 8 '  
  SELECT @UnionSQL =@UnionSQL + ' AND ifi.insurance_file_status_id = 1 ' 

   IF @PartyShortName IS NOT NULL   
   SELECT @UnionSQL =@UnionSQL + ' AND ifi.insured_cnt IN ( SELECT party_cnt FROM #TempParty) ' 
 
  IF @LapsedDate is not null  
   SELECT @UnionSQL =@UnionSQL + ' AND ifi.cover_start_date< ''' + CONVERT(Varchar,@LapsedDate,106) + ''')'   
  ELSE  
   SELECT @UnionSQL =@UnionSQL + ')'  
  END      
    
-- DO WE HAVE INSURANCE FILE CNT    
IF @InsuranceFileCnt IS NOT NULL    
    SELECT @PolicyCntSQL = 'AND ifi.insurance_file_cnt = ' + CONVERT(VARCHAR(20), @InsuranceFileCnt) + ' '    
    
--********************************************************************************    
-- CREATE TEMPORARY TABLE TO HOLD LATEST VERSION    
-- OF POLICIES WHICH MATCH OUR CRITERIA    
--********************************************************************************    
CREATE TABLE #tempLatestPolicyCnt    
(    
    Insurance_File_Cnt INT,
    Insurance_folder_cnt INT,
    RowNum int              
)    
    
    -- BUILD UP SQL STATEMENT TO SELECT POLICIES WHICH MATCH OUR CRITERIA    
    SELECT @SQL1 = 'INSERT INTO #tempLatestPolicyCnt (Insurance_file_cnt, RowNum)'  

IF @ClaimDate IS NOT NULL 
BEGIN  
	SELECT @SQL1 = @SQL1 + 'SELECT insurance_file_cnt,ROW_NUMBER() OVER (PARTITION BY insurance_folder_cnt ORDER BY cover_start_date DESC ,insurance_file_cnt DESC) FROM insurance_file '    
	SELECT @SQL1 = @SQL1 + 'WHERE insurance_file_cnt IN '    
	    
	SELECT @SQL2 = @SQL2 + '( '    
	SELECT @SQL2 = @SQL2 + 'SELECT ifi.insurance_file_cnt '    
	SELECT @SQL2 = @SQL2 + 'FROM insurance_file ifi '    
END  
ELSE
BEGIN
	SELECT @SQL1 = @SQL1 + 'SELECT insurance_file_cnt FROM insurance_file '  
	SELECT @SQL1 = @SQL1 + 'WHERE insurance_file_cnt IN '  
	  
	SELECT @SQL2 = @SQL2 + '( '  
	SELECT @SQL2 = @SQL2 + 'SELECT MAX(ifi.insurance_file_cnt) '  
	SELECT @SQL2 = @SQL2 + 'FROM insurance_file ifi '   
END  

--************************************************************************    
-- INCLUDE INSURANCE FOLDER TO VALIDATE CLIENT CODE    
--************************************************************************    
IF @PartyShortName IS NOT NULL    
    SELECT @SQL2 = @SQL2 + ', insurance_folder ifo '    
    
  SELECT @SQL2 = @SQL2 + ', product p WHERE ifi.product_id = p.product_id AND '    
    
 SELECT @SQL3 = @SQL3 + '(ifi.insurance_file_type_id IN  (2,5,6,9,11) OR (p.Valid_Policy_Version_At_Loss_Date = 0 and ifi.insurance_file_type_id = 8 and @claim_date>isnull(lapsed_date,''1899-12-29 00:00:00.000'')))'     

IF @ClaimDate IS NOT NULL         
	SELECT @GROUPBYSQL = ''
ELSE
BEGIN
	SELECT @GROUPBYSQL = @GROUPBYSQL + 'GROUP BY ifi.insurance_folder_cnt '
	SELECT @GROUPBYSQL = @GROUPBYSQL + ') ' 
END  
   
--********************************************************************************    
-- RUN SQL STATEMENT    
--********************************************************************************    
EXEC (@StartDateSQL + @ClaimDateSQL + @EndDateSQL + @SQL1 + @SQL2 + @SQL3 + @PolicyNoSQL + @ClientSQL + @PolicyStartSQL + @PolicyEndSQL + @PolicyClaimDateSQL + @PolicyCntSQL + @UnionSQL + @GROUPBYSQL)        

UPDATE T SET T.Insurance_folder_cnt = I.Insurance_folder_cnt
FROM #tempLatestPolicyCnt T JOIN Insurance_file I ON I.insurance_file_cnt = T.Insurance_File_Cnt

IF @ClaimDate IS NOT NULL -- Delete ones where cover date was retracted (within the the period of insurance)
		Delete From #tempLatestPolicyCnt where RowNum <>1
		Delete From #tempLatestPolicyCnt Where Insurance_File_Cnt in
		(Select ifi1.insurance_file_cnt FROM Insurance_File ifi1
            Join (  Select MAX(insurance_file_cnt) insurance_file_cnt, insurance_folder_cnt, inception_date_tpi
                    From insurance_file Where insurance_file_type_id IN (2, 5, 6, 9) AND Insurance_folder_cnt in ( select insurance_folder_cnt from #tempLatestPolicyCnt)
					Group By insurance_folder_cnt, inception_date_tpi) ifi2
                    ON ifi1.insurance_folder_cnt = ifi2.insurance_folder_cnt AND ifi2.inception_date_tpi = ifi1.inception_date_tpi
			AND ifi2.insurance_file_cnt = ifi1.insurance_file_cnt
			Where ifi1.expiry_date < @ClaimDate)

--********************************************************************************    
-- TEMPORARY TABLE TO STORED POLICY DETAILS WHICH MATCH OUR CRITERIA    
--********************************************************************************    
CREATE TABLE #tempLatestPolicyDetails    
(    
        InsFileCnt int,    
        InsuranceFolderCnt INT,   
        Statusid int, 
        PartyName VARCHAR(255) NULL,    
        PartyShortname VARCHAR(20) NULL,    
        InsRef VARCHAR(30) NULL,    
        GISIndex VARCHAR(100) NULL,    
        ProdCode VARCHAR(10) NULL,    
        CoverStart DATETIME NULL,    
        Expiry DATETIME NULL,    
        PostCode VARCHAR(20) NULL,    
        Address1 VARCHAR(255) NULL,    
		RenewalDate DATETIME NULL,    
		IsSourceClosed TINYINT NULL,    
		ClaimsAllowed TINYINT NULL,    
        AgentKey INT NULL,
        InsuranceFileStatusCode  VARCHAR(10) NULL,
		CoverFrom DATETIME NULL,
		CoverTo DATETIME NULL,
        LeadAgentName VARCHAR(255) NULL,
        InceptionDate DATETIME NULL,
        InsuranceFileTypeID INT,
        LapseDate datetime,  
        AllowedCLosedBranchClaims INT NULL,
        valid_Policy_Version_At_Loss_Date INT ,
        is_deleted INT,
        FileCode VARCHAR(50) NULL,
        DOBirth datetime,
		is_policy_Cancelled bit,
        ResolvedName VARCHAR(255) NULL
)    

INSERT INTO #tempLatestPolicyDetails    
(
    InsFileCnt, InsuranceFolderCnt, Statusid, PartyName, PartyShortname, InsRef, GISIndex, ProdCode, CoverStart, Expiry,
    PostCode, Address1, RenewalDate, IsSourceClosed, ClaimsAllowed, AgentKey, InsuranceFileStatusCode, CoverFrom, CoverTo, LeadAgentName, 
    InceptionDate, InsuranceFileTypeID, LapseDate, AllowedCLosedBranchClaims, valid_Policy_Version_At_Loss_Date , is_deleted,FileCode,DOBirth,ResolvedName
)
SELECT  ifi.insurance_file_cnt,    
        ifi.insurance_folder_cnt, 
	 ifi.insurance_file_status_id, 
        pty.name,    
        pty.shortname,    
        ifi.insurance_ref,    
        @GISValue,    
        prd.code,    
        ifi.cover_start_date,    
        ifi.expiry_date,    
        postal_code =   CASE adr.postal_code    
                        WHEN convert(VARCHAR(20), adr.address_id) THEN ''    
                        ELSE adr.postal_code    
                        END,    
        adr.address1,    
		ifi.renewal_date,    
		s.is_deleted,    
		ISNULL(s.closed_allow_claims, 0) closed_allow_claims,    
		ifi.lead_agent_cnt,
		ifs.code,
		ifi.cover_start_date AS CoverFrom,  
		ifi.expiry_date AS CoverTo, 
            ag.name AS LeadAgentName,
            ifi.inception_Date as InceptionDate,
            ifi.insurance_file_type_id as InsuranceFileTypeID,  
        NULL, 
        ISNULL(s.closed_allow_claims, 0) closed_allow_claims,
        valid_Policy_Version_At_Loss_Date,
        0,--is_deleted
		pty.file_code as FileCode,
		(SELECT TOP 1 date_of_birth FROM Party_Lifestyle WITH(NOLOCK)
		 WHERE party_cnt = pty.party_cnt AND category = 1) AS DOBirth,
         pty.resolved_name
        FROM    Insurance_File ifi    
		JOIN    Insurance_Folder ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt    
		JOIN    Party pty ON ifo.insurance_holder_cnt = pty.party_cnt    
		JOIN    Party_Address_Usage pau ON pau.address_usage_type_id = 4    
		AND pty.party_cnt = pau.party_cnt    
		JOIN Address adr ON pau.address_cnt = adr.address_cnt    
		JOIN    Product prd ON prd.product_id=ifi.product_id    
		JOIN    #tempLatestPolicyCnt tmp ON ifi.insurance_file_cnt = tmp.insurance_file_cnt    
		JOIN    source s ON ifi.source_id = s.source_id    
		LEFT JOIN (SELECT source_id FROM pmuser_source WITH(NOLOCK) WHERE user_id = @user_id) userbranch 
		ON userbranch.source_id=ifi.source_id
		LEFT OUTER JOIN Party ag ON ifi.lead_agent_cnt = ag.party_cnt
		LEFT JOIN Insurance_File_Status ifs ON ifi.insurance_file_status_id =ifs.insurance_file_status_id 		
		
    WHERE  ifi.insurance_file_type_id in (2,3, 5, 6, 8, 9,11)   
    AND ifi.source_id NOT IN (SELECT source_id FROM pmuser_source WITH(NOLOCK) WHERE user_id = @user_id)
    AND NOT EXISTS(SELECT InsFile.insurance_file_cnt From Insurance_File as InsFile
    WHERE InsFile.insurance_folder_cnt = ifi.insurance_folder_cnt and InsFile.insurance_file_type_id = 8
    AND @ClaimDate >= InsFile.cover_start_date and InsFile.insurance_file_cnt > ifi.insurance_file_cnt) 
     

    Update T 
    Set LapseDate = ( Select max(lapsed_date) FROM Insurance_file I 
                        WHERE  I.insurance_folder_cnt = T.InsuranceFolderCnt AND  insurance_file_status_id=1 AND insurance_file_type_id in (2, 5, 6, 8, 9) and ISNULL(t.InsuranceFileStatusCode,0)='CAN'
                        AND (ISNULL(Base_Insurance_File_Cnt,0) = 0 OR ISNULL(Base_Insurance_File_Cnt, 0) = insurance_file_cnt) AND ISNULL(out_of_sequence_replaced, 0) = 0
                        )
    FROM #tempLatestPolicyDetails T
    
   UPDATE T
   SET is_deleted = 1
   FROM #tempLatestPolicyDetails T
     WHERE valid_Policy_Version_At_Loss_Date = 1 AND (lapsedate < @ClaimDate OR (InsuranceFileTypeID =6 AND lapsedate > @ClaimDate)) -- AND lapsedate <>'1899-12-29 00:00:00.000'
 
 

   IF ( @agent_group_cnt <> 0)
   BEGIN
        Declare @party_cnts table ( party_cnt int) 
        INSERT INTO @party_cnts
            SELECT party_cnt FROM party_agent WHERE linked_account_group =@agent_group_cnt
        
        
        UPDATE T
        SET is_deleted = 1
        FROM #tempLatestPolicyDetails T
        JOIN Insurance_file ifi ON T.InsFileCnt = ifi.insurance_file_cnt
        JOIN    Party pty ON ifi.insured_cnt = pty.party_cnt    
        WHERE pty.agent_cnt NOT IN (Select party_cnt From @party_cnts) OR ifi.lead_agent_cnt not in (Select party_cnt From @party_cnts)
   END

          

--********************************************************************************    
-- FILTER OUT POLICIES WHICH DO NOT MATCH OUR POST CODE    
--********************************************************************************    
IF @PostCode IS NOT NULL    
DELETE FROM #tempLatestPolicyDetails    
WHERE PostCode NOT like @PostCode    
    
--********************************************************************************    
-- FILTER OUT POLICIES WHICH DO NOT MATCH OUR SPECIFIED AGENTS    
--********************************************************************************    

--If @agent_group_cnt is provided, do not filter by @AgentKey
IF @agent_group_cnt  <> 0  
BEGIN  
	DELETE FROM #tempLatestPolicyDetails WHERE ISNULL(AgentKey,0) NOT IN (Select party_cnt From @party_cnts)
END
ELSE IF @AgentKey IS NOT NULL AND @AgentKey <> 0  
BEGIN
	DELETE FROM #tempLatestPolicyDetails  WHERE AgentKey <> @AgentKey OR AgentKey IS NULL  
END
   
--********************************************************************************    
-- FILTER OUT POLICIES WHICH DO NOT LINK TO THE SPECIFIED COVER NOTE SHEET NUMBER    
--********************************************************************************    
IF @CoverNoteSheetNumber  IS NOT NULL AND @CoverNoteSheetNumber  <> 0    
DELETE FROM #tempLatestPolicyDetails    
WHERE InsFileCnt NOT IN (SELECT INSURANCE_FILE_CNT FROM COVER_NOTE_SHEET WHERE COVER_SHEET_NUMBER = @CoverNoteSheetNumber AND insurance_file_cnt  IS NOT NULL  )       
    
--********************************************************************************    
-- SELECT FINAL RESULT    
--********************************************************************************    
SELECT InsFileCnt,    
		PartyShortname,    
		InsRef,    
		GISIndex,    
		ProdCode,    
		CoverStart,    
		Expiry,    
		PostCode,    
		Address1,    
		PartyName,    
		RenewalDate,    
		IsSourceClosed,    
		ClaimsAllowed ,  
		AgentKey,   
		Statusid,
		InsuranceFileStatusCode,
		CoverFrom,
		CoverTo,
            LeadAgentName,
            InceptionDate,
            InsuranceFileTypeID,
    Code InsuranceFileTypeCode, 
    LapseDate, 
    AllowedCLosedBranchClaims,
    (CASE @RetrieveAssociates
    WHEN 1 THEN (SELECT
    P.resolved_name +' ('+ AT.description + ')'  as Name
	
    FROM insurance_file_associates Associate
    INNER JOIN party P ON Associate.party_cnt = P.party_cnt
    INNER JOIN Association_Type AT ON Associate.Association_Type_id=AT.Association_Type_id
    Where Associate.Insurance_file_cnt=#tempLatestPolicyDetails.InsFileCnt And 
	ISNUll(Associate.Is_Deleted,0) <> 1 FOR XML AUTO, TYPE )
    ELSE ''  END) As AssociatedClients,
	ISNULL(FileCode,'') As FileCode,
		ISNULL(DOBirth,'') As DOBirth,
        ResolvedName
		
FROM #tempLatestPolicyDetails 
JOIN Insurance_File_Type on InsuranceFileTypeID=insurance_file_type_id       
WHERE #tempLatestPolicyDetails.is_deleted = 0     
  
--********************************************************************************    
-- GET RID OF TEMPORARY TABLES    
--********************************************************************************    
IF @PolicyStartDate IS NOT NULL OR @PolicyEndDate IS NOT NULL    
    DROP TABLE #TempDate    
    
IF @PartyShortName IS NOT NULL    
    DROP TABLE #TempParty    
    
IF @PolicyNo IS NOT NULL    
    DROP TABLE #TempPolicyRef    
    
DROP TABLE #tempLatestPolicyDetails    
DROP TABLE #tempLatestPolicyCnt    
  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO