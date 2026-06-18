SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Bayliss_Coach 
GO

CREATE PROCEDURE spu_Report_Bayliss_Coach  
    @start_date DATETIME,  
    @end_date DATETIME
AS  
BEGIN

DECLARE  
    @table_name VARCHAR(30),  
    @table_id INT  
  
SELECT  
    D.document_id DocumentID,  
    RV.SBO_Vehicles2_ID SortID,  
    GReason.description Reason,  
    P.resolved_name PolicyHolder,  
    I.insurance_ref PolicyNo,  
    D.document_date DocumentDate,  
    RV.effective_date EffectiveDate,  
    T.amount Gross,  
    RV.NCB NCB,  
    T.ref_amount IPT,  
    PA.postal_code PostCode,  
    GDistrict.description District,  
    GCover.description Cover,  
    GUse.description VehicleUse,  
    GDriving.description Driving,  
    RV.Vehicle_Value Value,  
    RV.year Year,  
    RV.Registration Registration,  
    RV.Seats Seats,  
    PAg.shortname Agency,  
    I.payment_method PaymentMethod,  
    (  
     SELECT  
         SUM(TX.amount)  
     FROM  
         .Transdetail TX
         INNER JOIN .Account AX  
             ON AX.account_id = TX.account_id  
     WHERE  
         AX.ledger_id IN (
                          SELECT ledger_id 
                          FROM ledger 
                          WHERE ledger_short_name = 'IN'
                         ) --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
         AND TX.document_id = D.document_id  
         AND TX.spare = 'COMM'  
    ) Commission  
FROM  
    Insurance_File I 
    INNER JOIN GIS_Policy_Link L
        ON L.insurance_file_cnt = I.insurance_folder_cnt 
    INNER JOIN SBO_Policy_Binder B
        ON B.gis_policy_link_id = L.gis_policy_link_id  
    INNER JOIN SBO_Coach2 RC
        ON RC.SBO_policy_binder_id = B.SBO_policy_binder_id  
    INNER JOIN SBO_Vehicles2 RV
        ON RV.SBO_Coach2_id = RC.SBO_Coach2_id
        AND RV.SBO_policy_binder_id = RC.SBO_policy_binder_id  
        AND RV.effective_date <= @end_date 
    INNER JOIN GIS_User_Def_Detail GReason
        ON GReason.gis_user_def_detail_id = RC.Reason_For_Issue  
    INNER JOIN GIS_User_Def_Detail GDistrict
        ON GDistrict.gis_user_def_detail_id = RC.District  
    INNER JOIN GIS_User_Def_Detail GCover
        ON GCover.gis_user_def_detail_id = RV.Cover  
    INNER JOIN GIS_User_Def_Detail GUse
        ON GUse.gis_user_def_detail_id = RV.Vehicle_Use  
    INNER JOIN GIS_User_Def_Detail GDriving
        ON GDriving.gis_user_def_detail_id = RC.Driving_Restrictions  
    INNER JOIN Transaction_Export_Folder F
        ON F.insurance_file_cnt = I.insurance_file_cnt
        AND F.accounts_export_status = 'c'      
    INNER JOIN .Document D  
        ON D.document_ref = F.document_ref 
        AND D.document_date BETWEEN @start_date AND @end_date   
    INNER JOIN .Transdetail T
        ON T.document_id = D.document_id 
        AND  
       (  
        (  
         D.documenttype_id IN (4, 15, 17, 31, 35)  
         AND  
         T.amount > 0  
        )  
       OR  
        (  
         D.documenttype_id NOT IN (4, 15, 17, 31, 35)  
         AND  
         T.amount < 0  
        )  
       )  
    INNER JOIN .Account A
        ON A.account_id = T.account_id
        AND A.ledger_id IN (
                            SELECT ledger_id 
                            FROM ledger 
                            WHERE ledger_short_name = 'SA'
                           ) --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id     
    INNER JOIN Party P
        ON A.account_key = P.party_cnt 
    INNER JOIN Party_Address_Usage U
        ON U.party_cnt = P.party_cnt  
    INNER JOIN Address_Usage_Type UT
        ON UT.address_usage_type_id = U.address_usage_type_id
        AND UT.code = '3131 XCO'  
    INNER JOIN Address PA
        ON PA.address_cnt = U.address_cnt  
    LEFT OUTER JOIN Party PAg  
        ON PAg.party_cnt = I.lead_agent_cnt  

