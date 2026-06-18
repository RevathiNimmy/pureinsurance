SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_process_authorization_step'
GO

   
CREATE PROCEDURE spu_SAM_process_authorization_step  
@manualjournal_id INT,  
@lApproved        TINYINT,  
@source_id        INT,  
@user_id    INT  
  
AS  
BEGIN  
DECLARE @lNumberOfSteps INT  
DECLARE @lApprovalStep  INT  
DECLARE @bLastStep      TINYINT=0  
DECLARE @GroupType      VARCHAR(50)='Manual Journals'  
DECLARE @UserName  VARCHAR(100)  
DECLARE @GroupCode  VARCHAR(50)  
DECLARE @ReturnMessage  VARCHAR(500)=''  
DECLARE @JournalAmount  NUMERIC(20,2)   
DECLARE @UserMJAuthortyAmount NUMERIC(20,2)  
  
CREATE TABLE #TempResults (Step_Number INT,pmuser_group_id INT)  
INSERT INTO #TempResults  
EXEC spu_Get_Debtor_User_Groups @GroupType=@GroupType,@SourceID=@source_id  
SELECT @lNumberOfSteps = COUNT(1) FROM #TempResults  
  
SELECT @lApprovalStep = COUNT(1) + 1 FROM ManualJournalApproval WHERE manualjournal_id = @manualjournal_id  
  
IF @lApproved=0   
 SELECT @lApprovalStep = @lNumberOfSteps  
  
IF @lApprovalStep = @lNumberOfSteps  
 SELECT @bLastStep = 1  
  
SELECT     @GroupCode= RTRIM(PMUser_Group.code)   
FROM         Debtor_User_Groups INNER JOIN  
                      Debtor_User_Groups_Type ON   
                      Debtor_User_Groups.debtor_user_groups_type_id =  Debtor_User_Groups_Type.debtor_user_groups_type_id INNER JOIN  
                      PMUser_Group ON Debtor_User_Groups.pmuser_group_id = PMUser_Group.pmuser_group_id  
WHERE     (Debtor_User_Groups.is_deleted = 0) AND   
                (Debtor_User_Groups_Type.description =@GroupType) AND   
                (Debtor_User_Groups.source_id = @source_id) AND   
                (Debtor_User_Groups.Step_Number = @lApprovalStep)  
SELECT @UserName=username FROM PMUser WHERE user_id = @user_id  
  
CREATE TABLE #UserGroup(UserGroup VARCHAR(50))  
  
INSERT INTO #UserGroup  
EXEC spu_pmuser_is_name_member @username=@UserName,@group_code=@GroupCode  
  
SELECT @JournalAmount= SUM(amount) FROM ManualJournalDetail WHERE ManualJournal_id=@manualjournal_id AND Amount>0  
  
SELECT @UserMJAuthortyAmount= ISNULL(ManualJournal_currency_amount,0) FROM User_Authorities WHERE user_id=@user_id  
  
IF EXISTS(SELECT 1 FROM ManualJournalApproval WHERE manualjournal_id = @manualjournal_id AND user_id=@user_id) OR EXISTS (SELECT 1 FROM ManualJournal WHERE PMuser_id=@user_id AND manualjournal_id = @manualjournal_id)  
BEGIN  
 SELECT @ReturnMessage='SameUser'  
END 

IF @lNumberOfSteps= 0 AND @ReturnMessage=''  
BEGIN  
    SELECT @ReturnMessage='DebtorGroupMissing'  
END  
  
IF NOT EXISTS(SELECT NULL FROM #UserGroup) AND @ReturnMessage=''  
BEGIN  
 SELECT @ReturnMessage='NotMemberOfUserGroup'  
END  
  
IF @UserMJAuthortyAmount < @JournalAmount AND @ReturnMessage=''  
BEGIN  
 SELECT @ReturnMessage='ExceedUserLimit'  
END  

IF @lApproved=0 AND  @ReturnMessage='SameUser' 
  SELECT @ReturnMessage=''
  
SELECT @lApprovalStep CurrentStep, @bLastStep AS IsLaststep, @GroupCode AS PMUserGroup,@JournalAmount AS JournalAmount,@ReturnMessage AS ValidationMessage  
  
END    
    
  
Go