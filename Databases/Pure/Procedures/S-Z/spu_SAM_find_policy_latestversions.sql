EXECUTE DDLDropProcedure 'spu_SAM_find_policy_latestversions'
GO

CREATE PROCEDURE spu_SAM_find_policy_latestversions
(       @sQuoteType       VARCHAR(30)='ALL',
        @nProductId       INT=NULL,
        @sInsuranceRef    VARCHAR(50)=NULL,
        @nAgentKey        INT=NULL,
        @sInsuredName     VARCHAR(255)=NULL,
        @dtQuoteORLiveDate DATETIME = NULL,
        @dtCoverStartDate  DATETIME = NULL,  
        @nUserID           INT=NULL,
        @nMaxRowsToFetch   INT = -1,
        @RetrieveAssociates	INT = 0
)
AS
BEGIN
DECLARE @DisableTempMTA INT

SELECT @DisableTempMTA = ISNULL(value, 0) FROM System_Options WHERE option_number = 5116 AND branch_id = 1

IF @nMaxRowsToFetch<>-1
BEGIN
	SET ROWCOUNT @nMaxRowsToFetch
END
Create Table #CurrentVersion
(
	InsuranceFolderCnt INT,
	CoverStartDate DateTime,
	NoOfVersions INT
)



DECLARE @Query NVARCHAR(4000)
DECLARE @Where NVARCHAR(1000)
SET @Query = 'INSERT INTO #CurrentVersion Select ifi.insurance_folder_cnt, MAX(cover_start_date) CoverStartDate, 0 '
SET @Query = @Query + ' FROM Insurance_File ifi '
--SET @Query = @Query + ' INNER JOIN (Select Count(*) NoOfVersions, insurance_folder_cnt From insurance_File Group By insurance_folder_cnt) iFile ON iFile.insurance_folder_cnt = ifi.insurance_folder_cnt '
SET @Query = @Query + ' INNER JOIN Insurance_File_Type ift ON ift.insurance_file_type_id = ifi.insurance_file_type_id '
SET @Query = @Query + ' INNER JOIN Insurance_File_System ifs ON ifi.Insurance_file_cnt = ifs.Insurance_file_cnt  '
SET @Query = @Query + ' INNER JOIN Party P ON P.party_cnt = ifi.insured_cnt '
SET @Query = @Query + ' LEFT JOIN Insurance_file_associates ifa ON ifi.insurance_file_cnt = ifa.Insurance_file_cnt
						LEFT JOIN Party P1 ON P1.party_cnt = ifa.Party_cnt'

SET @Where =  ''
IF ISNULL(@nProductId,0) <> 0
	SET @Where = ' ifi.product_id  = ' + CONVERT(VARCHAR,@nProductId)

