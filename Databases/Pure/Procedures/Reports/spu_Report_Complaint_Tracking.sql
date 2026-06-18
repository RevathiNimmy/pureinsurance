SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Complaint_Tracking' 
GO

CREATE PROCEDURE spu_Report_Complaint_Tracking
    @start_date DATETIME,
    @end_date DATETIME,
    @complaint_status VARCHAR(30),
    @complaint_owner VARCHAR(30),
    @complaint_cob VARCHAR(30),
    @complaint_category VARCHAR(20)='' 
AS

CREATE TABLE #Complaints
(
    complaint_folder_cnt INT,
    date_opened DATETIME,
    reference VARCHAR(100),
    client_name VARCHAR(255),
    contact VARCHAR(255),
    complaint_category VARCHAR(255),
    complaint_method VARCHAR(255),
    complaint_file_cnt INT,
    complaint_actiontype_id INT,
    complaint_actiontype VARCHAR(10),
    complaint_action_detail VARCHAR(255),
    complaint_about VARCHAR(255),
    complaint_class_of_business VARCHAR(255),
    complaint_owner VARCHAR(255),
    long_complaint VARCHAR(3),
    compensation_paid VARCHAR(3),
    referred_to_fos VARCHAR(3),
    complaint_upheld VARCHAR(3),
    fsa_disabled BIT
)

IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    
    INSERT INTO #Complaints
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT 
        *
    FROM #Complaints
    
    DROP TABLE #Complaints
        
    RETURN  
END

INSERT INTO #Complaints
SELECT
    FCFo.FSA_complaint_folder_cnt,
    FCFo.date_opened,
    FCFo.reference,
    P.resolved_name,
    ISNULL(FCFo.contact,''),
    FCCat.Description,
    FCM.description,
    0,
    0,
    '',
    '',
    PMU.username,
    FCCob.description,
    PMUOwner.username,
    CASE FCFo.long_complaint
        WHEN 0 THEN 'NO'
        ELSE 'YES'
    END,
    CASE FCFo.compensation_paid
        WHEN 0 THEN 'NO'
        ELSE 'YES'
    END,
    CASE FCFo.complaint_referred_to_fos
        WHEN 0 THEN 'NO'
        ELSE 'YES'
    END,
    CASE FCFo.Complaint_upheld
        WHEN 0 THEN 'NO'
        ELSE 'YES'
    END,
    NULL
FROM FSA_Complaint_Folder FCFo
JOIN FSA_Complaint_Method FCM 
    ON FCM.FSA_complaint_method_id = FCFo.FSA_complaint_method_id
JOIN FSA_Class_of_business FCCob 
    ON FCCob.FSA_class_of_business_id = FCFo.FSA_class_of_business_id
JOIN FSA_Complaint_Category FCCat 
    ON FCCat.FSA_complaint_category_id = FCFo.FSA_complaint_category_id
JOIN Party P 
    ON P.party_cnt = FCFo.Party_Cnt
LEFT JOIN PMUser PMU 
    ON PMU.user_id = FCFo.party_handler_cnt
JOIN PMUser PMUOwner 
    ON PMUOwner.user_id = FCFo.complaint_owner_id
WHERE FCFo.date_opened BETWEEN @start_date AND @end_date
AND (
        @complaint_owner = 'ALL'
        OR 
        @complaint_owner = PMUOwner.username
    )
AND (
        @complaint_cob = 'ALL'
        OR 
        @complaint_cob = FCCob.description
    )
AND (
        @complaint_category = 'ALL'
        OR 
        @complaint_category = FCCat.description
    ) 
 
UPDATE C
SET C.complaint_file_cnt = 
    (
        SELECT 
            MAX(FSA_Complaint_file_cnt) 
            FROM FSA_complaint_file
            WHERE FSA_complaint_folder_cnt = C.complaint_folder_cnt
    )
FROM #Complaints C

UPDATE C
SET C.complaint_actiontype_id = FCA.FSA_complaint_actiontype_id,
    C.complaint_actiontype = FCA.code,
    C.complaint_action_detail = FCA.description
FROM #Complaints C 
JOIN FSA_Complaint_file FCFi 
    ON FCFi.FSA_complaint_file_cnt = C.complaint_file_cnt
JOIN FSA_Complaint_actiontype FCA 
    ON FCA.FSA_complaint_actiontype_id = FCFi.FSA_complaint_actiontype_id

DELETE 
FROM #Complaints 
WHERE @complaint_status = 'Open'
AND complaint_actiontype NOT IN ('OPEN_C','AMEND_C','REVIEW_C','REOPEN_C')

DELETE FROM #Complaints 
WHERE @complaint_status = 'Closed'
AND complaint_actiontype NOT IN ('CLOSE_C','REVISCLO_C') 

SELECT 
    *
FROM #Complaints 
ORDER BY 
    complaint_actiontype_id,
    complaint_owner

DROP TABLE #Complaints

GO
 