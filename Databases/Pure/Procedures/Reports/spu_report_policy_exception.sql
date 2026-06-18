SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Policy_Exception'
GO

CREATE PROCEDURE spu_Report_Policy_Exception
    @Start_Date datetime = NULL,
	@End_Date datetime = NULL,
	@RiskCode varchar(255) = '',
    @ReportOn varchar(255) = ''
AS

DECLARE @RiskCodeID int,
        @TargetDate datetime,
        @ReportType INT

IF @ReportOn = 'ALL'
    SELECT @ReportType = 0
ELSE IF @ReportOn = 'Dropped Instalments'
    SELECT @ReportType = 1
ELSE IF @ReportOn = 'Expired Policies'
    SELECT @ReportType = 2
ELSE IF @ReportOn = 'Inconsistent Renewal Dates'
    SELECT @ReportType = 3
ELSE IF @ReportOn = 'Uncompleted Renewals'
    SELECT @ReportType = 4

IF @End_Date > GETDATE()
    SELECT @TargetDate = GETDATE()
ELSE
    SELECT @TargetDate = @End_Date

IF @RiskCode = ''
BEGIN
    SELECT @RiskCodeID = NULL
END
ELSE
BEGIN
    SELECT @RiskCodeID = MAX(risk_code_id) FROM risk_code WHERE description = @RiskCode
END

-- Create a temp table to store the results
CREATE TABLE #tmpResults
    (
        ClientCode           VARCHAR(255),
        InsuranceRef         VARCHAR(1024),
        RiskCode             VARCHAR(255),
        Risk                 VARCHAR(255),
        CoverStartDate       DATETIME,
        CoverExpiryDate      DATETIME,
        Reason               VARCHAR(1025),
        PolicyType           VARCHAR(1024),
        PolicyVersion        INT,
        ReportType           VARCHAR(1024)
    )

-- Check for expired policies
IF @ReportType IN (0, 2)
BEGIN
    INSERT #tmpResults
    SELECT
    	p.shortname as ClientCode,
    	ifi.insurance_ref as InsuranceReference,
        rc.code as RiskCode,
    	rc.description as Risk,
    	ifi.cover_start_date as CoverFrom,
    	ifi.renewal_date as Renewal,
    	'Cover has expired, but policy has not been lapsed.' as Reason,
        ift.description + ' - ' + ISNULL(ifs.description, '') as PolicyType,
        ifi.policy_version as PolicyVersion,
        'Expired Policies' as ReportType
    
    FROM
    	insurance_file ifi
    	JOIN party p on p.party_cnt = ifi.insured_cnt
    	JOIN risk_code rc on rc.risk_code_id = ifi.risk_code_id
        JOIN insurance_file_type ift on ifi.insurance_file_type_id = ift.insurance_file_type_id
        LEFT JOIN insurance_file_status ifs on ifi.insurance_file_status_id = ifs.insurance_file_status_id
    	JOIN (	-- policies, permanent and temporary MTAs only
    		    SELECT MAX(policy_version) policy_version, insurance_folder_cnt
    		    FROM insurance_file ifi2
    		    WHERE ifi2.insurance_file_type_id IN (2, 5, 6)
                GROUP BY insurance_folder_cnt
    		) AS ifl on ifl.insurance_folder_cnt = ifi.insurance_folder_cnt AND ifi.policy_version = ifl.policy_version
    
    WHERE
    	IFI.insurance_file_type_id IN (2, 5, 6)
    	AND ISNULL(IFI.insurance_file_status_id, 3) = 3
    	AND (@Start_Date IS NULL OR renewal_date >= @Start_Date)
    	AND ((@End_Date IS NULL AND renewal_date <= getdate()) OR (renewal_date <= @TargetDate))
    	AND (@RiskCodeID IS NULL OR rc.risk_code_id = @RiskCodeID)
    	AND lapsed_reason_id IS NULL
    	AND ISNULL(IFI.policy_ignore, 0) <> 1
    
END

-- check for renewal dates out of sync between policy versions
IF @ReportType IN (0, 3)
BEGIN
    INSERT #tmpResults
    SELECT
    	p.shortname as ClientCode,
    	ifi.insurance_ref as InsuranceReference,
        rc.code as RiskCode,
    	rc.description as Risk,
    	ifi.cover_start_date as CoverStartDate,
    	ifi.renewal_date as CoverExpiryDate,
    	'Renewal dates for versions of this policy appear to be incorrect.' as Reason,
        ift.description + ' - ' + ISNULL(ifs.description, '') as PolicyType,
        ifi.policy_version as PolicyVersion,
        'Inconsistent Renewal Dates' as ReportType
    
    FROM
        insurance_file ifi
    	JOIN party p on p.party_cnt = ifi.insured_cnt
        JOIN risk_code rc on rc.risk_code_id = ifi.risk_code_id
        JOIN insurance_file_type ift on ifi.insurance_file_type_id = ift.insurance_file_type_id
        LEFT JOIN insurance_file_status ifs on ifi.insurance_file_status_id = ifs.insurance_file_status_id
    
        JOIN (   -- get the latest version of the policy
    		    SELECT MAX(policy_version) policy_version, insurance_folder_cnt
    		    FROM insurance_file ifi2
    		    WHERE ifi2.insurance_file_type_id IN (2, 5, 6)
                GROUP BY insurance_folder_cnt
    		) AS ifl on ifl.insurance_folder_cnt = ifi.insurance_folder_cnt AND ifi.policy_version = ifl.policy_version
    
        LEFT OUTER JOIN
            (   -- get the latest renewal date from each policy
                SELECT MAX(renewal_date) renewal_date, insurance_folder_cnt
                FROM insurance_file
                WHERE
                	insurance_file_type_id IN (2, 5, 6)
                	AND insurance_file_status_id IS NULL --
                	AND (@RiskCodeID IS NULL OR risk_code_id = @RiskCodeID)
                	AND lapsed_reason_id IS NULL
                	AND ISNULL(policy_ignore, 0) <> 1
    
                GROUP BY insurance_folder_cnt
            ) AS ifRen ON ifi.insurance_folder_cnt = ifRen.insurance_folder_cnt
    
    WHERE
         DATEDIFF(d, ifRen.renewal_date, ifi.renewal_date) < 0
         AND (@RiskCodeID IS NULL OR rc.risk_code_id = @RiskCodeID)
         AND (@Start_Date IS NULL OR ifi.renewal_date >= @Start_Date)
         AND ((@End_Date IS NULL AND ifi.renewal_date <= GETDATE()) OR (ifi.renewal_date <= @End_Date))
