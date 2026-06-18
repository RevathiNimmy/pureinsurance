SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

Execute DDLDropProcedure 'spu_SAM_updateparty_PC_via_messaging'
GO

CREATE PROCEDURE [dbo].[spu_SAM_updateparty_PC_via_messaging]
  @Party_Cnt INT,
  @Initials varchar(255),
  @Forename varchar(255),
  @Surname varchar(255), 
  @Title varchar(255),
  @NationalityCode varchar(255) = ''

AS

DECLARE 
@sOriginal_Resolved_Name varchar(255),
@resolved_name varchar(255), 
@EnhancedResolvedName varchar(2),
@UpdateExistingClient varchar(2)
Declare @Nationality_Id int = 0

select @sOriginal_Resolved_Name  = RTRIM(UPPER(resolved_name ))
from Party
where party_cnt = @Party_Cnt

select @EnhancedResolvedName = value from System_Options where option_number = 5148
select @UpdateExistingClient  = value from System_Options where option_number = 5064

select @Nationality_Id = Nationality_id from Nationality where code = @NationalityCode

if @EnhancedResolvedName = '1' And  @UpdateExistingClient = '1' 
Set @resolved_name = @Title + " " + @Forename + " " + @Surname
else
Set @resolved_name = @Title + " " + @Initials + " " + @Surname

Update Party 
set name = @Surname, 
	resolved_name = @resolved_name
where party_cnt = @Party_Cnt

Update Party_Personal_Client 
set party_title_code = @Title, 
	forename = @Forename,
	initials = @Initials,
	Nationality_id = Case when @Nationality_Id = 0 Then Null Else @Nationality_Id End
where party_cnt = @Party_Cnt
	
/*Updating Insured Name in Insurance File*/
if @sOriginal_Resolved_Name <> RTRIM(UPPER(@resolved_name ))
UPDATE  Insurance_File
SET insured_name = LEFT(@resolved_name, 100)
WHERE insured_cnt = @party_cnt

