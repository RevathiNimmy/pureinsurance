SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS ON
GO

/******
For Parmeter @FilterQuoter 
(0,All Default Value) 
(1,Exclude Expired Quotes) 
(2,Expired Quotes Only) 
(3,Exclude Cancelled Quotes) 
(4,Cancelled Quotes Only) 
(5,Exclude Cancelled and Lapsed Expired Quotes) 
(6,NB Quotes Only) 
(7,MTA Quotes Only) 
(8,Renewal Quotes Only)

For parameter @Filterolicies 
(0,All Default Value) 
(1,Exclude Lapsed Policies) 
(2,Lapsed Policies Only) 
(3,Exclude Cancelled Policies) 
(4,Cancelled Policies Only) 
(5,Exclude Cancelled and Lapsed Policies) 
******/

EXECUTE DDLDROPPROCEDURE 'spu_SAM_Get_Broker_Summary'
GO
CREATE PROCEDURE spu_SAM_Get_Broker_Summary 
			@AgentKey        INT=NULL,  								
			@InsuranceRef    VARCHAR(30)=NULL,  
			@QuoteType       VARCHAR(30)=NULL,-- Policy,Quote,All,  
			@ProductId       INT=NULL,  
			@InsuredName     VARCHAR(255)=NULL,  
			@bUserOnly       TINYINT=NULL,  
			@UserID          INT=NULL,  
			@MaxRowsToFetch  INT = -1,  
			@CoverStartDate  DATETIME = NULL,  
			@QuoteORLiveDate DATETIME = NULL,  
			@ContactUserId   INT =NULL,  
			@BranchId        INT =NULL,  
			@RetrieveAssociates As TINYINT=0,  						   
            @nFilterQuotes   INT = 0,  
            @nFilterPolicies INT= 0  
