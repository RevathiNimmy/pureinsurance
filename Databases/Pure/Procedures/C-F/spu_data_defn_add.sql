EXECUTE DDLDropProcedure 'spu_data_defn_add'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spu_data_defn_add
	@data_defn_id int OUTPUT ,
	@type_id int,
	@Description varchar(50) ,
	@Caption varchar(50),
	@type int,
	@display_order int,
	@Mandatory bit,
	@read_only bit,
	@Claim_party_type_id int,
	@Claim_Lookup_id int,
	@Tab_ID int,
	@Mode bit
AS
BEGIN
Declare @chkorder int

If @Mode=0	begin	

	INSERT INTO Risk_data_definition(Risk_type_id, Description, Caption,
				type, display_order, Mandatory, read_only,
				Claim_party_type_id, Claim_Lookup_id, Tab_ID)
	VALUES (@type_id, @Description, @Caption,
		@type, @display_order, @Mandatory, @read_only,
		@Claim_party_type_id, @Claim_Lookup_id, @Tab_ID)

	Select @chkorder=display_order from risk_data_definition where risk_data_defn_id=@@identity

	exec spu_sort_disp_order @chkorder,@type_id
	
	end

Else if @Mode=1
	
	begin

	INSERT INTO Peril_data_definition(Peril_type_id, Description, Caption,
				type, display_order, Mandatory, read_only,
				Claim_party_type_id, Claim_Lookup_id, Tab_ID)
	VALUES (@type_id, @Description, @Caption,
		@type, @display_order, @Mandatory, @read_only,
		@Claim_party_type_id, @Claim_Lookup_id, @Tab_ID)
	Select @chkorder=display_order from peril_data_definition where peril_data_defn_id=@@identity

	exec spu_sort_disp_order_peril @chkorder,@type_id

	end


END
BEGIN
SELECT @data_defn_id = @@IDENTITY
END





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
