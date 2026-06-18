
EXECUTE DDLDropProcedure 'spu_wp_insurancefileassociates'
GO


CREATE PROCEDURE spu_wp_insurancefileassociates
    @PartyCnt INT,
	@InsuranceFileCnt INT,
	@RiskId INT,
	@ClaimCnt INT,
	@DocumentRef VARCHAR(25),
	@Instance1 INT,
	@Instance2 INT,
	@Instance3 INT

AS
BEGIN
        SELECT  P.resolved_name As Party_Name,
        AT.description As Association_Type_Desc,
        IFA.date_attached,
        IFA.date_removed,IFA.Association_detail,PLS.date_of_birth as Party_DOB,PLS.gender_code as Party_Gender 
        FROM    Insurance_file_Associates IFA
        INNER JOIN Insurance_File INF   ON INF.insurance_file_cnt =  IFA.Insurance_file_cnt
        INNER JOIN Party P   ON IFA.Party_Cnt =  P.party_cnt
        INNER JOIN Association_Type AT  ON IFA.Association_type_id = AT.Association_Type_id
        LEFT JOIN party_lifestyle PLS on PLS.party_cnt = p.party_cnt 
        WHERE IFA.Insurance_File_Cnt = @InsuranceFileCnt 
        And IFA.Insurance_file_associates_cnt=@Instance2
        And IFA.date_attached<=INF.cover_start_date   -- Date Attached is less than or equal to the Cover From Date
        --AND (IFA.date_removed IS Null Or IFA.date_removed> INF.cover_start_date)  --  Date Removed is spaces or greater than the Cover from Date.
END 

GO
 
