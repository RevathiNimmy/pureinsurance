SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Bayliss_MIIC'
GO
CREATE PROCEDURE spu_Report_Bayliss_MIIC
    @start_date datetime,
    @end_date datetime,
    @insurer varchar(20)
AS

DECLARE
    @table_name varchar(30),
    @table_id int

SELECT
    RV.SBO_Vehicles2_ID SortID,
    GReason.description Reason,
    P.resolved_name PolicyHolder,
    I.insurance_ref PolicyNo,
    I.cover_start_date CoverStartDate,
    I.renewal_date RenewalDate,
    RV.effective_date EffectiveDate,
    RV.NCB NCB,
    PA.address1 Address1,
    PA.address2 Address2,
    PA.address3 Address3,
    PA.address4 Address4,
    PA.postal_code PostCode,
    GUse.description VehicleUse,
    GDriving.description Driving,
    RC.Driving_Other_Veh OtherVehicles,
    RC.Foreign_Use ForeignUse,
    RC.Company_Indicator IsCompany,
    RC.Add_Driver_Ind AdditionalDrivers,
    RC.Last_Eff_Date DateProcessed,
    '' DriverName,
    RV.Registration Registration
FROM
    Insurance_File I,
    Insurance_Folder F,
    GIS_Policy_Link L,
    SBO_Policy_Binder B,
    SBO_Coach2 RC,
    SBO_Vehicles2 RV,
    GIS_User_Def_Detail GReason,
    GIS_User_Def_Detail GUse,
    GIS_User_Def_Detail GDriving,
    Party P,
    Party_Address_Usage U,
    Address_Usage_Type UT,
    Address PA,
    Party PIN
WHERE
    L.insurance_file_cnt = I.insurance_folder_cnt
AND B.gis_policy_link_id = L.gis_policy_link_id
AND RC.SBO_policy_binder_id = B.SBO_policy_binder_id
AND RV.SBO_Coach2_id = RC.SBO_Coach2_id
AND RV.SBO_policy_binder_id = RC.SBO_policy_binder_id
AND RC.Last_Eff_Date BETWEEN @start_date AND @end_date
AND GReason.gis_user_def_detail_id = RC.Reason_For_Issue
AND GUse.gis_user_def_detail_id = RV.Vehicle_Use
AND GDriving.gis_user_def_detail_id = RC.Driving_Restrictions
AND F.insurance_folder_cnt = I.insurance_folder_cnt
AND P.party_cnt = F.insurance_holder_cnt
AND U.party_cnt = P.party_cnt
AND UT.address_usage_type_id = U.address_usage_type_id
AND UT.code = '3131 XCO'
AND PA.address_cnt = U.address_cnt
AND PIN.party_cnt = I.lead_insurer_cnt
AND (
        @insurer = 'ALL'
        OR
        (
            @insurer <> 'ALL'
            AND
            PIN.shortname = @insurer
        )
    )

UNION
SELECT
    RD.SBO_Coach2_Drivers_ID SortID,
    GReason.description Reason,
    P.resolved_name PolicyHolder,
    I.insurance_ref PolicyNo,
    I.cover_start_date CoverStartDate,
    I.renewal_date RenewalDate,
    RD.effective_date EffectiveDate,
    0 NCB,
    PA.address1 Address1,
    PA.address2 Address2,
    PA.address3 Address3,
    PA.address4 Address4,
    PA.postal_code PostCode,
    '' VehicleUse,
    GDriving.description Driving,
    RC.Driving_Other_Veh OtherVehicles,
    RC.Foreign_Use ForeignUse,
    RC.Company_Indicator IsCompany,
    RC.Add_Driver_Ind AdditionalDrivers,
    RC.Last_Eff_Date DateProcessed,
    RD.Driver_Name DriverName,
    '' Registration
FROM
    Insurance_File I,
    Insurance_Folder F,
    GIS_Policy_Link L,
    SBO_Policy_Binder B,
    SBO_Coach2 RC,
    SBO_Coach2_Drivers RD,
    GIS_User_Def_Detail GReason,
    GIS_User_Def_Detail GDriving,
    Party P,
    Party_Address_Usage U,
    Address_Usage_Type UT,
    Address PA,
    Party PIN
WHERE
    L.insurance_file_cnt = I.insurance_folder_cnt
AND B.gis_policy_link_id = L.gis_policy_link_id
AND RC.SBO_policy_binder_id = B.SBO_policy_binder_id
AND RD.SBO_Coach2_id = RC.SBO_Coach2_id
AND RD.SBO_policy_binder_id = RC.SBO_policy_binder_id
AND RC.Last_Eff_Date BETWEEN @start_date AND @end_date
AND
(
    RD.date_deleted IS NULL
    OR
    (
    RD.date_deleted IS NOT NULL
    AND
    RD.date_deleted NOT BETWEEN '1900/01/01' AND @end_date
    )
)
AND GReason.gis_user_def_detail_id = RC.Reason_For_Issue
AND GDriving.gis_user_def_detail_id = RC.Driving_Restrictions
AND F.insurance_folder_cnt = I.insurance_folder_cnt
AND P.party_cnt = F.insurance_holder_cnt
AND U.party_cnt = P.party_cnt
AND UT.address_usage_type_id = U.address_usage_type_id
AND UT.code = '3131 XCO'
AND PA.address_cnt = U.address_cnt
AND PIN.party_cnt = I.lead_insurer_cnt
AND (
        @insurer = 'ALL'
        OR
        (
            @insurer <> 'ALL'
            AND
            PIN.shortname = @insurer
        )
    )

ORDER BY
    I.insurance_ref,
    SortID

GO

