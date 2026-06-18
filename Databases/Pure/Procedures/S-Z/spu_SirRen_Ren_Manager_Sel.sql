SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Ren_Manager_Sel'
GO

CREATE PROCEDURE spu_SirRen_Ren_Manager_Sel 
    @RenewalStatus VARCHAR(255),
    @DueDateStart DATETIME,
    @DueDateLimit DATETIME,
    @ClientCode VARCHAR(255),
    @PolicyNo VARCHAR(255),
    @BusinessTypeID INT,
    @SchemeID INT,
    @InsurerID INT,
    @RiskGroup VARCHAR(255),
    @SuspensionLevel INT = NULL,
    @SuspendedOnlyFlag TINYINT = 0,
    @SourceId INT,
    @AgentCnt INT,
    @InsurerMode INT,
    @OverdueConfirmationsOnlyFlag TINYINT = 0,
    @SortOrderSQL VARCHAR(255) = '',
    @AccountHandlerCnt INT = 0
AS

DECLARE 
    @PolInsFileTypeID INT,
    @MTAPermInsFileTypeID INT,
    @RenInsFileTypeID INT,
    @MTAPermCanInsFileTypeID INT,
    @sSQL VARCHAR(1000),
    @IsUnderwriting BIT,
    @InsuranceFileStatusCancelled INT,
    @current_date DATETIME


IF @ClientCode = 'ALL'
BEGIN
    SELECT @ClientCode = NULL
END

IF @PolicyNo = 'ALL'
BEGIN
    SELECT @PolicyNo = NULL
END
   
IF @SchemeID = 0
BEGIN
    SELECT @SchemeID = NULL
END

IF @InsurerID = 0
BEGIN
    SELECT @InsurerID = NULL
END

IF @SuspensionLevel = 0
BEGIN
    SELECT @SuspensionLevel = NULL
END

IF @AgentCnt = 0
BEGIN
    SELECT @AgentCnt = NULL
END

IF @SourceId = 0
BEGIN
    SELECT @SourceId = NULL
END


SELECT @current_date = GETDATE()
    
SELECT @IsUnderwriting = 0

SELECT 
    @IsUnderwriting = 1
FROM hidden_options 
WHERE option_number = 1 
AND value = 'U'

SELECT @SuspensionLevel = ISNULL(@SuspensionLevel, 0)

SELECT @RenInsFileTypeID = (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code = 'RENEWAL')
SELECT @PolInsFileTypeID = (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code = 'POLICY')
SELECT @MTAPermInsFileTypeID = (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code = 'MTA PERM')
SELECT @MTAPermCanInsFileTypeID = (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code = 'MTAPERMCAN')
SELECT @InsuranceFileStatusCancelled = (SELECT insurance_file_status_id FROM Insurance_File_status WHERE code = 'CAN')

/*create temporary table to sort out agents*/
CREATE TABLE #PolicyLeadAgent
(
    insurance_file_cnt INT, 
    agent_cnt INT
)

