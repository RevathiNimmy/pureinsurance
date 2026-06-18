/*
This stored procedure is used by the following reports:

Claim_Details_Breakdown_By_Peril.rpt
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Claim_Details_Breakdown'
GO

CREATE PROCEDURE spu_Report_Claim_Details_Breakdown

    @party_code VARCHAR(50),
    @insurer_code VARCHAR(50),
    @policy_number VARCHAR(100),
    @incident_start_date DATETIME,
    @incident_end_date DATETIME,
    @acc_exec_code VARCHAR(50),
    @primary_cause VARCHAR(100),
    @handler VARCHAR(100),
    @progress_status VARCHAR(100),
    @claim_status VARCHAR(100),
    @inc_exc_risks VARCHAR(1),
    @inc_exc_risk_groups VARCHAR(1),
    @acc_hand_code VARCHAR(50),
    @session_id INT,
    @branch_id INT,
    @unique_report_name VARCHAR(300)
    

AS

DECLARE 
    @ExcRisks INT,
    @ExcGroups INT,
    @GroupByClient INT,
    @GroupByPolicy INT,
    @ClaimStatus INT,
    @iBranchID INT,
    @InsuranceFileAccountExec VARCHAR(1),
    @claim_id INT,
    @current_reserve MONEY,
    @reserve_amount MONEY,
    @recovery_amount MONEY,
    @paid_to_date MONEY,
    @acc_exec_cnt INT,
    @acc_hand_cnt INT,
    @ae_code VARCHAR(20),
    @ah_code VARCHAR(20),
    @ae_name VARCHAR(255),
    @ah_name VARCHAR(255),
    @claim_peril_id INT

SET NOCOUNT ON

IF @Claim_Status = 'ALL' BEGIN
    SELECT @Claim_Status = NULL
END

SELECT @iBranchID = ISNULL(@branch_id, 0)

SELECT @Claim_Status = ISNULL(@Claim_Status,'')

SELECT @ClaimStatus = (CASE @Claim_Status
    WHEN "" THEN 0
    WHEN "Provisional Open Claim" Then 1
    WHEN "Live Open Claim" THEN 2
    WHEN "Closed" THEN 3
    WHEN "ReOpened" THEN 4
    WHEN "ReClosed" THEN 5
    WHEN "All Open" THEN 6
    WHEN "All Closed" THEN 7
    END)

IF @party_code = 'ALL' BEGIN
    SELECT @party_code = NULL
END
SELECT @party_code = ISNULL(@party_code, '')

IF @insurer_code = 'ALL' BEGIN
    SELECT @insurer_code = NULL
END
SELECT @insurer_code = ISNULL(@insurer_code, '')

IF @policy_number = 'ALL' BEGIN
    SELECT @policy_number = NULL
END
SELECT @policy_number = ISNULL(@policy_number, '')

IF @primary_cause = 'ALL' BEGIN
    SELECT @primary_cause = NULL
END
SELECT @primary_cause = ISNULL(@primary_cause, '')

IF @acc_exec_code = 'ALL' BEGIN
    SELECT @acc_exec_code = NULL
END
SELECT @acc_exec_code = ISNULL(@acc_exec_code, '')

IF @acc_hand_code = 'ALL' BEGIN
    SELECT @acc_hand_code = NULL
END
SELECT @acc_hand_code = ISNULL(@acc_hand_code, '')

IF @handler = 'ALL' BEGIN
    SELECT @handler = NULL
END
SELECT @handler = ISNULL(@handler, '')

IF @progress_status = 'ALL' BEGIN
    SELECT @progress_status = NULL
END
SELECT @progress_status = ISNULL(@progress_status, '')

IF @claim_status = 'ALL' BEGIN
    SELECT @claim_status = NULL
END 
SELECT @claim_status = ISNULL(@claim_status, '')

SELECT @ExcRisks = 0
IF EXISTS ( SELECT * FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type='RC')
BEGIN
    SELECT @ExcRisks = 1
END

SELECT @ExcGroups = 0
IF EXISTS ( SELECT * FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type='RG')
BEGIN
    SELECT @ExcGroups = 1
END

SELECT @GroupByClient = 0
if exists ( select * from Temp_Report_Grouping WHERE session_id = @session_id AND [group] = "Client:" )
begin
    SELECT @GroupByClient  = 1
end

--a specific client entered, so group by that client on the report
IF @Party_Code <> ''
BEGIN
    SELECT @GroupByClient = 1
END

SELECT @GroupByPolicy = 0
if exists ( select * from Temp_Report_Grouping WHERE session_id = @session_id AND [group] = "Policy Number:" )
begin
    SELECT @GroupByPolicy  = 1
end

--a specific policy entered, so group by that policy on the report
IF @Policy_Number <> ''
BEGIN
    SELECT @GroupByPolicy = 1
END

--PN14246

SELECT @InsuranceFileAccountExec = value
    FROM Hidden_Options
    WHERE option_number = 40
IF ISNULL(@InsuranceFileAccountExec, '') = '' BEGIN
    SELECT @InsuranceFileAccountExec = '0'
END
--PN14246END

CREATE TABLE #Claim_Peril_Details
(
    claimid INT,
    claimperilid INT,
    currentreserve MONEY,
    paidtodate MONEY
)

CREATE TABLE #Claim_Details
(   
    claim_id INT,
    client_name VARCHAR(50),
    claim_number VARCHAR(50),
    claim_description VARCHAR(255),
    loss_from_date DATETIME,
    policy_number VARCHAR(50),
    risk_code_id INT,
    risk_description VARCHAR(255),
    risk_group_id INT,
    acc_exec_cnt INT,
    acc_exec_code VARCHAR(20),
    account_exec VARCHAR(255),
    acc_hand_cnt INT,
    acc_hand_code VARCHAR(20),
    account_handler VARCHAR(255),
    handler VARCHAR(50),
    progress_status VARCHAR(50),
    primary_cause VARCHAR(50),
    claim_status VARCHAR(50),
    insurer VARCHAR(60),
    claim_peril_id INT,
    claim_peril VARCHAR(50),
    reserve_amount MONEY,
    paid_to_date MONEY,
    group_by_client INT,
    group_by_policy INT
)
    
INSERT INTO #Claim_Details
(   
    claim_id,
    client_name,
    claim_number,
    claim_description,
    loss_from_date,
    policy_number,
    risk_code_id,
    risk_description,
    risk_group_id,
    acc_exec_cnt,
    acc_exec_code,
    account_exec,
    acc_hand_cnt,
    acc_hand_code,
    account_handler,
    handler,
    progress_status,
    primary_cause,
    claim_status,
    insurer,
    claim_peril_id,
    claim_peril,
    reserve_amount,
    paid_to_date,
    group_by_client,
    group_by_policy
)

select  c.claim_id,
    c.client_name,
    c.claim_number,
    LEFT(c.description,255),
    c.loss_from_date,
    c.policy_number,
    rc.risk_code_id,
    rc.description,
    rg.risk_group_id,    
    CASE @InsuranceFileAccountExec              --PN14246
        when 0 then ISNULL(p.consultant_cnt,0)
        when 1 then ISNULL(inf.account_executive_cnt,0)
    END                         --PN14246
    ,
    '',
    '',
        ISNULL(inf.account_handler_cnt,0),
        '',
    '',
    h.description,
    CASE c.progress_status_id 
        WHEN NULL THEN '' 
        ELSE ps.description
    END,
    CASE c.primary_cause_id 
        WHEN NULL THEN '' 
        ELSE pc.description
    END, 
    CASE c.claim_status_id
        WHEN 1 THEN 'Prov. Open'
        WHEN 2 THEN 'Live Open'
        WHEN 3 THEN 'Closed'
        WHEN 4 THEN 'ReOpened'
        WHEN 5 THEN 'ReClosed'
    END,
    c.insurer_name,
    cp.claim_peril_id,
    cp.description,
    0,
    0,
    0,
    0
    FROM claim c
    JOIN claim_peril cp
    ON cp.claim_id = c.claim_id
    JOIN party p
    ON c.client_short_name = p.shortname
    JOIN insurance_file inf
    ON c.policy_id = inf.insurance_file_cnt
    JOIN primary_cause pc
    ON c.primary_cause_id = pc.primary_cause_id
    JOIN progress_status ps
    ON c.progress_status_id = ps.progress_status_id
            JOIN risk_code rc
    ON rc.risk_code_id = inf.risk_code_id
    JOIN risk_group rg
    ON rc.risk_group_id = rg.risk_group_id
    JOIN Handler h
    ON h.handler_id = c.handler_id
    WHERE ( @party_code = '' OR p.shortname = @party_code )
        AND (
                @iBranchID = 0
                OR
                (
                    @iBranchID <> 0
                    AND
                    inf.source_id = @iBranchID
                )
            )
    AND ( @insurer_code = '' OR c.insurer_short_name = @insurer_code )
    AND ( @policy_number = '' OR inf.insurance_ref = @policy_number )
    AND ( @handler = '' OR h.description = @handler )
    AND ( @primary_cause = '' OR pc.description = @primary_cause )
    AND c.loss_from_date >= @incident_start_date
    AND c.loss_from_date <= @incident_end_date
    AND (@ExcRisks = 0 OR 
        (rc.risk_code_id NOT IN (SELECT id FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='RC') AND @ExcRisks = 1))
    AND (@ExcGroups = 0 OR 
        (rg.risk_group_id NOT IN (SELECT id FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='RG') AND @ExcGroups = 1))
    AND     (
            ( 
            @ClaimStatus = 0 OR
            c.claim_status_id = @ClaimStatus
            ) 
            OR
            (
            (@ClaimStatus = 6 AND c.claim_status_id IN (1, 2, 4)) OR
            (@ClaimStatus = 7 AND c.claim_status_id IN (3, 5))
            )
        )


DECLARE c_Cursor CURSOR FAST_FORWARD FOR    

    SELECT  claim_id,
        claim_peril_id,
        acc_exec_cnt,
        acc_hand_cnt

    FROM    #claim_details

OPEN    c_Cursor

FETCH NEXT FROM c_Cursor INTO   @claim_id,
                @claim_peril_id,
                @acc_exec_cnt,
                @acc_hand_cnt
            
WHILE @@FETCH_STATUS = 0
BEGIN   
    
    SELECT @Current_reserve = 0
    SELECT @reserve_amount = 0
    SELECT @paid_to_date = 0

    DELETE FROM #Claim_Peril_Details

    INSERT INTO #Claim_Peril_Details
    (
        claimid,
        claimperilid,           
        currentreserve,
        paidtodate
    )
    Select  Claim.claim_Id, 
        Claim_Peril.claim_peril_id,
        ISNULL(Reserve.initial_reserve, 0) - ISNULL(Reserve.Paid_to_date, 0),
        Reserve.Paid_To_Date
    from reserve right outer join Claim_Peril
    on reserve.claim_peril_id = Claim_Peril.claim_peril_id
    and reserve.Reserve_type_id in  (select reserve_type_id
                    from reserve_type
                    where Include_in_Total = 1),
    Claim, Peril_type
    where Claim_Peril.Claim_ID = Claim.claim_Id
    and Claim_Peril.Peril_Type_Id = Peril_type.Peril_Type_Id
    and claim.Claim_Id = @Claim_Id
    and claim_peril.claim_peril_id = @claim_peril_id
    and ( reserve.revised_reserve = 0 OR reserve.revised_reserve IS NULL )
    and ( reserve.revised_reserve_entered = 0 OR reserve.revised_reserve_entered IS NULL)
    and reserve.initial_reserve <> 0

    Insert Into #Claim_Peril_Details
    (
        claimid,
        claimperilid,
        currentreserve,
        paidtodate
    )
    select  Claim.claim_Id, 
        Claim_Peril.claim_peril_id,
        ISNULL(Reserve.Revised_reserve,0) - ISNULL(Reserve.Paid_to_date, 0),
        Reserve.Paid_To_Date
    from reserve right outer join Claim_Peril
    on reserve.claim_peril_id = Claim_Peril.claim_peril_id
    and reserve.Reserve_type_id in  (select reserve_type_id
                    from reserve_type
                    where Include_in_Total = 1),
    Claim, Peril_type
    where Claim_Peril.Claim_ID = Claim.claim_Id
    and Claim_Peril.Peril_Type_Id = Peril_Type.peril_type_id
    and claim.Claim_Id = @Claim_Id
    and reserve.revised_reserve <> 0

    SELECT @current_reserve =   ISNULL(
                    (   
                    select  SUM(currentreserve)
                    from #Claim_Peril_Details
                    where claimid = @claim_id
                    and claimperilid = @claim_peril_id
                    )
                    ,0)

    SELECT @paid_to_date =      ISNULL(
                    (   
                    select  SUM(paidtodate)
                    from #Claim_Peril_Details
                    where claimid = @claim_id
                    and claimperilid = @claim_peril_id
                    )
                    ,0)

    SELECT @recovery_amount =   ISNULL(
                    (
                    select  SUM(rec.revised_reserve)
                    from    [recovery] rec
                    join    claim_peril cp
                    on  cp.claim_peril_id = rec.claim_peril_id
                    join    claim c
                    on  c.claim_id = cp.claim_id
                    where   c.claim_id = @claim_id
                    and     cp.claim_peril_id = @claim_peril_id
                    )
                    ,0)

    UPDATE #claim_details
    SET reserve_amount = @current_reserve - @recovery_amount
    WHERE claim_id = @claim_id
    AND claim_peril_id = @claim_peril_id

    UPDATE #claim_details
    SET paid_to_date = @paid_to_date
    WHERE claim_id = @claim_id
    AND claim_peril_id = @claim_peril_id

    IF @acc_exec_cnt <> 0
    BEGIN

        SELECT  @ae_code = shortname, @ae_name = resolved_name
        FROM    party
        WHERE   party_cnt = @acc_exec_cnt
        
        UPDATE #claim_details 
        SET acc_exec_code = @ae_code, account_exec = @ae_name
        WHERE claim_id = @Claim_id
    
    END 

    IF @acc_hand_cnt <> 0
    BEGIN

        SELECT  @ah_code = shortname, @ah_name = resolved_name
        FROM    party
        WHERE   party_cnt = @acc_hand_cnt
        
        UPDATE #claim_details 
        SET acc_hand_code = @ah_code, account_handler = @ah_name
        WHERE claim_id = @Claim_id

    END 

    FETCH NEXT FROM c_Cursor INTO   @Claim_Id, @claim_peril_id, @acc_exec_cnt, @acc_hand_cnt

END

CLOSE       c_cursor
DEALLOCATE  c_cursor

IF @acc_exec_code <> '' 
BEGIN
    DELETE FROM #claim_details WHERE acc_exec_code <> @acc_exec_code
END

IF @acc_hand_code <> ''
BEGIN
    DELETE FROM #claim_details WHERE acc_hand_code <> @acc_hand_code
END

IF @ExcRisks = 1
BEGIN
    DELETE FROM #claim_details 
    WHERE risk_code_id IS NULL 
    AND EXISTS(SELECT NULL FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='RC' AND id=0)
END

IF @ExcGroups =1
BEGIN
    DELETE FROM #claim_details 
    WHERE risk_group_id IS NULL 
    AND EXISTS(SELECT NULL FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='RG' AND id=0)
END

UPDATE #claim_details 
SET group_by_client = @GroupByClient

UPDATE #claim_details
SET group_by_policy = @GroupByPolicy

if ((SELECT COUNT(*) FROM #claim_details) > 0 )
Begin
    If @GroupByClient = 0 AND @GroupByPolicy = 0
    Begin 
        select * from #claim_details
        order by loss_from_date
    end

    If @GroupByClient = 1 AND @GroupByPolicy = 0    
    Begin
        select * from #claim_details
        order by client_name, loss_from_date
    end

    If @GroupByClient = 0 AND @GroupByPolicy = 1
    Begin
        select * from #claim_details
        order by policy_number, loss_from_date
    end

    If @GroupByClient = 1 AND @GroupByPolicy = 1
    Begin
        select * from #claim_details
        order by client_name, policy_number, loss_from_date
    end
End
Else
Begin
    Insert into #claim_details 
    Select NULL,NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,3,3

    select * from #claim_details 
End

DELETE FROM #Claim_Peril_Details
DROP TABLE  #Claim_Peril_Details
DELETE FROM #Claim_Details
DROP TABLE  #Claim_Details

SET NOCOUNT OFF
GO

