SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Lapse_By_AccountHandler_BT'
GO 

--- spu_Report_Lapse_By_AccountHandler_BT 2,'01/01/2007','01/01/2009'
CREATE PROCEDURE spu_Report_Lapse_By_AccountHandler_BT  
    @branch_id int,  
    @start_date datetime,  
    @end_date datetime  
AS  
  
DECLARE @iBranchID int  
  
SELECT @iBranchID = ISNULL(@branch_id, 0)  
  
IF @iBranchID = 0  
BEGIN  
-----Begin New Code by Kuldeep Panwar NIIT
SELECT  
        ISNULL(I.source_id,0) Branch_Id,  
        ISNULL(SO.description,'') Branch,  
        ISNULL(BT.business_type_id,0)   Business_Type_Id,  
        ISNULL(BT.description, '') Business_Type,  
        ISNULL(PH.party_cnt, 0) Handler_Cnt,  
        ISNULL(PH.resolved_name, 'No Handler') Handler,  
        I.insurance_ref Policy_Ref,  
        ISNULL(PC.resolved_name, '') Client,  
        I.this_premium Premium,  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        ST.code Status,  
        /*Currency Details*/  
        C.currency_id,  
        C.code 'Currency_Code',  
        C.description 'Currency_Desc'  
    FROM Insurance_File_Status ST inner join Insurance_File I on  ST.insurance_file_status_id = I.insurance_file_status_id  
																AND (ST.code = 'LAP' OR ST.code = 'CAN')  
		inner join Insurance_File_System S on S.insurance_file_cnt = I.insurance_file_cnt 
		inner join  Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt 
		inner join Party PC on PC.party_cnt = F.insurance_holder_cnt
		left outer join Party PH on PH.party_cnt = I.account_handler_cnt 
		left outer join Lapsed_Reason L on L.lapsed_reason_id = I.lapsed_reason_id
		inner join Business_Type BT on I.business_type_id = BT.business_type_id
		inner join Source  SO on I.source_id = SO.source_id  
		inner join currency C on C.currency_id = I.currency_id  
		
	 
    WHERE  (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date  
        )  
    
    ORDER BY  
        C.currency_id,  
        Business_type_id,  
        Handler,  
        Lapse_reason,  
        Policy_Ref  

-----End New Code By Kuldeep

/*
Old Code By SSP Before NIIT
    SELECT  
        ISNULL(I.source_id,0) Branch_Id,  
        ISNULL(SO.description,'') Branch,  
        ISNULL(BT.business_type_id,0)   Business_Type_Id,  
        ISNULL(BT.description, '') Business_Type,  
        ISNULL(PH.party_cnt, 0) Handler_Cnt,  
        ISNULL(PH.resolved_name, 'No Handler') Handler,  
        I.insurance_ref Policy_Ref,  
        ISNULL(PC.resolved_name, '') Client,  
        I.this_premium Premium,  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        ST.code Status,  
        /*Currency Details*/  
        C.currency_id,  
        C.code 'Currency_Code',  
        C.description 'Currency_Desc'  
    FROM Insurance_File I,  
        Insurance_File_Status ST,  
        Insurance_File_System S,  
        Insurance_Folder F,  
        Party PC,  
        Party PH,  
        Lapsed_Reason L,  
        Business_Type BT,  
        Source  SO,  
        currency C  
    WHERE ST.insurance_file_status_id = I.insurance_file_status_id  
    AND  
    (  
        ST.code = 'LAP'  
        OR ST.code = 'CAN'  
    )  
    AND S.insurance_file_cnt = I.insurance_file_cnt  
    AND F.insurance_folder_cnt = I.insurance_folder_cnt  
    AND PC.party_cnt = F.insurance_holder_cnt  
    AND PH.party_cnt =* I.account_handler_cnt  
    AND L.lapsed_reason_id =* I.lapsed_reason_id  
    AND (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date  
        )  
    AND I.business_type_id = BT.business_type_id  
    AND I.source_id = SO.source_id  
    AND C.currency_id = I.currency_id  
    ORDER BY  
        C.currency_id,  
        Business_type_id,  
        Handler,  
        Lapse_reason,  
        Policy_Ref  
        
        */
