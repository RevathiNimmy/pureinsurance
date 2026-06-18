SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_HouseKeep_Sel'
GO


CREATE PROCEDURE spu_SIRRen_HouseKeep_Sel
    @DayNum int,
    @SourceID int = NULL,
	@InsuranceRef varchar(255) = NULL,
    @StatusId int = NULL,
    @RiskGroupId int = NULL
AS

SELECT r.Insurance_Folder_Cnt, i.Insurance_Ref, p.Resolved_Name, s.Description, gbt.Description, p1.Resolved_Name, r.renewal_date

    FROM Renewal_Control r
    JOIN Insurance_File i ON i.Insurance_File_Cnt = r.Old_Insurance_File_Cnt
    JOIN Renewal_Status_Type s ON  s.Renewal_Status_Type_Id = r.Renewal_Status_Type_Id
    JOIN Party P ON P.Party_cnt = r.Party_Cnt
    JOIN GIS_Business_Type gbt ON gbt.Gis_Business_Type_Id = i.Gemini_Business_Type
    JOIN Party P1 ON p1.Party_Cnt = i.Lead_Insurer_Cnt
    JOIN Risk_Code rc ON i.risk_code_id = rc.risk_code_id
    JOIN Risk_Group rg ON rc.risk_group_id = rg.risk_group_id

    WHERE
        -- specific or all branches
        ((@SourceID IS NOT NULL AND i.source_id = @SourceID) OR (@SourceID IS NULL))

    AND
        (
            (
                -- specific policy
                (@InsuranceRef IS NOT NULL AND i.Insurance_Ref = @InsuranceRef)
            )
            OR
            (
                -- number of days since renewal date
                DateDiff(day, (convert(datetime, r.Renewal_Date, 113)) ,(convert(datetime, getdate(), 113))) >= @DayNum
    
                -- specific or all renewal status'
                AND ((@StatusId IS NOT NULL AND s.Renewal_Status_Type_Id = @StatusId) OR (@StatusId IS NULL))
    
                -- specific or all risk groups
                AND ((@RiskGroupId IS NOT NULL AND rg.risk_group_id = @RiskGroupId) OR (@RiskGroupId IS NULL))
    
                -- all policies
                AND (@InsuranceRef IS NULL)
            )
        )

GO


