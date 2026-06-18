SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_find_party_claim'
GO


CREATE PROCEDURE spu_find_party_claim
    @partyclaimid int,
    @name varchar(100),
    @address varchar(100),
    @phonenumber varchar(100),
    @claimpartytype varchar(50)
AS


Declare @ssql varchar(255)
Declare @ssql1 varchar(255)
Declare @ssql2 varchar(255)
Declare @claimpartytypeid int
Select @ssql1= 'select a.Party_Claim_id,a.Claim_Party_type_id, a.Name, a.Address,a.License_type ,License_type.description,
 a.License_Number, a.Date_of_Birth, a.Sex, a.party_status, driver_status.description, a.Phone_Number, a.Fax_Number,a.Reference_Number,'
Select @ssql2='a.Reg_Number from party_claim a left outer join License_type on a.License_type = License_type.License_type_id,
 party_claim b left outer join Driver_Status on b.Party_Status = Driver_Status.driver_status_id where a.party_claim_id = b.party_claim_id'
if @partyclaimid IS NOT NULL
Begin
    Select @ssql=@ssql + ' AND a.party_claim_id = ' + convert(varchar(10),@partyclaimid)
End
if @name IS NOT NULL
Begin
    Select @ssql=@ssql + ' AND a.name Like "' + @name + '"'
End
if @address IS NOT NULL
Begin
    Select @ssql=@ssql + ' AND a.address like "' + @address + '"'
End
if @phonenumber IS NOT NULL
Begin
    Select @ssql=@ssql + ' AND a.phone_number like "' + @phonenumber + '"'
End
if @claimpartytype IS NOT NULL
BEGIN
Declare @string varchar(100)
    Select @claimpartytypeid=party_claim.claim_party_type_id from party_claim where party_claim.claim_party_type_id
 IN(Select claim_party_type.claim_party_type_id from claim_party_type where claim_party_type.description=@claimpartytype)
--  Select @string=convert(varchar(100),@claimpartytypeid)
--  Print @string
 If @claimpartytypeid IS NOT NULL
    Begin
        Select @ssql=@ssql + ' AND a.claim_party_type_id=' + convert(varchar(10),@claimpartytypeid) + ''
    End
--  print @ssql
END
--  print "Im out"--(@ssql1 + @ssql2 +@ssql)
    exec (@ssql1 + @ssql2 +@ssql)
GO


