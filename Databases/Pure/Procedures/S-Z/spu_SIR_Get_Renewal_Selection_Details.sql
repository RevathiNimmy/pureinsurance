--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************
EXECUTE DDLDropProcedure 'spu_SIR_Get_Renewal_Selection_Details'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIR_Get_Renewal_Selection_Details
     @insurance_file_cnt INT,
     @source_id INT,
     @compare_date DATETIME,
     @Start_Date DATETIME = NULL,
     @UserId INT = NULL,
     @IgnoreDate TINYINT = NULL
AS

DECLARE @insurance_file_status_id int

SELECT @insurance_file_status_id  = insurance_file_status_id FROM Insurance_File_Status WHERE CODE = 'VOID'


IF EXISTS (SELECT NULL FROM insurance_file WHERE insurance_file_cnt=@insurance_file_cnt AND insurance_file_type_id NOT IN (2,5,9))
RETURN 

IF EXISTS (SELECT NULL FROM Insurance_File i INNER JOIN Insurance_File inf ON i.insurance_folder_cnt=inf.insurance_folder_cnt  
    WHERE i.cover_start_date=inf.renewal_date AND inf.insurance_file_cnt=@insurance_file_cnt AND inf.insurance_file_type_id <> 5
	AND NOT (i.insurance_file_type_id in (3,4,10,11) and IsNull(i.insurance_file_status_id,0) = 1))  
RETURN

DECLARE @anniversary_copy int
SELECT @anniversary_copy=0
IF   NOT EXISTS (select NULL FROM Renewal_Status RS JOIN insurance_file ifi ON rs.renewal_insurance_file_cnt=ifi.insurance_file_cnt 
 WHERE RS.insurance_file_cnt= @insurance_file_cnt  AND anniversary_copy=1) 
 BEGIN
    SELECT @anniversary_copy= 1
 END

IF @UserId IS NULL
BEGIN
SELECT
        ifi.insurance_file_cnt,
        ifo.insurance_holder_cnt,
        ifi.product_id,
        ifi.lead_agent_cnt,
        ifi.Insurance_ref,
        ifi.cover_start_date,
        ifi.expiry_date,
        client_name = CASE WHEN ISNULL(h.resolved_name, '') = '' THEN h.name ELSE h.resolved_name END,
        agent_name = CASE WHEN ISNULL(a.resolved_name, '') = '' THEN a.name ELSE a.resolved_name END,
        p.is_auto_renewable,
        p.description Product_description,
        irsc.description Policy_stop_reason,
        hrsc.description Client_stop_reason,
        ifi.is_referred_at_renewal,
        ifi.insurance_folder_cnt,
        ifi.renewal_date,
        h.shortname client_code,
        arsc.description Agent_stop_reason,
        s.is_deleted,
        pa.is_in_transfer_mode,
 p.is_true_monthly_policy,
 ifi.anniversary_copy,
 ifi.renewal_day_number,
 ifi.anniversary_date,
 p.anniversary_renewal_weeks,
 ifi.put_on_next_instalment_renewal,
 pfIFile.insurance_file_cnt,
  ifi.lead_allow_consolidated_commission,
  ifi.sub_allow_consolidated_commission,
  ifo.renewal_count,
 ifi.renewal_product_id,
     ifi.original_product_id,
      p.tmpautrenfac,
      ifi.alternate_reference 
    FROM Insurance_File ifi
        JOIN Insurance_Folder ifo
            ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
        JOIN Product p
            ON  p.product_id = ifi.product_id
        LEFT JOIN Renewal_Stop_code irsc
            ON irsc.Renewal_stop_code_id = ifi.Renewal_stop_code_id
        JOIN Party h
            ON h.party_cnt = ifo.insurance_holder_cnt
        LEFT JOIN Renewal_Stop_code hrsc
            ON hrsc.Renewal_stop_code_id = h.Renewal_stop_code_id
        LEFT JOIN Party a
            ON a.Party_cnt = ifi.Lead_agent_cnt
        LEFT JOIN Renewal_Stop_code arsc
            ON arsc.Renewal_stop_code_id = a.Renewal_stop_code_id
        LEFT JOIN source s
            ON ifi.source_id = s.source_id
        LEFT JOIN Party_Agent pa
            ON pa.party_cnt = a.party_cnt
 LEFT JOIN (
  SELECT TOP 1 ifile.insurance_file_cnt, ifile.insurance_folder_cnt
  FROM insurance_file ifile
   INNER JOIN pfpremiumfinance pf ON
     pf.insurance_file_cnt = ifile.insurance_file_cnt
  ORDER BY ifile.insurance_file_cnt DESC) pfIFile ON
  ifi.insurance_folder_cnt = pfIFile.insurance_folder_cnt
    WHERE (ifi.insurance_file_status_id IS NULL OR ifi.insurance_file_status_id = 2 OR ifi.insurance_file_status_id = 309 OR ifi.insurance_file_status_id = 4)
        AND ((@Start_Date IS NULL AND ifi.renewal_date <= DATEADD(DAY, ISNULL(p.renewal_period, 0), @compare_date))
            OR (@Start_Date IS NOT NULL AND ifi.renewal_date BETWEEN @Start_Date AND @compare_date) OR @IgnoreDate=1)
        AND ifi.insurance_file_cnt = @insurance_file_cnt
        AND ((ifi.insurance_file_cnt NOT IN (SELECT insurance_file_cnt FROM renewal_status) AND @anniversary_copy=1) OR @anniversary_copy=0)
