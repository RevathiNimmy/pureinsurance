SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_SAM_Policies_In_Renewal'
GO

CREATE PROCEDURE spu_SAM_Policies_In_Renewal 
                @agent_cnt     INT = NULL,
                @party_cnt     INT = NULL,
                @source_id     INT=NULL,
                @product_id    INT=NULL,
                @renewal_date  DATETIME=NULL,
                @forAccept     BIT=0,
                @OnlyDirect    BIT=0,
                @InsuranceRef  VARCHAR(255)=NULL,
                @ContactuserId VARCHAR(255)=NULL,
                @RetrieveAssociates  As TINYINT =0
AS
BEGIN
      IF @agent_cnt IS NOT NULL
         AND @OnlyDirect <> 0
        SET @agent_cnt=NULL

      SET @source_id=ISNULL(@source_id, 1)
    
      DECLARE @SQL VARCHAR(6500)
      DECLARE @SQLWHERE VARCHAR(2000)
      DECLARE @QT VARCHAR(1)
      DECLARE @CR VARCHAR(2)

      SET @QT=CHAR(39)
      SET @CR=CHAR(13) + CHAR(10)

      SELECT @InsuranceRef = LTRIM(RTRIM(@InsuranceRef))

      SET @SQL='SELECT DISTINCT
                   rs.renewal_status_Cnt RenewalStatusKey,  
                   rs.insurance_holder_cnt PartyKey,  
                   s.code BranchCode,  
                   pa.resolved_name PartyName,
                   i.insurance_ref InsuranceFileRef,  
                   rs.renewal_insurance_file_cnt InsuranceFileKey,  
                   i.insurance_folder_cnt InsuranceFolderKey,  
                   ISNULL(ifs.Description,'
               + @QT + 'LIVE POLICY' + @QT
               + ') InsuranceFileStatusDescription,
                   ift.Description InsuranceFileTypeDescription,  
                   rst.Code RenewalStatusTypeCode,  
                   rst.description RenewalStatusTypeDescription,  
                   i.cover_start_date CoverStartDate,  
                   i.expiry_date CoverEndDate,  
                   i.renewal_date RenewalDate,  
                   (i.this_premium + ISNULL(total_taxes.totaltax,0) + ISNULL(total_fees.totalfee,0) + ISNULL(levy_tax.totallevytax,0)) as RenewalPremium,
                   p.code ProductCode,  
                   p.description ProductDescription,  
                   i.lead_agent_cnt LeadAgentKey,  
                   ISNULL(la.ShortName,'
               + @QT + 'DIRECT' + @QT
               + ') LeadAgent,
                   ISNULL(ah.ShortName,'
               + @QT + 'NONE' + @QT
               + ') AccHandler,
                   CASE  
                       WHEN EXISTS(  SELECT *  
                                     FROM Claim c  
                                     WHERE i.insurance_ref = c.policy_number)  
                           THEN 1  
                       ELSE 0  
                   END ClaimIndicator,  
                   s.is_deleted IsClosed,  
                   ISNULL(p.is_true_monthly_policy,0)IsTrueMonthlyPolicy,  
                   ISNULL(i.anniversary_copy,0)AnniversaryCopy,
                   i.is_marketplace_policy  IsMarketPlacePolicy,
                   CASE
						WHEN EXISTS ( SELECT 1 FROM insurance_file i WHERE i.insurance_file_cnt = rs.insurance_file_cnt AND i.insurance_file_cnt = rs.renewal_insurance_file_cnt)
						THEN 1
						ELSE 0
				  END	IsMigratedPolicy,
                  Cast((CASE '+ Cast(@RetrieveAssociates As Varchar(10)) +'
                          WHEN 1 THEN (SELECT (P1.resolved_name + '' (''+ AT.description+ '')'' )  Name
                                  FROM insurance_file_associates Associate
                                  INNER JOIN party P1 ON Associate.party_cnt = P1.party_cnt
								  INNER JOIN Association_Type AT on Associate.Association_Type_id=AT.Association_Type_id 
                                        Where Associate.Insurance_file_cnt=i.insurance_file_cnt And
                                       (CASE  WHEN ISNUll(Associate.Is_Deleted,0) = 1  And ISNull(Associate.date_removed,Dateadd(year,-99,Getdate())) <= GETDATE() THEN 0   ELSE 1      END =1)
                                         FOR XML AUTO, TYPE )
                         ELSE ''''
                         END) As Varchar(Max)) As AssociatedClients

    FROM insurance_file i  
        JOIN renewal_status rs  
            ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt  
        JOIN product p  
            ON p.product_id = rs.product_id  
        JOIN renewal_status_type rst  
            ON rst.renewal_status_type_id = rs.renewal_status_type_id  
        JOIN party pa  
            ON pa.party_cnt = rs.insurance_holder_cnt  
        JOIN party_type pt  
            ON pa.party_type_id = pt.party_type_id  
        LEFT JOIN party la  
            ON la.party_cnt = rs.lead_agent_cnt  
        LEFT JOIN party ah  
            ON ah.party_cnt = i.account_handler_cnt  
        LEFT JOIN source s  
            ON s.source_id = i.source_id  
        LEFT JOIN insurance_file_type ift  
            ON i.insurance_file_type_id=ift.insurance_file_type_id  
        LEFT JOIN insurance_file_status ifs  
            ON i.insurance_file_status_id=ifs.insurance_file_status_id  
 		JOIN insurance_file_risk_link irl   
     	    ON i.insurance_file_cnt = irl.insurance_file_cnt    
        JOIN    risk rsk                           
            ON irl.risk_cnt = rsk.risk_cnt    
        JOIN    risk_type rty                    
            ON rsk.risk_type_id = rty.risk_type_id  
        LEFT JOIN    
               (SELECT  insurance_file_cnt, sum(value) totaltax    
                FROM    tax_calculation tc   
                WHERE   transtype in ('
               + @QT + 'TTR' + @QT + ', ' + @QT + 'TTF' + @QT + ', ' + @QT + 'TTIF'
               + @QT
               + ')
                GROUP BY insurance_file_cnt   
                ) total_taxes               
	    ON i.insurance_file_cnt = total_taxes.insurance_file_cnt   
        LEFT JOIN    
               (SELECT  insurance_file_cnt, SUM(currency_amount) totalfee 
				FROM    policy_fee_u  
				GROUP BY insurance_file_cnt   
				) total_fees               
	    ON i.insurance_file_cnt = total_fees.insurance_file_cnt   
        LEFT JOIN    
               (SELECT  risk_cnt, sum(this_premium) totallevytax    
                FROM    Peril    
        	WHERE is_levy_tax=1 AND is_premium=0 AND is_taxed IS NULL    
                GROUP BY risk_cnt    
                ) levy_tax                                  
            ON irl.risk_cnt = levy_tax.risk_cnt'
               + @CR
      SET @SQLWHERE='WHERE (CASE WHEN ' +  CAST(@source_id AS VARCHAR(50)) + ' = 0 THEN 1  ELSE 0 END = 1 OR i.source_id = ' + CAST(@source_id AS VARCHAR(50))  + ') ' + @CR

      IF @renewal_date IS NOT NULL
        SET @SQLWHERE=@SQLWHERE + 'AND i.cover_start_date=' + @QT
                      + CONVERT(VARCHAR(20), @renewal_date, 111)
                      + @QT + @CR

      IF @agent_cnt IS NOT NULL
        SET @SQLWHERE=@SQLWHERE + 'AND i.lead_agent_cnt='
                      + CAST(@agent_cnt AS VARCHAR(50)) + @CR

      IF @party_cnt IS NOT NULL
        SET @SQLWHERE=@SQLWHERE + 'AND pa.party_cnt='
                      + CAST(@party_cnt AS VARCHAR(50)) + @CR

      IF @product_id IS NOT NULL
        SET @SQLWHERE=@SQLWHERE + 'AND p.product_id='
                      + CAST(@product_id AS VARCHAR(50)) + @CR

      IF @forAccept <> 0
        --    SET @SQLWHERE=@SQLWHERE+'AND rst.code<>'+@QT+'Update'+@QT+' AND rst.code<>'+@QT+'PolicyChan'+@QT+@CR
        --ELSE
        SET @SQLWHERE=@SQLWHERE + 'AND rst.code=' + @QT + 'Update' + @QT
                      + @CR

      IF @OnlyDirect = 1
        SET @SQLWHERE=@SQLWHERE
                      + 'AND (i.lead_agent_cnt is NULL)' + @CR

      IF @InsuranceRef IS NOT NULL
        SET @SQLWHERE=@SQLWHERE + 'AND i.insurance_ref LIKE ' + @QT
                      + CAST(@InsuranceRef AS VARCHAR(255)) + @QT
                      + @CR

      IF @ContactuserId IS NOT NULL
        SET @SQLWHERE =@SQLWHERE + 'AND i.Contact_user_id = '
                       + CAST(@ContactuserId AS VARCHAR(255)) + @CR
			
      SET @SQLWHERE=@SQLWHERE
                    + 'ORDER BY i.renewal_date, rs.insurance_holder_cnt,  i.insurance_folder_cnt'

      --PRINT (@SQL+@SQLWHERE)
      EXECUTE(@SQL+@SQLWHERE)
END