END  
ELSE  
BEGIN  
-------Begin New Code By KUldeep panwar Niit

SELECT  
    ISNULL(I.source_id,0) Branch_Id,  
    ISNULL(SO.description,'') Branch,  
    ISNULL(BT.business_type_id,0)   Business_Type_Id,  
        ISNULL(BT.description, '') Business_Type,  
        ISNULL(PH.party_cnt, 0) Handler_Cnt,  
        ISNULL(PH.resolved_name, 'No Handler') Handler,  
        I.insurance_ref Policy_Ref,  
        ISNULL(PC.resolved_name, '') Client,  
        I.this_premium Premium,  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        ST.code Status,  
        /*Currency Details*/  
        C.currency_id,  
        C.code 'Currency_Code',  
        C.description 'Currency_Desc'  
     FROM Insurance_File_Status ST inner join Insurance_File I on  ST.insurance_file_status_id = I.insurance_file_status_id  
																AND (ST.code = 'LAP' OR ST.code = 'CAN')  
		inner join Insurance_File_System S on S.insurance_file_cnt = I.insurance_file_cnt 
		inner join  Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt 
		inner join Party PC on PC.party_cnt = F.insurance_holder_cnt
		left outer join Party PH on PH.party_cnt = I.account_handler_cnt 
		left outer join Lapsed_Reason L on L.lapsed_reason_id = I.lapsed_reason_id
		inner join Business_Type BT on I.business_type_id = BT.business_type_id
		inner join Source  SO on I.source_id = SO.source_id  
		inner join currency C on C.currency_id = I.currency_id  
		
	 
    WHERE  (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date  
        )  AND I.source_id = @iBranchID  
    ORDER BY  
        C.currency_id,  
        Branch_id,  
        Business_type_id,  
        Handler,  
        Lapse_reason,  
        Policy_Ref  


-------End new Code By Kuldeep
/* Old Code By SSP Before NIIT Change
   SELECT  
    ISNULL(I.source_id,0) Branch_Id,  
    ISNULL(SO.description,'') Branch,  
    ISNULL(BT.business_type_id,0)   Business_Type_Id,  
        ISNULL(BT.description, '') Business_Type,  
        ISNULL(PH.party_cnt, 0) Handler_Cnt,  
        ISNULL(PH.resolved_name, 'No Handler') Handler,  
        I.insurance_ref Policy_Ref,  
        ISNULL(PC.resolved_name, '') Client,  
        I.this_premium Premium,  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        ST.code Status,  
        /*Currency Details*/  
        C.currency_id,  
        C.code 'Currency_Code',  
        C.description 'Currency_Desc'  
    FROM Insurance_File I,  
        Insurance_File_Status ST,  
        Insurance_File_System S,  
        Insurance_Folder F,  
        Party PC,  
        Party PH,  
        Lapsed_Reason L,  
        Business_Type BT,  
        Source  SO,  
        Currency C  
    WHERE ST.insurance_file_status_id = I.insurance_file_status_id  
    AND  
    (  
        ST.code = 'LAP'  
        OR ST.code = 'CAN'  
    )  
    AND S.insurance_file_cnt = I.insurance_file_cnt  
    AND F.insurance_folder_cnt = I.insurance_folder_cnt  
    AND PC.party_cnt = F.insurance_holder_cnt  
    AND PH.party_cnt =* I.account_handler_cnt  
    AND L.lapsed_reason_id =* I.lapsed_reason_id  
    AND (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date  
        )  
    AND I.business_type_id = BT.business_type_id  
    AND I.source_id = @iBranchID  
    AND I.source_id = SO.source_id  
    AND C.currency_id = I.currency_id  
    ORDER BY  
        C.currency_id,  
        Branch_id,  
        Business_type_id,  
        Handler,  
        Lapse_reason,  
        Policy_Ref  
        
        */
END  