END
ELSE
BEGIN
SELECT
            ifi.insurance_file_cnt,
            ifo.insurance_holder_cnt,
            ifi.product_id,
            ifi.lead_agent_cnt,
            ifi.Insurance_ref,
            ifi.cover_start_date,
            ifi.expiry_date,
            client_name = CASE WHEN ISNULL(h.resolved_name, '') = '' THEN h.name ELSE h.resolved_name END,
            agent_name = CASE WHEN ISNULL(a.resolved_name, '') = '' THEN a.name ELSE a.resolved_name END,
            p.is_auto_renewable,
            p.description Product_description,
            irsc.description Policy_stop_reason,
            hrsc.description Client_stop_reason,
            ifi.is_referred_at_renewal,
            ifi.insurance_folder_cnt,
            ifi.renewal_date,
      h.shortname client_code,
            arsc.description Agent_stop_reason,
            s.is_deleted,
            pa.is_in_transfer_mode,
        p.is_true_monthly_policy,
      ifi.anniversary_copy,
      ifi.renewal_day_number,
      ifi.anniversary_date,
      p.anniversary_renewal_weeks,
      ifi.put_on_next_instalment_renewal,
      pfIFile.insurance_file_cnt,
      ifi.lead_allow_consolidated_commission,
      ifi.sub_allow_consolidated_commission,
      ifo.renewal_count,
      ifi.renewal_product_id,
      ifi.original_product_id,
      p.tmpautrenfac,
      ifi.alternate_reference 
        FROM Insurance_File ifi
            JOIN Insurance_Folder ifo
                ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
            JOIN Product p
                ON  p.product_id = ifi.product_id
            LEFT JOIN Renewal_Stop_code irsc
                ON irsc.Renewal_stop_code_id = ifi.Renewal_stop_code_id
            JOIN Party h
                ON h.party_cnt = ifo.insurance_holder_cnt
            LEFT JOIN Renewal_Stop_code hrsc
                ON hrsc.Renewal_stop_code_id = h.Renewal_stop_code_id
            LEFT JOIN Party a
                ON a.Party_cnt = ifi.Lead_agent_cnt
            LEFT JOIN Renewal_Stop_code arsc
                ON arsc.Renewal_stop_code_id = a.Renewal_stop_code_id
            LEFT JOIN source s
                ON ifi.source_id = s.source_id
            LEFT JOIN Party_Agent pa
                ON pa.party_cnt = a.party_cnt
     LEFT JOIN (
      SELECT TOP 1 ifile.insurance_file_cnt, ifile.insurance_folder_cnt
      FROM insurance_file ifile
       INNER JOIN pfpremiumfinance pf ON
         pf.insurance_file_cnt = ifile.insurance_file_cnt
      ORDER BY ifile.insurance_file_cnt DESC) pfIFile ON
      ifi.insurance_folder_cnt = pfIFile.insurance_folder_cnt
        WHERE (ifi.insurance_file_status_id IS NULL OR ifi.insurance_file_status_id = 2 OR ifi.insurance_file_status_id = 309 OR ifi.insurance_file_status_id = 4)
            AND ((@Start_Date IS NULL AND ifi.renewal_date <= DATEADD(DAY, ISNULL(p.renewal_period, 0), @compare_date))
                OR (@Start_Date IS NOT NULL AND ifi.renewal_date BETWEEN @Start_Date AND @compare_date) OR @IgnoreDate=1)
            AND ifi.insurance_file_cnt = @insurance_file_cnt
            AND ((ifi.insurance_file_cnt NOT IN (SELECT insurance_file_cnt FROM renewal_status) AND @anniversary_copy=1) OR @anniversary_copy=0)
 AND ifi.source_id NOT IN(SELECT source_id FROM pmuser_source WHERE user_id=@userid)
END