IF ISNULL(@sInsuranceRef,'') <> ''
BEGIN
	IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

	SET @Where = @Where + ' ifi.insurance_ref like ''' + @sInsuranceRef + ''''
END
IF ISNULL(@nAgentKey,0) <> 0
BEGIN
	IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

	SET @Where = @Where + ' ifi.lead_agent_cnt = ' + CONVERT(VARCHAR,@nAgentKey )
END
IF ISNULL(@sInsuredName,'') <> ''
BEGIN
	IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

	SET @Where = @Where + ' ( P.resolved_name like ''' + @sInsuredName + ''' OR P1.party_cnt IN (select ifa.party_cnt from Insurance_file_associates where Insurance_file_cnt = ifi.insurance_file_cnt and ISNULL(Is_Deleted,0) <> 1 ))'
END

IF ISNULL(@dtQuoteORLiveDate,'') <> ''
BEGIN
	IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

	SET @Where = @Where + '  CONVERT(DATE, ISNULL(ifs.last_modified, ifs.date_created)) = '''+  CONVERT(VARCHAR,CONVERT(DATE,@dtQuoteORLiveDate))  +''''
END
IF ISNULL(@dtCoverStartDate,'') <> ''
BEGIN
	IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

	SET @Where = @Where + '  CONVERT(DATE, ifi.cover_start_date) = ''' + CONVERT(VARCHAR,CONVERT(DATE,@dtCoverStartDate))  + ''''
END

IF @WHERE <> ''
BEGIN
	SET @WHERE = @WHERE + ' AND'
	SET @Where = @Where + ' ISNULL(out_of_sequence_replaced, 0) <> 1 '
END
ELSE
BEGIN
	SET @Where = ' ISNULL(out_of_sequence_replaced, 0) <> 1 '
END

IF @sQuoteType = 'ALL'
BEGIN
IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

SET @WHERE =  @WHERE + ' ift.code IN (''QUOTE'', ''POLICY'', ''MTA PERM'', ''MTACAN'', ''MTAREINS'', ''WRITTEN'',''RENEWAL'')'
END
IF @sQuoteType = 'QUOTE'
BEGIN
IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

SET @WHERE =  @WHERE + ' ift.code IN (''QUOTE'')'
END

IF @sQuoteType = 'POLICY'
BEGIN
IF @WHERE <> ''
		SET @WHERE = @WHERE + ' AND'

SET @WHERE =  @WHERE + ' ift.code IN (''POLICY'', ''MTA PERM'', ''MTACAN'', ''MTAREINS'', ''WRITTEN'')'
END

SET @WHERE = @WHERE + ' Group By ifi.insurance_folder_cnt ' --, iFile.NoOfVersions '


SET @Query = @Query + ' WHERE ' + @Where

Print @Query

EXEC sp_executeSQL @Query

UPDATE CV  
SET NoOfVersions = 
(
 SELECT  Count(*) FROM insurance_File IFI WHERE ifi.insurance_folder_cnt = CV.InsuranceFolderCnt
) 
FROM #CurrentVersion CV

;WITH CurrentVersionCTE (InsuranceFolderCnt, CoverStartDate, NoOfVersions)
AS
(
    Select ifi.insurance_folder_cnt, MAX(cover_start_date) CoverStartDate, iFile.NoOfVersions
    FROM Insurance_File ifi
                INNER JOIN (Select Count(*) NoOfVersions, insurance_folder_cnt From insurance_File Group By insurance_folder_cnt) iFile ON iFile.insurance_folder_cnt = ifi.insurance_folder_cnt
                INNER JOIN Insurance_File_Type ift ON ift.insurance_file_type_id = ifi.insurance_file_type_id
                INNER JOIN Insurance_File_System ifs ON ifi.Insurance_file_cnt = ifs.Insurance_file_cnt 
                INNER JOIN Party P ON P.party_cnt = ifi.insured_cnt
    WHERE ifi.product_id = ISNULL(@nProductId, ifi.product_id)
            AND ifi.insurance_ref like ISNULL(@sInsuranceRef, ifi.insurance_ref)
            AND (ifi.lead_agent_cnt = @nAgentKey OR (@nAgentKey IS NULL))
            AND P.resolved_name like ISNULL(@sInsuredName, P.resolved_name)
            AND ISNULL(@dtQuoteORLiveDate, ISNULL(ifs.last_modified, ifs.date_created)) = ISNULL(ifs.last_modified, ifs.date_created)
            AND ifi.cover_start_date = ISNULL(@dtCoverStartDate, ifi.cover_start_date)
            AND ISNULL(out_of_sequence_replaced, 0) <> 1
            AND ((ift.code IN ('QUOTE', 'POLICY', 'MTA PERM', 'MTACAN', 'MTAREINS', 'WRITTEN','RENEWAL') AND @sQuoteType = 'ALL')
                                    OR
                                    (ift.code = 'QUOTE' AND @sQuoteType = 'QUOTE')
                                    OR
                                    (ift.code IN ('POLICY', 'MTA PERM', 'MTACAN', 'MTAREINS', 'WRITTEN') AND @sQuoteType = 'POLICY'))
    Group By ifi.insurance_folder_cnt, iFile.NoOfVersions
)


    SELECT 
		ifi.insurance_file_cnt InsuranceFileKey,
		ifi.insurance_folder_cnt InsuranceFolderKey,
		ifi.Insured_cnt PartyKey,
		ifi.insurance_ref InsuranceRef,
		Prod.code ProductCode,
		Prod.description ProductDescription,
		ift.description InsuranceFileTypeDescription,
		ift.code InsuranceFileTypeCode,
		ifi.insured_name ClientName,
		ifi.cover_start_date CoverStartDate,
		ifi.expiry_date ExpiryDate,
		ISNULL(insurance_file_status.code, 'LIVE') PolicyStatusCode,
		ISNULL(insurance_file_status.description, 'LIVE') PolicyStatusDescription,
		ifi.quote_status_id QuoteStatusKey,
		PA.name AgentName,
		ifi.lead_agent_cnt AgentKey,
		ifi.quote_expiry_date QuoteExpiryDate,
		ifi.is_marketplace_policy IsMarketPlacePolicy,
		ISNULL(ifi.Marked_for_collection,0) MarkedQuoteForCollection,
		Case When ISNull(ifi.insurance_file_status_id, 0) = 1 Then 1 Else 0 END IsCancelled,
		ct.NoOfVersions NoOfVersions,
		ISNULL(RS.NoOfRenewalVersions,0) NoOfRenewalVersions,
		(CASE WHEN IFT.Code in('POLICY','MTA PERM','MTA TEMP','MTACAN','MTAREINS','RENEWAL')
				Then ISNULL((select top 1 d.document_date FROM Document D WHERE ifi.insurance_file_cnt = D.insurance_file_cnt AND d.document_ref LIKE 'S%' ORDER BY D.document_id ASC),ISNULL(ifi.marked_date,IFS.date_created))
								Else ISNULL(ifs.last_modified,IFS.date_created) END )
								AS IssueDate,		
		ISNULL((SELECT TOP 1 d.document_date FROM Document D WHERE ifi.insurance_file_cnt = D.insurance_file_cnt AND d.document_ref LIKE 'S%' ORDER BY D.document_id ASC),ISNULL(IFS.last_modified, IFS.date_created))
								AS TransactionDate,
		ifi.renewal_date RenewalDate,
        CASE When (
            (ifi.source_id IN (SELECT source_id FROM pmuser_source WHERE [user_id] = @nUserId))
            OR 
            (ifi.insurance_file_status_id = 2 AND @DisableTempMTA = 0)
            )
            Then 1 Else 0 END IsReadOnly ,
         CASE  @RetrieveAssociates
         WHEN 1 THEN (SELECT  P.resolved_name +' ('+ AT.description + ')'   Name  FROM insurance_file_associates Associate  
                  INNER JOIN party P ON associate.party_cnt = P.party_cnt
                  INNER JOIN Association_Type AT ON associate.Association_Type_id=AT.Association_Type_id      
                  WHERE associate.Insurance_file_cnt=ifi.insurance_file_cnt AND ISNUll(associate.Is_Deleted,0) <> 1     
                   FOR XML AUTO, TYPE )    
         ELSE ''''    
         END As AssociatedClients
         FROM Insurance_file ifi
		 INNER JOIN (Select InsuranceFolderCnt, CoverStartDate, NoOfVersions, Max(iFileCnt.insurance_file_cnt) InsuranceFileCnt
                                        From #CurrentVersion ver 
                                        INNER JOIN Insurance_File iFileCnt ON iFileCnt.insurance_folder_cnt = ver.InsuranceFolderCnt AND iFileCnt.cover_start_date = ver.CoverStartDate
										INNER JOIN Insurance_file_type IFT ON iFileCnt.Insurance_file_type_id=IFT.Insurance_file_type_id
                                        Where ISNull(iFileCnt.out_of_sequence_replaced, 0) = 0 AND IFT.Code in('QUOTE', 'POLICY', 'MTA PERM', 'MTACAN', 'MTAREINS','RENEWAL','WRITTEN')
                                        Group By InsuranceFolderCnt, CoverStartDate, NoOfVersions
                                        ) ct 
										ON ct.InsuranceFileCnt = ifi.insurance_file_cnt
            INNER JOIN Insurance_file_system IFS ON ifi.Insurance_file_cnt=IFS.Insurance_file_cnt
            INNER JOIN Product Prod ON IFI.Product_id=PROD.Product_id
            INNER JOIN Insurance_file_type IFT ON ifi.Insurance_file_type_id=IFT.Insurance_file_type_id
            INNER JOIN Party P ON IFI.Insured_cnt=P.Party_cnt
            LEFT JOIN Party PA ON PA.party_cnt = ifi.lead_agent_cnt
            LEFT  JOIN (Select insurance_folder_cnt, COUNT(*) NoOfRenewalVersions From Insurance_File 
											JOIN #CurrentVersion CV ON Insurance_File.insurance_folder_cnt = CV.InsuranceFolderCnt
                                            Where insurance_file_type_id = 3 And ISNull(out_of_sequence_replaced, 0) = 0 
											Group By insurance_folder_cnt) RS 
											ON RS.insurance_folder_cnt = ifi.insurance_folder_cnt
            LEFT  JOIN insurance_file_status ON insurance_file_status.insurance_file_status_id = ifi.insurance_file_status_id
            
	SET ROWCOUNT 0
END 