IF @IsUnderwriting = 0
BEGIN

    SET NOCOUNT ON

    /*get all policies that have a sub-agent, as this is priority over agent / introducers*/
    INSERT INTO #PolicyLeadAgent
    SELECT 
        PA.insurance_file_cnt, 
        PA.agent_cnt
    FROM party_agent_type PAT 
    JOIN party_agent P 
        ON P.party_agent_type_id = PAT.party_agent_type_id
    JOIN policy_agents PA
        ON PA.agent_cnt = P.party_cnt
    WHERE PAT.description = 'SUB AGENT'

    /*get all policies that have an agent, as this is priority over introducers (but dont include if they have sub agent)*/
    INSERT INTO #PolicyLeadAgent
    SELECT 
        PA.insurance_file_cnt, 
        PA.agent_cnt
    FROM party_agent_type PAT 
    JOIN party_agent P 
        ON P.party_agent_type_id = PAT.party_agent_type_id
    JOIN policy_agents PA
        ON PA.agent_cnt = P.party_cnt
    WHERE PAT.description = 'AGENT'
    AND NOT EXISTS
        (
            SELECT 
                NULL
            FROM #PolicyLeadAgent
            WHERE insurance_file_cnt = PA.insurance_file_cnt
        )
    AND PA.agent_cnt IN
        (
            SELECT TOP 1
                 agent_cnt
            FROM policy_agents 
            WHERE insurance_file_cnt = PA.insurance_file_cnt
            ORDER BY agent_commission_value DESC
        )
        
    /*get all policies that have a introducer, (but dont include if they have sub agent or agent)*/
    INSERT INTO #PolicyLeadAgent
    SELECT 
        PA.insurance_file_cnt, 
        PA.agent_cnt
    FROM party_agent_type PAT 
    JOIN party_agent P 
        ON P.party_agent_type_id = PAT.party_agent_type_id
    JOIN policy_agents PA
        ON PA.agent_cnt = P.party_cnt
    WHERE PAT.description = 'INTRODUCER'
    AND NOT EXISTS
        (
            SELECT 
                NULL
            FROM #PolicyLeadAgent
            WHERE insurance_file_cnt = PA.insurance_file_cnt
        )
    AND PA.agent_cnt IN
        (
            SELECT TOP 1
                 agent_cnt
            FROM policy_agents 
            WHERE insurance_file_cnt = PA.insurance_file_cnt
            ORDER BY agent_commission_value DESC
        )

    SET NOCOUNT OFF
END

/*Create table to store the results so that we can perform a custom order by when we select the records from it*/
CREATE TABLE #PreOrderedResults 
(
    insurance_folder_cnt INT NOT NULL,
    scheme_desc VARCHAR(70) NULL,
    holding_insurer VARCHAR(255) NULL,
    original_insurance_file_cnt INT NULL,
    renewal_status VARCHAR(10) NULL,
    renewal_status_description VARCHAR(255) NULL,
    suspension_level INT NULL,
    renewal_edi_audit_id INT NULL,
    renewal_gis_scheme_id INT NULL,
    product_id INT NULL,
    due_date DATETIME NULL,
    insurance_ref VARCHAR(30) NULL,
    this_premium MONEY NULL ,
    resolved_name VARCHAR(255) NULL,
    shortname VARCHAR(20)  NULL,
    agent_name VARCHAR(255) NULL,
    data_model_code VARCHAR(10) NULL,
    insured_cnt INT NULL,
    business_type_code VARCHAR(10) NULL,
    renewal_premium MONEY NULL,
    scheme_id  INT NULL,
    business_type_id INT NULL,
    insurer_id  INT NULL,
    suspension_reason VARCHAR(1000) NULL,
    quote1 NUMERIC(14,2) NULL,
    excess1 INT NULL,
    quote2 NUMERIC(14,2) NULL,
    excess2 INT NULL,
    quote3 NUMERIC(14,2) NULL,
    excess3 INT NULL,
    is_insurer_lead SMALLINT NULL,
    Offer_Alt SMALLINT NULL,
    Lapse_Reason VARCHAR(255) NULL,
    Reset_Flag TINYINT NULL,
    gis_screen_id INT NULL, 
    risk_group VARCHAR(255) NULL,
    renewal_status_type_id INT NULL,
    [rea.renewal_edi_status] INT NULL,
    policy_type_id INT NULL,
    short_suspension_reason VARCHAR(70) NULL,
    source_description VARCHAR(255) NULL,
    alternate_reference VARCHAR(80) NULL,
    edi_message_sent TINYINT NULL,
    renewal_insurance_file_cnt INT NULL,
    reminder_day_num INT NULL,
    [RNL.cover_start_date] DATETIME NULL,
    [RNL.expiry_date] DATETIME NULL,
    renewal_insurance_file_type_id INTEGER NULL,
    renewal_gis_policy_link_id INTEGER NULL,
    original_gis_scheme_id INTEGER NULL,
    payment_method VARCHAR(70),
    CONSTRAINT PK__PreOrderedResults PRIMARY KEY CLUSTERED (
            insurance_folder_cnt)
)
CREATE INDEX I__PreOrderedResults__data_model_code ON #PreOrderedResults(data_model_code)

