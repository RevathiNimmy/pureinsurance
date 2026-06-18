SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Policy_Listing_Risk'
GO


CREATE PROCEDURE spu_Report_Policy_Listing_Risk
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 16/09/2000
** RSA Reports -        PolicyListingLong.rpt
**                       (Risk/Peril subreport)
**********************************************************************************************************************************
** 23/09/2000 - JMK -           Get Rating section details instead of Peril
**
** 20/10/2000 - JMK -           Change Rating Section, (prompted by DB change)
**
** 17/11/2000  Jude Killip      DB change: links between Risk and Insurance_File
**
** 21/03/2001  Jude Killip      increase PRD_Description to varchar (255)
**
** 23/04/2001  Jude Killip      update selection in line with sp_report_policy_listing_long
**                              allow for multiple rating section records per risk
**                              add sequence_number
***********************************************************************************************************************************/
SET NOCOUNT ON
DECLARE @CurrentDate AS datetime
SELECT @CurrentDate = getdate()

CREATE TABLE #tempRSAPolListLR1
(
        RiskCnt int NULL,
        InsuranceCnt int,
        RiskClassCde varchar (10) NULL,
        RiskDesc varchar (255) NULL,
        AccumulateCde varchar (10) NULL
)

-- get Risk details
INSERT INTO #tempRSAPolListLR1
        SELECT r.risk_cnt,
                ifi.insurance_file_cnt,
                (SELECT rt.code FROM Risk_Type rt WHERE r.risk_type_id = rt.risk_type_id),
                r.description,
                (SELECT a.code FROM Accumulation a WHERE r.accumulation_id = a.accumulation_id)
        FROM Insurance_file ifi,
                Insurance_File_Risk_Link ifrl,
                Risk r
        WHERE ifi.insurance_file_cnt = ifrl.insurance_file_cnt
        AND ifrl.risk_cnt = r.risk_cnt
        AND ifi.insurance_file_cnt IN
                (
                SELECT  max(insurance_file_cnt)
                FROM    insurance_file
                WHERE   cover_start_date <= @CurrentDate
                AND     expiry_date >= @CurrentDate
                GROUP BY insurance_folder_cnt
                )
        AND ifrl.status_flag = 'C'
-- add Rating Section details
CREATE TABLE #tempRSAPolListLR2
(
        RiskCnt int NULL,
        RST_Code varchar (10) NULL,
        RST_Description varchar (255) NULL,
        RS_Sequence int NULL,
        RS_SumInsured decimal (19,4) NULL,
        RS_AnnualRate decimal (19,4) NULL,
        RT_Description varchar (255) NULL,
        RS_AnnualPremium decimal (19,4) NULL
)
INSERT INTO #tempRSAPolListLR2
        SELECT rs.risk_cnt,
                rst.code,
                rst.description,
                rs.sequence_number,
                rs.sum_insured,
                rs.annual_rate,
                rt.description,
                rs.annual_premium
        FROM Rating_Section rs,
                Rating_Section_Type rst,
                Rate_Type rt
        WHERE rs.rating_section_type_id = rst.rating_section_type_id
        AND rs.rate_type_id = rt.rate_type_id
        AND rs.original_flag = 0

-- join tables and squirt it all out to the report
SET NOCOUNT OFF
SELECT *
FROM #tempRSAPolListLR1 temp1,
        #tempRSAPolListLR2 temp2
WHERE temp1.riskcnt = temp2.riskcnt
--order by RiskCnt
DROP TABLE #tempRSAPolListLR1
DROP TABLE #tempRSAPolListLR2
GO


