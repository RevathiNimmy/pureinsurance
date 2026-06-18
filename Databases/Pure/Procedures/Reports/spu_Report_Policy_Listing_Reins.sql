SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Policy_Listing_Reins'
GO


CREATE PROCEDURE spu_Report_Policy_Listing_Reins
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 06/07/2001
** RSA Reports - Policy_Listing_Long.rpt
**              (Reinsurance subreport)
**
**********************************************************************************************************************************
** 11/07/2001   JMK     datatype of ReinsBand from tinyint to int (pre-empt crystal problems with tinyints)
***********************************************************************************************************************************/
SET NOCOUNT ON
DECLARE @CurrentDate AS datetime
SELECT @CurrentDate = getdate()

CREATE TABLE #tempRSAPolReins
(
    InsuranceCnt int,
    FolderCnt int,
    ReinsType varchar (10),                     -- Policy or Risk
    ReinsBand int NULL,                         --Ins_File_RI_Arrangement.ri_band or Risk_RI_Arrangement
    ReinsAC varchar (20) NULL,                  --RI_Arrangement_Line.agreement_code
    ReinsPremPerc decimal (19,4) NULL,          --RI_Arrangement_Line.premium_percent
    ReinsPremValue decimal (19,4) NULL,         --(ifi.this_premium * RIal.premium_percent)/100
    ReinsCommissionPerc decimal (19,4) NULL,    --RI_Arrangement_Line.commission_percent
    ReinsCommissionVal decimal (19,4) NULL      --(ifi.this_premium * RIal.commission_percent)/100
)
/*
INSERT INTO #tempRSAPolReins
    SELECT ifi.insurance_file_cnt,
        ifi.insurance_folder_cnt,
        'Policy',                                               -- ReinsType
        ifRIa.ri_band_id,
        RIal.agreement_code,
        RIal.premium_percent,
        RIal.premium_value,     -- ReinsPremValue
        RIal.commission_percent,
        RIal.commission_value   -- ReinsCommissionVal
    FROM Insurance_file ifi
	JOIN Insurance_File_Risk_Link ifrl  ON ifi.insurance_file_cnt = ifrl.insurance_file_cnt
    	JOIN Risk r                         ON ifrl.risk_cnt = r.risk_cnt
     
    JOIN RI_Arrangement ifRIa  ON r.risk_cnt = ifRIa.risk_cnt
    JOIN RI_Arrangement_Line RIal       ON ifRIa.ri_arrangement_id = RIal.ri_arrangement_id
        AND isnull(RIal.premium_percent,0) <> 0
    WHERE ifi.insurance_folder_cnt IN
        (
        SELECT  insurance_folder_cnt
        FROM    insurance_file
        WHERE   cover_start_date <= @CurrentDate
        AND     expiry_date >= @CurrentDate
        GROUP BY insurance_folder_cnt
        )
    -- Tracy Richards - Don't want to see Lines that are set up but not used
    AND RIal.premium_value <> 0
    AND RIal.sum_insured <> 0 */

INSERT INTO #tempRSAPolReins
    SELECT ifi.insurance_file_cnt,
        ifi.insurance_folder_cnt,
        'Risk',                                                 -- ReinsType
        RRIa.ri_band_id,
        RIal.agreement_code,
        RIal.premium_percent,
        RIal.premium_value,     -- ReinsPremValue
        RIal.commission_percent,
        RIal.commission_value   -- ReinsCommissionVal
    FROM Insurance_file ifi
    JOIN Insurance_File_Risk_Link ifrl  ON ifi.insurance_file_cnt = ifrl.insurance_file_cnt
    JOIN Risk r                         ON ifrl.risk_cnt = r.risk_cnt
    JOIN RI_Arrangement RRIa       ON r.risk_cnt = RRIa.risk_cnt
    JOIN RI_Arrangement_Line RIal       ON RRIa.ri_arrangement_id = RIal.ri_arrangement_id
        AND isnull(RIal.premium_percent,0) <> 0
    WHERE ifi.insurance_folder_cnt IN
        (
        SELECT  insurance_folder_cnt
        FROM    insurance_file
        WHERE   cover_start_date <= @CurrentDate
        AND     expiry_date >= @CurrentDate
        GROUP BY insurance_folder_cnt
        )
    -- Tracy Richards - Don't want to see Lines that are set up but not used
    AND (RIal.premium_value <> 0
          OR RIal.sum_insured <> 0)
    AND ifi.insurance_file_cnt = (select MAX(insurance_file_cnt) from insurance_file where insurance_folder_cnt = ifi.insurance_folder_cnt)

SET NOCOUNT OFF
-- Squirt it out for the report
SELECT * FROM #tempRSAPolReins

DROP TABLE #tempRSAPolReins
GO


