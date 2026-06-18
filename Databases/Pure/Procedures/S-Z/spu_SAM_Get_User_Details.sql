
Execute DDLDropProcedure 'spu_SAM_Get_User_Details'
GO

CREATE PROCEDURE spu_SAM_Get_User_Details

@UserID int

AS

BEGIN

UPDATE PMUser SET lastlogin=GETDATE() where user_id=@UserID

SELECT PMUser.username 'Username',    
 PMUser.full_name 'FullUsername',    
 PMUser.password_change_date 'PasswordChangedDate',    
 PMUser.lastlogin 'LastLogin',    
 PMUser.email_address 'EmailAddress',    
 ISNULL(PMUser.other_party_id, PMUser.party_cnt) 'PartyCnt' ,    
 ISNULL(TPA.resolved_name, party.resolved_name) 'PartyResolvedName',    
 ISNULL(pt.code, party_type.code) 'PartyTypeCode',    
       CAST(party_agent.allow_consolidated_commission AS VARCHAR(1)) 'ConsolidatedAgentCommission',    
 ISNULL(TPA.shortname, party.shortname) 'PartyShortName',
PMUser.is_deleted 'IsDeleted',
       PMUser.user_id 'UserId',
       PMUser.is_temp_password 'IsTempPassword'
 FROM  PMUser    
 LEFT OUTER JOIN party ON party.party_cnt = pmuser.party_cnt    
 LEFT OUTER JOIN Party_Agent ON party.party_cnt = party_agent.party_cnt    
 LEFT OUTER JOIN party_type ON Party.party_type_id = party_type.party_type_id    
 LEFT OUTER JOIN party tpa ON tpa.party_cnt = pmuser.other_party_id    
 LEFT OUTER JOIN party_type pt ON tpa.party_type_id = pt.party_type_id    
 WHERE user_id = @UserID    
    
END 

GO
