SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_Get_Approval_Step_Details'
GO

CREATE PROCEDURE spu_Get_Approval_Step_Details
    @GroupType varchar(20),
    @SourceID   int,
    @StepNumber int,
	@IsViaBulkClaimPayment bit = 0

AS
BEGIN

DECLARE @MultiStepApproval AS VARCHAR(20) = ''
SET @MultiStepApproval = (SELECT TOP 1 Value FROM hidden_options WHERE option_number = 65 AND branch_id = @SourceID)
SET @MultiStepApproval = LTRIM(RTRIM(@MultiStepApproval))

If @IsViaBulkClaimPayment = 1 AND @MultiStepApproval ='1'
BEGIN

	SELECT	Debtor_User_Groups.pmuser_group_id, RTRIM( PMUser_Group.code) as Code,  
				Is_Payment_Type_Claim_Payment  
	FROM	Debtor_User_Groups 
		INNER JOIN Debtor_User_Groups_Type ON Debtor_User_Groups.debtor_user_groups_type_id = Debtor_User_Groups_Type.debtor_user_groups_type_id
		INNER JOIN PMUser_Group ON Debtor_User_Groups.pmuser_group_id = PMUser_Group.pmuser_group_id  
	WHERE	(Debtor_User_Groups.is_deleted = 0) AND  
			(Debtor_User_Groups_Type.description = 'Payments') AND  
			(Debtor_User_Groups.source_id = @SourceID) AND  
			(Debtor_User_Groups.effective_date <= GETDATE()) AND
			Is_Payment_Type_Claim_Payment = 1
END
ELSE
BEGIN

SELECT     Debtor_User_Groups.pmuser_group_id, RTRIM( PMUser_Group.code) as Code,
 Is_Payment_Type_Claim_Payment 
FROM         Debtor_User_Groups INNER JOIN
                      Debtor_User_Groups_Type ON 
                      Debtor_User_Groups.debtor_user_groups_type_id =
                      Debtor_User_Groups_Type.debtor_user_groups_type_id INNER JOIN
                      PMUser_Group ON Debtor_User_Groups.pmuser_group_id = PMUser_Group.pmuser_group_id
WHERE     (Debtor_User_Groups.is_deleted = 0) AND 
                (Debtor_User_Groups_Type.description =@GroupType) AND 
                (Debtor_User_Groups.source_id = @SourceID) AND 
                (Debtor_User_Groups.Step_Number = @StepNumber)
END
END               
GO

