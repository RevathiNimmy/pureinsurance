		 
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_Party_History_Add'
GO

CREATE  PROCEDURE spu_Party_History_Add
		@Party_cnt				INT,
		@User_Changed			VARCHAR(50),
		@Date_Of_Change			DATETIME,
		@Party_Builder_Data		VARCHAR(MAX)
		
AS
BEGIN

	IF ISNULL(@Party_Cnt,0) > 0 
	BEGIN
	
			DECLARE @Party_Data	VARCHAR(MAX) = NULL

			SET @Party_Data =
			(SELECT  party.*,
			(
			SELECT party_agent.* FROM party_agent(NOLOCK) party_agent
			WHERE party.Party_cnt  = party_agent.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)party_agent,
			(
			SELECT Party_Agent_Branch.* FROM Party_Agent_Branch(NOLOCK) Party_Agent_Branch
			WHERE party.Party_cnt  = Party_Agent_Branch.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_Agent_Branch,
			(
			SELECT opb.* FROM Other_Party_Branch(NOLOCK) opb
			WHERE party.Party_cnt  = opb.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Other_Party_Branch,
			(
			SELECT party_address_usage.*,
			(SELECT ADDRESS.* FROM ADDRESS(NOLOCK) ADDRESS WHERE party_address_usage.address_cnt=ADDRESS.address_cnt FOR XML AUTO, ELEMENTS, TYPE)ADDRESS
			FROM party_address_usage(NOLOCK) party_address_usage
			WHERE party.Party_cnt  = party_address_usage.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE )party_address_usage,
			(
			SELECT Contact_Address_Usage.*,
			(SELECT ADDRESS.* FROM ADDRESS(NOLOCK) ADDRESS WHERE party_address_usage.address_cnt=ADDRESS.address_cnt FOR XML AUTO, ELEMENTS, TYPE)ADDRESS
			FROM party_address_usage(NOLOCK) party_address_usage , contact_address_usage(NOLOCK) Contact_Address_Usage
			WHERE party.Party_cnt  = party_address_usage.Party_cnt and party_address_usage.address_cnt=Contact_Address_Usage.address_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Contact_Address_Usage,
			(
			SELECT Party_Contact_Usage.*,
			(SELECT Contact.* FROM Contact(NOLOCK) Contact WHERE Party_Contact_Usage.contact_cnt=Contact.contact_cnt FOR XML AUTO, ELEMENTS, TYPE)Contact
			FROM Party_Contact_Usage(NOLOCK) Party_Contact_Usage
			WHERE party.Party_cnt  = Party_Contact_Usage.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_Contact_Usage,
			(
			SELECT party_conviction.* FROM party_conviction(NOLOCK) party_conviction
			WHERE party.Party_cnt  = party_conviction.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)party_conviction,
			(
			SELECT Party_Net_Data.* FROM Party_Net_Data(NOLOCK) Party_Net_Data
			WHERE party.Party_cnt  = Party_Net_Data.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_Net_Data,
			(
			SELECT Party_Bank.* FROM party_bank(NOLOCK) Party_Bank
			JOIN Account ON Party_Bank.Account_id = Account.Account_id
			WHERE Account.Account_key = party.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_Bank,
			(
			SELECT Party_Loyalty_Scheme.* FROM Party_Loyalty_Scheme(NOLOCK) Party_Loyalty_Scheme
			WHERE party.Party_cnt = Party_Loyalty_Scheme.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_Loyalty_Scheme,
			(
			SELECT Party_Personal_Client.* FROM Party_Personal_Client(NOLOCK) Party_Personal_Client
			WHERE party.Party_cnt  = Party_Personal_Client.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_Personal_Client,
			(
			SELECT Previous_Accidents.* FROM previous_accidents(NOLOCK) Previous_Accidents
			WHERE party.Party_cnt = Previous_Accidents.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Previous_Accidents,
			(
			SELECT Party_Supplier_Business.* FROM Party_Supplier_Business(NOLOCK) Party_Supplier_Business
			WHERE party.Party_cnt = Party_Supplier_Business.Party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_Supplier_Business,
			(
			SELECT party_other.* FROM party_other(NOLOCK) party_other
			WHERE party.party_cnt = party_other.party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)party_other,
			(
			SELECT PMUser.* FROM PMUser(NOLOCK) PMUser
			WHERE party.party_cnt = PMUser.party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)PMUser,
			(
			SELECT party_relationship.* FROM party_relationship(NOLOCK) party_relationship
			WHERE party.party_cnt = party_relationship.party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)party_relationship,
			(
			SELECT Prospect_Policy.* FROM Prospect_Policy(NOLOCK) Prospect_Policy
			WHERE party.party_cnt = Prospect_Policy.party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Prospect_Policy,
			(
			SELECT party_prospect.* FROM party_prospect(NOLOCK) party_prospect
			WHERE party.party_cnt = party_prospect.party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)party_prospect,
			(
			SELECT Party_LifeStyle.* FROM party_lifestyle(NOLOCK) Party_LifeStyle
			WHERE party.party_cnt = Party_LifeStyle.party_cnt
			FOR XML AUTO, ELEMENTS, TYPE)Party_LifeStyle

			FROM Party(NOLOCK) party
			WHERE party_cnt=@party_Cnt
			FOR XML AUTO,  ELEMENTS
			)
			SELECT @Party_Data = Replace(@Party_Data,'<party>','<party xmlns="Party_Data">')
			INSERT INTO Party_History(party_Cnt,party_Data,Party_Builder_Data,Date_Of_Change,User_Changed)
			SELECT @party_Cnt,@Party_Data,@Party_Builder_Data,@Date_Of_Change,@User_Changed

	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
 
  
  