AS  
  BEGIN  
      DECLARE @sSQL                        VARCHAR (5000),  
              @iOption                     INT,  
              @QuoteVersioningSystemOption TINYINT = 0,  
              @ISAGENTPRODUCTLINK          TINYINT=0,  
              @iParty_Cnt       INT = 0     
  
  
      SELECT @sSQL = ""  
  
      --SELECT @iOption = ISNULL(Value, 0)  
      --FROM   System_Options  
      --WHERE  option_number = 5071  
  
      --SELECT @QuoteVersioningSystemOption = ISNULL(value, 0)  
      --FROM   System_Options  
      --WHERE  option_number = 5089  
  
	IF NOT EXISTS (SELECT NULL FROM Party_Agent WHERE party_cnt=@AgentKey)  
	 BEGIN  
		SET @AgentKey = null  
	 END  
	SELECT @iParty_Cnt = ISNULL(party_cnt,0) FROM PMUser WHERE user_id = @UserID;  																	 
      CREATE TABLE #TempPolicies  
        (  
           InsuranceFileKey             INT,  
           InsuranceFolderKey           INT,  
           PartyKey                     INT,  
           InsuranceRef                 VARCHAR(30),  
           ProductCode                  VARCHAR(10),  
           ProductDescription           VARCHAR(255),  
           InsuranceFileTypeDescription VARCHAR(255),  
           InsuranceFileTypeCode        VARCHAR(10),  
           ClientShortName              VARCHAR(30),  
           ClientName                   VARCHAR(255),  
           IssuedDate                   DATETIME,  
           StartDate                    DATETIME,  
           ExpiryDate                   DATETIME,  
           PolicyStatusCode             VARCHAR(10),  
           PolicyStatusDescription      VARCHAR(255),  
           PolicyTypeId                 INT,  
           IsCurrent                    TINYINT,  
           QuoteStatusKey               INT,  
           QuoteVersion                 INT,  
           BaseInsuranceFolderKey       INT,  
           AgentName                    VARCHAR(255),  
           AgentKey                     INT,  
           QuoteExpiryDate              DATETIME,  
           ContactUserId                INT,  
           RenewedVersion               TINYINT,  
           RiskStatus                   VARCHAR(255),  
           IsMarketPlacePolicy          TINYINT,  
     IsReinstateLink TINYINT,  
     BaseInsuranceFileKey INT,  
     RiskNumber     INT,  
     RiskDescription    VARCHAR(255),  
            AssociatedClients            XML  
        )  
  
      IF @MaxRowsToFetch <> -1  
        BEGIN  
            SELECT @sSQL = 'SET NOCOUNT ON '  
  
            SELECT @sSQL = @sSQL + ' SET ROWCOUNT '  
                           + CONVERT(VARCHAR, @MaxRowsToFetch)  
        END  
  
      SELECT @sSQL = @sSQL + ' SELECT distinct IFL.Insurance_file_cnt InsuranceFileKey,  
 IFL.Insurance_folder_cnt InsuranceFolderKey,  
 P.Party_cnt PartyKey,  
 IFL.Insurance_ref InsuranceRef,  
 PROD.Code ProductCode,  
 PROD.Description ProductDescription,  
 IFT.Description InsuranceFileTypeDescription,  
 IFT.Code InsuranceFileTypeCode,  
 P.ShortName ClientShortName,  
 P.resolved_name ClientName, '  
  
      SELECT @sSQL = @sSQL + 'Case When IFT.Code in(' + '''POLICY'',''MTA PERM'',''MTA TEMP'',''MTACAN'',''MTAREINS'')'  
      SELECT @sSQL = @sSQL + '  Then isnull(IFL.marked_date,IFS.date_created)   '  
      SELECT @sSQL = @sSQL + '   Else IFS.date_created END as IssuedDate ,  '  
  
      SELECT @sSQL = @sSQL  
                     + 'IFL.Cover_Start_date StartDate,  
  
 IFL.Expiry_Date ExpiryDate,  
     ISNULL(insurance_file_status.code,'  
                     + "'LIVE'"  
                     + ') PolicyStatusCode,  
     ISNULL(insurance_file_status.description,'  
                     + "'LIVE'"  
                     + ') PolicyStatusDescription,  
     IFL.Insurance_File_Type_id PolicyTypeId,  
 0 IsCurrent, quote_status_id, quote_version, base_insurance_folder_cnt,  
 (select PA.Name from Party PA where PA.party_cnt = IFL.lead_agent_cnt) AgentName,  
 IFL.lead_agent_cnt AgentKey,     IFL.quote_expiry_date  QuoteExpiryDate,  
 IFL.Contact_user_id, (CASE WHEN (IFL.insurance_file_status_id IS NULL AND IFL.policy_version > 1) THEN 1 ELSE 0 END),            
													   
									 
						  
													  
									 
						  
													 
									 
						  
													 
										 
  
																		   
																	   
		  ISNULL(tmp.RStatus,'+'''Quoted''' +') RiskStatus,
         IFL.is_marketplace_policy   IsMarketPlacePolicy,0, ISNULL(IFL.base_insurance_file_cnt,0),0 RiskNumber,' + "''" +' RiskDescription,  
    Cast((CASE '+ Cast(@RetrieveAssociates As Varchar(10)) +'  
        WHEN 1 THEN (SELECT  
                    P.resolved_name +'' (''+ AT.description + '')''  as Name  
     FROM insurance_file_associates Associate  
     INNER JOIN party P ON Associate.party_cnt = P.party_cnt  
     INNER JOIN Association_Type AT on Associate.Association_Type_id=AT.Association_Type_id  
                    Where Associate.Insurance_file_cnt=IFL.insurance_file_cnt And  
     ISNUll(Associate.Is_Deleted,0) <> 1  
     FOR XML AUTO, TYPE )  
        ELSE ''''  
        END) As Varchar(Max)) As AssociatedClients  
															   
							 
																			  
											   
															  
																							 
																				   
										  
						   
				   
													
  
     FROM Insurance_file IFL  
  JOIN Insurance_file_system IFS ON IFL.Insurance_file_cnt=IFS.Insurance_file_cnt  
  JOIN Product Prod ON IFL.Product_id=PROD.Product_id  
  JOIN Party P ON IFL.Insured_cnt=P.Party_cnt  
   JOIN Insurance_file_type IFT ON IFl.Insurance_file_type_id=IFT.Insurance_file_type_id   
  LEFT JOIN Party PA ON PA.party_cnt = IFL.lead_agent_cnt  
 LEFT JOIN insurance_file_status ON insurance_file_status.insurance_file_status_id = IFL.insurance_file_status_id  
 LEFT JOIN mta_insurance_file_link mifl ON ifl.Base_Insurance_File_Cnt = mifl.insurance_file_cnt
 LEFT JOIN (SELECT ifrl.insurance_file_cnt,ISNULL(rs.description,' + "'Quoted'" + ') as RStatus FROM insurance_file_risk_link ifrl
 JOIN Risk rsk ON ifrl.risk_cnt = rsk.risk_cnt AND (rsk.risk_status_id IN (1,2,4,8,10) OR rsk.risk_status_id IS NULL)									
  
																																						
												   
  
																																						
												   
  
																																						
												   
  
																																							
													   
  
																																							 
 LEFT JOIN Risk_status rs ON rsk.risk_status_id = rs.risk_status_id) tmp ON tmp.insurance_file_cnt = IFL.insurance_file_cnt
  '
  
      SELECT @sSQL = @sSQL  
                     + ' WHERE (ISNULL(mifl.IsDirty,0)=0 OR ifl.Base_Insurance_File_Cnt = ifl.insurance_file_cnt)'  
  
      IF @AgentKey IS NOT NULL  
        BEGIN  
  
            IF @bUserOnly IS NOT NULL  
              BEGIN  
                  IF @bUserOnly = 1  
                    BEGIN  
                        SELECT @sSQL = @sSQL + ' AND IFL.insurance_file_cnt=IFS.insurance_file_cnt  
      AND IFS.Created_By_id='  
                                       + CONVERT(VARCHAR, @UserID)  
                                       + ' and IFL.lead_agent_cnt='  
                                       + CONVERT(VARCHAR, @AgentKey) --added @AgentKey serch  
                    END  
                  ELSE IF @bUserOnly = 0  
                    BEGIN  
                        SELECT @sSQL = @sSQL + ' AND IFL.lead_agent_cnt='  
                                       + CONVERT(VARCHAR, @AgentKey)  
                    END  
              END  
        END --@Agent_Key  
      IF @InsuranceRef IS NOT NULL  
        BEGIN  
            IF CHARINDEX("%", CONVERT(VARCHAR, @InsuranceRef)) <> 0  
              BEGIN  
                  SELECT @sSQL = @sSQL + ' AND IFL.Insurance_ref LIKE ' + "'"  
                                 + CONVERT(VARCHAR, @InsuranceRef) + "'"  
              END  
            ELSE  
              BEGIN  
                  SELECT @InsuranceRef = REPLACE(@InsuranceRef, "'", "''")  
  
                  SELECT @sSQL = @sSQL + ' AND IFL.Insurance_ref =' + "'"  
                                 + CONVERT(VARCHAR, @InsuranceRef) + "'"  
              END  
        END  
  
      IF @QuoteType IS NOT NULL  
        BEGIN  
            IF @QuoteType = 'POLICY' 
				BEGIN
					IF ISNULL(@nFilterPolicies,0) = 0  
						BEGIN  
							SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''POLICY'',''MTA PERM'',''MTAREINS'',''WRITTEN'', ''MTACAN'''+') '   
						END   
					ELSE IF @nFilterPolicies = 1  
						BEGIN
							SELECT @sSQL = @sSQL + ' AND insurance_file_status.Code <> '+'''LAP'''   
						END    
					 ELSE IF @nFilterPolicies = 2  
						  BEGIN
							SELECT @sSQL = @sSQL + ' AND insurance_file_status.Code = '+'''LAP'''  
						  END    
					ELSE IF @nFilterPolicies = 3  
						BEGIN
							SELECT @sSQL = @sSQL + ' AND IFT.Code <> '+'''MTACAN'''   									
						END    
					ELSE IF @nFilterPolicies = 4  
						BEGIN
							SELECT @sSQL = @sSQL + ' AND IFT.Code = '+'''MTACAN'''   								 
						END  
					ELSE IF @nFilterPolicies = 5  
						BEGIN 
							SELECT @sSQL = @sSQL + ' AND IFT.Code <> '+'''MTACAN'' AND insurance_file_status.Code <> ' + '''LAP'''								
						END 
					 ELSE  
					   BEGIN  
						SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''POLICY'',''MTA PERM'',''MTAREINS'',''WRITTEN'', ''MTACAN'''+') '   
					   END  
				END				              
            ELSE IF @QuoteType = 'QUOTE'  
              BEGIN  
				 IF ISNULL(@nFilterQuotes,0) = 0  
					  BEGIN  
								SELECT @sSQL = @sSQL  
                                 + ' AND IFT.Code IN('+'''QUOTE'',''RENEWAL'',''MTAQUOTE'',''MTAQTETEMP'',''MTAQCAN'',''MTAQREINS'''+')'  
					  END  
  
				Else IF @nFilterQuotes = 1  
					  BEGIN  								 
						SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''QUOTE'',''RENEWAL'',''MTAQUOTE'',''MTAQTETEMP'',''MTAQCAN'',''MTAQREINS'''+')'  
						SELECT @sSQL = @sSQL + ' AND IFL.Quote_Expiry_Date > GetDate()'  
					  END    
				 Else IF @nFilterQuotes = 2  
					  BEGIN  
						SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''QUOTE'',''RENEWAL'',''MTAQUOTE'',''MTAQTETEMP'',''MTAQCAN'',''MTAQREINS'''+')'  
						SELECT @sSQL = @sSQL + ' AND IFL.Quote_Expiry_Date < GetDate()'
					  END    
				 Else IF @nFilterQuotes = 3  
					  BEGIN  
						SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''QUOTE'',''RENEWAL'',''MTAQUOTE'',''MTAQTETEMP'',''MTAQREINS'''+')'  
					  END    
				 Else IF @nFilterQuotes = 4  
					  BEGIN  
						SELECT @sSQL = @sSQL + ' AND IFT.Code = ' + '''MTAQCAN'''
					  END    
				 Else IF @nFilterQuotes = 5  
					  BEGIN  
						SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''QUOTE'',''RENEWAL'',''MTAQUOTE'',''MTAQTETEMP'',''MTAQREINS'''+')'
						SELECT @sSQL = @sSQL + ' AND IFL.Quote_Expiry_Date > GetDate() and ISNULL(IFL.Insurance_File_Status_Id, 0) <> 1' 
					  END   
				 Else IF @nFilterQuotes = 6  
					  BEGIN
						SELECT @sSQL = @sSQL + ' AND IFT.Code = ' + '''QUOTE'''
					  END    
				 Else IF @nFilterQuotes = 7  
					  BEGIN
						SELECT @sSQL = @sSQL + ' AND IFT.Code = ' + '''MTAQUOTE'''
					  END  
  
				 Else IF @nFilterQuotes = 8  
					  BEGIN
						SELECT @sSQL = @sSQL + ' AND IFT.Code = ' + '''RENEWAL''' 
					  END  
				 ELSE  
				   BEGIN  
					  SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''QUOTE'',''RENEWAL'',''MTAQUOTE'',''MTAQTETEMP'',''MTAQCAN'',''MTAQREINS'''+')'  				  
				   END  
				 END
		ELSE IF @QuoteType ="ALL"  
          BEGIN 
			SELECT @sSQL = @sSQL -- no filter required
              --SELECT @sSQL = @sSQL + ' AND IFT.Code IN('+'''QUOTE'',''RENEWAL'',''MTAQUOTE'',''MTAQTETEMP'',''MTAQCAN'',''MTAQREINS'''+')'  				    
          END 
        END  
           
  
      IF @ProductId IS NOT NULL  
        BEGIN  
            SELECT @sSQL = @sSQL + ' AND IFL.Product_id= '  
                           + CONVERT(VARCHAR, @ProductId)  
        END  
      ELSE  
        BEGIN  
            SELECT @ISAGENTPRODUCTLINK = Value  
            FROM   System_Options  
            WHERE  option_number = 5088  
  
            IF @ISAGENTPRODUCTLINK = 1 AND NOT @AgentKey IS NULL    
              BEGIN  
                  SELECT @sSQL = @sSQL  
                                 + '  AND IFL.Product_id IN(SELECT Product_Id FROM  party_agent_product WHERE Party_Cnt='  
                                 + CONVERT(VARCHAR, @AgentKey) + ')'  
              END  
			ELSE IF @ISAGENTPRODUCTLINK = 1 AND @iParty_Cnt <> 0  
			  BEGIN  
					  SELECT @sSQL = @sSQL  
									 + '  AND IFL.Product_id IN(SELECT Product_Id FROM  party_agent_product WHERE Party_Cnt='  
									 + CONVERT(VARCHAR, @iParty_Cnt) + ')'  
			  END  
        END  
  
      IF @InsuredName IS NOT NULL  
        BEGIN  
            IF CHARINDEX("%", CONVERT(VARCHAR(255), @InsuredName)) <> 0  
              BEGIN  
                  SELECT @InsuredName =REPLACE(@InsuredName,"'","''")  
                  SELECT @sSQL = @sSQL  
                                 + ' AND IFL.insured_cnt=P.Party_cnt AND P.resolved_name LIKE '  
                                 + "'" + CONVERT(VARCHAR(255), @InsuredName) + "'"  
              END  
            ELSE  
              BEGIN  
                  SELECT @InsuredName = REPLACE(@InsuredName, "'", "''")  
  
                  SELECT @sSQL = @sSQL  
                                 + ' AND IFL.insured_cnt=P.Party_cnt AND P.resolved_name ='  
                                 + "'" + CONVERT(VARCHAR(255), @InsuredName) + "'"  
              END  
        END  
  
      IF @CoverStartDate IS NOT NULL  
        BEGIN  
            SELECT @sSQL = @sSQL  
                           + ' AND Convert(date,IFL.Cover_start_date) >= Convert(date,'''  
                           + CONVERT(VARCHAR, @CoverStartDate) + ''')  '  
        END  
  
      IF @QuoteORLiveDate IS NOT NULL  
        BEGIN  
            SELECT @sSQL = @sSQL + ' AND Convert(date,'''  
                           + CONVERT(VARCHAR, @QuoteORLiveDate)  
                           + ''') <= Case when IFL.quote_status_id IN (4,5) Then Convert(date,IFL.date_issued) '  
  
            SELECT @sSQL = @sSQL  
                           + ' ELSE Convert(date,IFS.last_modified) '  
  
            SELECT @sSQL = @sSQL + ' END '  
        END  
  
      IF @UserID IS NOT NULL  
        BEGIN  
            SELECT @sSQL = @sSQL  
                           + ' AND IFL.Source_Id IN (SELECT s.source_id FROM   source s WHERE  source_id NOT IN (SELECT source_id FROM pmuser_source WHERE [user_id] ='  
               + CONVERT(VARCHAR, @UserId)  
                           + ' AND is_deleted = 0 ))'  
        END  
  
      IF @ContactUserId IS NOT NULL  
        BEGIN  
            SELECT @sSQL = @sSQL + ' AND IFL.Contact_user_id ='  
                           + CONVERT (VARCHAR, @ContactUserId )  
        END  
  
      SELECT @sSQL = @sSQL  
                     + 'ORDER BY IFL.Base_Insurance_folder_cnt DESC,IFL.Quote_Version DESC ' -- any change in sort order will affect Nexus [WPR 63][Sagicor]  
      IF @MaxRowsToFetch <> -1  
        BEGIN  
            SELECT @sSQL = @sSQL + ' SET ROWCOUNT 0'  
  
            SELECT @sSQL = @sSQL + ' SET NOCOUNT OFF'  
        END  
  
      INSERT INTO #TempPolicies  
      EXECUTE (@sSQL)  
	 PRINT @sSQL
  --UPDATE t SET IssuedDate = d.document_date FROM #TempPolicies t  
  --        INNER JOIN document d ON d.insurance_file_cnt=t.InsuranceFileKey  
  --        WHERE document_ref LIKE 'S%'  
  
  --UPDATE t SET IssuedDate = d.document_date FROM #TempPolicies t  
		--INNER JOIN Insurance_File ifi ON t.InsuranceFileKey = ifi.insurance_file_cnt
  --        INNER JOIN document d ON d.insurance_file_cnt=t.InsuranceFileKey  
  --        WHERE insurance_file_type_id IN (2,5,6,8,9)
      UPDATE #TempPolicies  
  SET    IsCurrent = 1 WHERE InsuranceFileTypeCode IN('QUOTE','POLICY','RENEWAL','MTA PERM','MTACAN','MTAREINS','WRITTEN')  
             AND InsuranceFileKey IN (SELECT MAX(InsuranceFileKey)  
                                      FROM   #TEMPPOLICIES  
                                      WHERE  InsuranceFileTypeCode IN('QUOTE','POLICY','RENEWAL','MTA PERM','MTAQCAN','MTAREINS','WRITTEN')  
                                             AND PolicyStatusCode NOT IN ( 'LAP', 'CAN' )  
                                      GROUP  BY InsuranceFolderKey)  
  
   UPDATE #TempPolicies  
    SET ISReinstatelink=1  
    FROM #TempPolicies JOIN Insurance_File IFL ON #TempPolicies.InsuranceFolderKey=IFL.insurance_folder_cnt  
    AND #TempPolicies.InsuranceFileKey=IFL.insurance_file_cnt  
    AND #TempPolicies.InsuranceFileKey=(Select Max(insurance_file_cnt) From Insurance_File IFL1 JOIN Insurance_FIle_Type IFT1 ON IFT1.insurance_file_type_id=IFL1.insurance_file_type_id  
    WHERE IFT1.code<>'MTAQREINS' AND iFL1.insurance_folder_cnt= #TempPolicies.InsuranceFolderKey AND (ISNULL(IFL1.Base_Insurance_File_Cnt,0)=0 OR ISNULL(IFL1.base_insurance_file_cnt,0)=IFL1.insurance_file_cnt) )  
    AND #TempPolicies.InsuranceFileTypeCode='MTACAN'  
  
--IF @QuoteType ="ALL"  
--          BEGIN  
--              SELECT *  
--              FROM   #TempPolicies  
--              ORDER  BY InsuranceFileKey DESC  
--          END  
  
--  ELSE IF @QuoteType ="QUOTE"  
--    BEGIN  
--        IF @nFilterQuotes = 0  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--        Else IF @nFilterQuotes = 1  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where QuoteExpiryDate > GetDate()  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     Else IF @nFilterQuotes = 2  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where QuoteExpiryDate < GetDate()  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     Else IF @nFilterQuotes = 3  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where PolicyStatusCode <> 'CAN'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     Else IF @nFilterQuotes = 4  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where PolicyStatusCode = 'CAN'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     Else IF @nFilterQuotes = 5  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where QuoteExpiryDate > GetDate() and PolicyStatusCode <> 'CAN'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     Else IF @nFilterQuotes = 6  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where InsuranceFileTypeCode = 'QUOTE'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     Else IF @nFilterQuotes = 7  
--   BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where InsuranceFileTypeCode = 'MTAQUOTE'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     Else IF @nFilterQuotes = 8  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where InsuranceFileTypeCode='RENEWAL'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
--     ELSE  
--       BEGIN  
--        SELECT *  
--                    FROM   #TempPolicies  
--                    ORDER  BY InsuranceFileKey DESC  
--    END  
--          END  
  
--  ELSE IF @QuoteType ="POLICY"  
--    BEGIN  
--        IF @nFilterPolicies = 0  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--        ELSE IF @nFilterPolicies = 1  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where PolicyStatusCode <> 'LAP'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     ELSE IF @nFilterPolicies = 2  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where PolicyStatusCode = 'LAP'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--        ELSE IF @nFilterPolicies = 3  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where PolicyStatusCode <> 'CAN'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--        ELSE IF @nFilterPolicies = 4  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where PolicyStatusCode = 'CAN'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     ELSE IF @nFilterPolicies = 5  
--          BEGIN  
--                    SELECT *  
--                    FROM   #TempPolicies where PolicyStatusCode <> 'CAN' and PolicyStatusCode <> 'LAP'  
--                    ORDER  BY InsuranceFileKey DESC  
--          END  
  
--     ELSE  
--       BEGIN  
--        SELECT *  
--                    FROM   #TempPolicies  
--                    ORDER  BY InsuranceFileKey DESC  
--    END  
--          END  
  
--  ELSE  
--    BEGIN  
--        SELECT *  
--              FROM   #TempPolicies  
--              ORDER  BY InsuranceFileKey DESC  
--    END  
  SELECT *  
              FROM   #TempPolicies  
              ORDER  BY InsuranceFileKey DESC  
		 
  
DROP TABLE #TempPolicies  
  END  

SET QUOTED_IDENTIFIER ON    
GO