INSERT INTO #PreOrderedResults
SELECT  
    RC.insurance_folder_cnt,
    GS.scheme_desc,
    (
        SELECT 
            GISINS.description
        FROM GIS_Insurer GISINS 
        WHERE GISINS.gis_insurer_id = GS.gis_insurer_id 
    ),
    IFL.insurance_file_cnt,
    RTRIM(RST.code),
    RST.description,
    ISNULL(RC.suspension_level, 0),
    RC.renewal_edi_audit_id,
    RC.renewal_gis_scheme_id,
    RC.product_id,
    RC.renewal_date,
    RTRIM(IFL.insurance_ref),
    IFL.annual_premium,
    P1.resolved_name,
    P1.shortname,
    CASE @IsUnderwriting
        WHEN 1 THEN
            (
                SELECT
                    ISNULL(P2.resolved_name, '-')
                FROM Party P2 
                WHERE P2.party_cnt = IFL.lead_agent_cnt 
            )
        WHEN 0 THEN
            (
                SELECT
                    ISNULL(P3.resolved_name, '-')
                FROM Party P3 
                WHERE P3.party_cnt = PLA.agent_cnt 
            )
    END,
    (
        SELECT
            GDM.code
        FROM GIS_Data_Model GDM 
        WHERE GDM.gis_data_model_id = RC.gis_data_model_id 
    ),
    
    IFL.insured_cnt,
    (
        SELECT
            GISBT.code
        FROM GIS_Business_Type GISBT 
        WHERE GISBT.gis_business_type_id = IFL.gemini_business_type 
    ),
    rif.this_premium,
    RC.renewal_gis_scheme_id,
    GS.gis_business_type_id,
    IFL.lead_insurer_cnt,
    (
        SELECT
            ELOG.description
        FROM Event_Log ELOG 
        WHERE ELOG.event_cnt = RC.suspension_level 
    ),
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    GS.is_insurer_lead,
    RC.offer_alt,
    (
        SELECT 
            LR.description 
        FROM Lapsed_Reason LR 
        JOIN Insurance_File IF2
            ON LR.lapsed_reason_id=IF2.lapsed_reason_id
        WHERE IF2.insurance_file_cnt = 
            (
                SELECT 
                    MAX(IF3.insurance_file_cnt) 
                FROM Insurance_File IF3
                WHERE IF3.insurance_folder_cnt = RC.insurance_folder_cnt
            )
    ),
    RC.Reset_Flag,
    ISNULL(RG.gis_screen_id, 0),
    RG.description,
    RC.renewal_status_type_id,
    (
        SELECT
            REA.renewal_edi_status
        FROM renewal_edi_audit REA
        WHERE REA.insurance_folder_cnt = RC.insurance_folder_cnt 
        AND REA.renewal_edi_audit_id = 
            (
                SELECT 
                    MAX(renewal_edi_audit_id)
                FROM renewal_edi_audit
                --WHERE policy_no = REA.policy_no
                WHERE insurance_folder_cnt = rc.insurance_folder_cnt
                AND DATEDIFF (day, date_created, getdate() ) < 60
            )  
    ),
    ISNULL(IFL.policy_type_id, 0),
    
    (
        SELECT
            ELOG.short_description
        FROM Event_Log ELOG 
        WHERE ELOG.event_cnt = RC.suspension_level 
    ),
    S.description,
    IFL.alternate_reference,
    IFL.edi_message_sent,
    RC.renewal_insurance_file_cnt,
    CASE
        WHEN (SELECT ISNULL(reminder_day_num, 0) FROM GIS_Scheme WHERE gis_scheme_id = RC.renewal_gis_scheme_id) <> 0 THEN
            (SELECT reminder_day_num FROM GIS_Scheme WHERE gis_scheme_id = RC.renewal_gis_scheme_id)
        WHEN (SELECT ISNULL(pre_selection_day_num, 0) FROM Renewal_Settings WHERE product_id = RCode.Risk_Group_Id) <> 0 THEN
            (SELECT reminder_day_num FROM Renewal_Settings WHERE product_id = RCode.Risk_Group_Id)
        ELSE
            (SELECT ISNULL(reminder_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
    END,
    (
        SELECT
            RNL.cover_start_date
        FROM Insurance_File RNL 
        WHERE RNL.insurance_file_cnt = RC.renewal_insurance_file_cnt 
    ),
    (
        SELECT
            RNL.expiry_date
        FROM Insurance_File RNL 
        WHERE RNL.insurance_file_cnt = RC.renewal_insurance_file_cnt 
    ),
    rif.insurance_file_type_id,
    rgpl.gis_policy_link_id,
    RC.gis_scheme_id,
    CASE
        WHEN rif.payment_method IS NULL THEN IFL.payment_method
        ELSE rif.payment_method
    END   
FROM Renewal_Control RC
JOIN Renewal_Status_Type RST 
    ON RST.renewal_status_type_id = RC.renewal_status_type_id 
JOIN GIS_Scheme GS 
    ON GS.gis_scheme_id = RC.gis_scheme_id 
JOIN Insurance_File IFL 
    ON IFL.Insurance_file_cnt =
        (
            SELECT 
                MAX(IF1.insurance_file_cnt) 
            FROM Insurance_File IF1
            WHERE IF1.insurance_folder_cnt = RC.insurance_folder_cnt
            AND  IF1.insurance_file_type_id IN (@PolInsFileTypeID,@MTAPermInsFileTypeID)
        )
JOIN Risk_Code RCode 
    ON RCode.risk_code_id = IFL.risk_code_id
JOIN Risk_Group RG 
    ON RG.risk_group_id = RCode.risk_group_id
JOIN Party P1 
    ON P1.party_cnt = IFL.insured_cnt 
JOIN source S
    ON S.source_id = IFL.source_id
LEFT JOIN #PolicyLeadAgent PLA 
    ON PLA.insurance_file_cnt = IFL.insurance_file_cnt   
LEFT JOIN insurance_file rif
    ON rif.insurance_file_cnt = RC.renewal_insurance_file_cnt
LEFT JOIN gis_policy_link rgpl
    ON rgpl.insurance_file_cnt = RC.renewal_insurance_file_cnt
WHERE NOT EXISTS
    (   /*Do not include cancelled policies*/
        SELECT 
            NULL
        FROM insurance_file
        WHERE insurance_folder_cnt = IFL.insurance_folder_cnt
        AND policy_version >= IFL.policy_version
        AND (
                insurance_file_type_id = @MTAPermCanInsFileTypeID
                OR
                ISNULL(insurance_file_status_id, 0) = @InsuranceFileStatusCancelled
            )
    )
AND ISNULL(RC.suspension_level, 0) >= @SuspendedOnlyFlag
AND (
        @RenewalStatus = 'ALL'
        OR
        (
            @RenewalStatus = 'Incomplete'
            AND
            RST.code NOT IN ('RENEWED', 'LAPSED')
        )
        OR
        (
            @RenewalStatus NOT IN ('ALL', 'Incomplete')
            AND
            RST.code = @RenewalStatus
        )
    )
AND RC.renewal_date >= ISNULL(@DueDateStart, RC.renewal_date)
AND RC.renewal_date <= ISNULL(@DueDateLimit, RC.renewal_date)
AND P1.shortname = ISNULL(@ClientCode, P1.shortname)
AND (
        (
            @InsurerMode = 0
            AND
            IFL.insurance_ref = ISNULL(@PolicyNo, IFL.insurance_ref)
        )
        OR
        (
            @InsurerMode = 1
            AND
            IFL.alternate_reference = ISNULL(@PolicyNo, IFL.alternate_reference)
        )
    )
AND (
        @BusinessTypeID = 0
        OR
        (
            @BusinessTypeID <> 0
            AND
            (
                (
                    LEFT(@RiskGroup, 5) = '[GII]'
                    AND
                    GS.gis_business_type_id = @BusinessTypeID
                )
                OR
                (
                    LEFT(@RiskGroup, 5) <> '[GII]'
                    AND
                    RG.description = @RiskGroup
                )
            )
        )
    )
AND ISNULL(RC.renewal_gis_scheme_id, 0) = ISNULL(@SchemeID, ISNULL(RC.renewal_gis_scheme_id, 0))
AND ISNULL(GS.gis_insurer_id, 0) = ISNULL(@InsurerID, ISNULL(GS.gis_insurer_id, 0))
AND (
        @SuspensionLevel = 0
        OR
        (
            @SuspensionLevel <> 0
            AND
            ISNULL(RC.suspension_level, 0) = @SuspensionLevel
        )
    )
AND (
        (
            @IsUnderwriting = 1
            AND
            ISNULL(IFL.lead_agent_cnt, 0) = ISNULL(@AgentCnt, ISNULL(IFL.lead_agent_cnt, 0))
        )
        OR
        (
            @IsUnderwriting = 0
            AND
            ISNULL(PLA.agent_cnt, 0) = ISNULL(@AgentCnt, ISNULL(PLA.agent_cnt, 0))
        )
    )
AND ISNULL(IFL.source_id, 0) = ISNULL(@SourceId, ISNULL(IFL.source_id, 0))
AND (
        (
            @InsurerMode = 0
            AND
            ISNULL(S.underwriting_branch_ind, 0) = 0
        )
        OR
        (
            @InsurerMode = 0
            AND
            ISNULL(S.underwriting_branch_ind, 0) = 1
            AND
            ISNULL(IFL.alternate_reference, '') = ''
        )
        OR
        (
            @InsurerMode = 1
            AND
            ISNULL(S.underwriting_branch_ind, 0) = 1
            AND
            ISNULL(IFL.alternate_reference, '') <> ''
        )
    )
AND (
        @OverdueConfirmationsOnlyFlag = 0
        OR
        (
            @OverdueConfirmationsOnlyFlag = 1
            AND
            ISNULL(RC.renewal_status_type_id, 0) < 5
            AND @current_date >=
                DATEADD(d,
                    CASE
                    WHEN (SELECT ISNULL(reminder_day_num, 0) FROM GIS_Scheme WHERE gis_scheme_id = RC.gis_scheme_id) <> 0 THEN
                        (SELECT -reminder_day_num FROM GIS_Scheme WHERE gis_scheme_id = RC.gis_scheme_id)
                    WHEN (ISNULL(reminder_day_num, 0)) <> 0 THEN
                        -reminder_day_num
                    ELSE
                        (SELECT -ISNULL(reminder_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
                    END
                , RC.renewal_date)
         )
    )
AND (
        @AccountHandlerCnt = 0
        OR
        (
            @AccountHandlerCnt = -1
            AND
            IFL.account_handler_cnt IS NULL
        )
        OR
        (
            @AccountHandlerCnt > 0
            AND
            IFL.account_handler_cnt = @AccountHandlerCnt 
        )
    )

/* Update Alternate Quotes For Gemini Only */
EXEC spu_SirRen_Alt_Quotes_Ins

/*Select all records with any ordering as required*/
SELECT @sSQL= 'select * from #PreOrderedResults' + CHAR(13) + CHAR(10)
IF @SortOrderSQL = ''
BEGIN
    SELECT @sSQL= @sSQL  + 'ORDER BY 01 ASC' + CHAR(13) + CHAR(10)
END
ELSE
BEGIN
    SELECT @sSQL= @sSQL  + 'ORDER BY ' + @SortOrderSQL + CHAR(13) + CHAR(10)
END

EXEC (@sSQL) 

DROP TABLE #PolicyLeadAgent
DROP TABLE #PreOrderedResults

GO

