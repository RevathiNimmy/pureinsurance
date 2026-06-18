SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_LapseCancel  '
GO 

  -------  spu_Report_LapseCancel 2,'01/01/2007','01/01/2009'
  
CREATE procedure spu_Report_LapseCancel  
    @branch_id int,  
    @start_date datetime,  
    @end_date datetime  
AS  
  
DECLARE @iBranchID int  
  
SELECT @iBranchID = ISNULL(@branch_id, 0)  
  
IF @iBranchID = 0  
BEGIN  

---Begin new COde By Kuldeep Panwar NIIT
SELECT I.insurance_ref Policy_Ref,  
        ISNULL(P.resolved_name, '') Client,         -- PN 14825 showing Resolved name in place of name...  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        CASE ST.code  
                when 'LAP' then 'LAPSED'  
                when 'CAN' then 'CANCELLED'  
                END  Status,  
        Sr.source_id,  
        Sr.description Source_Name  
    FROM Insurance_File_Status ST inner join Insurance_File I on ST.insurance_file_status_id = I.insurance_file_status_id
																And( ST.code = 'LAP'  OR ST.code = 'CAN'  
    )  
		 inner join Insurance_File_System S on 	S.insurance_file_cnt = I.insurance_file_cnt
		 inner join Insurance_Folder F on 	F.insurance_folder_cnt = I.insurance_folder_cnt
		 inner join Party P on 	P.party_cnt = I.insured_cnt 
		 left outer Join Lapsed_Reason L on L.lapsed_reason_id = I.lapsed_reason_id	
		 inner join Source Sr on  Sr.source_id  = I.source_id 										
    
  
    WHERE 
    
   (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date
        )  
    
    ORDER BY  
        Sr.source_id, I.insurance_ref  

---End new Code By Kuldeep Panwar



   /*
   Old Code By SSP before NIIT
    SELECT I.insurance_ref Policy_Ref,  
        ISNULL(P.resolved_name, '') Client,         -- PN 14825 showing Resolved name in place of name...  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        CASE ST.code  
                when 'LAP' then 'LAPSED'  
                when 'CAN' then 'CANCELLED'  
                END  Status,  
        Sr.source_id,  
        Sr.description Source_Name  
    FROM Insurance_File I,  
        Insurance_File_Status ST,  
        Insurance_File_System S,  
        Insurance_Folder F,  
        Party P,  
        Lapsed_Reason L,  
        Source Sr  
    WHERE ST.insurance_file_status_id = I.insurance_file_status_id  
    AND  
    (  
        ST.code = 'LAP'  
        OR ST.code = 'CAN'  
    )  
    AND S.insurance_file_cnt = I.insurance_file_cnt  
    --AND F.insurance_folder_cnt = S.insurance_file_cnt   PN14415  
    AND F.insurance_folder_cnt = I.insurance_folder_cnt --PN14415  
    AND P.party_cnt = I.insured_cnt  
    AND L.lapsed_reason_id =* I.lapsed_reason_id  
    AND (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date  
        )  
    AND Sr.source_id  = I.source_id  
    ORDER BY  
        Sr.source_id, I.insurance_ref  
        */
        
END  
ELSE  
BEGIN  
----Begin new COde BY Kuldeep Panwar NIIT
SELECT I.insurance_ref Policy_Ref,  
        ISNULL(P.resolved_name, '') Client,                  -- PN 14825 showing Resolved name in place of name...  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        CASE ST.code  
                when 'LAP' then 'LAPSED'  
                when 'CAN' then 'CANCELLED'  
                END  Status,  
        Sr.source_id,  
        Sr.description Source_Name  
    FROM Insurance_File_Status ST inner join Insurance_File I on ST.insurance_file_status_id = I.insurance_file_status_id
															  And (ST.code = 'LAP' OR ST.code = 'CAN') 
    inner join Insurance_File_System S on S.insurance_file_cnt = I.insurance_file_cnt
    inner join Insurance_Folder F on F.insurance_folder_cnt = S.insurance_file_cnt
    inner join Party P on P.party_cnt = I.insured_cnt
    left outer Join Lapsed_Reason L on L.lapsed_reason_id =I.lapsed_reason_id  
    inner join Source Sr  on Sr.source_id  = I.source_id 
       
    
    WHERE  
    (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date
        )  
    AND I.source_id = @iBranchID  
   
    ORDER BY  
        Sr.source_id, I.insurance_ref 
---End New Code By kuldeep Panwar


/* Old Code By SSP Before NIIT

    SELECT I.insurance_ref Policy_Ref,  
        ISNULL(P.resolved_name, '') Client,                  -- PN 14825 showing Resolved name in place of name...  
        ISNULL(I.lapsed_date, '') Date,  
        ISNULL(L.description, '') Lapse_Reason,  
        CASE ST.code  
                when 'LAP' then 'LAPSED'  
                when 'CAN' then 'CANCELLED'  
                END  Status,  
        Sr.source_id,  
        Sr.description Source_Name  
    FROM Insurance_File I,  
        Insurance_File_Status ST,  
        Insurance_File_System S,  
        Insurance_Folder F,  
        Party P,  
        Lapsed_Reason L,  
        Source Sr  
    WHERE ST.insurance_file_status_id = I.insurance_file_status_id  
    AND  
    (  
        ST.code = 'LAP'  
        OR ST.code = 'CAN'  
    )  
    AND S.insurance_file_cnt = I.insurance_file_cnt  
    AND F.insurance_folder_cnt = S.insurance_file_cnt  
    AND P.party_cnt = I.insured_cnt  
    AND L.lapsed_reason_id =* I.lapsed_reason_id  
    AND (  
        I.lapsed_date >= @start_date  
        AND  
        I.lapsed_date <= @end_date  
        )  
    AND I.source_id = @iBranchID  
    AND Sr.source_id  = I.source_id  
    ORDER BY  
        Sr.source_id, I.insurance_ref 
        */ 
END  
  