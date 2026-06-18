SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Treaty_Party_add'
GO


CREATE PROCEDURE spu_Treaty_Party_add
    @treaty_party_id int output,
    @party_cnt int,
    @treaty_id int,
    @share_percent float,
    @commission_percent float,
    --E016
    @is_Reinsurer_Approved tinyint,
	@UserId int,  
	@UniqueId varchar(50),  
	@ScreenHierarchy varchar(500)
AS

    Insert Into Treaty_Party (
            party_cnt,
            treaty_id,
            share_percent,
            commission_percent,
	    --E016l
	    is_Reinsurer_Approved,
		UserId,
		UniqueId,
		ScreenHierarchy
	)
    Values (@party_cnt,
            @treaty_id,
            @share_percent,
            @commission_percent,
	    --E016
	    @is_Reinsurer_Approved,
		@UserId,
		@UniqueId,
		@ScreenHierarchy)

    Select @treaty_party_id = SCOPE_IDENTITY()

Go