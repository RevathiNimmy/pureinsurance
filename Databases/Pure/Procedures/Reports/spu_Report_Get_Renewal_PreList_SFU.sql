
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Report_Get_Renewal_PreList_SFU'
GO

CREATE PROCEDURE spu_Report_Get_Renewal_PreList_SFU  
        @product_code varchar(255),  
        @branch_id int,  
        @end_date NVARCHAR(50)      -- Selection Date  
AS  
  
/*  
*****************************************************************************************************************  
** Created by Jude Killip  
**  
** VERSION:     1.00    05/Jan/2003  
**  
** NOTE :       Copy of sp_Get_Renewal_PreList 05 Jan 2003 to break the direct link with Renewals programs  
**              (incompatible parameter name)  
**  
** USED BY:     RSA Reports - Renewal_Pre_List.rpt  
*****************************************************************************************************************  
** VER  WHO DATE        WHAT  
** 1.01 JMK 29/Jan/03   Change Product parameter to a lookup in report  
**  
** 1.02 JMK 18/Feb/03   Rename date parameter to match report  
******************************************************************************************************************  
*/  
  
/*  
-- For Testing  
DECLARE @product_code varchar(255),  
        @branch_id int,  
        @end_date datetime      -- Selection Date  
  
SELECT  @product_code = 'Motor Commercial - Comprehensive',  
        @branch_id = 0,  
        @end_date = getdate()  
*/  
SET NOCOUNT ON  
SELECT @end_date=CONVERT(DATETIME,@end_date,103)
-- get product_id  
DECLARE @product_id int  
SELECT @product_id = product_id  
FROM Product  
WHERE description = @product_code  
  
--Create temporary table  
CREATE TABLE #TempInsFileCnt  
    (  
    insurance_file_cnt INT NULL  
    )  
  
IF isnull(@product_id, 0)= 0  
    IF isnull(@branch_id, 0)= 0  
        INSERT INTO #TempInsFileCnt  
            SELECT  MAX(insurance_file_cnt) FROM insurance_file  
            WHERE   insurance_file_type_id in (2, 5, 9)  
                GROUP BY insurance_folder_cnt  
    ELSE  
        INSERT INTO #TempInsFileCnt  
            SELECT  MAX(insurance_file_cnt) FROM insurance_file  
            WHERE   insurance_file_type_id in (2, 5, 9)  
            AND source_id = @branch_id  
                GROUP BY insurance_folder_cnt  
ELSE  
    IF isnull(@branch_id, 0)= 0  
        INSERT INTO #TempInsFileCnt  
            SELECT  MAX(insurance_file_cnt) FROM insurance_file  
            WHERE   insurance_file_type_id in (2, 5, 9)  
            AND product_id = @product_id  
                GROUP BY insurance_folder_cnt  
    ELSE  
        INSERT INTO #TempInsFileCnt  
            SELECT  MAX(insurance_file_cnt) FROM insurance_file  
            WHERE   insurance_file_type_id in (2, 5, 9)  
            AND source_id = @branch_id  
            AND product_id = @product_id  
                GROUP BY insurance_folder_cnt  
  
CREATE INDEX I__TempInsFileCnt ON #TempInsFileCnt(insurance_file_cnt)  
  
DELETE  #TempInsFileCnt  
WHERE insurance_file_cnt IN (  
    SELECT  renewal_insurance_file_cnt
    FROM    renewal_status  
    )  
  
        SELECT InsFile.insurance_file_cnt,  
                InsFolder.insurance_holder_cnt,  
                InsFile.product_id,  
                InsFile.lead_agent_cnt,  
                InsFile.Insurance_ref,  
                InsFile.cover_start_date,  
                InsFile.expiry_date,  
                client_name = CASE ISNULL(pt.resolved_name, '')  
                WHEN '' THEN pt.name  
                ELSE pt.resolved_name  
                  END,  
                agent_name = CASE ISNULL(pt2.resolved_name, '')  
                WHEN '' THEN pt2.name  
                ELSE pt2.resolved_name  
                 END,  
                Prod.is_auto_renewable,  
                Prod.description Product_description,  
                RSC1.description Policy_stop_reason,  
                RSC2.description Client_stop_reason,  
                InsFile.is_referred_at_renewal,  
                InsFile.insurance_folder_cnt,  
                InsFile.renewal_date,  
                pt.shortname client_code,  
  ISNULL(claim.NoOfClaims ,0) NoOfClaims  
  
        FROM Insurance_File InsFile  
  
 LEFT JOIN Product Prod ON  
        InsFile.product_id = Prod.product_id  
  
 LEFT JOIN Party Pt2 ON  
     InsFile.lead_agent_cnt = Pt2.party_cnt  
  
        INNER JOIN Insurance_Folder InsFolder ON  
            InsFile.insurance_folder_cnt = InsFolder.insurance_folder_cnt  
  
  LEFT JOIN Party Pt ON  
   pt.party_cnt = InsFolder.insurance_holder_cnt  
  
           LEFT JOIN Renewal_stop_code RSC2 ON  
           Pt.renewal_stop_code_id = RSC2.renewal_stop_code_id  
  
         LEFT JOIN Renewal_stop_code RSC1 ON  
          InsFile.renewal_stop_code_id = RSC1.renewal_stop_code_id  
  
        LEFT JOIN #TempInsFileCnt Tmp ON  
   Tmp.insurance_file_cnt = InsFile.insurance_file_cnt  
  
 LEFT JOIN (select policy_number, count(*) as noofclaims from claim  
         group by policy_number) Claim ON  
  InsFile.Insurance_ref = Claim.Policy_Number  
  
 WHERE Tmp.insurance_file_cnt = InsFile.insurance_file_cnt  
        AND InsFile.insurance_file_status_id IS NULL  
 AND InsFile.renewal_date <= dateadd(day,Prod.renewal_period,@end_date)  
  
DROP TABLE #TempInsFileCnt  





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