UNION  
SELECT  
    D.document_id DocumentID,  
    RD.SBO_Coach2_Drivers_ID SortID,  
    GReason.description Reason,  
    P.resolved_name PolicyHolder,  
    I.insurance_ref PolicyNo,  
    D.document_date DocumentDate,  
    RD.effective_date EffectiveDate,  
    T.amount Gross,  
    0 NCB,  
    T.ref_amount IPT,  
    PA.postal_code PostCode,  
    GDistrict.description District,  
    '' Cover,  
    '' VehicleUse,  
    GDriving.description Driving,  
    0 Value,  
    '' Year,  
    '' Registration,  
    0 Seats,  
    PAg.shortname Agency,  
    I.payment_method PaymentMethod,  
    (  
     SELECT  
         SUM(TX.amount)  
     FROM  
         .Transdetail TX
         INNER JOIN .Account AX  
             ON AX.account_id = TX.account_id  
     WHERE  
         AX.ledger_id IN (
                          SELECT ledger_id 
                          FROM ledger 
                          WHERE ledger_short_name = 'IN'
                         ) --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
         AND TX.document_id = D.document_id  
         AND TX.spare = 'COMM'  
    ) Commission  
FROM  
    Insurance_File I
    INNER JOIN GIS_Policy_Link L
        ON L.insurance_file_cnt = I.insurance_folder_cnt  
    INNER JOIN SBO_Policy_Binder B
        ON B.gis_policy_link_id = L.gis_policy_link_id  
    INNER JOIN SBO_Coach2 RC
        ON RC.SBO_policy_binder_id = B.SBO_policy_binder_id 
    INNER JOIN SBO_Coach2_Drivers RD
        ON RD.SBO_Coach2_id = RC.SBO_Coach2_id  
        AND RD.SBO_policy_binder_id = RC.SBO_policy_binder_id
        AND RD.effective_date <= @end_date    
    INNER JOIN GIS_User_Def_Detail GReason
        ON GReason.gis_user_def_detail_id = RC.Reason_For_Issue  
    INNER JOIN GIS_User_Def_Detail GDistrict
        ON GDistrict.gis_user_def_detail_id = RC.District  
    INNER JOIN GIS_User_Def_Detail GDriving
        ON GDriving.gis_user_def_detail_id = RC.Driving_Restrictions  
    INNER JOIN Transaction_Export_Folder F
        ON F.insurance_file_cnt = I.insurance_file_cnt
        AND F.accounts_export_status = 'c'    
    INNER JOIN .Document D
        ON D.document_ref = F.document_ref 
        AND D.document_date BETWEEN @start_date AND @end_date  
    INNER JOIN .Transdetail T
        ON T.document_id = D.document_id 
        AND  
       (  
        (  
         D.documenttype_id IN (4, 15, 17, 31, 35)  
         AND  
         T.amount > 0  
        )  
       OR  
        (  
         D.documenttype_id NOT IN (4, 15, 17, 31, 35)  
         AND  
         T.amount < 0  
        )  
       )   
    INNER JOIN .Account A
        ON A.account_id = T.account_id
        AND A.ledger_id IN (
                            SELECT ledger_id 
                            FROM ledger 
                            WHERE ledger_short_name = 'SA'
                           ) --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id    
    INNER JOIN Party P
        ON A.account_key = P.party_cnt  
    INNER JOIN Party_Address_Usage U
        ON U.party_cnt = P.party_cnt  
    INNER JOIN Address_Usage_Type UT
        ON UT.address_usage_type_id = U.address_usage_type_id
        AND UT.code = '3131 XCO'    
    INNER JOIN Address PA
        ON PA.address_cnt = U.address_cnt  
    LEFT OUTER JOIN Party PAg  
        ON PAg.party_cnt = I.lead_agent_cnt  
ORDER BY  
    D.document_id,  
    SortID  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