END
    
-- check for dropped instalment plans
IF @ReportType IN (0, 1)
BEGIN
    INSERT #tmpResults
    SELECT  
    	p.shortname as ClientCode,
    	ifi.insurance_ref as InsuranceReference,
        rc.code as RiskCode,
    	rc.description as Risk,
    	ifi.cover_start_date as CoverStartDate,
    	ifi.renewal_date as CoverExpiryDate,
    	'Possible dropped instalment/finance plan.' as Reason,
        ift.description + ' - ' + ISNULL(ifs.description, '') as PolicyType,
        ifi.policy_version as PolicyVersion,
        'Dropped Instalments' as ReportType
    
    FROM insurance_file ifi
        JOIN party p ON ifi.insured_cnt = p.party_cnt
        JOIN risk_code rc on rc.risk_code_id = ifi.risk_code_id
        JOIN insurance_file_type ift on ifi.insurance_file_type_id = ift.insurance_file_type_id
        LEFT JOIN insurance_file_status ifs on ifi.insurance_file_status_id = ifs.insurance_file_status_id
    
    JOIN
        (
            -- Get all policies that have ever had a finance plan
            SELECT DISTINCT(i.insurance_ref)
            FROM insurance_file i
            WHERE i.insurance_file_cnt IN
                (
                    SELECT pf.insurance_file_cnt 
                    FROM pfpremiumfinance pf
                )
        )AS iFin ON ifi.insurance_ref = iFin.insurance_ref
    
    JOIN
        (
            -- get the latest version of the policy
            SELECT MAX(insurance_file_cnt) insurance_file_cnt, insurance_ref
            FROM insurance_file
            WHERE insurance_file_type_id IN (2, 5)
            AND (insurance_file_status_id IS NULL OR insurance_file_status_id = 3) 
            GROUP BY insurance_ref
        )AS iMax ON ifi.insurance_file_cnt = iMax.insurance_file_cnt
    
    WHERE iMax.insurance_file_cnt NOT IN 
        (
            -- no finance plan for latest version
            SELECT pf.insurance_file_cnt 
            FROM pfpremiumfinance pf
        )
    	AND IFI.insurance_file_type_id IN (2, 5, 6)
    	AND ISNULL(IFI.insurance_file_status_id, 3) = 3
     	AND (@Start_Date IS NULL OR ifi.renewal_date >= @Start_Date)
     	AND ((@End_Date IS NULL AND ifi.renewal_date <= GETDATE()) OR (ifi.renewal_date <= @TargetDate))
    	AND (@RiskCodeID IS NULL OR rc.risk_code_id = @RiskCodeID)
    	AND lapsed_reason_id IS NULL
    	AND ISNULL(IFI.policy_ignore, 0) <> 1
        AND @ReportType IN (0, 1)
END

-- check for incomplete renewals on expired policies
IF @ReportType IN (0, 4)
BEGIN
    INSERT #tmpResults
    SELECT 
    	p.shortname as ClientCode,
    	ifi.insurance_ref as InsuranceReference,
        rc.code as RiskCode,
    	rc.description as Risk,
    	ifi.cover_start_date as CoverFrom,
    	ifi.renewal_date as Renewal,
    	'Policy renewal appears to be incomplete - (' + rs.description + ').' as Reason,
        ift.description + ' - ' + ISNULL(ifs.description, '') as PolicyType,
        ifi.policy_version as PolicyVersion,
        'Uncompleted Renewals' as ReportType
    
    FROM renewal_control rct
        JOIN insurance_file ifi ON rct.renewal_insurance_file_cnt = ifi.insurance_file_cnt
        JOIN party p ON ifi.insured_cnt = p.party_cnt
        JOIN risk_code rc on rc.risk_code_id = ifi.risk_code_id
        LEFT JOIN insurance_file_status ifs on ifi.insurance_file_status_id = ifs.insurance_file_status_id
        JOIN insurance_file_type ift on ifi.insurance_file_type_id = ift.insurance_file_type_id
        JOIN renewal_status_type rs ON rct.renewal_status_type_id = rs.renewal_status_type_id
    
    WHERE
    	IFI.insurance_file_type_id IN (2, 5, 6)
    	AND ISNULL(IFI.insurance_file_status_id, 3) = 3
     	AND (@Start_Date IS NULL OR ifi.cover_start_date >= @Start_Date)
     	AND ((@End_Date IS NULL AND ifi.cover_start_date <= GETDATE()) OR (ifi.cover_start_date <= @TargetDate))
    	AND (@RiskCodeID IS NULL OR rc.risk_code_id = @RiskCodeID)
    	AND lapsed_reason_id IS NULL
    	AND ISNULL(IFI.policy_ignore, 0) <> 1
        AND @ReportType IN (0, 4)
    
    ORDER BY
    	ifi.renewal_date
END
    
SELECT * FROM #tmpResults ORDER BY InsuranceRef
DROP TABLE #tmpResults

GO